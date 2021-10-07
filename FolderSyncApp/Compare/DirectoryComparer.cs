using System.IO;

namespace FolderSyncApp.Compare {
    internal sealed class DirectoryComparer : GenericFileComparer<DirectoryInfo> {
        public override bool Equals(DirectoryInfo firstDirectory, DirectoryInfo secondDirecotry) =>
            firstDirectory != null &&
            secondDirecotry != null &&
            GetRelativeName(firstDirectory.FullName) == GetRelativeName(secondDirecotry.FullName);

        public override int GetHashCode(DirectoryInfo directoryInfo) => directoryInfo.Name.GetHashCode();

        public DirectoryComparer(string[] baseUrls) : base(baseUrls) { }
    }
}