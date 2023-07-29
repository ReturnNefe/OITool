namespace OITool.Interface.Worker.Judge.Eventer
{
    /// <summary>
    /// The eventer for <see cref="IJudger" />.
    /// </summary>
    public interface IJudgerEventer : IWorker
    {
        /// <summary>
        /// Triggered before calling <see cref="IJudger.Judge" />.
        /// </summary>
        /// <param name="argument">The arguments used.</param>
        /// <param name="option">The options used.</param>
        /// <param name="handler">An action handler generated in independently for this public-method, not for <see cref="IJudger.Judge" />.</param>
        /// <returns>Indicates whether processing is complete.</returns>
        public void BeforeJudge(ref JudgerArgument argument, ref JudgerOption option, ActionHandler handler);
        
        /// <summary>
        /// Triggered after calling <see cref="IJudger.Judge" />.
        /// </summary>
        /// <param name="argument">The arguments used.</param>
        /// <param name="option">The options used.</param>
        /// <param name="result">The result of <see cref="IJudger.Judge" />.</param>
        /// <param name="handler">An action handler generated in independently for this public-method, not for <see cref="IJudger.Judge" />.</param>
        /// <returns>Indicates whether processing is complete.</returns>
        public void AfterJudge(ref JudgerArgument argument, ref JudgerOption option, ref JudgerResult[] result, ActionHandler handler);
    }
}