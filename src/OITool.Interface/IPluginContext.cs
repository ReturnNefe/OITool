namespace OITool.Interface
{
    /// <summary>
    /// The context for <see cref="Interface.Worker"/>.
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// The context which you can register judgers.
        /// </summary>
        public Worker.Judge.IContext Judge { get; }
    }
}