using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FolderSyncApp.Compare {
    internal abstract class GenericFileComparer<T> : IEqualityComparer<T> where T : FileSystemInfo {
        private readonly string[] _baseUrls;

        protected string GetRelativeName(string absoluteName) {
            var builder = new StringBuilder(absoluteName);
            foreach (var url in _baseUrls) {
                builder.Replace(url, string.Empty);
            }

            return builder.ToString();
        }

        public abstract bool Equals(T firstItem, T secondItem);

        public abstract int GetHashCode(T item);

        protected GenericFileComparer(string[] baseUrls) {
            _baseUrls = baseUrls;
        }
    }
}