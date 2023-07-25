namespace OITool.Interface.Judge
{
    /// <summary>
    /// The Argument of IJudger
    /// </summary>
    public struct Argument
    {
        private string mode = "";
        
        /// <summary>
        /// The mode used when judging.
        /// </summary>
        /// <returns>The lower mode string.</returns>
        public string Mode { get => mode; init => mode = value.ToLower(); }

        /// <summary>
        /// The program to be judged provided by the user.
        /// </summary>
        /// <value>The original string.</value>
        public string ProgramFile { get; init; } = "";

        /// <summary>
        /// The data used to judge provided by the user.
        /// </summary>
        /// <returns>The array of original string.</returns>
        public string[] DataFiles { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The time limit of the program to be judged.
        /// </summary>
        public int Timeout { get; init; } = 1000;

        /// <summary>
        /// The report file or name.
        /// </summary>
        /// <value>The original string.</value>
        public string? ReportFile { get; init; } = null;

        public Argument() { }
    }
}
