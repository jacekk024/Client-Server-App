using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Protocols
{
    class ClientTCP
    {
      

        public static void StartTCP()
        {
            try
            {
                string server = "localhost";
                TcpClient client = new TcpClient(server, 12345); // mozna dodac kontruktor w suumie
                NetworkStream stream = client.GetStream();

                Stopwatch stopWatch = new Stopwatch();


                //  while (true) // zabrane <= bedzie siedziec na pingu 
                //  {
                string command = Client.ExecuteCommand(); // <== tu// zmienic OK

                    byte[] data = System.Text.Encoding.ASCII.GetBytes(command); // <== tu zmienic// OK

                    stopWatch.Start();
                    stream.Write(data, 0, data.Length);

                    data = new byte[256];

                    string odp = string.Empty;

                    int bytes;

                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        stopWatch.Stop();
                        odp += System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    }
                    while (stream.DataAvailable); // tu zabrane 



                    Console.WriteLine("Download: {0}", odp);
                    Console.WriteLine("Time elapsed: " + stopWatch.Elapsed);
                    stream.Close();

               // }
                //stream.Close();
                //client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }


        }

    }
}
