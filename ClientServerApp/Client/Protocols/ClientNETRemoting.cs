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
            
            TcpChannel clientChannel = new TcpChannel();
            Stopwatch stopWatch = new Stopwatch();
            string command = Client.ExecuteCommand();

            try
            {
                if(ChannelServices.GetChannel(clientChannel.ChannelName) == null) //sprawdzamy czy dany kanal jest zarejestrowany - obiekt ten moze byc zarejestrowany tylko raz
                    ChannelServices.RegisterChannel(clientChannel, false);    

                stopWatch.Start();
                WellKnownClientTypeEntry remoteType = RemotingConfiguration.IsWellKnownClientType(typeof(RemoteObject));

                // takie rozwiazanie zapobiega wystapieniu tego wyjatku:
                // Nastąpiła próba przekierowania aktywacji typu 'Commands.RemoteObject, Commands', która została już przekierowana.

                if (remoteType == null) 
                    RemotingConfiguration.RegisterWellKnownClientType(typeof(RemoteObject), $"tcp://localhost:8082/RemoteObject"); 

                RemoteObject obj = new RemoteObject();

                Console.WriteLine(obj.AnswerCommand(command));
                if (command.Split()[0].Contains("ping")) Console.WriteLine("Time elapsed : {0}", stopWatch.Elapsed);

            }
            catch (RemotingException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
