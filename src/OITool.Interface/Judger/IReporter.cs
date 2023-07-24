namespace OITool.Interface.Judger
{
    public interface IReporter
    {
        public Task<bool> Make(string reportFilePath, Judger.JudgeResult[] results);
    }
}
