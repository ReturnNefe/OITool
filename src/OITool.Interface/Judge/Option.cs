namespace OITool.Interface.Judge
{
    public struct Option
    {
        public string[] StdInputFileExtensions { get; init; } = Array.Empty<string>();
        public string[] StdOnputFileExtensions { get; init; } = Array.Empty<string>();
        
        public Option() { }
    }
}