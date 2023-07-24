namespace OITool.Interface
{
    public interface PluginContext
    {
        public void AddReporter(string key, Judger.IReporter reporter);
    }
}