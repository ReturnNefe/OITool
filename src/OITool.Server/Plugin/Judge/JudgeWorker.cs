namespace OITool.Server.Plugin.Judge
{
    internal class Worker
    {
        public EventerWorker Eventer;
        public List<Interface.Worker.Judge.IJudger> Judgers = new();
        public List<Interface.Worker.Judge.IReporter> Reporters = new();

        public Worker(EventerWorker eventer)
        {
            this.Eventer = eventer;
        }
    }
}