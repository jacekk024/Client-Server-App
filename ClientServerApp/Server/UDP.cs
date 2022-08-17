using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class UDPCommunicator : ICommunicator
    {
        private UdpClient client;

        public UDPCommunicator(UdpClient client) //port lokalny jako 11001
        {
            this.client = client;
        }

        public void Run(CommandD onCommand, CommunicatorD onDisconnect)
        {
          
            try 
            {
                //IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                //Byte[] bytes = client.Receive(ref RemoteIpEndPoint);
                //string receiveData = Encoding.ASCII.GetString(bytes);

                //Console.WriteLine("Received {0} from {1}, port {2}",receiveData,RemoteIpEndPoint.Address,RemoteIpEndPoint.Port);


                //string receive = onCommand(receiveData);
                //Byte[] sendData = Encoding.ASCII.GetBytes(receive);
                //Console.WriteLine("Sent to {0}:{1}: {2}", sendData, RemoteIpEndPoint.Address, RemoteIpEndPoint.Port);
                
                //kolejne wywolanie zawiesza 

                while (true) 
                {
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] bytes = client.Receive(ref RemoteIpEndPoint);
                    string receiveData = Encoding.ASCII.GetString(bytes);

                    Console.WriteLine(" - UDP - Received {0} from {1}, port {2}", receiveData, RemoteIpEndPoint.Address, RemoteIpEndPoint.Port);

                    string receive = onCommand(receiveData);
                    byte[] sendData = Encoding.ASCII.GetBytes(receive);
                    client.Send(sendData, sendData.Length, RemoteIpEndPoint);
                    Console.WriteLine(" - UDP - Sent to {0}:{1}: {2}", receive, RemoteIpEndPoint.Address, RemoteIpEndPoint.Port);

                    break;
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
                onDisconnect(this);
            }
        }
        public void Start(CommandD onCommand, CommunicatorD onDisconnect) 
        {
            Task.Run(() => Run(onCommand, onDisconnect));
        }

        public void Stop()
        {
            Console.WriteLine("- UDP - Communicator stopped");
        }
    }

    class UDPListner : IListner
    {
        private int localPort;
        private UdpClient client;


        public UDPListner(int localPort) 
        {
            this.localPort = localPort;
        }
        public void Run(CommunicatorD onConnect) 
        {
            try
            {
                client = new UdpClient(localPort);
                onConnect(new UDPCommunicator(client));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void Start(CommunicatorD onConnect)
        {
            Task.Run(() => Run(onConnect));
        }


        public void Stop()
        {
            Task.Run(()=> client.Close());
            Console.WriteLine("- UDP - Listner stopped");
        }
    }
}
