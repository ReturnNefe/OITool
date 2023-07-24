using System;
using System.Diagnostics;
using OITool.Interface.Judger;

namespace OITool.Base.Worker
{
    public class Judger
    {
        #region [Private Properties]

        private Argument.JudgeArugment argument { get; }
        private Option.JudgeOption option { get; }

        // Plugin
        private IReporter? reporter { get; }

        #endregion

        #region [Private Method]

        private async Task<JudgeResult> judge(string program, string inputFile, string outputFile, int timeout)
        {
            var status = JudgeResult.JudgeStatus.Accepted;

            void changeStatus(JudgeResult.JudgeStatus newStatus)
            {
                if (status != JudgeResult.JudgeStatus.TimeLimitExceed)
                    status = newStatus;
            }

            // Check
            if (!File.Exists(inputFile))
                throw new FileNotFoundException($"stdInputFile not found. ({inputFile})");
            if (!File.Exists(outputFile))
                throw new FileNotFoundException($"stdOnputFile not found. ({outputFile})");

            // Start Program
            var process = Process.Start(new ProcessStartInfo
            {
                Arguments = "",
                FileName = program,
                WorkingDirectory = Path.GetDirectoryName(program),
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            });

            if (process == null)
                throw new ApplicationException($"Faild to start program ({program})");

            var startTime = DateTime.Now;

            // Redirect Input
            using (var inputFileStream = new StreamReader(inputFile))
                while (!inputFileStream.EndOfStream)
                    await process.StandardInput.WriteLineAsync(await inputFileStream.ReadLineAsync());

            // Kill the process if the time limit is exceeded.
            if (!process.WaitForExit(timeout))
            {
                changeStatus(JudgeResult.JudgeStatus.TimeLimitExceed);
                process.Kill();
            }

            // Redirect Output
            var prgOut = "";
            using (var outputFileStream = new StreamReader(outputFile))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var prgLine = await process.StandardOutput.ReadLineAsync() ?? "";
                    prgOut += prgLine + Environment.NewLine;

                    if (outputFileStream.EndOfStream && !string.IsNullOrWhiteSpace(prgLine))
                        changeStatus(JudgeResult.JudgeStatus.WrongAnswer);
                    else
                    {
                        var stdOut = await outputFileStream.ReadLineAsync() ?? "";
                        if (prgLine.TrimEnd() != stdOut.TrimEnd())
                            changeStatus(JudgeResult.JudgeStatus.WrongAnswer);
                    }
                }

                // Check StdOutputFile
                while (!outputFileStream.EndOfStream)
                {
                    if (!string.IsNullOrWhiteSpace(await outputFileStream.ReadLineAsync()))
                        changeStatus(JudgeResult.JudgeStatus.WrongAnswer);
                }
            }

            if (process.ExitCode != 0)
                changeStatus(JudgeResult.JudgeStatus.RuntimeError);

            var result = new JudgeResult()
            {
                Time = startTime,
                ProgramFile = program,
                ProgramOutput = prgOut,
                DataFile = inputFile,
                AnswerFile = outputFile,
                TimeUsed = (process.ExitTime - startTime).TotalMilliseconds,
                Timeout = timeout,
                Status = status,
            };

            // Close Process
            process.Close();
            return result;
        }

        #endregion

        #region [Public Struct]

        public struct Result
        {
            public JudgeResult[] Judge { get; init; }

            public string ReportFilePath { get; init; }
            public bool ReportGenerated { get; init; }
        }

        #endregion

        #region [Public Method]

        public Judger(Argument.JudgeArugment arugment, Option.JudgeOption option, IReporter? reporter = null)
        {
            this.argument = arugment;
            this.option = option;
            if (reporter != null)
                this.reporter = reporter;
        }

        public async Task<Result> Judge()
        {
            switch (this.argument.JudgeMode)
            {
                case Argument.JudgeArugment.JudgeModes.SingleFile:
                    {
                        var result = await judge(Path.GetFullPath(this.argument.ProgramPath),
                                                 Path.GetFullPath(this.argument.StdInputFilePath),
                                                 Path.GetFullPath(this.argument.StdOutputFilePath),
                                                 this.argument.Timeout);

                        // Make Report
                        var reportGenerated = false;
                        var reportFile = "";
                        var reportDirectory = "";
                        if (this.argument.ReportFilePath != null)
                        {
                            reportFile = Path.IsPathFullyQualified(this.argument.ReportFilePath) ? this.argument.ReportFilePath : Path.Combine(Path.GetDirectoryName(this.argument.ProgramPath) ?? "", this.argument.ReportFilePath);
                            reportDirectory = Path.GetDirectoryName(reportFile);

                            if (!Directory.Exists(reportDirectory))
                                Directory.CreateDirectory(reportDirectory ?? "");

                            if (reporter == null)
                                throw new Exception("No available reporter.");

                            reportGenerated = await reporter.Make(reportFile, new JudgeResult[] { result });
                        }

                        return new()
                        {
                            Judge = new JudgeResult[] { result },
                            ReportGenerated = reportGenerated,
                            ReportFilePath = reportFile
                        };
                    }

                case Argument.JudgeArugment.JudgeModes.Folder:
                    {
                        var results = new List<JudgeResult>();
                        var files = Directory.GetFiles(this.argument.StdDataFolderPath).Where(file => Path.GetExtension(file) == ".in");

                        foreach (var iter in files)
                        {
                            var inputFile = iter;
                            var outputFile = Path.ChangeExtension(iter, "out");

                            results.Add(await judge(Path.GetFullPath(this.argument.ProgramPath),
                                                 inputFile, outputFile, this.argument.Timeout));
                        }

                        // Make Report
                        var reportGenerated = false;
                        var reportFile = "";
                        var reportDirectory = "";
                        if (this.argument.ReportFilePath != null)
                        {
                            reportFile = Path.IsPathFullyQualified(this.argument.ReportFilePath) ? this.argument.ReportFilePath : Path.Combine(Path.GetDirectoryName(this.argument.StdDataFolderPath) ?? "", this.argument.ReportFilePath);
                            reportDirectory = Path.GetDirectoryName(reportFile);

                            if (!Directory.Exists(reportDirectory))
                                Directory.CreateDirectory(reportDirectory ?? "");

                            if (reporter == null)
                                throw new Exception("No available reporter.");

                            reportGenerated = await reporter.Make(reportFile, results.ToArray());
                        }

                        return new()
                        {
                            Judge = results.ToArray(),
                            ReportGenerated = reportGenerated,
                            ReportFilePath = reportFile
                        };
                    }

                default:
                    throw new ArgumentException($"Unknown JudgeMode: {this.argument.JudgeMode}");
            }
        }

        #endregion
    }
}
