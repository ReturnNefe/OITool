using System.Diagnostics;

namespace OITool.Plugin.Default.Judge
{
    public class Judger : Interface.Judge.IJudger
    {
        #region [Private Method]

        private async Task<Interface.Judge.JudgerResult> judge(string program, string inputFile, string outputFile, int timeout, double memoryLimit)
        {
            var status = Interface.Judge.JudgerStatus.Accepted;

            void changeStatus(Interface.Judge.JudgerStatus newStatus)
            {
                if (status != Interface.Judge.JudgerStatus.TimeLimitExceed)
                    status = newStatus;
            }

            // Check
            if (!File.Exists(inputFile))
                throw new FileNotFoundException($"stdInputFile not found. ({inputFile})");
            if (!File.Exists(outputFile))
                throw new FileNotFoundException($"stdOnputFile not found. ({outputFile})");

            // Start Program
            using var process = Process.Start(new ProcessStartInfo
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

            // Monitor Memory Usage
            var memoryMonitorInterval = AppInfo.Setting?.MemoryDetectInterval ?? -1;
            var memoryUsed = 0d;
            if (memoryMonitorInterval > 0)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        while (true)
                        {
                            if (process.HasExited) break;
                            process.Refresh();
                            memoryUsed = Math.Max(memoryUsed, process.PrivateMemorySize64 / 1024d / 1024d);
                            await Task.Delay(memoryMonitorInterval);
                        }
                    }
                    catch { }
                });

            var startTime = DateTime.Now;

            // Redirect Input
            using (var inputFileStream = new StreamReader(inputFile))
                while (!inputFileStream.EndOfStream)
                    await process.StandardInput.WriteLineAsync(await inputFileStream.ReadLineAsync());

            // Kill the process if the time limit is exceeded.
            if (!process.WaitForExit(timeout))
            {
                changeStatus(Interface.Judge.JudgerStatus.TimeLimitExceed);
                process.Kill();
            }

            // Check the time
            var endTime = DateTime.Now;
            var totalTime = (endTime - startTime).TotalMilliseconds;
            if (totalTime > (double)timeout)
                changeStatus(Interface.Judge.JudgerStatus.TimeLimitExceed);

            if (memoryUsed > memoryLimit)
                changeStatus(Interface.Judge.JudgerStatus.MemoryLimitExceed);

            // Redirect Output
            var prgOut = "";
            using (var outputFileStream = new StreamReader(outputFile))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var prgLine = await process.StandardOutput.ReadLineAsync() ?? "";
                    prgOut += prgLine + Environment.NewLine;

                    if (outputFileStream.EndOfStream && !string.IsNullOrWhiteSpace(prgLine))
                        changeStatus(Interface.Judge.JudgerStatus.WrongAnswer);
                    else
                    {
                        var stdOut = await outputFileStream.ReadLineAsync() ?? "";
                        if (prgLine.TrimEnd() != stdOut.TrimEnd())
                            changeStatus(Interface.Judge.JudgerStatus.WrongAnswer);
                    }
                }

                // Check StdOutputFile
                while (!outputFileStream.EndOfStream)
                {
                    if (!string.IsNullOrWhiteSpace(await outputFileStream.ReadLineAsync()))
                        changeStatus(Interface.Judge.JudgerStatus.WrongAnswer);
                }
            }

            if (process.ExitCode != 0)
                changeStatus(Interface.Judge.JudgerStatus.RuntimeError);

            var result = new Interface.Judge.JudgerResult()
            {
                Time = startTime,
                InputFile = inputFile,
                AnswerFile = outputFile,
                ProgramOutput = prgOut,
                TimeUsed = totalTime,
                Timeout = timeout,
                MemoryUsed = memoryUsed,
                MemoryLimit = memoryLimit,
                Status = status,
            };

            // Close Process
            // process.Close();
            return result;
        }

        #endregion

        #region [Public Property]

        public string Identifier { get; }

        #endregion

        #region [Public Method]

        public Judger(string identifier)
        {
            this.Identifier = identifier;
        }

        public async Task<Interface.Judge.JudgerResult[]> Judge(Interface.Judge.JudgerArgument argument, Interface.Judge.JudgerOption option, Interface.ActionHandler handler)
        {
            // Check Judge Mode
            if (argument.Mode != "common")
                return Array.Empty<Interface.Judge.JudgerResult>();

            var programFile = handler.ConvertToWorkingDirectory(argument.ProgramFile);
            var results = new List<Interface.Judge.JudgerResult>();

            foreach (var path in argument.DataFiles)
            {
                var fullPath = handler.ConvertToWorkingDirectory(path);

                // Judge datas in directory
                // path e.g. /test/
                if (Directory.Exists(fullPath))
                {
                    // Get Data
                    var files = Directory.GetFiles(fullPath).Where(file => option.StdInputFileExtensions.Contains(Path.GetExtension(file)[1..]));

                    foreach (var iter in files)
                    {
                        var stdInputFile = iter;
                        var stdOutputFile = "";

                        // Scan OutputData
                        foreach (var outExtension in option.StdOnputFileExtensions)
                            if (File.Exists(stdOutputFile = Path.ChangeExtension(iter, outExtension)))
                                results.Add(await judge(programFile, stdInputFile, stdOutputFile, argument.Timeout, argument.MemoryLimit));
                    }
                }

                // Judge single file
                // Identify InputData file
                // path e.g. /test/point1.in
                else if (File.Exists(fullPath) && option.StdInputFileExtensions.Contains(Path.GetExtension(fullPath)[1..]))
                {
                    var stdInputFile = fullPath;
                    var stdOutputFile = "";

                    // Scan OutputData
                    foreach (var outExtension in option.StdOnputFileExtensions)
                        if (File.Exists(stdOutputFile = Path.ChangeExtension(fullPath, outExtension)))
                            results.Add(await this.judge(programFile, stdInputFile, stdOutputFile, argument.Timeout, argument.MemoryLimit));

                    if (!File.Exists(stdOutputFile))
                        throw new FileNotFoundException($"StdOutputData not found: {fullPath}");
                }

                // Judge single file without extension
                // path e.g. /test/point1
                else
                {
                    var stdInputFile = "";
                    var stdOutputFile = "";

                    foreach (var inExtension in option.StdInputFileExtensions)
                        if (File.Exists(stdInputFile = fullPath + "." + inExtension))
                            foreach (var outExtension in option.StdOnputFileExtensions)
                                if (File.Exists(stdOutputFile = fullPath + "." + outExtension))
                                    break;

                    if (File.Exists(stdInputFile) && File.Exists(stdOutputFile))
                    {
                        results.Add(await this.judge(programFile, stdInputFile, stdOutputFile, argument.Timeout, argument.MemoryLimit));
                    }
                }
            }

            return results.ToArray();
        }

        #endregion
    }
}