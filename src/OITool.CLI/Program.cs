using System.Security.Cryptography;
using System.Text.Json;
using System.Reflection;
using Nefe.ColorConsole;

using Cocona;
using System.Text;

using JudgerStatus = OITool.Interface.Worker.Judge.JudgerStatus;

namespace OITool.CLI
{
    internal class Program
    {
        static void WriteRateBar(double rate, int width, bool oppositeColor = false, bool newLine = false)
        {
            var pos = Console.GetCursorPosition();

            width = Math.Min(width, Console.WindowWidth - pos.Left);
            if (width < 0) width = 0;
            if (rate < 0) rate = 0;
            if (rate > 1) rate = 1;

            var rateWidth = (int)Math.Round(rate * width);
            var colorName = "DarkGreen";

            if (oppositeColor)
            {
                if (rate < 0.3d)
                    colorName = "Red";
                else if (rate >= 0.3d && rate < 0.5d)
                    colorName = "DarkYellow";
                else if (rate > 0.9d)
                    colorName = "Green";
            }
            else
            {
                if (rate < 0.3d)
                    colorName = "Green";
                else if (rate > 0.6d && rate <= 0.8d)
                    colorName = "DarkYellow";
                else if (rate > 0.8d)
                    colorName = "Red";
            }

            new ColorString((":DarkGray", new string(' ', width))).Output(newLine);
            var newPos = Console.GetCursorPosition();
            Console.SetCursorPosition(pos.Left, pos.Top);

            new ColorString(($":{colorName}", new string(' ', rateWidth))).Output();
            Console.SetCursorPosition(newPos.Left, newPos.Top);
        }

