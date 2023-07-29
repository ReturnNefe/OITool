using System.Text.Json.Serialization;

namespace OITool.Plugin.LanguageEnhancement
{
    public class Setting
    {
        public class CppLangSetting
        {
            public class BuildSetting
            {
                [JsonPropertyName("builder")]
                public string? Builder { get; init; }

                [JsonPropertyName("args")]
                public string? Argument { get; init; }
                
                [JsonPropertyName("output")]
                public string? Output { get; init; }

                [JsonPropertyName("timeout")]
                public int? Timeout { get; init; }
            }

            [JsonPropertyName("extension")]
            public string[]? Extension { get; init; }

            [JsonPropertyName("build")]
            public BuildSetting? Build { get; init; }
        }

        [JsonPropertyName("cpp")]
        public CppLangSetting? CppLang { get; init; }
    }
}