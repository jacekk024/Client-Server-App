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
        CommunicatorD onDisconnect;
        public FileCommunicator(string readFilePath) => this.readFilePath = readFilePath;

        public async void Listener(CommandD onCommand, CommunicatorD onDisconnect) 
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
                            await sw.WriteLineAsync(message);
                            //}
                        }
                    
                }
                //}
            }
            catch (Exception)
            {
               // Console.WriteLine("The process failed {0}", e.ToString());
            }
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onDisconnect = onDisconnect;
            Task.Run(() => Listener(onCommand, onDisconnect));
        }

        public void Stop()
        {
            onDisconnect(this);
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
        }

        private void ServerWatcherChanged(object sender, FileSystemEventArgs e)
        {

            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                onConnect(new FileCommunicator(e.FullPath));
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        public void Stop()
        {
            Console.WriteLine("- FILE - Listnere closed!");
            serverWatcher.Dispose();
        }
    }

}
