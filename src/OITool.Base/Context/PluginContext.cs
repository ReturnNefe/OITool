using OITool.Interface;
namespace OITool.Base.Context
{
    public class PluginContext : IPluginContext
    {
        public Interface.Worker.Judge.IContext Judge { get; }
        
        public PluginContext(Interface.Worker.Judge.IContext judge)
        {
            this.Judge = judge;
        }
    }
}