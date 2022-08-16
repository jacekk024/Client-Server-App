using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FileCommunicator : ICommunicator
    {
        readonly string readFilePath;
        //bool shoulTerminate = false;

        public FileCommunicator(string readFilePath) => this.readFilePath = readFilePath;

      


        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            try
            {
                string data;
                // while (!shoulTerminate) 
                // {
                using (StreamReader sr = new StreamReader(new FileStream(readFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    string writeFilePath = readFilePath.Replace(".in", ".out");
                    //Console.WriteLine(sr.ReadLine());
                    using (var fs = new FileStream(writeFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(writeFilePath))/// <---- tu problem 
                    {
                        data = sr.ReadLine();
                        //Console.WriteLine(data);
                        string message = onCommand(data);
                        // while (sr.EndOfStream)
                        //{
                        // data =  sr.ReadLine();
                        //string message = onCommand(data);
                        Console.WriteLine(message);
                        sw.WriteLine(message);
                        //}
                    }
                }
                //}
            }
            catch (Exception )
            {
                onDisconnect(this);
              //  Console.WriteLine("The process failed {0}", e.ToString());
            }
        }

        public void Stop()
        {
            //shoulTerminate = true;
            Console.WriteLine("- File - File Disconnected!");
        }
    }

    class FileListner : IListner
    {
        FileSystemWatcher serverWatcher;
        public string path;
        CommunicatorD onConnect;

        public FileListner(string path)
        {
            this.path = path;
            serverWatcher = new FileSystemWatcher(path);
        }



        public void Start(CommunicatorD onConnect)
        {
            this.onConnect = onConnect;
            serverWatcher.Changed += ServerWatcherChanged;
            serverWatcher.Filter = "*.in";
            serverWatcher.NotifyFilter = NotifyFilters.LastWrite |
                                         NotifyFilters.LastAccess |
                                         NotifyFilters.FileName |
                                         NotifyFilters.DirectoryName; // co dokladnie obserwujemy 
            serverWatcher.EnableRaisingEvents = true;
            serverWatcher.IncludeSubdirectories = true;


            //uruchom nasluch w osobnym watku lub ustaw file system watcher
            //Task.Run(() => ServerWatcherCreated(this.onConnect));
        }

        private void ServerWatcherChanged(object sender, FileSystemEventArgs e)
        {

            //if (!FileIsReady(path + @"\question.in"))
            // {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                onConnect(new FileCommunicator(e.FullPath));
            }

            Console.WriteLine($"Changed: {e.FullPath}");
            //}
        }

        private static bool FileIsReady(string path)
        {
            //One exception per file rather than several like in the polling pattern
            try
            {
                //If we can't open the file, it's still copying
                using (var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }



        public void Stop()
        {
            Console.WriteLine("- FILE - Listnere closed!");
            serverWatcher.Dispose();
        }
    }

}
