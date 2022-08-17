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

        public string AnswerCommand(string command)
        {
            string answer = onCommand(command);
            Console.WriteLine("-Server [.NET Remoting]- {0}", answer);
            return answer;
        }
        public void SetAnswer(CommandD onCommand) => this.onCommand = onCommand;
    }
}
