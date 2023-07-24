namespace OITool.Interface
{
    public interface IPlugin : IEventPlugin
    {
        public PluginInfo Info { get; }
        
        public void Initialize(PluginContext context);
    }
}
