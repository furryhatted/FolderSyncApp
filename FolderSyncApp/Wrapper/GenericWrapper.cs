using System.IO;

namespace FolderSyncApp.Wrapper {
    public abstract class GenericWrapper<T> where T : FileSystemInfo {
        private readonly T _info;
        protected string Path => _info.FullName;

        private byte[] hash;

        public byte[] Hash => hash ??= CalculateHash();
        protected abstract byte[] CalculateHash();

        protected GenericWrapper(T info) {
            _info = info;
        }
    }
}