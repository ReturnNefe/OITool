using System.IO;
namespace OITool.Plugin.Default.Judge
{
    public class Reporter : Interface.Judge.IReporter
    {
        #region [Private Method]

        private string getColor(Interface.Judge.JudgerStatus status)
        {
            switch (status)
            {
                case Interface.Judge.JudgerStatus.Accepted:
                    return AppInfo.Setting?.TextColor?.Accepted ?? "";

                case Interface.Judge.JudgerStatus.WrongAnswer:
                    return AppInfo.Setting?.TextColor?.WrongAnswer ?? "";

                case Interface.Judge.JudgerStatus.TimeLimitExceed:
                    return AppInfo.Setting?.TextColor?.TimeLimitExceed ?? "";

                case Interface.Judge.JudgerStatus.MemoryLimitExceed:
                    return AppInfo.Setting?.TextColor?.MemoryLimitExceed ?? "";

                case Interface.Judge.JudgerStatus.RuntimeError:
                    return AppInfo.Setting?.TextColor?.RuntimeError ?? "";

                default:
                    return AppInfo.Setting?.TextColor?.WrongAnswer ?? "";
            }
        }

        private string getStatusText(Interface.Judge.JudgerStatus status)
        {
            switch (status)
            {
                case Interface.Judge.JudgerStatus.Accepted:
                    return "AC";

                case Interface.Judge.JudgerStatus.WrongAnswer:
                    return "WA";

                case Interface.Judge.JudgerStatus.TimeLimitExceed:
                    return "TLE";

                case Interface.Judge.JudgerStatus.MemoryLimitExceed:
                    return "MLE";

                case Interface.Judge.JudgerStatus.RuntimeError:
                    return "RE";

                default:
                    return "UR";
            }
        }

        #endregion

        #region [Public Property]

        public string Identifier { get; }

        #endregion

        #region [Public Method]

        public Reporter(string identifier)
        {
            this.Identifier = identifier;
        }

        public async Task<string?> Make(string reportFile, Interface.Judge.JudgerResult[] results, Interface.ActionHandler handler)
        {
            try
            {
                var reportFilePath = handler.ConvertToWorkingDirectory(reportFile);
                var reportDirectory = Path.GetDirectoryName(reportFilePath);
                if (!Directory.Exists(reportDirectory))
                    Directory.CreateDirectory(reportDirectory ?? "");

                using var bodyFrameworkReader = new StreamReader(Path.Combine(AppInfo.BaseDirectory, "asset", "body.framework.html"));
                using var totalFrameworkReader = new StreamReader(Path.Combine(AppInfo.BaseDirectory, "asset", "total.framework.html"));
                using var pointFrameworkReader = new StreamReader(Path.Combine(AppInfo.BaseDirectory, "asset", "point.framework.html"));
                using var writer = new StreamWriter(reportFilePath, false);

                var bodyTemplate = await bodyFrameworkReader.ReadToEndAsync();
                var totalTemplate = await totalFrameworkReader.ReadToEndAsync();
                var pointTemplate = await pointFrameworkReader.ReadToEndAsync();
                var pointsHtml = "";

                if (results.Count() is var count && count == 0)
                {
                    totalTemplate = "";
                    pointsHtml = "<div class=\"p-3 mb-2 bg-warning text-white\">Oops! There is no result here :(</div>";
                }
                else
                {
                    // Calculate Total
                    var passCount = 0;
                    var avgTimeUsed = 0d;
                    var avgMemoryUsed = 0d;
                    foreach (var iter in results)
                    {
                        avgTimeUsed += iter.TimeUsed;
                        avgMemoryUsed += iter.MemoryUsed;
                    }
                    avgTimeUsed /= count;
                    avgMemoryUsed /= count;

                    var index = 1;
                    foreach (var result in results)
                    {
                        if (result.Status == Interface.Judge.JudgerStatus.Accepted)
                            ++passCount;
                        
                        var pointHtml = pointTemplate.Replace("${point-index}", $"{index}")
                                                     .Replace("${result-color}", getColor(result.Status))
                                                     .Replace("${result-text}", getStatusText(result.Status))
                                                     .Replace("${judge-time}", $"{result.Time}")
                                                     .Replace("${data}", $"{result.InputFile}<br/>{result.AnswerFile}")
                                                     .Replace("${time}", $"{Math.Round(result.TimeUsed, 1)} / {result.Timeout} ms")
                                                     .Replace("${memory}", $"{Math.Round(result.MemoryUsed, 1)} / {result.MemoryLimit} MiB");

                        // Program Output
                        var lengthLimit = AppInfo.Setting?.BytesLimit ?? 1024;
                        var prgOut = result.ProgramOutput;
                        var shownPrgOut = "";

                        if (prgOut.Length > lengthLimit)
                            shownPrgOut = prgOut[..lengthLimit] + $"...({prgOut.Length - lengthLimit} byte)";
                        else
                            shownPrgOut = prgOut;

                        pointHtml = pointHtml.Replace("${program-output}", $"{shownPrgOut}");

                        // Standard Input
                        if (File.Exists(result.InputFile))
                        {
                            var buffer = new char[lengthLimit];
                            using var inStream = new StreamReader(result.InputFile);
                            var actualLength = await inStream.ReadAsync(buffer);
                            pointHtml = pointHtml.Replace("${standard-input}", $"{string.Concat(actualLength == lengthLimit ? buffer : buffer.Where((_, index) => index < actualLength))}{(inStream.EndOfStream ? "" : "...")}");
                        }

                        // Standard Output
                        if (File.Exists(result.AnswerFile))
                        {
                            var buffer = new char[lengthLimit];
                            using var ansStream = new StreamReader(result.AnswerFile);
                            var actualLength = await ansStream.ReadAsync(buffer);
                            pointHtml = pointHtml.Replace("${standard-output}", $"{string.Concat(actualLength == lengthLimit ? buffer : buffer.Where((_, index) => index < actualLength))}{(ansStream.EndOfStream ? "" : "...")}");
                        }
                        else pointHtml = pointHtml.Replace("${standard-output}", "");

                        pointsHtml += pointHtml;
                        ++index;
                    }
                    
                    totalTemplate = totalTemplate.Replace("${total-pass}", $"{passCount} / {count}")
                                                 .Replace("${total-time}", $"{Math.Round(avgTimeUsed, 1)} / {results[0].Timeout} ms")
                                                 .Replace("${total-memory}", $"{Math.Round(avgMemoryUsed, 1)} / {results[0].MemoryLimit} MiB");
                }

                await writer.WriteAsync(
                    bodyTemplate.Replace("${total}", totalTemplate)
                                .Replace("${points}", pointsHtml)
                );
                return reportFilePath;
            }
            catch { return null; }
        }

        #endregion
    }
}