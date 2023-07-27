namespace OITool.Interface.Judge
{
    /// <summary>
    /// The Result of <see cref="IJudger.Judge" />.
    /// </summary>
    public struct JudgerResult
    {
        /// <summary>
        /// The time the program started running. 
        /// </summary>
        public DateTime Time { get; init; } = DateTime.Now;
        
        /// <summary>
        /// The input data-file provided by users or converted by IJudger.
        /// </summary>
        public string InputFile { get; init; } = "";
        
        /// <summary>
        /// The anwser data-file provided by users or converted by IJudger.
        /// </summary>
        public string AnswerFile { get; init; } = "";
        
        /// <summary>
        /// The content of the program output.
        /// </summary>
        public string ProgramOutput { get; init; } = "";
        
        /// <summary>
        /// The time it took for the program to run.
        /// </summary>
        public double TimeUsed { get; init; } = 0;
        
        /// <summary>
        /// The time limit of the judged program.
        /// </summary>
        public int Timeout { get; init; } = 0;
        
        /// <summary>
        /// The memory it used for the program to run.
        /// </summary>
        /// <value></value>
        public double MemoryUsed { get; init; } = 0;
        
        /// <summary>
        /// The memory limit of the judged program.
        /// </summary>
        public double MemoryLimit { get; init; } = 0;
        
        /// <summary>
        /// The judge status of the program.
        /// </summary>
        /// <value></value>
        public JudgerStatus Status { get; init; } = JudgerStatus.Accepted;

        /// <summary />
        public JudgerResult() { }
    }
}
