namespace OITool.Interface.Judge
{
    public interface IReporter : IWorker
    {
        /// <summary>
        /// Make a report from Result.
        /// </summary>
        /// <param name="reportFile">The report file.</param>
        /// <param name="results">The result of judging.</param>
        /// <param name="handler">An action handler.</param>
        /// <returns>The path of report file, if not generate, it should be null.</returns>
        public Task<string?> Make(string reportFile, Result[] results, ActionHandler handler);
    }
}
