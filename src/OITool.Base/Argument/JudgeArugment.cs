namespace OITool.Base.Argument
{
    /// <summary>
    /// The Argument of Judger
    /// </summary>
    public struct JudgeArugment
    {
        // The way to judge
        public enum JudgeModes
        {
            SingleFile,
            Folder,
            // DataPackage
        }

        public string ProgramPath { get; init; } = "";

        public JudgeModes JudgeMode { get; init; } = JudgeModes.SingleFile;

        public string StdInputFilePath { get; init; } = "";
        public string StdOutputFilePath { get; init; } = "";

        public string StdDataFolderPath { get; init; } = "";

        // NOTE:
        // To be added.
        // public string StdDataPackagePath;

        public int Timeout { get; init; } = 1000;

        // Report File (html)
        public string? ReportFilePath { get; init; } = null;

        public JudgeArugment() { }
    }
}