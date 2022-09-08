using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class FileTransfer
    {
        public static string FileTransferMenu(string command) 
        {
            switch (command.Split()[1]) 
            {
                case "dir":
                    return GetDirectory(); 
                case "get":
                    return GetFile(command);
                case "put":
                    return WriteToFile(command);                               
                default:
                    return "(file) Wrong option! Choose again.";
            }
        }

        static string GetDirectory()
        {
            DirectoryInfo place = new DirectoryInfo(@"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Client-Server-App\File");
            if (place.Exists) { 
                
                FileInfo[] Files = place.GetFiles();
                string list = null;

                foreach (FileInfo i in Files)
                    list += i.Name + "\n";

                return list;
            }
            else
                return "No such directory!";     
        }
        static string GetFile(string command)
        {
            string filename = @"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Client-Server-App\File\" + command.Split()[2];

            if (File.Exists(filename))
            {
                using (FileStream sr = new FileStream(filename, FileMode.Open))
                {
                    byte[] data = new byte[sr.Length];
                    sr.Read(data, 0, (int)sr.Length);
                    string text = Encoding.ASCII.GetString(data);
                    return text;
                }
            }
            else
                return "(file) There is no such file";
        }
        static string WriteToFile(string command)
        {
            string filename = @"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Client-Server-App\File\" + command.Split()[2];
            var sb = new StringBuilder();
            for (int i = 3; i < command.Split().Length; i++)
                sb.Append(command.Split()[i] + " ");
            Console.WriteLine(sb);
            using (FileStream sw = new FileStream(filename,FileMode.Create))
            {
                byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
                sw.Write(data,0,data.Length);
                sw.Close();
                return "(file) Data saved in file!";
            }
        }
    }
}
