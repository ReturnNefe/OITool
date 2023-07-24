using System.Collections.Generic;
using OITool.Interface.Judger;

namespace OITool.CLI
{
    internal class PluginContext : OITool.Interface.PluginContext
    {
        private Dictionary<string, object> dictionary = null!;
        
        public PluginContext(Dictionary<string, object> dictionary)
        {
            this.dictionary = dictionary;
        }
        
        public void AddReporter(string key, IReporter reporter)
        {
            dictionary.Add(key, reporter);
        }
    }
}