using System.Text.Json;

namespace OITool.Plugin.LanguageEnhancement
{
    public class Main : Interface.IPlugin
    {
        private Interface.PluginInfo info = new() { Name = "LanguageEnhancement", Description = "The enhancement of language for OITool.", Author = "ReturnNefe", Version = new(0, 1, 1) };
        public Interface.PluginInfo Info => this.info;

        public void Initialize(Interface.IPluginContext context, Interface.Console.IConsole console)
        {
            AppInfo.Console = console;
            
            context.Judge.Eventer.AddJudgerEventer(new Judge.Eventer.JudgerEventer("languageEnhancement.eventer.judger"));
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
