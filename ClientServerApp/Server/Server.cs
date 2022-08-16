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

        List<ICommunicator> communicators = new List<ICommunicator>(); // collection answerers

        List<IListner> listners = new List<IListner>(); //collection listeners

        Dictionary<string, IServiceModule> services = new Dictionary<string, IServiceModule>();

        private object lockListener = new object();
        private object lockComunnicator = new object();


        public void AddServiceModule(string name, IServiceModule service) 
        {
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
            lock (lockListener)
            {
                Task.Run(() => listners.Add(listner)); // w osobnym watku
                listner.Start(new CommunicatorD(AddCommunicator));
            }
            Console.WriteLine("- SERVER - Listener Added!");
        }

        public void RemoveCommunicator(ICommunicator communicator)
        {
            // wybor listnera


            communicators.Remove(communicator);//usuwanie z listu communicators ?? 
        }

        public void RemoveListener(IListner listner) 
        {


            Console.WriteLine("- SERVER - Listener Removed!");
        }

        public string Answer(string command)
        {
            if (command != null)
            {
                string serviceName = command.Split()[0];
                if (services.ContainsKey(serviceName)) return services[serviceName].AnswerCommand(command);
                return "Services is unavailable.";
            }
            return "Command was null!";
        }
            
        public void Start() 
        {
            foreach (var listener in listners) // dodac startowanie komunikatorow bo teraz nic nie startuje tego TCP! DAJESZ ZROB TEN PROJEKT NYGUSIE !!!!
                listener.Start(new CommunicatorD(AddCommunicator));
        }
        public void Stop()
        {
            foreach (var listener in listners)
                listener.Stop();

            foreach (var communicator in communicators)
                communicator.Stop();

            listners.Clear();
            communicators.Clear();
            services.Clear();
        }

        void TaskDelay() // kluczowe jesli serwer ma chodzic i zeby dodac listnery w nowych watkach 
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

            server.Start();

            server.AddListner(new TCPListener(12345));
            server.AddListner(new UDPListner(11001));
            //server.AddListner(new RS232Listner("COM3", 9600, Parity.None, 8, StopBits.One));
            server.AddListner(new NETRemotingListner(8085));
            server.AddListner(new FileListner(@"D:\dokumenty\C#\Communication"));

            //oczekiwanie na zamkniecie wszystkich uslug 
            //dodac pomiar czasu przesylu pakietu 
            server.TaskDelay();

            server.Stop();
        }

    }
}
