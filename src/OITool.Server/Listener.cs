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

                                        if (data == null || !data.Argument.HasValue || data.CurrentDirectory == null)
                                            throw new ArgumentException($"Failed to parse data");

                                        // TODO:
                                        // Write ExtraInfo into file

                                        var judger = new Base.Worker.Judger(
                                            argument: data.Argument.Value,
                                            option: new()
                                            {
                                                StdInputFileExtensions = new string[]
                                                {
                                                    "in"
                                                },
                                                StdOnputFileExtensions = new string[]
                                                {
                                                    "ans",
                                                    "out"
                                                }
                                            },
                                            judgers: AppInfo.Workers.Judges.Judgers.ToArray(),
                                            reporters: AppInfo.Workers.Judges.Reporters.ToArray()
                                        );

                                        await server.SendBytesAsync(encoder.GetBytes(
                                            JsonSerializer.Serialize<Comm.Data.Judge.ResultData>(new()
                                            {
                                                Result = await judger.Judge(data.CurrentDirectory)
                                            })
                                        ), cancellationToken);

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