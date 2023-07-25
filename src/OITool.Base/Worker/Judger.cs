using System.Diagnostics;

namespace OITool.Base.Worker
{
    public class Judger
    {
        #region [Private Property]

        private Interface.Judge.Argument argument { get; }
        private Interface.Judge.Option option { get; }

        // Plugin
        private Interface.Judge.IJudger[] judgers { get; }
        private Interface.Judge.IReporter[] reporters { get; }

        #endregion

        #region [Public Struct]

        public struct Result
        {
            public Interface.Judge.Result[] Judge { get; init; }

            public string[] ReportFiles { get; init; }
        }

        #endregion

        #region [Public Method]

        public Judger(Interface.Judge.Argument argument, Interface.Judge.Option option, Interface.Judge.IJudger[] judgers, Interface.Judge.IReporter[] reporters)
        {
            this.argument = argument;
            this.option = option;
            this.judgers = judgers;
            this.reporters = reporters;
        }

        public async Task<Result> Judge()
        {
            var judgeResults = new List<Interface.Judge.Result>();

            // Invoke Interface.Judge.IJudgers
            foreach (var judger in this.judgers)
            {
                var handler = new Interface.ActionHandler() { WorkingDirectory = System.Environment.CurrentDirectory };
                judgeResults.AddRange(await judger.Judge(this.argument, this.option, handler));

                if (handler.PreventOther)
                    break;
            }

            // Send Result to Interface.Judge.IReportervar reportFile = "";
            var reportFiles = new List<string>();
            if (this.argument.ReportFile != null)
            {
                foreach (var reporter in this.reporters)
                {
                    var handler = new Interface.ActionHandler() { WorkingDirectory = System.Environment.CurrentDirectory };
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
