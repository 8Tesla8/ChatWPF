using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using EntityExtension;

namespace SPClient
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            Console.Write("Введите имя пользователя: ");
            string name = Console.ReadLine();

            ClientObj client = new ClientObj(name);
            client.Start();
        }
    }
}