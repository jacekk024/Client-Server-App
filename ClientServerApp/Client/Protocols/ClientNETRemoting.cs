using System;
using System.Collections.Generic;
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
        public delegate string CommandD(string command);
        public static void StartNETRemoting() 
        {

            try
            {
                TcpChannel clientChannel = new TcpChannel(); // w kliencie mozna konstruktor bez argumentowy 
                ChannelServices.RegisterChannel(clientChannel, true);

                WellKnownClientTypeEntry remoteType = new WellKnownClientTypeEntry(
                    typeof(RemoteObject), "tcp://localhost:8085/RemoteObject");

                RemotingConfiguration.RegisterWellKnownClientType(remoteType);
                string command = Client.ExecuteCommand();

                RemoteObject obj = new RemoteObject(command);
                //var obj = (RemoteObject)Activator.GetObject(typeof(RemoteObject),
                //            "tcp://localhost:8085/RemoteObject");

                Console.WriteLine(obj.AnswerCommand(command));   
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static string Answer(string command) 
        {
            return command;
        }

    }
}
