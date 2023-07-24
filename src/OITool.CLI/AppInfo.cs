using System.Collections.Generic;
namespace OITool.CLI
{
    internal class AppInfo
    {
        static internal List<Nefe.PluginCore.Plugin> Plugins = new();
        static internal Dictionary<string, object> PluginDictionary = new();
    }
}