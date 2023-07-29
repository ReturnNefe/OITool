namespace OITool.Interface
{
    /// <summary>
    /// The interface of plugins.
    /// </summary>
    public interface IPlugin : IEventPlugin
    {
        /// <summary>
        /// The infomation of the plugin.
        /// </summary>
        public PluginInfo Info { get; }
        
        /// <summary>
        /// Occurs when the plugin is being initializing.
        /// </summary>
        /// <param name="context">The context for the plugin which you can register <see cref="Worker.IWorker"/></param>
        /// <param name="console">The interface of console.</param>
        public void Initialize(IPluginContext context, Interface.Console.IConsole console);
    }
}
