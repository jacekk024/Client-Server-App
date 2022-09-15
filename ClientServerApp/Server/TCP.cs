using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TCPCommunicator : ICommunicator
    {
        TcpClient client;
        CommunicatorD onDisconnect;
        public TCPCommunicator(TcpClient client)
        {
            this.client = client;
        }

        public void Run(CommandD onCommand, CommunicatorD onDisconnect)
        {
            Byte[] bytes = new Byte[256];
            string data = string.Empty;
            int len;
            NetworkStream stream = client.GetStream();//strumień danych na potrzeby dostępu do sieci
            Console.WriteLine("(TCP) Running Comunicator");

            try
            {
                while (client.Connected)
                {
                    if (stream.DataAvailable)
                    {
                        len = stream.Read(bytes, 0, bytes.Length);
                        data += Encoding.ASCII.GetString(bytes, 0, len);
                    }


                    while (data != string.Empty)
                    {

                        string message = onCommand(data);


                        Byte[] msg = Encoding.ASCII.GetBytes(message);

                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("(TCP) Send: {0}", message);
                        data = string.Empty;

                    }
                }
                stream.Close();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onDisconnect = onDisconnect;
            Task.Run(()=>Run(onCommand, onDisconnect)); // w osobnym watku
        }

        public void Stop()
        {
            client.Close();
            onDisconnect(this);
            Console.WriteLine("(TCP) Communicator stopped");
        }
    }

    class TCPListener : IListner
    {
        int port;
        string ip;
        TcpListener tcpListener;


        public TCPListener(string ip,int port)
        {
            this.port = port;
            this.ip = ip;
        }

        public void Run(CommunicatorD onConnect)
        {

            tcpListener = new TcpListener(IPAddress.Parse(ip), port); 
            TcpClient client;

            tcpListener.Start();

            while (true)
            {

                client = tcpListener.AcceptTcpClient(); 

                Console.WriteLine("(TCP) Connected!"); 

                onConnect(new TCPCommunicator(client));

                Console.WriteLine("(TCP) Connected with: " + client.Client.RemoteEndPoint); 
            }
        }

        public void Start(CommunicatorD onConnect)
        {
             Task.Run(()=>Run(onConnect)); // w osobnym watku  
        }
        public void Stop()
        {
            tcpListener.Stop();
            Console.WriteLine("(TCP) Listner stopped");
        }

    }

}
