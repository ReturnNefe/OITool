namespace OITool.Interface.Worker.Judge.Eventer
{
    /// <summary>
    /// The context for <see cref="Interface.Worker.Judge.Eventer" />
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Register a <see cref="IJudgerEventer" />.
        /// </summary>
        public void AddJudgerEventer(IJudgerEventer judgerEventer);
        
        /// <summary>
        /// Register a <see cref="IReporterEventer" />.
        /// </summary>
        public void AddReporterEventer(IReporterEventer reporterEventer);
    }
}