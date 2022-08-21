using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


namespace Server
{
    class RS232Communicator : ICommunicator
    {
        private SerialPort client;
        private CommunicatorD onDisconnect;
        public RS232Communicator(SerialPort client) 
        {
            this.client = client;
        }


        public void Run(CommandD onCommand, CommunicatorD onDisconnect) 
        {
            string message;
            client.Open();         
            try
            {
                while (true)
                {
                    message = client.ReadLine();
                    Console.WriteLine("(RS-232) Download: {0}", message);
                    string command = onCommand(message);
                    client.WriteLine(command);
                    Console.WriteLine("(RS-232) Send: {0}", command);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                onDisconnect(this);
            }
        
        }
        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onDisconnect = onDisconnect;
            Task.Run(() => Run(onCommand, onDisconnect));
        }

        public void Stop()
        {

            client.Close();
            onDisconnect(this);
            Console.WriteLine("(RS-232) Communicator stopped");
        }
    }
    class RS232Listner : IListner
    {
        private string portName;
        private int baudRate;
        private int dataBits;
        private Parity parityBits;
        private StopBits stopBits;

        public RS232Listner(string portName, int baudRate, Parity parityBits, int dataBits, StopBits stopBits)
        {
            this.portName = portName;
            this.baudRate = baudRate;
            this.parityBits = parityBits;
            this.dataBits = dataBits;
            this.stopBits = stopBits;
        }

        public void Start(CommunicatorD onConnect)
        {
            SerialPort client = new SerialPort(portName, baudRate, parityBits, dataBits, stopBits);
            onConnect(new RS232Communicator(client));
        }

        public void Stop()
        {
            Console.WriteLine("(RS-232) Listner stopped");
        }
    }
}
