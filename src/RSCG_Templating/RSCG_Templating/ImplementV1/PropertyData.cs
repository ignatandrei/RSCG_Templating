using RSCG_TemplatingCommon.InterfacesV1;

namespace RSCG_Templating.ImplementV1;

internal class PropertyData : IPropertyData
{
    private readonly IPropertySymbol propertySymbol;
    public string PropertyName { get; private set; }
    public string PropertyType { get; private set; }
    public bool CanCallSetMethod { get; private set; }
    public bool CanCallGetMethod { get; private set; }
    public Accessibility GetAccesibility { get; private set; }
    public Accessibility SetAccesibility { get; private set; }

    public bool IsString { get; private set; }
    public bool IsClass { get; private set; }
    public bool IsInterface { get; private set; }
    public bool IsArray { get; private set; }
    public PropertyData(IPropertySymbol propertySymbol)
    {
        this.propertySymbol = propertySymbol;
        PropertyName = propertySymbol.Name;
        PropertyType = propertySymbol.Type.ToDisplayString();
        IsString = propertySymbol.Type.SpecialType == SpecialType.System_String;
        IsClass = propertySymbol.Type.TypeKind == TypeKind.Class;
        IsInterface = propertySymbol.Type.TypeKind == TypeKind.Interface;
        IsArray = propertySymbol.Type.TypeKind == TypeKind.Array;
        var getAcces = propertySymbol.GetMethod;
        CanCallGetMethod = getAcces != null;
        if (CanCallGetMethod)
        {
            GetAccesibility= (Accessibility)((int)getAcces!.DeclaredAccessibility);
        }
        var setAcces = propertySymbol.SetMethod;
        CanCallSetMethod = setAcces != null;
        if(CanCallSetMethod)
        {
            SetAccesibility = (Accessibility)((int)setAcces!.DeclaredAccessibility);
        }

    }

    //public string PropertyCode
    //{

    //    get
    //    {


    //        var get = CanCallGetMethod ? $$"""
    //            get{
    //            return original.{{PropertyName}};
    //            }
    //    """ : "";
    //        var set = CanCallSetMethod ? $$"""
    //        set{
    //            original.{{PropertyName}}=value;
    //        }            

    //    """ : "";
    //        return $$"""
    //        public {{Type}} {{PropertyName}} {
    //            {{get}}
    //            {{set}}            
    //        } 
    //        """;

    //    }
    //}

}


