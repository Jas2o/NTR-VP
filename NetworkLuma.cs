using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NTR_ViewerPlus {

    public static class NetworkLuma {

        private static UdpClient udpClient;
        private static IPEndPoint udpEP;

        public static void Start(string ip) {
            if (udpClient == null) {

                udpClient = new UdpClient();
                udpEP = new IPEndPoint(IPAddress.Parse(ip), 4950);
                udpClient.Connect(udpEP);

                //udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true); //New
                //udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 4950)); //New

                Console.WriteLine("Connect?");
            } else {
                Console.WriteLine("Luma UDP already existed.");
            }
        }

        public static void Write(byte[] buffer) {
            if (buffer.Length == 20)
                udpClient.Send(buffer, buffer.Length);
        }


    }
}
