using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ContactGloveSDK
{
    internal class DivingStationPairingServer
    {
        internal delegate void SetDivingStationIpAddress(string ipAddress);

        private readonly SetDivingStationIpAddress setDivingStationIpAddress;

        private const int DivingStationPort = 25800;
        private const int ApplicationPort = 25801;

        private const string RequestToSend = "DivingStationPairing_ApplicationIP";
        private const string RequestToReceive = "DivingStationPairing_DivingStationIP";
        private const string RequestToStartPairing = "DivingStationPairing_StartPairing";

        private readonly UdpClient server;

        private readonly SynchronizationContext context;

        internal DivingStationPairingServer(SetDivingStationIpAddress setDivingStationIpAddress)
        {
            this.setDivingStationIpAddress = setDivingStationIpAddress;
            context = SynchronizationContext.Current;

            server = new UdpClient(new IPEndPoint(IPAddress.Any, ApplicationPort));

            RequestPairing();

            Task.Run(ReceiveMessage);
        }

        internal void CloseServer()
        {
            server.Close();
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                byte[] messageBytes = server.Receive(ref ep);
                var messageStr = Encoding.UTF8.GetString(messageBytes);

                var messages = messageStr.Split(',');
                if (messages.Length != 2)
                    continue;

                var request = messages[0];
                var ipAddress = messages[1];

                if (request != RequestToReceive)
                    continue;

                context.Post(_ => setDivingStationIpAddress(ipAddress), null);

                SendThisIpAddress(IPAddress.Parse(ipAddress));
            }
        }

        private void SendThisIpAddress(IPAddress divingStationIpAddress)
        {
            var client = new UdpClient();
            var ep = new IPEndPoint(divingStationIpAddress, DivingStationPort);

            var messageStr = RequestToSend + "," + GetThisIpAddress();
            var messageBytes = Encoding.UTF8.GetBytes(messageStr);

            client.Send(messageBytes, messageBytes.Length, ep);
        }

        private void RequestPairing()
        {
            var client = new UdpClient();
            client.EnableBroadcast = true;

            var ep = new IPEndPoint(IPAddress.Broadcast, ApplicationPort);

            var messageBytes = Encoding.UTF8.GetBytes(RequestToStartPairing);

            client.Send(messageBytes, messageBytes.Length, ep);
        }

        private string GetThisIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }

            return "";
        }
    }
}