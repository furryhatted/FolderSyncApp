using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace FolderSyncApp.Logger {
    public class SimpleLogger {
        private readonly CultureInfo dateFormat = CultureInfo.CurrentCulture;
        private readonly FileInfo outputFile;

        public void Write(string message) {
            var dateTime = DateTime.Now;
            var refinedMessage =
                new StringBuilder()
                    .Append(dateTime.ToString(dateFormat))
                    .Append(' ')
                    .Append(message)
                    .Append('\n')
                    .ToString();
            File.AppendAllTextAsync(outputFile.FullName, refinedMessage);
            Console.Write(refinedMessage);
        }

        public SimpleLogger(string logPath) {
            outputFile = new FileInfo(logPath);
            if (!outputFile.Exists) outputFile.Create().Close();
            if (outputFile.IsReadOnly) throw new IOException($"File {outputFile.FullName} is in read-only mode!");
        }
    }
}