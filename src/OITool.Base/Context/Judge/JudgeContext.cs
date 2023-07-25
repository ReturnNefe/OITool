using OITool.Interface.Judge;

namespace OITool.Base.Context.Judge
{
    public class JudgeContext : IContext
    {
        private List<IJudger> judgers;
        private List<IReporter> reporters;
        
        public JudgeContext(List<IJudger> judgers, List<IReporter> reporters)
        {
            this.judgers = judgers;
            this.reporters = reporters;
        }
        
        public void AddJudger(IJudger judger)
        {
            if (this.judgers.Where(iter => iter.Identifier == judger.Identifier).Count() > 0)
                throw new ArgumentException("The identifier of judger already exists.");
            
            judgers.Add(judger);
        }

        public void AddReporter(IReporter reporter)
        {
            if (this.reporters.Where(iter => iter.Identifier == reporter.Identifier).Count() > 0)
                throw new ArgumentException("The identifier of reporter already exists.");
            
            reporters.Add(reporter);
        }
    }
}