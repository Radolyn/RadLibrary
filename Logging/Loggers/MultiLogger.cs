#region

using System;
using System.Linq;

#endregion

namespace RadLibrary.Logging.Loggers
{
    public class MultiLogger : Logger
    {
        private Logger[] _loggers;

        /// <inheritdoc />
        public override void Initialize(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args), "No loggers provided");

            _loggers = args.Cast<Logger>().ToArray();
        }

        /// <inheritdoc />
        public override void Log(LogType type, string message, string formatted)
        {
            foreach (var logger in _loggers) logger?.Log(type, message, formatted);
        }
    }
}