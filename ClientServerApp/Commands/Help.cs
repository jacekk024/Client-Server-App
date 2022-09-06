using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class Help
    {
        public static string HelpExecute(string command) 
        {
            Console.WriteLine("Which service description you need?\n" +
                "*default \n*ping \n*chat \n*conf\n");
            command = Console.ReadLine();
            var obj = command.Trim('\n',' ');
            while (true) // serwer powinien odsylac odpowiednie odpowiedzi
            {
                switch (obj)
                {
                    case "default":
                        return "help deafult";
                    case "ping":
                        return "help ping";
                    case "chat":
                        return "help chat";
                    case "conf":
                        return "help conf";
                    default:
                        Console.WriteLine("Wrong option! Choose again.");
                        obj = Console.ReadLine();
                        obj.Trim('\n', ' ');
                        break;
                }
            }
        }

        public static string Default()
        {
            return "\nTo communicate with server use should choose type of communicator and service that you want use.\n" +
                "Ping - use to measure speed of connection\n" +
                "Chat - basic communication between clients\n"+
                "Conf - configuration of server`s API\n"+
                "Help - gives information about available services\n";
        }

        public static string Ping()
        {
            return "\n [ping] [trash length] [message]\n" + "This function allows to measure speed of specific communicator such as:\n" +
                "*TCP*\n" +
                "*UDP*\n" +
                "*File communication*\n" +
                "*RS-232*\n" +
                "*.Net Remoting*\n" +
                "\nExamples of configuration ping command:\n" +
                "send: [ping] [your-length] [your-message]; response: [pong] [trash-length] [your message]\n" +
                "send: [ping] [your-length] []; response: [pong] [trash-length] [message!]\n" +
                "send: [ping] [] [your-message]; response: [pong] [10-length trash] [your message]\n" +
                "send: [ping] [] []; response: [pong] [10-length trash] [message!]\n" +
                "send: [ping] [] []; response: [pong] [10-length trash] [message!]\n";
               
        }

        public static string Chat()
        {
            return "Need to add!!!";
        }

        public static string Conf()
        {
            return "\n[conf] [add]/[remove] [listener]/[service]\n" +
                "\nExamples of configuration:\n" +
                "conf addservice ping/chat/help/conf\n" +
                "conf removeservice ping/chat/help/conf\n" +
                "conf addlistener tcplistener adress port\n" +
                "conf addlistener udplistener port";
        }
    }
}
