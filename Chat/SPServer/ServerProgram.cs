using EntityExtension;
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
    class ServerProgram
    {
        static Thread listenThread;
        static void Main(string[] args)
        {
            ServerObj server = new ServerObj();

            listenThread = new Thread(new ThreadStart(server.Listen));
            listenThread.Start();
        }
    }
}