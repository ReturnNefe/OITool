namespace OITool.Interface.Console
{
    /// <summary>
    /// The interface of the console of OITool.CLI which you can output text to.
    /// </summary>
    public interface IClientConsole
    {
        /// <summary>
        /// Output normal text in CLI, and switch to next line.
        /// </summary>
        /// <param name="text">The text that you want to output.</param>
        public void OutputLine(string text);
        
        /// <summary>
        /// Output colored text in CLI, and switch to next line.
        /// </summary>
        /// <param name="coloredText">The colored text.</param>
        public void OutputLine(params (string color, string text)[] coloredText);
        
        /// <summary>
        /// Output colored text in CLI, and switch to next line.
        /// </summary>
        /// <param name="separator">The sparator of each text span.</param>
        /// <param name="coloredText">The colored text.</param>
        public void OutputLine(string separator, params (string color, string text)[] coloredText);
    }
}