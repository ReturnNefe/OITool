namespace OITool.Interface.Worker.Judge
{
    /// <summary>
    /// The interface of judgers.
    /// </summary>
    public interface IJudger : IWorker
    {
        /// <summary>
        /// Judge program from arguments and options.
        /// </summary>
        /// <param name="argument">The arguments used.</param>
        /// <param name="option">The options used.</param>
        /// <param name="handler">An action handler generated for this public-method.</param>
        /// <returns>The result of judging.</returns>
        public Task<JudgerResult[]> Judge(JudgerArgument argument, JudgerOption option, ActionHandler handler);
    }
}