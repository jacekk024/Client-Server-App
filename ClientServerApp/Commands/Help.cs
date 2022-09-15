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
            Console.WriteLine("please type service you need:\n" +
                "default\nping\nchat\nconf\nfile\n");
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
                    case "file":
                        return "help file";
                    default:
                        Console.WriteLine("(help) Wrong option! Choose again.");
                        obj = Console.ReadLine();
                        obj.Trim('\n', ' ');
                        break;
                }
            }
        }

        public static string Default()
        {
            return "\n(help) To communicate with server use should choose type of communicator and service that you want use.\n" +
                "ping - use to measure speed of connection\n" +
                "chat - basic communication between clients\n" +
                "conf - configuration of server`s API\n" +
                "help - gives information about available services\n" +
                "file - file transfer- downloand/send data to binary file\r\n";
        }

        public static string Ping()
        {
            return "\n(help) [ping] [trash length] [message]\n" + "This function allows to measure speed of specific communicator such as:\n" +
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
                "send: [ping] [] []; response: [pong] [10-length trash] [message!]\r\n";
        }

        public static string Chat()
        {
            return "(help) [chat] [option-to-use] [my-id] [id-send-to] [message]\n" + "Simple chat to communicate with other clients.\n" +
                "[chat] [send] [my-id] [id-send-to] [message] - Send message to specific client.\n" +
                "[chat] [get] [my-id] - Get message sent to specific client.\n" +
                "[chat] [who] - Show list of available users which have message to read.\r\n";
        }

        public static string File() 
        {
            return "(help) [file] [option-to-use] [filename] [data]\n" + "This service allows send/read data to binary file and show list of files in server base.\n" +
                "[file] [dir] - show list of available files in server,\n" +
                "[file] [put] [filename] [data] - put binary data in existing file or create file with specific binary data,\n" +
                "[file] [get] [filename] - get data from binary file- encrypted as text\r\n";
        }


        public static string Conf()
        {
            return "\n(help) [conf] [add]/[remove]/[infoservice]/ [listener]/[service]\n" + "This method allows to configure services and media of server." +
                "\nExamples of configuration:\n" +
                "conf addservice ping/chat/help/conf\n" +
                "conf removeservice ping/chat/help/conf/file\n" +
                "conf addlistener tcplistener adress port\n" +
                "conf addlistener udplistener port\r\n";
        }
    }
}
