namespace OITool.Interface
{
    /// <summary>
    /// The information of plugins.
    /// </summary>
    public struct PluginInfo
    {
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        public string Name { get; init; } = "";
        
        /// <summary>
        /// The author of the plugin.
        /// </summary>
        public string Author { get; init; } = "";
        
        /// <summary>
        /// The description of the plugin.
        /// </summary>
        public string Description { get; init; } = "";
        
        /// <summary>
        /// The version of the plugin.
        /// </summary>
        public Version Version { get; init; } = new("1.0.0");

        /// <summary/>
        public PluginInfo() { }
        
        /// <summary/>
        public PluginInfo(string name, string author, string description, Version version)
        {
            Name = name;
            Author = author;
            Description = description;
            Version = version;
        }
    }
}