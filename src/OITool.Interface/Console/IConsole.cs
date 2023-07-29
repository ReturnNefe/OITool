namespace OITool.Interface.Console
{
    /// <summary />
    public interface IConsole
    {
        /// <summary>
        /// The console of CLI.
        /// </summary>
        public IClientConsole Client { get; }
    }
}