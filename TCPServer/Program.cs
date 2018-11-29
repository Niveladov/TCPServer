using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class Program
    {
        public static string LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TCPServerLog.txt");

        private static void Main(string[] args)
        {
            Console.WriteLine($"Log file path: {LogFilePath}");
            var server = new Server();
            server.Run();
        }

    }
}
