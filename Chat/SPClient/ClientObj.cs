using EntityExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPClient
{
    public class ClientObj
    {

        private string host;
        private int port;

        public Guid Id { get; private set; }
        public NetworkStream stream { get; private set; }
        public string userName { get; private set; }
        TcpClient client;
        
        public ClientObj(string name, string host = "127.0.0.1", int port = 8888)
        {
            userName = name;
            this.host = host;
            this.port = port;

            client = new TcpClient();

            Id = Guid.NewGuid();
        }

        public void Connect()
        {
            //подключение клиента
            client.Connect(host, port);
            // получаем поток
            stream = client.GetStream(); 

            Console.WriteLine("Соединяется с Server: {0} ", host);
        }

        public void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            for (;;)
            {
                string messageStr = Console.ReadLine();

                //Создание своего обьекта типа Massage 
                Message message = new Message
                {
                    Id = Guid.NewGuid(),
                    Content = null,
                    stringMessage = string.Format("{0}: {1}", userName, messageStr)
                };

                //Серилизация обьекта типа Massage, делаем из обьекта массив бит 
                byte[] data = message.SerializeToByteArray();

                //сообщение отсылается
                stream.Write(data, 0, data.Length);
            }
        }

        public void ReceiveMessage()
        {
            try
            {
                for (;;)
                {
                    // буфер для получаемых данных
                    byte[] data = new byte[1024];

                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                    }
                    while (stream.DataAvailable);

                    Message message = data.DeserializeToObject() as Message;
                    //вывод сообщения           
                    Console.WriteLine(message.ToString());
                }
            }
            catch
            {
                Disconnect();
                Console.WriteLine("Подключение прервано!"); //соединение было прервано
                Console.ReadLine();
            }
        }

        public void Disconnect()
        {
            //отключение потока
            if (stream != null)
                stream.Close();
            //отключение клиента
            if (client != null)
                client.Close();
            //Environment.Exit(0); //завершение процесса
        }
        private void SendFirstMessage()//посылка данных о себе
        {
            //Создание своего обьекта типа Massage 
            Message message = new Message
            {
                Id = this.Id,
                Content = null,
                stringMessage = this.userName
            };

            //Серилизация обьекта типа Massage, делаем из обьекта массив бит 
            byte[] data = message.SerializeToByteArray();

            //сообщение отсылается
            stream.Write(data, 0, data.Length);
        }


        public void Start()
        {
            try
            {
                Connect();

                //посылка своей сылки
                SendFirstMessage();

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();

                SendMessage();
            }
            catch
            {
                Disconnect();
                Console.WriteLine("Подключение прервано!"); //соединение было прервано
                Console.ReadLine();
            }
        }
    }
}
