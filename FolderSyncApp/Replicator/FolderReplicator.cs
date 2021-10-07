using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FolderSyncApp.Compare;
using FolderSyncApp.Logger;
using FolderSyncApp.Wrapper;

namespace FolderSyncApp.Replicator {
    public class FolderReplicator {
        private readonly int _syncInterval;
        private readonly SimpleLogger _logger;
        private readonly DirectoryInfoWrapper _sourceDir;
        private readonly DirectoryInfoWrapper _replicaDir;
        private readonly FileComparer _fileComparer;
        private readonly DirectoryComparer _directoryComparer;

        public FolderReplicator(ReplicationOptions options) {
            _sourceDir = new DirectoryInfoWrapper(options.SourcePath, true);
            _replicaDir = new DirectoryInfoWrapper(options.ReplicaPath);
            var baseUrls = new[] {options.SourcePath, options.ReplicaPath};
            _fileComparer = new FileComparer(baseUrls);
            _directoryComparer = new DirectoryComparer(baseUrls);
            _logger = new SimpleLogger(options.LogPath);
            _syncInterval = options.SyncInterval * 1000;
        }

        private bool IsEqual =>
            _sourceDir.SubDirectories().SequenceEqual(_replicaDir.SubDirectories(), _directoryComparer) &&
            _sourceDir.SubFiles().SequenceEqual(_replicaDir.SubFiles(), _fileComparer);

        private IEnumerable<DirectoryInfo> GetObsoleteDirectories() =>
            _replicaDir.SubDirectories()
                .Except(_sourceDir.SubDirectories(), _directoryComparer)
                .Reverse()
                .ToArray();

        private IEnumerable<DirectoryInfo> GetNewDirectories() =>
            _sourceDir.SubDirectories()
                .Except(_replicaDir.SubDirectories(), _directoryComparer)
                .ToArray();

        private IEnumerable<FileInfo> GetObsoleteFiles() =>
            _replicaDir.SubFiles()
                .Except(_sourceDir.SubFiles(), _fileComparer)
                .ToArray();

        private IEnumerable<FileInfo> GetNewFiles() =>
            _sourceDir.SubFiles()
                .Except(_replicaDir.SubFiles(), _fileComparer)
                .ToArray();

        private void RemoveObsoleteFiles() {
            foreach (var v in GetObsoleteFiles()) {
                v.Delete();
                _logger.Write($"-{v.FullName}");
            }
        }

        private void RemoveObsoleteDirectories() {
            foreach (var v in GetObsoleteDirectories()) {
                v.Delete();
                _logger.Write($"-{v.FullName}");
            }
        }

        private void CreateNewDirectories() {
            foreach (var v in GetNewDirectories()) {
                var itemName = v.FullName.Replace(_sourceDir.Path, _replicaDir.Path);
                new DirectoryInfo(itemName).Create();
                _logger.Write($"+{itemName}");
            }
        }

        private void CreateNewFiles() {
            foreach (var v in GetNewFiles()) {
                var itemName = v.FullName.Replace(_sourceDir.Path, _replicaDir.Path);
                v.CopyTo(itemName, true);
                _logger.Write($"+{itemName}");
            }
        }

        private void ExecSync() {
            Task.Run(delegate {
                RemoveObsoleteFiles();
                RemoveObsoleteDirectories();
            });
            Task.Run(delegate {
                CreateNewDirectories();
                CreateNewFiles();
            });
        }

        public void Run() {
            while (true) {
                ExecSync();
                Thread.Sleep(_syncInterval);
            }
        }

        [Obsolete("Use only for debug purpose")]
        public void ExecCompare() {
            Console.WriteLine($"Comparing source {_sourceDir.Path} with {_replicaDir.Path}...");
            if (!IsEqual) {
                foreach (var dir in GetObsoleteDirectories()) {
                    Console.WriteLine($"Obsolete folder {dir.FullName}");
                }

                foreach (var dir in GetNewDirectories()) {
                    Console.WriteLine($"New folder {dir.FullName}");
                }

                foreach (var file in GetObsoleteFiles()) {
                    Console.WriteLine($"Obsolete file {file.FullName}");
                }

                foreach (var file in GetNewDirectories()) {
                    Console.WriteLine($"New file {file.FullName}");
                }
            }
            else {
                Console.WriteLine("Replica folder is up-to-date...");
            }
        }
    }
}