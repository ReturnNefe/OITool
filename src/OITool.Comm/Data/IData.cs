using System.Text.Json.Serialization;

namespace OITool.Comm.Data
{
    public class IData
    {
        public class TextLine
        {
            [JsonPropertyName("sep")]
            public string? Separator { get; init; }

            [JsonPropertyName("ctxt")]
            public Tuple<string, string>[]? ColoredText { get; init; }
        }

        [JsonPropertyName("extrain")]
        public string? ExtraInformation { get; init; }

        [JsonPropertyName("conin")]
        public TextLine[]? ConsoleInformation { get; init; }
    }
}