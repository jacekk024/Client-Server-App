using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using Commands;

namespace Client.Protocols
{
    class ClientNETRemoting
    {
        public static void StartNETRemoting() 
        {
            try
            {

                Stopwatch stopWatch = new Stopwatch();

                string command = Client.ExecuteCommand();
                ChannelServices.RegisterChannel(new TcpChannel(),false);// gdy dodane true zrywa polaczenie

                stopWatch.Start();
                var obj = (RemoteObject)Activator.GetObject(typeof(RemoteObject),
                               "tcp://localhost:8082/RemoteObject");

                Console.WriteLine(obj.AnswerCommand(command));
                Console.WriteLine("Time elapsed : {0}", stopWatch.Elapsed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
