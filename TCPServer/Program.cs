using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class Program
    {
        private static string _addres = "127.0.0.1"; // 127.0.0.1
        private static int _port = 8888;
        private static TcpListener _listener;
        private static void Main(string[] args)
        {
            //Console.WriteLine("Введите IP-адресс сервера:");
            //_addres = Console.ReadLine();
            //Console.WriteLine("Введите порт сервера:");
            //_port = Convert.ToInt32(Console.ReadLine());

            try
            {
                _listener = new TcpListener(IPAddress.Parse(_addres), _port);
                _listener.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    var connectedClient = new ConnectedClient(client);

                    // создаем новый поток для обслуживания нового клиента
                    var clentTask = new Task(connectedClient.Process);
                    clentTask.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (_listener != null)
                    _listener.Stop();
            }
        }
    }
}
