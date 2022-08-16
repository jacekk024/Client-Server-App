using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class RemoteObject : MarshalByRefObject
    {
        public delegate string CommandD(string command);
        private CommandD onCommand;
        public RemoteObject(CommandD onCommand) => this.onCommand = onCommand;
      
        public string AnswerCommand(string command) 
        {
            string answer = onCommand(command);
            Console.WriteLine(answer);
            return answer;
        }
    }
}
