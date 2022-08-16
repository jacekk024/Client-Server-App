using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client.Protocols;
using Commands;

namespace Client
{
    class Client
    {
        static void Main()
        {
            Start();
        }
        static void Start() 
        {
            bool flag = true;
            int state;

            while (flag) 
            {
                Console.WriteLine("Select type of communication and command to procede");
                Console.WriteLine("1. TCP");
                Console.WriteLine("2. UDP");
                Console.WriteLine("3. File Communication");
                Console.WriteLine("4. RS232");
                Console.WriteLine("5. .NET Remoting");
                Console.WriteLine("6. Exit");
          
                while(!int.TryParse(Console.ReadLine(),out state));

                switch (state)
                {
                    case 1:
                        ClientTCP.StartTCP();
                        break;

                    case 2:
                        ClientUDP.StartUDP();
                        break;
                    case 3:
                        ClientFile.StartFile();
                        break;
                    case 4:
                        ClientRS232.StartRS232();
                        break;
                    case 5:
                        ClientNETRemoting.StartNETRemoting();
                        break;
                    case 6:
                        flag = false;
                        break;

                    default: Console.WriteLine("Choose wrong option! Choose again");
                        break;
                }
            }
        }
        public static string ExecuteCommand()
        {
            string command;
            Console.WriteLine("Give the command:");
            command = Console.ReadLine();
            while (true)
            {
                    if (command.Contains("ping"))
                        return Ping.PingCom(command);

                    else if (command.Contains("chat"))
                        return "XYZ";
                    else 
                    {
                        Console.WriteLine("Wrong Option! Choose again! \n");
                        Console.WriteLine("Give the command:");
                        command = Console.ReadLine();
                    }
            }
        }
    }
}

