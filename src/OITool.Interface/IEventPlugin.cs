namespace OITool.Interface
{
    /// <summary />
    public interface IEventPlugin
    {
        /// <summary>
        /// Occurs when the plugin is being loading.
        /// </summary>
        /// <param name="baseDirectory">The base working directory of the plugin.</param>
        public Task OnLoading(string baseDirectory);
        
        /// <summary>
        /// Occurs when the plugin is being unloading.
        /// </summary>
        public Task OnUnloading();
    }
}