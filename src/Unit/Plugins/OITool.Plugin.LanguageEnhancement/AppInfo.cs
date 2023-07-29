using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace OITool.Plugin.LanguageEnhancement
{
    static internal class AppInfo
    {
        static internal string BaseDirectory = "";
        
        static internal Interface.Console.IConsole Console = null!;
        
        static internal Setting? Setting;
        static internal JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkCompatibilityIdeographs, UnicodeRanges.CjkSymbolsandPunctuation),
            IgnoreReadOnlyProperties = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };
    }
}