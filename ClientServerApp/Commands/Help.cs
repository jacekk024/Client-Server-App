using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class Help
    {
        static string ShowCommands() 
        {

            while (true) 
            {
                Console.WriteLine("Choose ");
                string command = Console.ReadLine();
                var obj = command.Trim(' ');
                switch (obj)
                {
                    case "default":
                        return Default();

                    case "ping":
                        return Ping();

                    case "chat":
                        return Chat();

                    case "conf":
                        return Conf();
                    default:
                        Console.WriteLine("Choose wrong option! Choose again");
                        break;
                }
            }
        }

        static string Default() 
        {
            return "";
        }

        static string Ping()
        {
            return "";
        }

        static string Chat()
        {
            return "";
        }

        static string Conf()
        {
            return "";
        }

    }
}
