using System.IO;
using System.Text;
using System.Text.Json;
using System.Reflection;
using Nefe.ColorConsole;

namespace OITool.Server
{
    internal class Program
    {
        static async Task Init()
        {
            using (StreamReader reader = new(Path.Combine(AppContext.BaseDirectory, "config.txt"), Encoding.UTF8))
                AppInfo.Setting = JsonSerializer.Deserialize<Setting>(await reader.ReadToEndAsync(), AppInfo.JsonOptions) ?? new();

            AppInfo.Workers = new()
            {
                Judge = new(new(
                    judgerWhiteList: AppInfo.Setting?.Whitelist?.Judge?.Judgers,
                    reporterWhiteList: AppInfo.Setting?.Whitelist?.Judge?.Reporters
                ))
            };
        }

        static async Task LoadPlugin(string directory)
        {
            foreach (var dirPath in Directory.GetDirectories(directory))
            {
                var pluginName = Path.GetFileName(dirPath);
                var pluginFile = Path.Combine(dirPath, $"{pluginName}.dll");

                if (File.Exists(pluginFile))
                {
                    var pluginContext = new Base.Context.PluginContext(
                        judge: new Base.Context.Judge.JudgeContext(
                            eventer: new Base.Context.Judge.EventerContext(
                                judgerEventers: AppInfo.Workers.Judge.Eventer.JudgerEventers,
                                reporterEventers: AppInfo.Workers.Judge.Eventer.ReporterEventers
                            ),
                            judgers: AppInfo.Workers.Judge.Judgers,
                            reporters: AppInfo.Workers.Judge.Reporters
                        )
                    );

                    var plugin = new Nefe.PluginCore.Plugin(pluginFile, isCollectible: true);
                    plugin.LoadFromFile();
                    AppInfo.Plugins.Add(plugin);

                    var iPlugin = plugin.CreateInstances<Interface.IPlugin>().First();
                    await iPlugin.OnLoading(dirPath);
                    iPlugin.Initialize(pluginContext, AppInfo.Console);

                    // Notice plugin when plugin will be unloaded.
                    plugin.Unloading += (e) => iPlugin.OnUnloading();
                }
            }
        }

        static async Task Main(string[] args)
        {
            await Init();

            Console.CancelKeyPress += (_, _) =>
            {
                foreach (var plugin in AppInfo.Plugins)
                    plugin.Unload();
            };

            var serverVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unkown";
            new ColorString(" ", ("darkgreen", "OITool Server"), ("cyan", serverVersion)).Output(true);
            Console.WriteLine();

            await LoadPlugin(Path.Combine(AppContext.BaseDirectory, "plugins"));

            try
            {
                var listener = new Listener(serverVersion);
                await listener.ListenAsync();
            }
            catch (IOException) { Console.WriteLine("Another server is running."); }
        }
    }
}