        static async Task Main(string[] args)
        {
            var serverHelper = new ServerHelper();
            var app = CoconaLiteApp.Create();

            app.AddSubCommand("server", (configure) =>
            {
                configure.AddCommand("start", async ([Option('b', Description = "Indicates whether to run server in the background. (Default is false)")] bool? hidden) =>
                {
                    try
                    {
                        if (await serverHelper.DetectSurvivalAsync())
                            new ColorString(("green", "Server is running.")).Output(true);
                        else
                        {
                            await serverHelper.StartServerAsync(hidden ?? false);

                            // Detect if server is running in the background.
                            if ((hidden ?? false) && !await serverHelper.DetectSurvivalAsync())
                                new ColorString(("red", "Failed to start server.")).Output(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        new ColorString(("red", $"Error: "), ("", ex.ToString())).Output(true);
                    }
                })
                .WithDescription("Try to start a server if OITool.Server is not running.");
            });
            app.AddCommand("judge", async ([Argument("program", Description = "The program to be judged.")] string programFile,
                                           [Argument("data", Description = "The data used to judge. It could be data-file or folder.")] string[] dataFiles,
                                           [Option("timeout", new char[] { 't' }, Description = "The time limit of the program to be judged.")] int? timeout,
                                           [Option("memory", new char[] { 'm' }, Description = "The memory limit of the program to be judged.")] int? memoryLimit,
                                           [Option("report", new char[] { 'r' }, Description = "Indicates where the report file should be generated.")] string? reportFile,
                                           [Option(Description = "The mode used when judging. The default is \"common\".")] string? mode) =>
            {
                // Check Argument
                if (dataFiles.Count() == 0)
                {
                    new ColorString(" ", ("yellow", "Data-file cannot be empty. Type"), ("cyan", "oitool judge -h"), ("yellow", "for details.")).Output(true);
                    return;
                }

                try
                {
                    var clientVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
                    var encoder = Encoding.UTF8;

                    await using var client = new Nefe.Pipe.NamedPipe.NamedPipeClient("OITool.Server");
                    await client.ConnectAsync(3000);

                    if (client.IsConnected)
                    {
                        await client.SendBytesAsync(encoder.GetBytes("helo"));
                        await client.SendBytesAsync(encoder.GetBytes(clientVersion));

                        if (encoder.GetString(await client.ReceiveBytesAsync()) != "accept")
                            throw new Exception("Reponse Error");

                        await client.SendBytesAsync(encoder.GetBytes("judge"));

                        var data = new Comm.Data.Judge.ArgumentData()
                        {
                            Mode = mode,
                            ProgramFile = programFile,
                            DataFiles = dataFiles,
                            Timeout = timeout,
                            MemoryLimit = memoryLimit,
                            ReportFile = reportFile,
                            CurrentDirectory = Environment.CurrentDirectory
                        };

                        await client.SendBytesAsync(encoder.GetBytes(JsonSerializer.Serialize<Comm.Data.Judge.ArgumentData>(data)));

                        var resultData = JsonSerializer.Deserialize<Comm.Data.Judge.ResultData>(encoder.GetString(await client.ReceiveBytesAsync()));

                        // Confirm Result
                        if (resultData == null)
                            throw new Exception("Failed to get result data from server.");
                        else if (!resultData.Result.HasValue)
                        {
                            new ColorString(("red", $"Error: "), ("", resultData.ExtraInformation ?? "Unkown Error")).Output(true);
                        }
                        else
                        {
                            // Try to output console information from plugins.
                            if (resultData.ConsoleInformation is var infoLines && infoLines != null)
                            {
                                foreach (var line in infoLines)
                                    if (line.ColoredText != null)
                                        new ColorString(line.Separator, line.ColoredText).Output(true);
                            }

                            if (resultData.Result.Value is var resultValue && resultData.Result.Value.Judge.Count() > 0)
                            {
                                var count = resultValue.Judge.Count();
                                var passCount = resultValue.Judge.Where(point => point.Status == JudgerStatus.Accepted).Count();

                                // Output Pass Rate (Bar)
                                new ColorString(("", "Pass    ")).Output();
                                WriteRateBar((double)passCount / (double)count, 24, oppositeColor: true);
                                new ColorString(("", $" ({passCount}/{count})")).Output(true);

                                // Calculate Average
                                var avgTimeUsed = 0d;
                                var avgMemoryUsed = 0d;
                                foreach (var iter in resultValue.Judge)
                                {
                                    avgTimeUsed += iter.TimeUsed;
                                    avgMemoryUsed += iter.MemoryUsed;
                                }
                                avgTimeUsed /= count;
                                avgMemoryUsed /= count;

                                // Output Time Avg Used (Bar)
                                new ColorString(("", "Time    ")).Output();
                                WriteRateBar(avgTimeUsed / (double)resultValue.Judge[0].Timeout, 24);
                                new ColorString(("", $" (avg {Math.Round(avgTimeUsed, 1)}ms)")).Output(true);

                                // Output Mmeory Avg Used (Bar)
                                new ColorString(("", "Memory  ")).Output();
                                WriteRateBar(avgMemoryUsed / (double)resultValue.Judge[0].MemoryLimit, 24);
                                new ColorString(("", $" (avg {Math.Round(avgMemoryUsed, 1)}MiB)")).Output(true);

                                // Points (List)
                                var failedPoints = resultValue.Judge.Select((point, index) => (value: point, index: index + 1)).Where(point => point.value.Status != JudgerStatus.Accepted);
                                if (failedPoints.Count() > 0)
                                {
                                    new ColorString(("", "Failures:")).Output(true);
                                    foreach (var point in failedPoints)
                                    {
                                        new ColorString(("yellow", $"    #{point.index} | {point.value.Status} ")).Output();

                                        if (point.value.Status == JudgerStatus.TimeLimitExceed)
                                            new ColorString(("yellow", $"({Math.Round(point.value.TimeUsed, 1)}ms)")).Output();
                                        else if (point.value.Status == JudgerStatus.MemoryLimitExceed)
                                            new ColorString(("yellow", $"({Math.Round(point.value.MemoryUsed, 1)}MiB)")).Output();

                                        Console.WriteLine();
                                    }
                                }
                                Console.WriteLine();

                                // Report (list)
                                if (resultValue.ReportFiles.Count() > 0)
                                {
                                    new ColorString(("", "View details in the reports:")).Output(true);
                                    foreach (var report in resultValue.ReportFiles)
                                        new ColorString(("cyan", $"    {report}")).Output(true);
                                }
                            }
                            else
                            {
                                new ColorString(("", "No File Judged.")).Output(true);

                                // Check if no data-file exists.
                                if (dataFiles.Where(file => File.Exists(file) || Directory.Exists(file)).Count() == 0)
                                    new ColorString(("yellow", $"Warn: It seems none of data-file exist. Check the paths.")).Output(true);

                                // Report (list)
                                if (resultValue.ReportFiles.Count() > 0)
                                {
                                    new ColorString(("", "We still generate reports:")).Output(true);
                                    foreach (var report in resultValue.ReportFiles)
                                        new ColorString(("cyan", $"    {report}")).Output(true);
                                }
                            }
                        }

                        await client.SendBytesAsync(encoder.GetBytes("close"));
                    }
                    else throw new Exception("Failed to connect.");
                }
                catch (TimeoutException)
                {
                    new ColorString(("red", $"Failed to connect.")).Output(true);
                    new ColorString(" ", ("", $"Type"), ("cyan", "oitool server start"), ("", "to launch server.")).Output(true);
                }
                catch (Exception ex)
                {
                    new ColorString(("red", $"Error: "), ("", ex.ToString())).Output(true);
                }
            })
            .WithDescription("Try to judge your program by given data-file.");

            await app.RunAsync();
        }
    }
}
