using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class Chat
    {
        //private static Dictionary<int, string> myIdBase = new Dictionary<int, string>(); // moje ID
        private static List<int> myIdBase = new List<int>();

        private static Dictionary<int, string> otherIdBase = new Dictionary<int, string>(); // ID rozmówcy 
        private static Dictionary<int, string> userMessageBase = new Dictionary<int, string>(); // wiadomosc
        private static List<int> idBase = new List<int>();

        //private object lockMessage = new object();

        public static string Messenger(string message) 
        {
            string[] obj = message.Split(' ');

            if (message == null)
                return "Please type your message!";

            switch (obj[1])// chat[0] send[1] mój_id[2] id_rozmówcy[3] wiadomosc[4]
            {
                case "send":
                    return SendMessage(obj[2], obj[3], obj[4]);

                case "get":
                    return ReceiveMessage(obj[2]);

                default:
                    return "(chat) Incorrect option!";
                   
            }

            //chat send mój_id id_rozmówcy wiadomosc
            //serwer: chat ok
            //zapytanie o wiadomosc chat receive msg list


        }

        static string SendMessage(string myId, string otherId,string userMessage) 
        {
            otherIdBase.Add(Convert.ToInt32(otherId), myId);
            userMessageBase.Add(Convert.ToInt32(otherId),userMessage);
            Console.WriteLine("(chat) OK");
            return "You send a message!";
        }

        static string ReceiveMessage(string myId) 
        {
            string data = null;

            if(!userMessageBase.Any(x => x.Key == Convert.ToInt32(myId)))
                return "There is no message!";

            var message = userMessageBase.First(x => x.Key == Convert.ToInt32(myId));
            var user = otherIdBase.First(x => x.Key == Convert.ToInt32(myId));

            data += user.Value + " send: " + message.Value;

            userMessageBase.Remove(Convert.ToInt32(myId));
            otherIdBase.Remove(Convert.ToInt32(myId));

            return data;
        }
    }
}
