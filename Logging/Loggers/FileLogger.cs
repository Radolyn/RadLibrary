#region

using System;
using System.IO;
using System.Text;

#endregion

namespace RadLibrary.Logging.Loggers
{
    public class FileLogger : Logger
    {
        private FileStream _fileStream;

        /// <inheritdoc />
        public override void Initialize(params object[] args)
        {
            if (args == null || args.Length == 0)
                _fileStream = new FileStream(DateTime.Now.ToString("HH_mm_") + Settings.Name + ".txt", FileMode.Append);
            else if (args.Length == 1)
                _fileStream =
                    new FileStream(args[0]?.ToString() ?? DateTime.Now.ToString("HH_mm_") + Settings.Name + ".txt",
                        FileMode.Append);
            else
                _fileStream = args[1] is FileMode
                    ? new FileStream(args[0]?.ToString() ?? DateTime.Now.ToString("HH_mm_") + Settings.Name + ".txt",
                        (FileMode) args[1])
                    : new FileStream(args[0]?.ToString() ?? DateTime.Now.ToString("HH_mm") + ".txt", FileMode.Append);

            var startLog =
                Encoding.UTF8.GetBytes("\n\nLog started at: " + DateTime.Now.ToString(Settings.TimeFormat) + "\n\n\n");
            _fileStream.Write(startLog, 0, startLog.Length);
        }

        /// <inheritdoc />
        public override void Log(LogType type, string message, string formatted)
        {
            var bytes = Encoding.UTF8.GetBytes(formatted + Environment.NewLine);
            _fileStream.Write(bytes, 0, bytes.Length);
            _fileStream.Flush();
        }
    }
}