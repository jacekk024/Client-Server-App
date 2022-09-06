using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;

namespace Server.ServerCommands
{
    class PingCommand : IServiceModule
    {
        public string AnswerCommand(string command)  => Ping.Pong(command); 
    }
}
