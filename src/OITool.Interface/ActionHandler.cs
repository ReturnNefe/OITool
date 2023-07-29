namespace OITool.Interface
{
    /// <summary>
    /// Generated and passed in independently for each public-method of <see cref="Worker.IWorker" />
    /// </summary>
    public class ActionHandler
    {
        /// <summary>
        /// The working directory where this action was executed.
        /// </summary>
        /// <value>The full path.</value>
        public string WorkingDirectory { get; init; } = "";

        /// <summary>
        /// <para>Indicates whether to block other plugins after this action is executed.</para>
        /// <para>Note that do not set this value to true unless necessary.</para>
        /// </summary>
        /// <value></value>
        public bool PreventOther { get; set; } = false;

        /// <summary/>
        public ActionHandler() { }

        /// <summary>
        /// Try to recover a relative path to <see cref="WorkingDirectory" />.
        /// If <paramref name="path"/> is absolute, it will do nothing.
        /// </summary>
        /// <param name="path">A relative or absolute path.</param>
        /// <returns>The converted path.</returns>
        public string ConvertToWorkingDirectory(string path) => Path.GetFullPath(Path.IsPathFullyQualified(path) ? path : Path.Combine(WorkingDirectory, path));
    }
}