using System;
using System.Collections.Generic;
using System.IO;

namespace FolderSyncApp.Wrapper {
    public class DirectoryInfoWrapper {
        private readonly DirectoryInfo _directoryInfo;
        public IEnumerable<FileInfo> SubFiles() => _directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

        public IEnumerable<DirectoryInfo> SubDirectories() =>
            _directoryInfo.GetDirectories("*", SearchOption.AllDirectories);

        public string Path => _directoryInfo.FullName;


        public DirectoryInfoWrapper(string path, bool shouldExist = false) {
            _directoryInfo = new DirectoryInfo(path);
            if (_directoryInfo.Exists) return;
            if (shouldExist) throw new IOException($"{path} does not exists!");
            _directoryInfo.Create();
        }
    }
}