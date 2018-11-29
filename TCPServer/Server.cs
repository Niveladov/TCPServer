﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    internal sealed class Server
    {
        private string _addres; //= "127.0.0.1"
        private int _port; //= 8888;
        private TcpListener _listener;

        public void Run()
        {
            Console.WriteLine("Введите IP-адресс сервера:");
            _addres = Console.ReadLine();
            Console.WriteLine("Введите порт сервера:");
            _port = Convert.ToInt32(Console.ReadLine());

            try
            {
                _listener = new TcpListener(IPAddress.Parse(_addres), _port);
                _listener.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    var message = $"You connected to the server {_addres} {_port}";
                    var bytes = Encoding.ASCII.GetBytes(message);
                    client.GetStream().Write(bytes, 0, bytes.Length);
                    var connectionObject = new Connection(client);

                    //to create new task to service a new client
                    var clentTask = new Task(connectionObject.Process);
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
