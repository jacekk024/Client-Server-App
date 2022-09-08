using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Server.ServerCommands
{
    class ConfCommand :IServiceModule
    {
        private Server serv;

        public ConfCommand(Server serv)
        {
            this.serv = serv;
        }

        public string AnswerCommand(string command)
        {
            Console.WriteLine(command);
            switch (command.Split()[1].Trim(' ')) 
            {
                case "addservice": // zwrcamy informacje i wykonujemy funckje usuniecia lub dodania 
                    return AddServiceModule(command);
                case "removeservice":
                    return RemoveServiceModule(command);
                case "addlistener":
                    return AddListener(command);
                case "removelistener":
                    return RemoveListener(command);
                case "infomedia":
                    return ShowMedia();
                case "infoservice":
                    return ShowServices();
                default:
                    Console.WriteLine(command);
                    return "Incorrect configuration option!\nPlease choose again.";
            }
        }

        public string ShowServices() 
        {
            string info = null;
            if (serv.listeners != null)
            {
                info += "\n" + "Number of services:" + serv.services.Count.ToString() + "\n";
                foreach (var service in serv.services)
                    info += service.Key + "\n";
                return info+"\r\n";
            }
            return "(conf) There is no medias!\r\n";
        }

        public string ShowMedia() 
        {
            string info = null;

            if (serv.listeners != null) 
            {
                info +="\n" + "Number of listeners:" + serv.listeners.Count.ToString() + "\n";
                foreach (var listner in serv.listeners)
                    info += listner.ToString() + "\n";
                return info+"\r\n";
            }
            return "There is no medias!\r\n";
        }

        public string AddServiceModule(string command)
        {
            //ew dodac opje rozbicia komendy 

            if (command.Contains("ping"))
            {
                Console.WriteLine("(Server) Service module added!\n Service: ping");
                serv.AddServiceModule("ping",new PingCommand());
                return "Service module added!\n Service: ping";
            }
            else if (command.Contains("chat"))
            {
                Console.WriteLine("(Server) Service module added!\n Service: chat");
                serv.AddServiceModule("chat", new ChatCommand());
                return "Service module added!\n Service: chat";
            }
            else if (command.Contains("help"))
            {
                Console.WriteLine("(Server) Service module added!\n Service: help");
                serv.AddServiceModule("help", new HelpCommad());
                return "Service module added!\n Service: help";
            }
            else if (command.Contains("conf")) 
            {
                Console.WriteLine("(Server) Client doesn`t have entitlements to modify this service!\n Service: conf");
                return "You don`t have entitlements to modify this service!";
            }
            else 
            {
                Console.WriteLine("(Server) Can not add a module !");
                return "Incorrect module!";
            }
        }

        public string AddListener(string command) //dodawanie dziala
        {
            string[] parameters = command.Split();
            if (command.Contains("tcplistener"))
            {
                serv.AddListner(new TCPListener(parameters[3],  int.Parse(parameters[4])));

                return "(Server) Listener added! Listener: tcplistener";
            }
            else if (command.Contains("udplistener"))
            {
                serv.AddListner(new UDPListner(Convert.ToInt32(parameters[3])));
                return "(Server) Listener added! Listener: udpListener";
            }
            else if (command.Contains("rs232listener"))
            {
                serv.AddListner(new RS232Listner(parameters[3],
                    int.Parse(parameters[4]),
                    (Parity)Enum.Parse(typeof(Parity), parameters[5]),
                    int.Parse(parameters[6]),
                    (StopBits)Enum.Parse(typeof(StopBits), parameters[7])));
                return "(Server) Listener added! Listener: rs232Listener";
            }
            else if (command.Contains("filelistener"))
            {
                serv.AddListner(new FileListner(parameters[3]));
                return "(Server) Listener added! Listener: fileListener";
            }
            else if (command.Contains("remotinglistener"))
            {
                serv.AddListner(new NETRemotingListner(Convert.ToInt32(parameters[3])));
                return "(Server) Listener added! Listener: NETRemotingListner";
            }
            else
            {
                return "(Server) Incorrect listner!";
            }
        }

        public string RemoveServiceModule(string command)
        {
            if (command.Contains("ping"))
            {
                Console.WriteLine("(Server) Service module removed!\n Service: ping");
                serv.RemoveServiceModule("ping");
                return "Service module removed!\n Service: ping";
            }
            else if (command.Contains("chat"))
            {
                Console.WriteLine("(Server) Service module removed!\n Service: chat");
                serv.RemoveServiceModule("chat");
                return "Service module added!\n Service: chat";
            }
            else if (command.Contains("help"))
            {
                Console.WriteLine("(Server) Service module removed!\n Service: help");
                serv.RemoveServiceModule("help");
                return "Service module added!\n Service: help";
            }
            else if (command.Contains("conf"))
            {
                Console.WriteLine("(Server) Client doesn`t have entitlements to modify this service!\n Service: conf");
                return "You don`t have entitlements to modify this service!";
            }
            else
            {
                Console.WriteLine("(Server) Incorrect module!");
                return "Incorrect module!";
            }
        }

        public string RemoveListener(string command)
        {


            if (command.Contains("tcplistener"))
            {
                var listener = serv.listeners.Find(x => x.Equals( new TCPListener("127.0.0.1",12345)));

                serv.RemoveListener(listener);
                return "(Server) Listener removed! Listener: tcplistener";
            }
            else if (command.Contains("udplistener"))
            {
                serv.RemoveListener(new UDPListner(Convert.ToInt32(command.Split()[4])));
                return "(Server) Listener added! Listener: udpListener";
            }
            else if (command.Contains("rs232listener"))
            {
                serv.RemoveListener(new UDPListner(Convert.ToInt32(command.Split()[4])));
                return "(Server) Listener added! Listener: udpListener";
            }
            else if (command.Contains("filelistener"))
            {
                serv.RemoveListener(new UDPListner(Convert.ToInt32(command.Split()[4])));
                return "(Server) Listener added! Listener: udpListener";
            }
            else if (command.Contains("remotinglistener"))
            {
                serv.RemoveListener(new UDPListner(Convert.ToInt32(command.Split()[4])));
                return "(Server) Listener added! Listener: udpListener";
            }
            else
            {
                return "(Server) Incorrect listner!";
            }
        }
    }
}
