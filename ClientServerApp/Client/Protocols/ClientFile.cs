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

        public static async void StartFile()
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
                    await sr.WriteLineAsync(command);
                }
                while (!IsFileLocked(new FileInfo(path + @"\question.out")) && !IsFileLocked(new FileInfo(path + @"\question.in")) && waitForRead)
                {
                    //Task.Delay(1000);

                    using (var fs = new FileStream(path + @"\question.out", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string line;
                        Console.Write("- File - Download: ");
                        while ((line = await sr.ReadLineAsync()) != null)
                            Console.WriteLine(line);
                        stopWatch.Stop();
                        if(command.Split()[0].Contains("ping"))Console.WriteLine("Time elapsed : {0}", stopWatch.Elapsed);
                        waitForRead = false;
                    }
                   // Task.Delay(1000);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("The file could not be read:");
               // Console.WriteLine(e.ToString());
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

        //public void Start(CommunicatorD onConnect)
        //{
        //    this.onConnect = onConnect;
        //    serverWatcher.Changed += ServerWatcherChanged;
        //    serverWatcher.Filter = "*.in";
        //    serverWatcher.NotifyFilter = NotifyFilters.LastWrite |
        //                                 NotifyFilters.LastAccess |
        //                                 NotifyFilters.FileName |
        //                                 NotifyFilters.DirectoryName; // co dokladnie obserwujemy 
        //    serverWatcher.EnableRaisingEvents = true;
        //    serverWatcher.IncludeSubdirectories = true;
        //}

        //private void ServerWatcherChanged(object sender, FileSystemEventArgs e)
        //{

        //    if (e.ChangeType == WatcherChangeTypes.Changed)
        //    {
        //        onConnect(new FileCommunicator(e.FullPath));
        //    }
        //    Console.WriteLine($"Changed: {e.FullPath}");
        //}


    }
}
