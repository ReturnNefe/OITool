using System.IO;
namespace OITool.Plugin.Default.Judge
{
    public class Reporter : Interface.Judge.IReporter
    {
        #region [Private Method]

        private string getColor(Interface.Judge.Status status)
        {
            switch (status)
            {
                case Interface.Judge.Status.Accepted:
                    return AppInfo.Setting?.TextColor?.Accepted ?? "";

                case Interface.Judge.Status.WrongAnswer:
                    return AppInfo.Setting?.TextColor?.WrongAnswer ?? "";

                case Interface.Judge.Status.TimeLimitExceed:
                    return AppInfo.Setting?.TextColor?.TimeLimitExceed ?? "";

                case Interface.Judge.Status.MemoryLimitExceed:
                    return AppInfo.Setting?.TextColor?.MemoryLimitExceed ?? "";

                case Interface.Judge.Status.RuntimeError:
                    return AppInfo.Setting?.TextColor?.RuntimeError ?? "";

                default:
                    return AppInfo.Setting?.TextColor?.WrongAnswer ?? "";
            }
        }

        private string getStatusText(Interface.Judge.Status status)
        {
            switch (status)
            {
                case Interface.Judge.Status.Accepted:
                    return "AC";

                case Interface.Judge.Status.WrongAnswer:
                    return "WA";

                case Interface.Judge.Status.TimeLimitExceed:
                    return "TLE";

                case Interface.Judge.Status.MemoryLimitExceed:
                    return "MLE";

                case Interface.Judge.Status.RuntimeError:
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

        public async Task<string?> Make(string reportFile, Interface.Judge.Result[] results, Interface.ActionHandler handler)
        {
            try
            {
                var reportFilePath = handler.ConvertToWorkingDirectory(reportFile);
                var reportDirectory = Path.GetDirectoryName(reportFilePath);
                if (!Directory.Exists(reportDirectory))
                    Directory.CreateDirectory(reportDirectory ?? "");

                using var bodyFrameworkReader = new StreamReader(Path.Combine(AppInfo.BaseDirectory, "body.framework.html"));
                using var pointFrameworkReader = new StreamReader(Path.Combine(AppInfo.BaseDirectory, "point.framework.html"));
                using var writer = new StreamWriter(reportFilePath, false);

                var bodyTemplate = await bodyFrameworkReader.ReadToEndAsync();
                var pointTemplate = await pointFrameworkReader.ReadToEndAsync();

                var pointsHtml = "";


                if (results.Count() == 0)
                {
                    pointsHtml = "<div class=\"p-3 mb-2 bg-warning text-white\">Oops! There is no result here :(</div>";
                }
                else
                {
                    var index = 1;
                    foreach (var result in results)
                    {
                        var pointHtml = pointTemplate.Replace("${point-index}", $"{index}")
                                                     .Replace("${result-color}", getColor(result.Status))
                                                     .Replace("${result-text}", getStatusText(result.Status))
                                                     .Replace("${judge-time}", $"{result.Time}")
                                                     .Replace("${data}", $"{result.InputFile}<br/>{result.AnswerFile}")
                                                     .Replace("${time}", $"{Math.Round(result.TimeUsed, 1)}ms ({result.Timeout})");

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
                }

                await writer.WriteAsync(bodyTemplate.Replace("${points}", pointsHtml));
                return reportFilePath;
            }
            catch { return null; }
        }

        #endregion
    }
}