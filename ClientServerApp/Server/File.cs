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

        public  void Listener(CommandD onCommand, CommunicatorD onDisconnect) 
        {
            try
            {
                string data;
                using (StreamReader sr = new StreamReader(new FileStream(readFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                 

                        string writeFilePath = readFilePath.Replace(".in", ".out");                
                        using (StreamWriter sw = new StreamWriter(writeFilePath))
                        {
                            data = sr.ReadLine(); // komenda miesci sie w jednej linii
                            string message = onCommand(data);
                            sw.WriteLine(message);
                        }
                    
                }
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
           // Console.WriteLine($"Changed: {e.FullPath}"); wypisywanie 
        }

        public void Stop()
        {
            Console.WriteLine("- FILE - Listnere closed!");
            serverWatcher.Dispose();
        }
    }

}
