namespace OITool.Interface
{
    public struct PluginInfo
    {
        public string Name { get; init; } = "";
        public string Author { get; init; } = "";
        public string Description { get; init; } = "";
        public Version Version { get; init; } = new("1.0.0");

        public PluginInfo() { }
        public PluginInfo(string name, string author, string description, Version version)
        {
            Name = name;
            Author = author;
            Description = description;
            Version = version;
        }
    }
}