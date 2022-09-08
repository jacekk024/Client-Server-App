using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;

namespace Server.ServerCommands
{
    class FileTransferCommand : IServiceModule
    {
        public string AnswerCommand(string command) => FileTransfer.FileTransferMenu(command);
    }
}
