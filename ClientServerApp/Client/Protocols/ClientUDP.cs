using Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Protocols
{
    class ClientUDP
    {
        public static void StartUDP() 
        {

            int localPort = 11000; // mozna przekazywac jako argument 
            int remotePort = 11001;
            //System.Threading.Thread.Sleep(1000);
            UdpClient udpClient = new UdpClient(localPort);
            Stopwatch stopWatch = new Stopwatch();


            try
            {
                string message = Client.ExecuteCommand();
                byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                udpClient.Connect("localhost", remotePort);
                stopWatch.Start();
                udpClient.Send(sendBytes, sendBytes.Length);

                IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = udpClient.Receive(ref RemoteIPEndPoint);
                stopWatch.Stop();
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                Console.WriteLine("Download: {0}", returnData);
                Console.WriteLine("Time elapsed: {0}", stopWatch.Elapsed);
                Console.WriteLine("This message was sent from {0}:{1}",RemoteIPEndPoint.Address,RemoteIPEndPoint.Port);
                Console.WriteLine();

                udpClient.Close();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
