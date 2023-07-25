using System.Diagnostics;

namespace OITool.Plugin.Default.Judge
{
    public class Judger : Interface.Judge.IJudger
    {
        #region [Private Method]

        private async Task<Interface.Judge.Result> judge(string program, string inputFile, string outputFile, int timeout)
        {
            var status = Interface.Judge.Status.Accepted;

            void changeStatus(Interface.Judge.Status newStatus)
            {
                if (status != Interface.Judge.Status.TimeLimitExceed)
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

            var startTime = DateTime.Now;

            // Redirect Input
            using (var inputFileStream = new StreamReader(inputFile))
                while (!inputFileStream.EndOfStream)
                    await process.StandardInput.WriteLineAsync(await inputFileStream.ReadLineAsync());

            // Kill the process if the time limit is exceeded.
            if (!process.WaitForExit(timeout))
            {
                changeStatus(Interface.Judge.Status.TimeLimitExceed);
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
                        changeStatus(Interface.Judge.Status.WrongAnswer);
                    else
                    {
                        var stdOut = await outputFileStream.ReadLineAsync() ?? "";
                        if (prgLine.TrimEnd() != stdOut.TrimEnd())
                            changeStatus(Interface.Judge.Status.WrongAnswer);
                    }
                }

                // Check StdOutputFile
                while (!outputFileStream.EndOfStream)
                {
                    if (!string.IsNullOrWhiteSpace(await outputFileStream.ReadLineAsync()))
                        changeStatus(Interface.Judge.Status.WrongAnswer);
                }
            }

            if (process.ExitCode != 0)
                changeStatus(Interface.Judge.Status.RuntimeError);

            var result = new Interface.Judge.Result()
            {
                Time = startTime,
                InputFile = inputFile,
                AnswerFile = outputFile,
                ProgramOutput = prgOut,
                TimeUsed = (process.ExitTime - startTime).TotalMilliseconds,
                Timeout = timeout,
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

        public async Task<Interface.Judge.Result[]> Judge(Interface.Judge.Argument argument, Interface.Judge.Option option, Interface.ActionHandler handler)
        {
            // Check Judge Mode
            if (argument.Mode != "common")
                return Array.Empty<Interface.Judge.Result>();
            
            var results = new List<Interface.Judge.Result>();

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
                                results.Add(await judge(argument.ProgramFile, stdInputFile, stdOutputFile, argument.Timeout));
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
                            results.Add(await this.judge(argument.ProgramFile, stdInputFile, stdOutputFile, argument.Timeout));

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
                        results.Add(await this.judge(argument.ProgramFile, stdInputFile, stdOutputFile, argument.Timeout));
                    }
                    else throw new FileNotFoundException($"StdData not found: {fullPath}");
                }
            }

            return results.ToArray();
        }

        #endregion
    }
}