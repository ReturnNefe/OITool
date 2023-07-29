using System;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace OITool.Server
{
    internal class Listener : IAsyncDisposable
    {
        #region [Private Field]

        private Encoding encoder = Encoding.UTF8;
        private string serverVersion;
        private Nefe.Pipe.NamedPipe.NamedPipeServer server;

        #endregion

        #region [Public Method]

        public Listener(string serverVersion, string serverName = "OITool.Server")
        {
            this.serverVersion = serverVersion;
            this.server = new(serverName);
        }

        public ValueTask DisposeAsync() => this.server.DisposeAsync();

        public async Task ListenAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        await this.server.WaitForConnectionAsync(cancellationToken);

                        // Shake hands
                        // Verify Server:
                        // client> verify-alive
                        // server> exist
                        if (encoder.GetString(await server.ReceiveBytesAsync(cancellationToken)) != "helo")
                        {
                            await server.SendBytesAsync(encoder.GetBytes("exist"), cancellationToken);
                            server.Disconnect();
                            continue;
                        }

                        // Verify Client Version
                        if (encoder.GetString(await server.ReceiveBytesAsync(cancellationToken)) != this.serverVersion)
                        {
                            await server.SendBytesAsync(encoder.GetBytes("mismatched"), cancellationToken);
                            server.Disconnect();
                            continue;
                        }

                        await server.SendBytesAsync(encoder.GetBytes("accept"), cancellationToken);

                        while (true)
                        {
                            switch (encoder.GetString(await server.ReceiveBytesAsync(cancellationToken)))
                            {
                                case "judge":
                                    {
                                        var data = JsonSerializer.Deserialize<Comm.Data.Judge.ArgumentData>(encoder.GetString(await server.ReceiveBytesAsync(cancellationToken)));

                                        if (data == null || data.CurrentDirectory == null)
                                            throw new ArgumentException($"Failed to parse data");

                                        // TODO:
                                        // Write ExtraInfo into file

                                        // Parse json-based data
                                        var judge = new Base.Worker.Judge(
                                            argument: new()
                                            {
                                                Mode = data.Mode ?? AppInfo.Setting?.Option?.Mode ?? "common",
                                                ProgramFile = data.ProgramFile,
                                                DataFiles = data.DataFiles,
                                                Timeout = data.Timeout ?? AppInfo.Setting?.Option?.Timeout ?? 1000,
                                                MemoryLimit = data.MemoryLimit ?? AppInfo.Setting?.Option?.MemoryLimit ?? 512,
                                                ReportFile = data.ReportFile ?? AppInfo.Setting?.Option?.ReportFile
                                            },
                                            option: new()
                                            {
                                                StdInputFileExtensions = AppInfo.Setting?.Option?.Extension?.InputData ?? new string[] { "in" },
                                                StdOnputFileExtensions = AppInfo.Setting?.Option?.Extension?.OutputData ?? new string[] { "ans", "out" }
                                            },
                                            workerArgument: new()
                                            {
                                                Judgers = AppInfo.Workers.Judge.Judgers.ToArray(),
                                                Reporters = AppInfo.Workers.Judge.Reporters.ToArray(),
                                                JudgerEventers = AppInfo.Workers.Judge.Eventer.GetActiveJudgerEventers().ToArray(),
                                                ReporterEventers = AppInfo.Workers.Judge.Eventer.GetActiveReporterEventers().ToArray()
                                            }
                                        );

                                        try
                                        {
                                            await server.SendBytesAsync(encoder.GetBytes(
                                                JsonSerializer.Serialize<Comm.Data.Judge.ResultData>(new()
                                                {
                                                    Result = await judge.ExecuteJudger(data.CurrentDirectory),
                                                    ConsoleInformation = AppInfo.Console.GetClientConsole().GetBuffer().ToArray()
                                                })
                                            ), cancellationToken);
                                        }
                                        finally
                                        {
                                            AppInfo.Console.GetClientConsole().ClearBuffer();
                                        }

                                        break;
                                    }
                            }

                            if (encoder.GetString(await server.ReceiveBytesAsync(cancellationToken)) != "continue")
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        new Nefe.ColorConsole.ColorString(("red", "Exception "), ("white", ex.ToString())).Output(true);

                        // TODO: Exception Handle in CLI
                        try
                        {
                            await server.SendBytesAsync(encoder.GetBytes(
                                JsonSerializer.Serialize(new Comm.Data.IData() { ExtraInformation = ex.ToString() })
                            ));
                        }
                        catch { }
                    }

                    try { server.Disconnect(); } catch { }
                }
            }
            catch (OperationCanceledException) { return; }
        }

        #endregion
    }
}