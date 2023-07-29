namespace OITool.Base.Context.Judge
{
    public class JudgeContext : Interface.Worker.Judge.IContext
    {
        private List<Interface.Worker.Judge.IJudger> judgers;
        private List<Interface.Worker.Judge.IReporter> reporters;
        private Interface.Worker.Judge.Eventer.IContext eventer;
        
        public JudgeContext(Interface.Worker.Judge.Eventer.IContext eventer, List<Interface.Worker.Judge.IJudger> judgers, List<Interface.Worker.Judge.IReporter> reporters)
        {
            this.eventer = eventer;
            this.judgers = judgers;
            this.reporters = reporters;
        }

        // TODO: Provide a blacklist
        public Interface.Worker.Judge.Eventer.IContext Eventer => this.eventer;

        public void AddJudger(Interface.Worker.Judge.IJudger judger)
        {
            if (this.judgers.Where(iter => iter.Identifier == judger.Identifier).Count() > 0)
                throw new ArgumentException("The identifier of judger already exists.");
            
            judgers.Add(judger);
        }

        public void AddReporter(Interface.Worker.Judge.IReporter reporter)
        {
            if (this.reporters.Where(iter => iter.Identifier == reporter.Identifier).Count() > 0)
                throw new ArgumentException("The identifier of reporter already exists.");
            
            reporters.Add(reporter);
        }
    }
}