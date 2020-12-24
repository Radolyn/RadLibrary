#region

using System;
using System.IO;

#endregion

namespace RadLibrary.Logging.Loggers
{
    /// <summary>
    ///     Logger that prints logs in file. Arguments: filename (opt.), FileMode (opt.)
    /// </summary>
    public class FileLogger : RadLoggerBase<FileLoggerSettings>, IDisposable
    {
        private StreamWriter _fileStream;

        /// <inheritdoc />
        public void Dispose()
        {
            _fileStream?.Dispose();
            _fileStream?.BaseStream.Dispose();
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            var name = DateTime.Now.ToString("HH_mm_") + Settings.Name + ".txt";

            if (Settings == null)
                _fileStream = new StreamWriter(new FileStream(name, FileMode.OpenOrCreate, FileAccess.Write,
                    FileShare.Read));
            else
                _fileStream = new StreamWriter(new FileStream(Settings.FileName ?? name, Settings.FileMode,
                    FileAccess.Write, FileShare.Read));

            _fileStream.WriteLine($"\nLog started at: {DateTime.Now}\n");
        }

        /// <inheritdoc />
        protected override void Log(LogType type, string message, string formatted)
        {
            _fileStream.WriteLine(formatted);
            _fileStream.Flush();
        }
    }

    /// <summary>
    ///     The file logger settings
    /// </summary>
    public sealed class FileLoggerSettings : LoggerSettings
    {
        public readonly FileMode FileMode = FileMode.Append;
        public readonly string FileName;

        public FileLoggerSettings()
        {
        }

        public FileLoggerSettings(string fileName)
        {
            FileName = fileName;
        }

        public FileLoggerSettings(string fileName, FileMode fileMode)
        {
            FileName = fileName;
            FileMode = fileMode;
        }
    }
}