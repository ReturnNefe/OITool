namespace OITool.Interface
{
    public class ActionHandler
    {
        /// <summary>
        /// The working directory where this action was executed.
        /// </summary>
        /// <value>The full path.</value>
        public string WorkingDirectory { get; init; } = "";

        /// <summary>
        /// Indicates whether to block other plugins after this action is executed.
        /// </summary>
        /// <value></value>
        public bool PreventOther { get; set; } = false;

        public ActionHandler() { }

        public string ConvertToWorkingDirectory(string path) => Path.GetFullPath(Path.IsPathFullyQualified(path) ? path : Path.Combine(WorkingDirectory, path));
    }
}