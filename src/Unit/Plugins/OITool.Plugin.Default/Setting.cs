using System.Text.Json.Serialization;

namespace OITool.Plugin.Default
{
    public class Setting
    {
        public class TextColorSetting
        {
            [JsonPropertyName("accepted")]
            public string? Accepted { get; set; } = null;

            [JsonPropertyName("wrong-answer")]
            public string? WrongAnswer { get; set; } = null;

            [JsonPropertyName("time-limit-exceed")]
            public string? TimeLimitExceed { get; set; } = null;

            [JsonPropertyName("memory-limit-exceed")]
            public string? MemoryLimitExceed { get; set; } = null;

            [JsonPropertyName("runtime-error")]
            public string? RuntimeError { get; set; } = null;
        }

        [JsonPropertyName("text-color")]
        public TextColorSetting? TextColor { get; set; } = null;

        [JsonPropertyName("bytes-limit")]
        public int? BytesLimit { get; set; } = null;
    }
}