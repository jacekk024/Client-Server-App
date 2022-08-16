﻿using System;
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

        public NETRemotingCommunicator(TcpChannel serverChannel) 
        {
            this.serverChannel = serverChannel;
        }  


        public void Run(CommandD onCommand, CommunicatorD onDisconnect) 
        {
            try
            {
                ChannelServices.RegisterChannel(serverChannel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof
                             (RemoteObject), "RemoteObject", WellKnownObjectMode.Singleton);

                RemoteObject remoteObject = new RemoteObject(new RemoteObject.CommandD(onCommand));
                RemotingServices.Marshal(remoteObject, "AnswerCommand");

                // RemoteObject remoteObject = new RemoteObject(new RemoteObject.CommandD(onCommand));
                //s remoteObject.AnswerCommand();
                //RemotingServices.Marshal(remoteObject, "AnswerCommand");


                //Console.WriteLine("Listening on {0}",
                //                  serverChannel.GetChannelUri());
                //ShowServerConfiguration();
            }
            catch (Exception e)
            {
                onDisconnect(this);
                Console.WriteLine(e.ToString());
            }
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            Task.Run(() => Run( onCommand,onDisconnect));
        }

        public void Stop()
        {
            ChannelServices.UnregisterChannel(serverChannel);
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
                Console.WriteLine("Object: {0}",entry.ObjectUri);
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


        public void Start(CommunicatorD onConnect)
        {
            serverChannel = new TcpChannel(port);
            onConnect(new NETRemotingCommunicator(serverChannel));
            Console.WriteLine("[.NET Remoting] Waiting for clients!");
        }

        public void Stop()
        {
            Console.WriteLine(" - NETRemoting - Listner Stopped");
        }
    }
}
