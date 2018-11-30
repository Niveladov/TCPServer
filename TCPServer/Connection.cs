using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    internal sealed class Connection
    {
        private readonly TcpClient _client;

        public string RecievedMessage { get; private set; }
        public IConnectionMode[] Modes { get; }
        public Stopwatch Timer { get; }

        public Connection(TcpClient tcpClient)
        {
            _client = tcpClient;
            Timer = new Stopwatch();
            Modes = new IConnectionMode[]
            {
                new ReportMode(),
                new LogMode(),
                new DefaultMode()
            };
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = _client.GetStream();
                var data = new byte[64];
                while (true)
                {
                    var builder = new StringBuilder();
                    int bytes = 0;
                    var str = "";
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        str = Encoding.ASCII.GetString(data, 0, bytes);
                        builder.Append(str);
                    }
                    while (!MyCommands.IsEndOfCommand(str));

                    Timer.Start();
                    RecievedMessage = builder.ToString();
                    if (!MyCommands.IsEndOfCommand(RecievedMessage))
                    {
                        Console.WriteLine(RecievedMessage);
                        var replyMessage = DoWorkAndGetReply();
                        if (!string.IsNullOrWhiteSpace(replyMessage))
                        {
                            data = Encoding.ASCII.GetBytes(replyMessage);
                            stream.Write(data, 0, data.Length);
                        }
                    }
                    if (Timer.IsRunning) Timer.Reset();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (_client != null)
                    _client.Close();
            }
        }
        
        private string DoWorkAndGetReply()
        {
            string result = "";
            foreach (var mode in Modes)
            {
                mode.DoWork(this);
                if (!string.IsNullOrWhiteSpace(mode.Reply))
                {
                    result += mode.Reply;
                }
            }
            return result;
        }

    }
}
