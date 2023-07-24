namespace OITool.CLI
{
    internal class Program
    {
        static async Task LoadPlugin(string directory)
        {
            foreach (var dirPath in Directory.GetDirectories(directory))
            {
                var pluginName = Path.GetFileName(dirPath);
                var pluginFile = Path.Combine(dirPath, $"{pluginName}.dll");

                if (File.Exists(pluginFile))
                {
                    var pluginContext = new PluginContext(AppInfo.PluginDictionary);
                    var plugin = new Nefe.PluginCore.Plugin(pluginFile, isCollectible: true);
                    plugin.LoadFromFile();
                    AppInfo.Plugins.Add(plugin);
                    
                    var iPlugin = plugin.CreateInstances<OITool.Interface.IPlugin>().First();
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
            
            var judger = new OITool.Base.Worker.Judger(
                arugment: new()
                {
                    JudgeMode = Base.Argument.JudgeArugment.JudgeModes.Folder,
                    
                    ProgramPath = Path.Combine(path, "test/test.exe"),
                    // StdInputFilePath = Path.Combine(path, "test/test.in"),
                    // StdOutputFilePath = Path.Combine(path, "test/test.out"),
                    StdDataFolderPath = Path.Combine(path, "test/group/"),
                    
                    ReportFilePath = "report.html"
                },
                option: new(),
                reporter: AppInfo.PluginDictionary["default.reporter"] as OITool.Interface.Judger.IReporter
            );
            
            var result = await judger.Judge();
            Console.WriteLine($"Status: {result.Judge[0].Status}");
            Console.WriteLine($"Time Used: {Math.Round(result.Judge[0].TimeUsed, 1)}ms");
            Console.WriteLine();
            Console.WriteLine($"Report: {result.ReportFilePath}");
        }
    }
}
