using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;

namespace Server.ServerCommands
{
    class HelpCommad : IServiceModule
    {
        public string AnswerCommand(string command)
        {
            if (command.Contains("ping"))
                return Help.Ping();
            else if (command.Contains("chat"))
                return Help.Chat();
            else if (command.Contains("conf"))
                return Help.Conf();
            else if (command.Contains("file"))
                return Help.File();
            else if (command.Contains("deafult"))
                return Help.Default();
            else
                return "Incorrect option!";
        }
    }
}
