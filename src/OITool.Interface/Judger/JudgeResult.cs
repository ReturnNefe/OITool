namespace OITool.Interface.Judger
{
    public struct JudgeResult
    {
        public enum JudgeStatus
        {
            Accepted,
            WrongAnswer,
            TimeLimitExceed,
            MemoryLimitExceed,
            RuntimeError
        }

        public DateTime Time { get; init; }
        public string ProgramFile { get; init; }
        public string ProgramOutput { get; init; }
        public string DataFile { get; init; }
        public string AnswerFile { get; init; }
        public double TimeUsed { get; init; }
        public int Timeout { get; init; }
        public JudgeStatus Status { get; init; }

    }
}
