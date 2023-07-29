using System.Text.Json.Serialization;

namespace OITool.Server
{
    public class Setting
    {
        public class WhitelistSetting
        {
            public class JudgeSetting
            {
                [JsonPropertyName("judgers")]
                public string[]? Judgers { get; init; }

                [JsonPropertyName("reporters")]
                public string[]? Reporters { get; init; }
            }

            [JsonPropertyName("judge")]
            public JudgeSetting? Judge { get; init; }
        }

        public class OptionSetting
        {
            public class ExtensionSetting
            {
                [JsonPropertyName("input-data")]
                public string[]? InputData { get; init; }

                [JsonPropertyName("output-data")]
                public string[]? OutputData { get; init; }
            }

            [JsonPropertyName("mode")]
            public string? Mode { get; init; }

            [JsonPropertyName("timeout")]
            public int? Timeout { get; init; }

            [JsonPropertyName("memory-limit")]
            public int? MemoryLimit { get; init; }

            [JsonPropertyName("report-file")]
            public string? ReportFile { get; init; }

            [JsonPropertyName("extension")]
            public ExtensionSetting? Extension { get; init; }
        }

        [JsonPropertyName("whitelist")]
        public WhitelistSetting? Whitelist { get; init; }

        [JsonPropertyName("option")]
        public OptionSetting? Option { get; init; }
    }
}