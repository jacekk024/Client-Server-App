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
        //static string path = @"D:\dokumenty\C#\Communication";
        //static bool waitForRead = true;
        public static void StartFile()
        {
            //klient odczytuje komende
            //FileSystemWatcher clientWatcher = new FileSystemWatcher(path);
            //clientWatcher.Changed += ClientWatcherCreated;
            //clientWatcher.Filter = " *.out";
            //clientWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.DirectoryName | NotifyFilters.FileName;

            //clientWatcher.EnableRaisingEvents = true;
            //clientWatcher.IncludeSubdirectories = true;

            Stopwatch stopWatch = new Stopwatch();
            string path = @"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Client-Server-App\Communication";
            bool waitForRead = true;

            try
            {

                string command = Client.ExecuteCommand();
                stopWatch.Start();
                using (StreamWriter sr = new StreamWriter(path + @"\question.in"))
                {
                    sr.WriteLine(command);
                }
                while (!IsFileLocked(new FileInfo(path + @"\question.out")) && !IsFileLocked(new FileInfo(path + @"\question.in")) && waitForRead)
                {
                    //Task.Delay(1000);

                    using (var fs = new FileStream(path + @"\question.out", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string line;
                        Console.Write("- File - Download: ");
                        while ((line = sr.ReadLine()) != null)
                            Console.WriteLine(line);
                        stopWatch.Stop();
                        Console.WriteLine("Time elapsed : {0}", stopWatch.Elapsed);
                        waitForRead = false;
                    }
                   // Task.Delay(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.ToString());
            }
        }

        static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            //file is not locked
            return false;
        }

     
    }
}
