namespace OITool.Interface.Worker.Judge.Eventer
{
    /// <summary>
    /// The eventer for <see cref="IReporter" />.
    /// </summary>
    public interface IReporterEventer : IWorker
    {
        /// <summary>
        /// Triggered before calling <see cref="IReporter.Make" />.
        /// </summary>
        /// <param name="reportFile">The report file.</param>
        /// <param name="results">The result of judging.</param>
        /// <param name="handler">An action handler generated in independently for this public-method, not for <see cref="IReporter.Make" />.</param>
        /// <returns>Indicates whether processing is complete.</returns>
        public void BeforeMake(ref string reportFile, ref JudgerResult[] results, ActionHandler handler);

        /// <summary>
        /// Triggered after calling <see cref="IReporter.Make" />.
        /// </summary>
        /// <param name="reportFiles">The result of <see cref="IReporter.Make" />.</param>
        /// <param name="results">The result of judging.</param>
        /// <param name="handler">An action handler generated in independently for this public-method, not for <see cref="IReporter.Make" />.</param>
        /// <returns>Indicates whether processing is complete.</returns>
        public void AfterMake(ref string[] reportFiles, ref JudgerResult[] results, ActionHandler handler);
    }
}