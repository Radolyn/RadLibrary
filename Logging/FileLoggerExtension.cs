#region

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>
    ///     Log to file extension
    /// </summary>
    public class FileLoggerExtension : ILoggerExtension
    {
        private readonly FileStream _fileStream;
        private readonly Regex _regex = new Regex(@"\x1b\[((\d+;2;\d+;\d+;\d+)|(\d+))m");

        /// <summary>
        ///     Initializes file logger
        /// </summary>
        /// <param name="filename">The log filename</param>
        public FileLoggerExtension(string filename)
        {
            _fileStream = File.OpenWrite(filename);
        }

        /// <inheritdoc />
        public void Log(string message, string formatted, LoggerSettings settings)
        {
            formatted = _regex.Replace(formatted, "");
            formatted = formatted.Remove(formatted.Length - 1);
            var msg = Encoding.UTF8.GetBytes(formatted + "\n");
            _fileStream.Write(msg, 0, msg.Length);
            _fileStream.Flush();
        }
    }
}