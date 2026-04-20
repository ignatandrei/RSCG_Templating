using RSCG_TemplatingCommon.InterfacesV1;

namespace RSCG_Templating.ImplementV1;

internal class ClassData : IClassData
{
    public string Version { get; set; } = "2026.420.2103";
    public string? nameSpace { get; set; }
    public string? className { get; set; }
    public IMethodData[] methods { get; set; } = new MethodData[0];
    public IPropertyData[] properties { get; set; } = new PropertyData[0];
    public string[] Interfaces { get; set; } = new string[0];
}
