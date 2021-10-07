using System;
using FolderSyncApp.Replicator;

namespace FolderSyncApp {
    internal static class App {
        private static void PrintUsage() {
            Console.WriteLine("USAGE INFO:\n" +
                              "app.exe -OPTION VALUE\n" +
                              "s, source - path to source folder (mandatory)\n" +
                              "r, replica - path to replica (mandatory)\n" +
                              "l, log - path to log (file name included, optional)\n" +
                              "i, interval - synchronization interval (optional)\n");
        }

        private static void Main(string[] args) {
            var replicatorOptions = new ReplicationOptions();

            for (var i = 0; i < args.Length; i++) {
                switch (args[i]) {
                    case "-l":
                    case "-log":
                        replicatorOptions.LogPath = args[++i];
                        break;
                    case "-s":
                    case "-source":
                        replicatorOptions.SourcePath = args[++i];
                        break;
                    case "-r":
                    case "-replica":
                        replicatorOptions.ReplicaPath = args[++i];
                        break;
                    case "-i":
                    case "-interval":
                        replicatorOptions.SyncInterval = int.Parse(args[++i]);
                        break;
                    default:
                        PrintUsage();
                        Environment.Exit(100);
                        break;
                }
            }

            if (!replicatorOptions.Valid) {
                PrintUsage();
                Console.WriteLine("Application options not set properly. Exiting...");
                Environment.Exit(100);
            }

            var replicatorThread = new FolderReplicator(replicatorOptions);
            replicatorThread.Run();
        }
    }
}