namespace OITool.Base.Context.Judge
{
    public class EventerContext : Interface.Worker.Judge.Eventer.IContext
    {
        private List<Interface.Worker.Judge.Eventer.IJudgerEventer> judgerEventers;
        private List<Interface.Worker.Judge.Eventer.IReporterEventer> reporterEventers;
        
        // TODO: Provide a blacklist
        public EventerContext(List<Interface.Worker.Judge.Eventer.IJudgerEventer> judgerEventers, List<Interface.Worker.Judge.Eventer.IReporterEventer> reporterEventers)
        {
            this.judgerEventers = judgerEventers;
            this.reporterEventers = reporterEventers;
        }
        
        public void AddJudgerEventer(OITool.Interface.Worker.Judge.Eventer.IJudgerEventer judgerEventer)
        {
            if (this.judgerEventers.Where(iter => iter.Identifier == judgerEventer.Identifier).Count() > 0)
                throw new ArgumentException("The identifier of judgerEventer already exists.");
            
            judgerEventers.Add(judgerEventer);
        }

        public void AddReporterEventer(OITool.Interface.Worker.Judge.Eventer.IReporterEventer reporterEventer)
        {
            if (this.reporterEventers.Where(iter => iter.Identifier == reporterEventer.Identifier).Count() > 0)
                throw new ArgumentException("The identifier of reporterEventer already exists.");
            
            reporterEventers.Add(reporterEventer);
        }
    }
}