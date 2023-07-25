namespace OITool.Server.Worker
{
    internal struct Workers
    {
        internal JudgeWorkers Judges = new();
        
        public Workers() { }
    }
}