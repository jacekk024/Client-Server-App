using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class Ping
    {
        public static string PingCom(string command)
        {
            string[] tab = command.Split();
            return $"{tab[0]} {tab[1]} {tab[2]}";
        }


        public static string Pong(string command) 
        {
            string[] tab = command.Split();

            return $"pong {GenerateRand(int.Parse(tab[1]))} :: {tab[2]}";
        }

        public static string GenerateRand(int length) 
        {
            Random randNum = new Random();
            char[] tab = new char[length];
            for (int i = 0; i < length; i++)
                tab[i] = (char)randNum.Next(48, 122);
            
            return new string(tab);
        }
    }
}
