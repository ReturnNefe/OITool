namespace OITool.Interface.Worker.Judge
{
    /// <summary>
    /// The option of <see cref="IJudger" />.
    /// </summary>
    public struct JudgerOption
    {
        /// <summary>
        /// The collection of input data-file extensions that IJudger needs to indentify.
        /// </summary>
        public string[] StdInputFileExtensions { get; set; } = Array.Empty<string>();
        
        /// <summary>
        /// The collection of answer data-file extensions that IJudger needs to indentify.
        /// </summary>
        public string[] StdOnputFileExtensions { get; set; } = Array.Empty<string>();
        
        /// <summary />
        public JudgerOption() { }
    }
}