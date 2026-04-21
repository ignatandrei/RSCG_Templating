using Scriban.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSCG_Templating;

[Generator]

public class GenAddFiles : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {

        var classesAddFiles = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (s, _) => IsSyntaxTargetForGeneration(s),
                transform: (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null)!
            ;

        IncrementalValuesProvider<AdditionalText> textFiles = context
            .AdditionalTextsProvider
            .Where(file => true);

        IncrementalValuesProvider<(AdditionalText item, string path)> namesAndPath = textFiles
           .Select((text, cancellationToken) => (item: text, path: text.Path));

        var compilationAndData
            = namesAndPath.Collect().Combine(classesAddFiles.Collect());

        ;
        context.RegisterSourceOutput(compilationAndData,
            (spc, dataX) =>
            ExecuteGen(spc, dataX));
    }

    private void ExecuteGen(SourceProductionContext spc,
        (ImmutableArray<(AdditionalText item, string path)> Left,
        ImmutableArray<Tuple<ClassDeclarationSyntax, INamedTypeSymbol>?> Right) dataX)
    {
        var addtional = dataX.Left.ToArray();
        var pathFiles = addtional
            .Select(it =>
                new { it.path
                ,name=Path.GetFileName(it.path)
                ,namePascal = StringUtils.ConvertToPascalCase(Path.GetFileNameWithoutExtension(it.path))
                ,nameNoExt = Path.GetFileNameWithoutExtension(it.path)
                ,pathArr= it.path.Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries).ToArray()                
                }
            ) 
            .ToArray();
        var classes = dataX.Right.Where(it => it != null && it.Item1 != null).Select(it => it!).ToArray();
        if(classes.Length== 0)   return;
        foreach (var tpl in classes)
        {
            var symbolClass = tpl.Item2;

            var ts = symbolClass as ITypeSymbol;
            if (ts == null) continue;

            var allAtt = ts.GetAttributes()
                .Where(it => it.AttributeClass != null && it.AttributeClass.Name.Contains("IGenerateDataFromAdditionalFiles"))
                .ToArray();
            ;
            if (allAtt.Length == 0) continue;

            var cds = tpl.Item1;
            var data = new ClassData();
            var baseNamespace = symbolClass.ContainingNamespace.IsGlobalNamespace? string.Empty: symbolClass.ContainingNamespace.ToDisplayString();
            var name = baseNamespace;
            data.nameSpace = name;
            data.className = cds.Identifier.ValueText;

            foreach (var myAtt in allAtt)
            {
                var nameAdd = myAtt.ConstructorArguments.First().Value?.ToString();

                var addText = addtional.Where(it => it.path.EndsWith($"{nameAdd}.txt")).ToArray();


                if (addText.Length != 1) continue;

                var templateText = addText[0].item.GetText();
                if (templateText == null) continue;
                Template? template=null;
                try
                {
                    template = Template.Parse(templateText.ToString());
                    if (template.HasErrors)
                    {
                        var errors = template.Messages.Select(it => it.Message+"=="+ it.ToString()).ToArray();
                        var dd = new DiagnosticDescriptor(
                        "RSCG_TEMPLATING_ERROR1",
                        "ParseError",
                        "ParseError: {0}",
                        "RSCG_TEMPLATING",
                        DiagnosticSeverity.Error,
                        true);
                        spc.ReportDiagnostic(Diagnostic.Create(dd, Location.None, errors));
                        continue;
                    }
                }
                catch (Exception ex)
                {                    
                    var dd = new DiagnosticDescriptor("RSCG_TEMPLATING_ERROR1", "ParseError", "ParseError: {0}", "RSCG_TEMPLATING", DiagnosticSeverity.Error, true);
                    Diagnostic d = Diagnostic.Create(dd, Location.None, ex.Message);
                    spc.ReportDiagnostic(d);
                    continue;
                }

                ScriptObject scriptObject = new ();
                scriptObject.Import(
                    new { data, fileName = addText[0].path, pathFiles}
                ,null,member => member.Name

                );
                TemplateContext context = new ();
                context.MemberRenamer = member => member.Name;
                context.LoopLimit = int.MaxValue-1; 
                context.PushGlobal(scriptObject);
                //will do with SCRIBAN . Every class has a corresponding scriban additional file.
                var result = template!.Render(context);//
                                                                                            //var result = "namespace asd{ class MyData{ public int id=9;}}";
                var fileName = $"{data.nameSpace}.{data.className}.{nameAdd}";
                spc.AddSource(fileName, result);

            }


        }
    }

    private Tuple<ClassDeclarationSyntax, INamedTypeSymbol>? GetSemanticTargetForGeneration(GeneratorSyntaxContext ctx)
    {
        var cds = ctx.Node as ClassDeclarationSyntax;
        if (cds == null) return null;

        if (cds.Parent != null && (cds.Parent is not BaseNamespaceDeclarationSyntax)) return null;
        var data = ctx.SemanticModel.GetDeclaredSymbol(cds);
        if (data == null) return null;
        return new Tuple<ClassDeclarationSyntax, INamedTypeSymbol>(cds, data);

    }

    private bool IsSyntaxTargetForGeneration(SyntaxNode s)
    {
        if (s is not ClassDeclarationSyntax cds) return false;
        if (cds.AttributeLists.Count == 0) return false;
        var exists= cds.AttributeLists.Any(it => it.ToFullString().Trim().Contains("IGenerateDataFromAdditionalFiles"));
        return exists;
    }

}