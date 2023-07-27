using System.IO;
using System.Text;
using System.IO.Pipes;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Reflection;
using Nefe.ColorConsole;

namespace OITool.Server
{
    class Program
    {
        static async Task loadPlugin(string directory)
        {
            foreach (var dirPath in Directory.GetDirectories(directory))
            {
                var pluginName = Path.GetFileName(dirPath);
                var pluginFile = Path.Combine(dirPath, $"{pluginName}.dll");

                if (File.Exists(pluginFile))
                {
                    var pluginContext = new Base.Context.PluginContext(
                        judge: new Base.Context.Judge.JudgeContext(
                            judgers: AppInfo.Workers.Judges.Judgers,
                            reporters: AppInfo.Workers.Judges.Reporters
                        )
                    );

                    var plugin = new Nefe.PluginCore.Plugin(pluginFile, isCollectible: true);
                    plugin.LoadFromFile();
                    AppInfo.Plugins.Add(plugin);

                    var iPlugin = plugin.CreateInstances<Interface.IPlugin>().First();
                    await iPlugin.OnLoading(dirPath);
                    iPlugin.Initialize(pluginContext);

                    // Notice plugin when plugin will be unloaded.
                    plugin.Unloading += (e) => iPlugin.OnUnloading();
                }
            }
        }

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += (_, _) =>
            {
                foreach (var plugin in AppInfo.Plugins)
                    plugin.Unload();
            };

            var serverVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unkown";
            new ColorString(" ", ("darkgreen", "OITool Server"), ("cyan", serverVersion)).Output(true);
            Console.WriteLine();

            await loadPlugin(Path.Combine(AppContext.BaseDirectory, "plugins"));

            try
            {
                var listener = new Listener(serverVersion);
                await listener.ListenAsync();
            }
            catch (IOException) { Console.WriteLine("Another server is running."); }
        }
    }
}
