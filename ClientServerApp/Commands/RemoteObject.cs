using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class RemoteObject : MarshalByRefObject
    {
        public static CommandD onCommand;
        public delegate string CommandD(string command);
        public string AnswerCommand(string command) => onCommand(command);
    }   
}
