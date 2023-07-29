namespace OITool.Base.Worker
{
    public class Judge
    {
        #region [Private Field]

        private Interface.Worker.Judge.JudgerArgument argument;
        private Interface.Worker.Judge.JudgerOption option;

        // Plugin
        private WorkerArgument workerArgument;

        #endregion

        #region [Public Struct]

        public struct Result
        {
            public Interface.Worker.Judge.JudgerResult[] Judge { get; init; }

            public string[] ReportFiles { get; init; }
        }

        public struct WorkerArgument
        {
            public Interface.Worker.Judge.IJudger[] Judgers;
            public Interface.Worker.Judge.IReporter[] Reporters;

            public Interface.Worker.Judge.Eventer.IJudgerEventer[] JudgerEventers;
            public Interface.Worker.Judge.Eventer.IReporterEventer[] ReporterEventers;
        }

        #endregion

        #region [Public Method]

        public Judge(Interface.Worker.Judge.JudgerArgument argument, Interface.Worker.Judge.JudgerOption option, WorkerArgument workerArgument)
        {
            this.argument = argument;
            this.option = option;
            this.workerArgument = workerArgument;
        }

        public async Task<Result> ExecuteJudger(string baseDirectory)
        {
            var argumentClone = this.argument;
            var optionClone = this.option;
            var judgeResults = new List<Interface.Worker.Judge.JudgerResult>();

            // Invoke JudgerEventer.BeforeJudge
            foreach (var judgerEventer in this.workerArgument.JudgerEventers)
            {
                var handler = new Interface.ActionHandler() { WorkingDirectory = baseDirectory };
                judgerEventer.BeforeJudge(ref argumentClone, ref optionClone, handler);

                if (handler.PreventOther)
                    break;
            }

            // Judge
            foreach (var judger in this.workerArgument.Judgers)
            {
                var handler = new Interface.ActionHandler() { WorkingDirectory = baseDirectory };
                judgeResults.AddRange(await judger.Judge(argumentClone, optionClone, handler));

                if (handler.PreventOther)
                    break;
            }

            // Invoke JudgerEventer.AfterJudge
            var judgeResultsArray = judgeResults.ToArray();
            foreach (var judgerEventer in this.workerArgument.JudgerEventers)
            {
                var handler = new Interface.ActionHandler() { WorkingDirectory = baseDirectory };
                judgerEventer.AfterJudge(ref argumentClone, ref optionClone, ref judgeResultsArray, handler);

                if (handler.PreventOther)
                    break;
            }

            var reportFile = argumentClone.ReportFile;
            var reportFilesArray = new string[] { };
            if (reportFile != null)
            {
                // Invoke ReporterEventer.BeforeMake
                foreach (var reporterEventer in this.workerArgument.ReporterEventers)
                {
                    var handler = new Interface.ActionHandler() { WorkingDirectory = baseDirectory };
                    reporterEventer.BeforeMake(ref reportFile, ref judgeResultsArray, handler);

                    if (handler.PreventOther)
                        break;
                }

                // Send Result to Interface.Worker.Judge.IReporter
                var reportFiles = new List<string>();
                foreach (var reporter in this.workerArgument.Reporters)
                {
                    var handler = new Interface.ActionHandler() { WorkingDirectory = baseDirectory };
                    var file = await reporter.Make(reportFile, judgeResultsArray, handler);
                    if (file != null)
                        reportFiles.Add(file);

                    if (handler.PreventOther)
                        break;
                }

                // Invoke ReporterEventer.AfterMake
                reportFilesArray = reportFiles.ToArray();
                foreach (var reporterEventer in this.workerArgument.ReporterEventers)
                {
                    var handler = new Interface.ActionHandler() { WorkingDirectory = baseDirectory };
                    reporterEventer.AfterMake(ref reportFilesArray, ref judgeResultsArray, handler);

                    if (handler.PreventOther)
                        break;
                }
            }

            return new()
            {
                Judge = judgeResultsArray,
                ReportFiles = reportFilesArray
            };
        }

        #endregion
    }
}
