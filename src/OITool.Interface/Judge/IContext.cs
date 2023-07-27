namespace OITool.Interface.Judge
{
    /// <summary />
    public interface IContext
    {
        /// <summary>
        /// Register a <see cref="IJudger" />.
        /// </summary>
        public void AddJudger(IJudger judger);
        
        /// <summary>
        /// Register a <see cref="IReporter" />.
        /// </summary>
        public void AddReporter(IReporter reporter);
    }
}