namespace OITool.Server.Plugin
{
    internal class Console : Interface.Console.IConsole
    {
        internal class ClientConsole : Interface.Console.IClientConsole
        {
            private List<Nefe.ColorConsole.ColorString> bufferLines = new();

            public void OutputLine(string text)
            {
                this.bufferLines.Add(new(("", text)));
            }

            public void OutputLine(params (string color, string text)[] coloredText)
            {
                this.bufferLines.Add(new(coloredText));
            }

            public void OutputLine(string separator, params (string color, string text)[] coloredText)
            {
                this.bufferLines.Add(new(separator, coloredText));
            }

            public IEnumerable<Comm.Data.IData.TextLine> GetBuffer()
            {
                foreach (var line in this.bufferLines)
                    yield return new()
                    {
                        Separator = line.Separator,
                        ColoredText = line.ToTuples().ToArray()
                    };
            }

            public void ClearBuffer() => this.bufferLines.Clear();
        }

        private ClientConsole client;
        public Interface.Console.IClientConsole Client => client;

        public ClientConsole GetClientConsole() => client;

        public Console(ClientConsole client)
        {
            this.client = client;
        }
    }
}