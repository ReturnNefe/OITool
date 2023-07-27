using System.Text.Json.Serialization;

namespace OITool.Comm.Data.Judge
{
    public class ResultData : IData
    {
        [JsonPropertyName("res")]
        public Base.Worker.Judger.Result? Result { get; init; }
    }
}
