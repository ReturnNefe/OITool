namespace OITool.Interface.Judge
{
    public interface IContext
    {
        public void AddJudger(IJudger judger);
        public void AddReporter(IReporter reporter);
    }
}