using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class Chat
    {

        private static Dictionary<int, string> IdBase = new Dictionary<int, string>(); // ID rozmówcy 
        private static Dictionary<int, string> userMessageBase = new Dictionary<int, string>(); // wiadomosc

        // dodac opcje kto jest dostepny
        public static string Messenger(string message) 
        {
            string[] obj = message.Split(' ');

            if (message == null)
                return "(chat) Please type your message!";

            switch (obj[1])// chat[0] send[1] mój_id[2] id_rozmówcy[3] wiadomosc[4]
            {
                case "send":
                    return SendMessage(obj[2], obj[3], obj[4]);

                case "get":
                    return ReceiveMessage(obj[2]);
                case "who":
                    return ShowUserList();
                default:
                    return "(chat) Wrong option! Choose again.";
                   
            }
            //chat send mój_id id_rozmówcy wiadomosc
            //serwer: chat ok
            //zapytanie o wiadomosc chat receive msg list
            // chat who
        }

        static string ShowUserList() 
        {
            string message = "(chat) Users:\n";

            foreach (var id in IdBase)
                message += id.Key + "\n";

            return message;
        
        }

        static string SendMessage(string myId, string otherId,string userMessage) 
        {
            IdBase.Add(Convert.ToInt32(otherId), myId);
            userMessageBase.Add(Convert.ToInt32(otherId),userMessage);
            Console.WriteLine("(chat) OK");
            return "(chat) You send a message!";
        }

        static string ReceiveMessage(string myId) 
        {
            string data = null;

            if(!userMessageBase.Any(x => x.Key == Convert.ToInt32(myId)))
                return "(chat) There is no message!";

            var message = userMessageBase.First(x => x.Key == Convert.ToInt32(myId));
            var user = IdBase.First(x => x.Key == Convert.ToInt32(myId));

            data += user.Value + " send: " + message.Value;

            userMessageBase.Remove(Convert.ToInt32(myId));
            IdBase.Remove(Convert.ToInt32(myId));

            return data;
        }
    }
}
