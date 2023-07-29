namespace OITool.Server.Plugin.Judge
{
    internal class EventerWorker
    {
        #region [Private Field]
        
        private string[]? judgerWhiteList;
        private string[]? reporterWhiteList;
        
        #endregion

        #region [Public Field]
        
        public List<Interface.Worker.Judge.Eventer.IJudgerEventer> JudgerEventers = new();
        public List<Interface.Worker.Judge.Eventer.IReporterEventer> ReporterEventers = new();

        #endregion
        
        #region [Public Method]

        public EventerWorker(string[]? judgerWhiteList, string[]? reporterWhiteList)
        {
            this.judgerWhiteList = judgerWhiteList;
            this.reporterWhiteList = reporterWhiteList;
        }

        public IEnumerable<Interface.Worker.Judge.Eventer.IJudgerEventer> GetActiveJudgerEventers()
        {
            foreach (var item in this.JudgerEventers)
            {
                if (this.judgerWhiteList == null || this.judgerWhiteList.Contains(item.Identifier))
                    yield return item;
            }
        }
        
        public IEnumerable<Interface.Worker.Judge.Eventer.IReporterEventer> GetActiveReporterEventers()
        {
            foreach (var item in this.ReporterEventers)
            {
                if (this.reporterWhiteList == null || this.reporterWhiteList.Contains(item.Identifier))
                    yield return item;
            }
        }
        
        #endregion
    }
}