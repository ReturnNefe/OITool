using System.Runtime.InteropServices;
using System.Linq;
using System.IO;
namespace OITool.Plugin.Default
{
    public class Reporter : OITool.Interface.Judger.IReporter
    {
        private string getColor(OITool.Interface.Judger.JudgeResult.JudgeStatus status)
        {
            switch (status)
            {
                case Interface.Judger.JudgeResult.JudgeStatus.Accepted:
                    return AppInfo.Setting?.TextColor?.Accepted ?? "";

                case Interface.Judger.JudgeResult.JudgeStatus.WrongAnswer:
                    return AppInfo.Setting?.TextColor?.WrongAnswer ?? "";

                case Interface.Judger.JudgeResult.JudgeStatus.TimeLimitExceed:
                    return AppInfo.Setting?.TextColor?.TimeLimitExceed ?? "";

                case Interface.Judger.JudgeResult.JudgeStatus.MemoryLimitExceed:
                    return AppInfo.Setting?.TextColor?.MemoryLimitExceed ?? "";

                case Interface.Judger.JudgeResult.JudgeStatus.RuntimeError:
                    return AppInfo.Setting?.TextColor?.RuntimeError ?? "";

                default:
                    return AppInfo.Setting?.TextColor?.WrongAnswer ?? "";
            }
        }

        private string getStatusText(OITool.Interface.Judger.JudgeResult.JudgeStatus status)
        {
            switch (status)
            {
                case Interface.Judger.JudgeResult.JudgeStatus.Accepted:
                    return "AC";

                case Interface.Judger.JudgeResult.JudgeStatus.WrongAnswer:
                    return "WA";

                case Interface.Judger.JudgeResult.JudgeStatus.TimeLimitExceed:
                    return "TLE";

                case Interface.Judger.JudgeResult.JudgeStatus.MemoryLimitExceed:
                    return "MLE";

                case Interface.Judger.JudgeResult.JudgeStatus.RuntimeError:
                    return "RE";

                default:
                    return "UR";
            }
        }

        public async Task<bool> Make(string reportFilePath, OITool.Interface.Judger.JudgeResult[] results)
        {
            try
            {
                using var bodyFrameworkReader = new StreamReader(Path.Combine(AppInfo.BaseDirectory, "body.framework.html"));
                using var pointFrameworkReader = new StreamReader(Path.Combine(AppInfo.BaseDirectory, "point.framework.html"));
                using var writer = new StreamWriter(reportFilePath, false);

                var bodyTemplate = await bodyFrameworkReader.ReadToEndAsync();
                var pointTemplate = await pointFrameworkReader.ReadToEndAsync();

                var pointsHtml = "";

                var index = 1;
                foreach (var result in results)
                {
                    var pointHtml = pointTemplate.Replace("${point-index}", $"{index}")
                                                 .Replace("${result-color}", getColor(result.Status))
                                                 .Replace("${result-text}", getStatusText(result.Status))
                                                 .Replace("${judge-time}", $"{result.Time}")
                                                 .Replace("${program}", result.ProgramFile)
                                                 .Replace("${data}", $"{result.DataFile}<br/>{result.AnswerFile}")
                                                 .Replace("${time}", $"{Math.Round(result.TimeUsed, 1)}ms ({result.Timeout})");

                    var lengthLimit = AppInfo.Setting?.BytesLimit ?? 10240;
                    var prgOut = result.ProgramOutput;
                    pointHtml = pointHtml.Replace("${program-output}", $"{prgOut[..Math.Min(prgOut.Length, lengthLimit)]}{(prgOut.Length < lengthLimit ? "" : "...")}");

                    var buffer = new char[lengthLimit];
                    using var standardOutputStream = new StreamReader(result.AnswerFile);
                    await standardOutputStream.ReadAsync(buffer);
                    pointHtml = pointHtml.Replace("${standard-output}", $"{string.Concat(buffer)}{(standardOutputStream.EndOfStream ? "" : "...")}");

                    pointsHtml += pointHtml;
                    ++index;
                }

                await writer.WriteAsync(bodyTemplate.Replace("${points}", pointsHtml));
                return true;
            }
            catch { return false; }
        }
    }
}