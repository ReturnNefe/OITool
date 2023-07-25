namespace OITool.Server
{
    public class Listener : IDisposable
    {
        #region [Private Property]
        
        private Nefe.Pipe.NamedPipe.NamedPipeServer server = null!;

        #endregion
        
        #region [Public Method]
        
        public Listener(string serverName = "OITool.Server")
        {
            server = new(serverName);
        }

        public void Dispose() => server.Dispose();

        public async Task Listen(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    await server.WaitForConnectionAsync();
                    
                }
            }
            catch (OperationCanceledException) { return; }
        }
        
        #endregion
    }
}