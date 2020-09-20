﻿#region

using System;
using System.IO;
using System.Text;

#endregion

namespace RadLibrary.Logging.Loggers
{
    /// <summary>
    ///     Logger that prints logs in file. Arguments: filename (opt.), FileMode (opt.)
    /// </summary>
    public class FileLogger : RadLoggerBase, IDisposable
    {
        private FileStream _fileStream;

        /// <summary>
        ///     Disposes logger
        /// </summary>
        public void Dispose()
        {
            _fileStream?.Dispose();
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            var settings = Settings as FileLoggerSettings;

            var name = DateTime.Now.ToString("HH_mm_") + Settings.Name + ".txt";

            if (settings == null)
                _fileStream = new FileStream(name, FileMode.Append);
            else
                _fileStream = new FileStream(settings.FileName ?? name, settings.FileMode);

            var startLog =
                Encoding.UTF8.GetBytes("\n\nLog started at: " + DateTime.Now.ToString(Settings.TimeFormat) + "\n\n\n");

            _fileStream.Write(startLog, 0, startLog.Length);
        }

        /// <inheritdoc />
        protected override void Log(LogType type, string message, string formatted)
        {
            var bytes = Encoding.UTF8.GetBytes(formatted + Environment.NewLine);
            _fileStream.Write(bytes, 0, bytes.Length);
            _fileStream.Flush();
        }
    }

    /// <summary>
    ///     The file logger settings
    /// </summary>
    public class FileLoggerSettings : LoggerSettings
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