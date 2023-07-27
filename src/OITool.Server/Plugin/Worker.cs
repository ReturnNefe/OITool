namespace OITool.Server.Plugin
{
    internal struct Worker
    {
        internal JudgeWorker Judges = new();
        
        public Worker() { }
    }
}