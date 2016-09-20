using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfServer
{
    public class ClientForServer
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public NetworkStream Stream { get; private set; }
        public TcpClient Tcp { get; private set; }
        public ClientForServer(TcpClient Tcp, NetworkStream Stream, Guid Id, string UserName)
        {
            this.Id = Id;
            this.Tcp = Tcp;
            this.Stream = Stream;
            this.UserName = UserName;
        }
    }
}
