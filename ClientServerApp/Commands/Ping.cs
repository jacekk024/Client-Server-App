using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Commands
{
    public class Ping
    {
        public static string PingExecute(string command)
        {
            var obj = command.Trim(' '); // podzial komendy np ping 20 z wykluczeniem spacji 
            string[] tab = obj.Split();

            if (tab.Length < 3) // jesli ping i jeden argument 
            {
                if (tab.Length < 2 ) // jesli sam ping
                {
                    Console.WriteLine("(ping) Send default message!");
                    string[] temp = new string[] { tab[0], Convert.ToString(10), "message!" };
                    string response = String.Join(" ", temp);
                    return $"{response.Split()[0]} {response.Split()[1]} {response.Split()[2]}";
                }
                else
                {
                    if (int.TryParse(tab[1], out int value))// jesli ping 20 - bez podania wiadomosci 
                    {
                        string[] temp = new string[] { tab[0], tab[1], "message!" };
                        string response = String.Join(" ", temp);
                        return $"{response.Split()[0]} {response.Split()[1]} {response.Split()[2]}";
                    }
                    else // jesli ping abcd - bez podania dlugosci spamu
                    {
                        string[] temp = new string[] { tab[0], Convert.ToString(10), tab[1] };
                        string response = String.Join(" ", temp);
                        return $"{response.Split()[0]} {response.Split()[1]} {response.Split()[2]}";
                    }
                }
            }
            else if(tab.Length > 3)// jesli ping 20 abcd cbda cdek - za duzo argumentow
            {
                Console.WriteLine("(ping) Too many arguments!");
                Console.WriteLine("(ping) Send default message!");

                tab[1] = Convert.ToString(10);
                tab[2] = "message!";
                return $"{tab[0]} {tab[1]} {tab[2]}";
            }
            else 
            { // jesli ping 20 abcd || ping abcd 20 - trzy argumenty w dowolnej kolejnosci

                if (int.TryParse(tab[1], out int value)) // jesli ping 20 abcd 
                {
                    string[] temp = new string[] { tab[0], tab[1], tab[2] };
                    string response = String.Join(" ", temp);
                    return $"{response.Split()[0]} {response.Split()[1]} {response.Split()[2]}";
                }
                else // ping abcd 20 
                {
                    string[] temp = new string[] { tab[0], tab[2], tab[1] };
                    string response = String.Join(" ", temp);
                    return $"{response.Split()[0]} {response.Split()[1]} {response.Split()[2]}";
                }
            }
        }

        public static string Pong(string command) 
        {
            string[] tab = command.Split();

            return $"(ping) pong {GenerateRand(int.Parse(tab[1]))} :: {tab[2]}";
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
