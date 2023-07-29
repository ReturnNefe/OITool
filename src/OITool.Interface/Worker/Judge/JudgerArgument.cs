namespace OITool.Interface.Worker.Judge
{
    /// <summary>
    /// The Argument of <see cref="IJudger" />.
    /// </summary>
    public struct JudgerArgument
    {
        #region [Private Field]

        private string mode = "";

        #endregion

        #region [Public Property]

        /// <summary>
        /// The mode used when judging.
        /// </summary>
        /// <returns>The lower mode string.</returns>
        public string Mode { get => mode; set => mode = value.ToLower(); }

        /// <summary>
        /// The program to be judged provided by users.
        /// </summary>
        /// <value>
        /// The original path which was not converted.
        /// You may need to convert it to absolute path by using <see cref="OITool.Interface.ActionHandler.ConvertToWorkingDirectory(string)"/>.
        /// </value>
        public string ProgramFile { get; set; } = "";

        /// <summary>
        /// The data used to judge provided by users.
        /// </summary>
        /// <returns>The array of original string.</returns>
        public string[] DataFiles { get; set; } = Array.Empty<string>();

        /// <summary>
        /// The time limit of the program to be judged.
        /// </summary>
        public int Timeout { get; set; } = 1000;

        /// <summary>
        /// The memory limit (MB) of the program to be judged.
        /// </summary>
        public double MemoryLimit { get; set; } = 256;

        /// <summary>
        /// The report file or name.
        /// </summary>
        /// <value>The original string.</value>
        public string? ReportFile { get; set; } = null;

        #endregion

        /// <summary />
        public JudgerArgument() { }
    }
}
