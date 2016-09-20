using EntityExtension;
using EntityMessage.ChangeText;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfClient
{
    public class ClientObj
    {
        //for wpf++++
        IChangeText classForChangeText;
        public ClientObj(IChangeText changeText, string name, string host = "127.0.0.1", int port = 8888)
        {
            this.classForChangeText = changeText;

            userName = name;
            this.host = host;
            this.port = port;

            client = new TcpClient();

            Id = Guid.NewGuid();
        }
        //for wpf++++

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

            classForChangeText.ChangeText(string.Format("Соединяется с Server: {0} ", host));
        }

        public void SendMessage(string messageStr)
        {
            try
            {
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
            catch (Exception ex)
            {
                Disconnect();
                classForChangeText.ChangeText("Подключение прервано!");
                classForChangeText.ChangeText(ex.Message);
                return;
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
                    classForChangeText.ChangeText(message.ToString());
                }
            }
            catch (Exception ex)
            {
                Disconnect();
                classForChangeText.ChangeText("Подключение прервано!");
                classForChangeText.ChangeText(ex.Message);
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

                //SendMessage();
            }
            catch (Exception ex)
            {
                Disconnect();
                classForChangeText.ChangeText("Подключение прервано!");
                classForChangeText.ChangeText(ex.Message);
            }
        }
    }
}
