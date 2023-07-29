using System.Text.Json.Serialization;

namespace OITool.Comm.Data.Judge
{
    public class ArgumentData : IData
    {
        [JsonPropertyName("mode")]
        public string? Mode { get; init; }
        
        [JsonPropertyName("prf")]
        public string ProgramFile { get; init; } = "";
        
        [JsonPropertyName("daf")]
        public string[] DataFiles { get; init; } = Array.Empty<string>();
        
        [JsonPropertyName("tmt")]
        public int? Timeout { get; init; }
        
        [JsonPropertyName("mel")]
        public double? MemoryLimit { get; init; }
        
        [JsonPropertyName("ref")]
        public string? ReportFile { get; init; }

        [JsonPropertyName("crd")]
        public string? CurrentDirectory { get; init; }
    }
}
