namespace OITool.Server.Plugin
{
    internal struct JudgeWorker
    {
        public List<Interface.Judge.IJudger> Judgers = new();
        public List<Interface.Judge.IReporter> Reporters = new();

        public JudgeWorker() { }
    }
}