using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels;
using Commands;

namespace Server
{

    class NETRemotingCommunicator : ICommunicator
    {

        private TcpChannel serverChannel;
        CommunicatorD onDisconnect;
       
        public NETRemotingCommunicator(TcpChannel serverChannel) 
        {
            this.serverChannel = serverChannel;
        }

        public void Run(CommandD onCommand, CommunicatorD onDisconnect) 
        {
            try
            {    
                RemoteObject.onCommand += new RemoteObject.CommandD(onCommand);                
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemoteObject), "RemoteObject", WellKnownObjectMode.Singleton);
                //ShowServerConfiguration();
            }
            catch (Exception e)
            {               
                Console.WriteLine(e.Message);
            }
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onDisconnect = onDisconnect;

           // serverChannel = new TcpChannel(port);
           // ChannelServices.RegisterChannel(serverChannel, false);

          //  RemoteObject.onCommand += new RemoteObject.CommandD(onCommand);
           // RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemoteObject), "RemoteObject", WellKnownObjectMode.Singleton);

            Task.Run(() => Run(onCommand,onDisconnect));
        }

        public void Stop()
        {
            ChannelServices.UnregisterChannel(serverChannel);
            Console.WriteLine("(.NET Remoting) Communicator stopped");
            onDisconnect(this);
        }

        private static void ShowServerConfiguration() 
        {
            IChannel[] channels = ChannelServices.RegisteredChannels; //pobieranie wszystkich zarejestrowanych kanałów
            foreach (var channel in channels)
            {
                ShowChannelInfo(channel);
                Console.WriteLine();
            }
            WellKnownServiceTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownServiceTypes();
            foreach (var entry in entries)
                Console.WriteLine("Object: {0}", entry.ObjectUri);
        }

        private static void ShowChannelInfo(IChannel serverChannel) 
        {
            Console.WriteLine("The name of the channel is {0}", serverChannel.ChannelName);
            Console.WriteLine("The priority of the channel is {0}", serverChannel.ChannelPriority);
            ChannelDataStore data = null;

            if (serverChannel is IChannelReceiver)
                data = (ChannelDataStore)((IChannelReceiver)serverChannel).ChannelData;
            if (data != null)
                foreach (string uri in data.ChannelUris)
                    Console.WriteLine("The channel URI is {0}", uri);
        }
    }


    class NETRemotingListner : IListner
    {

        private int port;
        TcpChannel serverChannel;

        public NETRemotingListner(int port) 
        {
            this.port = port;
        }

        //[Obsolete]
        public void Start(CommunicatorD onConnect)
        {
            serverChannel = new TcpChannel(port);
            ChannelServices.RegisterChannel(serverChannel,false);
            onConnect(new NETRemotingCommunicator(serverChannel));
        }

        public void Stop()
        {
            Console.WriteLine("(.NET Remoting) Listner Stopped");
        }
    }
}
