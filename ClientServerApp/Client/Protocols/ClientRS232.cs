using Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace Client.Protocols
{
    class ClientRS232
    {
        public static void StartRS232() 
        {
            Stopwatch stopWatch = new Stopwatch();
            SerialPort serial = new SerialPort("COM2", 9600, Parity.None, 8, StopBits.One);
            
            try
            {
                string command = Client.ExecuteCommand();

                stopWatch.Start();
                serial.Open();
                
                serial.WriteLine(command);
                stopWatch.Stop();
                if(command.Contains("help") || command.Contains("conf"))
                    serial.NewLine = "\r\n";
                else
                    serial.NewLine = "\n";

                string returnMessage = serial.ReadLine();

                Console.WriteLine("Download: {0}", returnMessage);
                if (command.Split()[0].Contains("ping")) Console.WriteLine("Time elapsed: {0}", stopWatch.Elapsed);
                Console.WriteLine();

                serial.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
