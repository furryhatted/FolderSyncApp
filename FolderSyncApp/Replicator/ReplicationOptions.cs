namespace FolderSyncApp.Replicator {
    public sealed class ReplicationOptions {
        internal string LogPath { get; set; }
        internal string SourcePath { get; set; }
        internal string ReplicaPath { get; set; }
        internal int SyncInterval { get; set; }

        internal bool Valid =>
            LogPath != "" && SourcePath != "" && ReplicaPath != "";

        public ReplicationOptions(
            string sourcePath = "",
            string replicaPath = "",
            string logPath = @"./app.log",
            int syncInterval = 1000
        ) {
            LogPath = logPath;
            ReplicaPath = replicaPath;
            SourcePath = sourcePath;
            SyncInterval = syncInterval;
        }
    }
}