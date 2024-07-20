namespace RSCG_TemplatingCommon.InterfacesV1
{

    public interface IPropertyData
    {
        bool CanCallSetMethod { get; }
        bool CanCallGetMethod { get; }
        string PropertyName { get; }
        string PropertyType { get; }

        bool IsString { get; }
        bool IsClass { get;  }
        bool IsInterface { get;  }
        bool IsArray { get; }


    }

}