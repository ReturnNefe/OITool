namespace OITool.Interface.Worker.Judge
{
    /// <summary>
    /// The context for <see cref="Interface.Worker.Judge" />.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// The context which you can register eventers.
        /// </summary>
        public Eventer.IContext Eventer { get; }
        
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