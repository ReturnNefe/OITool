namespace OITool.Interface
{
    /// <summary>
    /// The context for plugins.
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// The judge context which you can register judgers.
        /// </summary>
        public Judge.IContext Judge { get; }
    }
}