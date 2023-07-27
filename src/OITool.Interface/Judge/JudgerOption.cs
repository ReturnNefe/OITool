namespace OITool.Interface.Judge
{
    /// <summary>
    /// The option of <see cref="IJudger" />.
    /// </summary>
    public struct JudgerOption
    {
        /// <summary>
        /// The collection of input data-file extensions that IJudger needs to indentify.
        /// </summary>
        public string[] StdInputFileExtensions { get; init; } = Array.Empty<string>();
        
        /// <summary>
        /// The collection of answer data-file extensions that IJudger needs to indentify.
        /// </summary>
        public string[] StdOnputFileExtensions { get; init; } = Array.Empty<string>();
        
        /// <summary />
        public JudgerOption() { }
    }
}