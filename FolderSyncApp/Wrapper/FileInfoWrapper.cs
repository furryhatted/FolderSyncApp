using System.IO;
using System.Security.Cryptography;
using FolderSyncApp.Wrapper;

namespace FolderSyncApp {
    public class FileInfoWrapper : GenericWrapper<FileInfo> {
        protected override byte[] CalculateHash() {
            return MD5.Create().ComputeHash(File.ReadAllBytes(Path));
        }

        public FileInfoWrapper(FileInfo fileInfo) : base(fileInfo) { }

        public FileInfoWrapper(string filePath) : base(new FileInfo(filePath)) { }
    }
}