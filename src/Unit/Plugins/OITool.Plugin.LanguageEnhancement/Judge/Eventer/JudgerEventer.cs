using System;
using System.Diagnostics;

namespace OITool.Plugin.LanguageEnhancement.Judge.Eventer
{
    public class JudgerEventer : Interface.Worker.Judge.Eventer.IJudgerEventer
    {
        private string replaceVariable(string text, string file) =>
            text.Replace("${file}", file)
                .Replace("${file-without-extension}", Path.ChangeExtension(file, "")[..^1])
                .Replace("${exec-extension}", OperatingSystem.IsWindows() ? ".exe" : "");

        public string Identifier { get; }

        public JudgerEventer(string identifier)
        {
            this.Identifier = identifier;
        }

        public void BeforeJudge(ref Interface.Worker.Judge.JudgerArgument argument, ref Interface.Worker.Judge.JudgerOption option, Interface.ActionHandler handler)
        {
            var programCode = handler.ConvertToWorkingDirectory(argument.ProgramFile);
            if (AppInfo.Setting?.CppLang?.Extension?.Contains(Path.GetExtension(programCode).Replace(".", "")) ?? false)
            {
                using var process = Process.Start(new ProcessStartInfo()
                {
                    Arguments = replaceVariable(AppInfo.Setting?.CppLang?.Build?.Argument ?? "", programCode),
                    FileName = replaceVariable(AppInfo.Setting?.CppLang?.Build?.Builder ?? "", programCode),
                    CreateNoWindow = true,
                    RedirectStandardError = true
                });

                if (process == null)
                    AppInfo.Console.Client.OutputLine(" ", ("red", $"[LangEn]"), ("", "Failed to run builder."));
                else
                {
                    if (!process.WaitForExit(AppInfo.Setting?.CppLang?.Build?.Timeout ?? 60000))
                    {
                        process.Kill();
                        AppInfo.Console.Client.OutputLine(("red", "[LangEn] Builder timed out."));
                    }

                    var errorMessage = process.StandardError.ReadToEnd();
                    if (errorMessage.Length > 8 || !string.IsNullOrEmpty(errorMessage))
                    {
                        AppInfo.Console.Client.OutputLine(" ", ("red", "[LangEn] Error from the builder:" + Environment.NewLine), ("", errorMessage));
                    }

                    if (process.ExitCode == 0)
                    {
                        argument.ProgramFile = replaceVariable(AppInfo.Setting?.CppLang?.Build?.Output ?? "", programCode);
                    }
                }
            }
        }

        public void AfterJudge(ref Interface.Worker.Judge.JudgerArgument argument, ref Interface.Worker.Judge.JudgerOption option, ref Interface.Worker.Judge.JudgerResult[] result, Interface.ActionHandler handler)
        { }
    }
}