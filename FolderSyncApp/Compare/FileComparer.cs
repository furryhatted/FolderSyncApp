using System.IO;

namespace FolderSyncApp.Compare {
    internal sealed class FileComparer : GenericFileComparer<FileInfo> {
        public override bool Equals(FileInfo firstFile, FileInfo secondFile) =>
            firstFile != null &&
            secondFile != null &&
            GetRelativeName(firstFile.FullName) == GetRelativeName(secondFile.FullName) &&
            firstFile.Length == secondFile.Length &&
            firstFile.LastWriteTime == secondFile.LastWriteTime;

        public override int GetHashCode(FileInfo fileInfo) =>
            $"{fileInfo.Name}{fileInfo.LastWriteTime.ToLongTimeString()}{fileInfo.Length.ToString()}".GetHashCode();

        public FileComparer(string[] baseUrls) : base(baseUrls) { }
    }
}