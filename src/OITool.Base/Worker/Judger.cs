using System.Diagnostics;

namespace OITool.Base.Worker
{
    public class Judger
    {
        #region [Private Field]

        private Interface.Judge.JudgerArgument argument;
        private Interface.Judge.JudgerOption option;

        // Plugin
        private Interface.Judge.IJudger[] judgers;
        private Interface.Judge.IReporter[] reporters;

        #endregion

        #region [Public Struct]

        public struct Result
        {
            public Interface.Judge.JudgerResult[] Judge { get; init; }

            public string[] ReportFiles { get; init; }
        }

        #endregion

        #region [Public Method]

        public Judger(Interface.Judge.JudgerArgument argument, Interface.Judge.JudgerOption option, Interface.Judge.IJudger[] judgers, Interface.Judge.IReporter[] reporters)
        {
            this.argument = argument;
            this.option = option;
            this.judgers = judgers;
            this.reporters = reporters;
        }

        public async Task<Result> Judge(string baseDirectory)
        {
            var judgeResults = new List<Interface.Judge.JudgerResult>();

            // Invoke Interface.Judge.IJudgers
            foreach (var judger in this.judgers)
            {
                var handler = new Interface.ActionHandler() { WorkingDirectory = baseDirectory };
                judgeResults.AddRange(await judger.Judge(this.argument, this.option, handler));

                if (handler.PreventOther)
                    break;
            }

            // Send Result to Interface.Judge.IReporter
            var reportFiles = new List<string>();
            if (this.argument.ReportFile != null)
            {
                foreach (var reporter in this.reporters)
                {
                    var handler = new Interface.ActionHandler() { WorkingDirectory = baseDirectory };
                    var file = await reporter.Make(this.argument.ReportFile, judgeResults.ToArray(), handler);
                    if (file != null)
                        reportFiles.Add(file);

                    if (handler.PreventOther)
                        break;
                }
            }
            
            return new()
            {
                Judge = judgeResults.ToArray(),
                ReportFiles = reportFiles.ToArray()
            };
        }

        #endregion
    }
}
