using OITool.Interface;
namespace OITool.Base.Context
{
    public class PluginContext : IPluginContext
    {
        public Interface.Judge.IContext Judge { get; }
        
        public PluginContext(Interface.Judge.IContext judge)
        {
            this.Judge = judge;
        }
    }
}