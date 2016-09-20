using EntityExtension;
using SPClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPServer
{
    public class ServerObj
    {
        private string host;
        private int port;
        private TcpListener tcpListener;
        
        private List<ClientForServer> clientsList = new List<ClientForServer>();

        public ServerObj(string host = "127.0.0.1", int port = 8888)
        {
            this.host = host;
            this.port = port;
        }

        public void Listen()
        {           
            IPAddress localAddr = IPAddress.Parse(host);
            tcpListener = new TcpListener(localAddr, port);
            tcpListener.Start();

            //MainWindow.ChatBox.
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            Start();
        }

        private void Start()
        {
            try
            {
                for(;;)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();                    

                    if (tcpClient.Connected)
                    {
                        //принимаем данные о пользователе                       
                        GetFirstMessageFromNewClient(tcpClient);

                        Thread clientThreadNew = new Thread(new ParameterizedThreadStart(GetAndReciveToAll));
                        clientThreadNew.Start(tcpClient);
                    }
                }
            }
            catch(Exception ex)
            {
                Disconnect();
        
                Console.WriteLine(ex.Message);
            }

        }

        private void GetFirstMessageFromNewClient(TcpClient clientTcp)
        {
            NetworkStream streamCl = clientTcp.GetStream();

            // буфер для получаемых данных
            byte[] data = new byte[1024];
            int bytes = 0;
            do
            {
                bytes = streamCl.Read(data, 0, data.Length);
            }
            while (streamCl.DataAvailable);

            Message msg = data.DeserializeToObject() as Message;
            
            //создаем нового клиента и добавляем в колекцию
            ClientForServer cl = new ClientForServer(clientTcp, streamCl, msg.Id, msg.stringMessage);

            AddNewClient(cl);

        }
        
        private void AddNewClient(ClientForServer newClient)
        {
            clientsList.Add(newClient);

            string newUserIn = string.Format("Подключился пользователь {0}", newClient.UserName);

            Console.WriteLine(newUserIn);

            //Создание своего обьекта типа Massage 
            Message message = new Message
            {
                Id = Guid.NewGuid(),
                Content = null,
                stringMessage = newUserIn
            };

            //Серилизация обьекта типа Massage, делаем из обьекта массив бит 
            byte[] data = message.SerializeToByteArray();
            //отправку всем пользователям, в чат вошел новый пользователь 
            for (int index = 0; index < clientsList.Count; index++)
            {
                clientsList[index].Stream.Write(data, 0, data.Length);
            }
        }    
        
        private void GetAndReciveToAll(object obj)
        {
            TcpClient cl = obj as TcpClient;
            NetworkStream netStream = cl.GetStream();

            try
            {
                for (;;)
                {
                    // буфер для получаемых данных
                    byte[] data = new byte[1024];
                    int bytes = 0;
                    do
                    {
                        bytes = netStream.Read(data, 0, data.Length);
                    }
                    while (netStream.DataAvailable);

                    Message message = data.DeserializeToObject() as Message;
                    //вывод сообщения  
                    Console.WriteLine(message.ToString());

                    //отправка сообщения всем кто в чате
                    byte[] dataSend = new byte[1024];
                    dataSend = message.SerializeToByteArray();

                    for (int index = 0; index < clientsList.Count; index++)
                    {
                        clientsList[index].Stream.Write(data, 0, data.Length);
                    }
                }
            }
            catch
            {
                DeleteUser(cl);
            }
        }
        private void DeleteUser(TcpClient client)
        {
            string nameDeleteUser = null;
            int indexDelete = -1;

            for (int index = 0; index < clientsList.Count; index++)
            {
                if (client == clientsList[index].Tcp)
                {
                    nameDeleteUser = clientsList[index].UserName;
                    indexDelete = index;
                }
            }

            if (indexDelete != -1)
            {
                clientsList.Remove(clientsList[indexDelete]);

                string newMsg = string.Format("Из чата вышел пользователь: {0}", nameDeleteUser);

                //Создание своего обьекта типа Massage 
                Message message = new Message
                {
                    Id = Guid.NewGuid(),
                    Content = null,
                    stringMessage = newMsg
                };

                //Серилизация обьекта типа Massage, делаем из обьекта массив бит 
                byte[] data = message.SerializeToByteArray();
                //отправку всем пользователям, из чата вышел пользователь 
                for (int index = 0; index < clientsList.Count; index++)
                {
                    clientsList[index].Stream.Write(data, 0, data.Length);
                }
            }
        }

        public void Disconnect()
        {
            //отключение потока
            if (clientsList != null)
            {
                for (int i = 0; i < clientsList.Count; i++)
                    clientsList[i].Stream.Close();
            }
            //отключение сервер
            if (tcpListener != null)
                tcpListener.Stop();
            //Environment.Exit(0); //завершение процесса
        }
    }
}
