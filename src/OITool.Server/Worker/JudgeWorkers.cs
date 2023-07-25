namespace OITool.Server.Worker
{
    internal struct JudgeWorkers
    {
        public List<Interface.Judge.IJudger> Judgers = new();
        public List<Interface.Judge.IReporter> Reporters = new();

        public JudgeWorkers() { }
    }
}