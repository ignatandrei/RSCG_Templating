using System;
using System.Collections.Generic;
using System.Text;

namespace RSCG_TemplatingCommon
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class IGenerateDataFromAdditionalFilesAttribute: Attribute
    {
        public IGenerateDataFromAdditionalFilesAttribute(string nameTemplateFile)
        {
            NameTemplateFile = nameTemplateFile;
        }
        public string NameTemplateFile { get;  }

    }
}
