using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    internal interface IConnectionMode
    {
        bool Enable { get; set; }
        string Reply { get; }
        void DoWork(Connection connection);
    }

    internal sealed class DefaultMode : IConnectionMode
    {
        private ICommand _command;

        public bool Enable { get; set; } = true;
        public string Reply { get { return _command?.Reply ?? ""; } }

        public void DoWork(Connection connection)
        {
            try
            {
                _command = MyCommands.GetCommand(connection.RecievedMessage);
                _command?.DoWork(connection.Modes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

    internal sealed class ReportMode : IConnectionMode
    {
        public bool Enable { get; set; }
        public string Reply { get; private set; }

        public void DoWork(Connection connection)
        {
            try
            {
                Reply = "";
                if (Enable)
                {
                    connection.Timer.Stop();
                    Reply = $"{connection.RecievedMessage.Replace("\r\n", "")} {connection.Timer.Elapsed.TotalMilliseconds} milliseconds\r\n";
                    connection.Timer.Reset();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    internal sealed class LogMode : IConnectionMode
    {
        public bool Enable { get; set; }
        public string Reply { get { return ""; } }

        public void DoWork(Connection connection)
        {
            try
            {
                if (Enable)
                {
                    using (var streamWriter = new StreamWriter(Program.LogFilePath, true, Encoding.ASCII))
                    {
                        streamWriter.WriteLine(connection.RecievedMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }



}
