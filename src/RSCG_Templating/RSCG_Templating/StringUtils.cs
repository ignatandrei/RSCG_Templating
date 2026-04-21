using System;
using System.Text;
//copied from svgicongenerator
namespace RSCG_Templating;
internal static class StringUtils
{
    public static string ConvertToPascalCase(string kebabCase)
    {
        // Replace hyphens with underscores in the kebab case to create a valid identifier
        // This preserves the original structure while making it a valid C# identifier
        // e.g., "arrow-down-0-1" -> "ArrowDown0_1"
        //       "arrow-down-01" -> "ArrowDown01"
        StringBuilder result = new();
        bool capitalizeNext = true;
        bool lastWasDigit = false;

        foreach (char ch in kebabCase)
        {
            if (ch == '-')
            {
                // If the last character was a digit and we're at a hyphen,
                // check if the next character is also a digit
                // If so, use underscore to preserve distinction
                if (lastWasDigit)
                {
                    result.Append('_');
                    capitalizeNext = false;
                }
                else
                {
                    capitalizeNext = true;
                }
                lastWasDigit = false;
            }
            else if (char.IsDigit(ch))
            {
                result.Append(ch);
                lastWasDigit = true;
                capitalizeNext = false;
            }
            else
            {
                if (capitalizeNext)
                {
                    result.Append(char.ToUpperInvariant(ch));
                    capitalizeNext = false;
                }
                else
                {
                    result.Append(ch);
                }
                lastWasDigit = false;
            }
        }

        return result.ToString();
    }
}

