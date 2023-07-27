using System.Diagnostics;
using System.Text;
using Nefe.Pipe.NamedPipe;

namespace OITool.CLI
{
    internal class ServerHelper
    {
        #region [Private Filed]

        private string serverName;
        private string serverPath;

        #endregion

        #region [Public Method]

        public ServerHelper(string serverName = "OITool.Server", string serverPath = "server/OITool.Server")
        {
            this.serverName = serverName;
            this.serverPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, OperatingSystem.IsWindows() ? serverPath + ".exe" : serverPath));
        }

        public async Task<bool> DetectSurvivalAsync(int timeout = 3000, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = new NamedPipeClient(this.serverName);
                await client.ConnectAsync(timeout, cancellationToken);

                if (client.IsConnected)
                {
                    await client.SendBytesAsync(Encoding.UTF8.GetBytes("verify-alive"), cancellationToken);
                    if (Encoding.UTF8.GetString(await client.ReceiveBytesAsync(cancellationToken)) == "exist")
                        return true;
                }
            }
            catch { }

            return false;
        }

        // Make sure DetectSurvivalAsync() is false
        public async Task StartServerAsync(bool hidden = false, CancellationToken cancellationToken = default)
        {
            if (File.Exists(serverPath))
            {
                var process = Process.Start(
                    new ProcessStartInfo()
                    {
                        Arguments = "",
                        FileName = serverPath,
                        WorkingDirectory = Path.GetDirectoryName(serverPath),
                        CreateNoWindow = hidden
                    }
                );
                
                if (process != null && !hidden)
                {
                    await process.WaitForExitAsync(cancellationToken);
                    process.Close();
                }
            }
            else
                throw new FileNotFoundException("Server not found.");
        }

        #endregion
    }
}