using System.Text.Json.Serialization;

namespace OITool.Comm.Data.Judge
{
    public class ArgumentData : IData
    {
        [JsonPropertyName("arg")]
        public OITool.Interface.Judge.JudgerArgument? Argument { get; init; }

        [JsonPropertyName("crd")]
        public string? CurrentDirectory { get; init; }
    }
}
