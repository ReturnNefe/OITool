namespace OITool.Interface.Worker
{
    /// <summary>
    /// Responsible for implementing specific features.
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Indicates the global indentifier of the worker. 
        /// </summary>
        public string Identifier { get; }
    }
}