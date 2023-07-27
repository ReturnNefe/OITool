using System.Text.Json.Serialization;

namespace OITool.Comm.Data
{
    public class IData
    {
        [JsonPropertyName("extinfo")]
        public string? ExtraInformation { get; init; }
    }
}