using System.Text.Json;
using OITool.Interface;

namespace OITool.Plugin.Default
{
    public class Default : OITool.Interface.IPlugin
    {
        public PluginInfo Info => new() { Name = "Default", Description = "The default plugin for OITool.", Author = "ReturnNefe", Version = new(0, 1, 0) };

        public void Initialize(PluginContext context)
        {
            context.AddReporter("default.reporter", new Reporter());
        }

        public async Task OnLoading(string baseDirectory)
        {
            AppInfo.BaseDirectory = baseDirectory;
            
            using (var reader = new StreamReader(Path.Combine(baseDirectory, "config.txt")))
            {
                AppInfo.Setting = JsonSerializer.Deserialize<Setting>(await reader.ReadToEndAsync(), AppInfo.JsonOptions);
            }
        }

        public Task OnUnloading() => Task.CompletedTask;
    }
}
