using System.Text;
using System.IO.Pipes;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace OITool.Server
{
    class Program
    {
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

                    Console.WriteLine($"Load Plugin Successfully: {iPlugin.Info.Name}");
                }
            }
        }

        static async Task Main(string[] args)
        {
            var path = AppContext.BaseDirectory;
            await LoadPlugin(Path.Combine(path, "plugins"));

            var judger = new Base.Worker.Judger(
                argument: new()
                {
                    Mode = "common",
                    ProgramFile = Path.Combine(path, "test/test.exe"),
                    
                    // StdInputFilePath = Path.Combine(path, "test/test.in"),
                    // StdOutputFilePath = Path.Combine(path, "test/test.out"),
                    DataFiles = new string[] {
                        "./group/"
                    },
                    
                    Timeout = 1000,

                    ReportFile = "report.html"
                },
                option: new()
                {
                    StdInputFileExtensions = new string[]
                    {
                        "in"
                    },
                    StdOnputFileExtensions = new string[]
                    {
                        "out"
                        //,"ans"
                    }
                },
                judgers: AppInfo.Workers.Judges.Judgers.ToArray(),
                reporters: AppInfo.Workers.Judges.Reporters.ToArray()
            );

            var result = await judger.Judge();
            Console.WriteLine($"Status: {result.Judge[0]}");
            Console.WriteLine($"Time Used: {Math.Round(result.Judge[0].TimeUsed, 1)}ms");
            Console.WriteLine();
            Console.WriteLine($"Report: {result.ReportFiles[0]}");
        }
    }
}
