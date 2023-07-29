using System.Text.Json;
using OITool.Interface;

namespace OITool.Plugin.Default
{
    public class Main : OITool.Interface.IPlugin
    {
        private PluginInfo info = new() { Name = "Default", Description = "The default plugin for OITool.", Author = "ReturnNefe", Version = new(0, 1, 1) };
        public PluginInfo Info => this.info;

        public void Initialize(IPluginContext context, Interface.Console.IConsole console)
        {
            context.Judge.AddJudger(new Judge.Judger("default.judge.judger"));
            context.Judge.AddReporter(new Judge.Reporter("default.judge.reporter"));
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
