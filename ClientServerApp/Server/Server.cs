using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.ServerCommands;


namespace Server
{
    class Server
    {

        List<ICommunicator> communicators = new List<ICommunicator>(); // kolekcja odpowiaczy - komunikatorow

        public List<IListner> listeners = new List<IListner>(); //kolekcja nasluchiwaczy

        public Dictionary<string, IServiceModule> services = new Dictionary<string, IServiceModule>(); // slownik uslug

        private object lockListener = new object();
        private object lockComunnicator = new object();


        public void AddServiceModule(string name, IServiceModule service) 
        {
            Console.WriteLine("- SERVER - New Service Added- {0}!",name);
            services.Add(name, service);
        }

        public void AddCommunicator(ICommunicator communicator)
        {

            lock (lockComunnicator)
            {
                communicators.Add(communicator);
                communicator.Start(new CommandD(Answer), RemoveCommunicator);
            }
            Console.WriteLine("- SERVER - Communicator Added!");
        }

        public void AddListner(IListner listner)
        {
            listeners.Add(listner); // w osobnym watku
            lock (lockListener)
            {
                Task.Run(()=>listner.Start(new CommunicatorD(AddCommunicator)));
            }
            Console.WriteLine("- SERVER - Listener Added!");
        }

        public void RemoveCommunicator(ICommunicator communicator)
        {
            lock (lockComunnicator)
            {
                communicators.Remove(communicator);
            }
            Console.WriteLine("Communicator removed: {0}", communicator.ToString());
        }

        public void RemoveListener(IListner listener) //
        {
            var _listener = listeners.Find(x => x.Equals(listener));

            lock (lockListener) 
            {
                _listener.Stop();
                listeners.Remove(_listener);
            }
            Console.WriteLine("- SERVER - Listener Stopped!");
        }

        public void RemoveServiceModule(string name)
        {
            services.Remove(name);
            Console.WriteLine("- SERVER - Service Module Removed!");
        }
        public string Answer(string command)
        {
            if (command != null)
            {
                string serviceName = command.Split()[0];
                if (services.ContainsKey(serviceName)) return services[serviceName].AnswerCommand(command);
                return "Service is unavailable.";
            }
            return "Command was null!";
        }
            
        public void Start() 
        {
            foreach (var listener in listeners) 
                listener.Start(new CommunicatorD(AddCommunicator));
        }
        public void Stop()
        {
            foreach (var listener in listeners)
                listener.Stop();

            foreach (var communicator in communicators)
                communicator.Stop();

            listeners.Clear();
            communicators.Clear();
            services.Clear();
        }
        void TaskDelay() 
        {
            while (communicators.Count != 0) 
                Task.Delay(1000);
            while (services.Count != 0)
                Task.Delay(1000);
        }

        static void Main() 
        {
            Console.WriteLine("- SERVER - Running Server!");
            var server = new Server();

            server.AddServiceModule("ping", new PingCommand());
            server.AddServiceModule("chat", new ChatCommand());
            server.AddServiceModule("conf", new ConfCommand(server));
            server.AddServiceModule("help", new HelpCommad());
            server.AddServiceModule("file", new FileTransferCommand());


            server.Start();

            server.AddListner(new TCPListener("127.0.0.1",12345));
            server.AddListner(new UDPListner(11001));
            server.AddListner(new RS232Listner("COM3", 9600, Parity.None, 8, StopBits.One));
            server.AddListner(new NETRemotingListner(8082));
            server.AddListner(new FileListner(@"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Client-Server-App\Communication"));

            server.TaskDelay();
            server.Stop();

        }
    }
}
