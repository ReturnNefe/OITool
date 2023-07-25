namespace OITool.Interface.Judge
{
    /// <summary>
    /// The Result of IJudger.Judge
    /// </summary>
    public struct Result
    {
        public DateTime Time { get; init; } = DateTime.Now;
        public string InputFile {get;init;} = "";
        public string AnswerFile { get; init; } = "";
        public string ProgramOutput { get; init; } = "";
        public double TimeUsed { get; init; } = 0;
        public int Timeout { get; init; } = 0;
        public Status Status { get; init; } = Status.Accepted;
        
        public Result() { }
    }
}
