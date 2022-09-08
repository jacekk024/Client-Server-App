using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Protocols
{
    class ClientFile
    {

        private static string path;
        private static Stopwatch stopWatch;
        private static bool waitForEnd;
        private static string command;


        public static  void StartFile()
        {
            stopWatch = new Stopwatch();
            path = @"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Client-Server-App\Communication";
            FileSystemWatcher clientWatcher = new FileSystemWatcher(path);

            clientWatcher.Changed += ClientWatcherChanged;
            clientWatcher.Filter = "*.out";
            clientWatcher.NotifyFilter = NotifyFilters.LastWrite |
                                         NotifyFilters.FileName |
                                         NotifyFilters.DirectoryName; // co dokladnie obserwujemy 
            clientWatcher.EnableRaisingEvents = true;
            clientWatcher.IncludeSubdirectories = true;

            try
            {
                command = Client.ExecuteCommand();
                stopWatch.Start();
                waitForEnd = true;
                using (StreamWriter sw = new StreamWriter(path + @"\question.in"))
                {
                     sw.WriteLine(command);
                }
                while (waitForEnd);
            }
            catch (Exception)
            {
                Console.WriteLine("The file could not be read:");
               // Console.WriteLine(e.ToString());
            }
        }

        private  static void ClientWatcherChanged(object sender, FileSystemEventArgs e)
        {

            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                try
                {                  
                    using (StreamReader sr = new StreamReader(path + @"\question.out"))
                    {
                        string line;
                        Console.Write("- File - Download: ");
                        while ((line =  sr.ReadLine()) != null)
                            Console.WriteLine(line);
                        stopWatch.Stop();
                        if (command.Split()[0].Contains("ping")) Console.WriteLine("Time elapsed : {0}", stopWatch.Elapsed);
                        waitForEnd = false;
                    }
                }
                catch (Exception) 
                {
                    Console.WriteLine("Can not read file!");                
                }
            }
           // Console.WriteLine($"Changed: {e.FullPath}"); wypisywanie jaka sciezka byla zmodyfikowana
        }
    }
}
