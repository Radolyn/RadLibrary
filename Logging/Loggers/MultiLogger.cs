#region

using System;
using System.Linq;

#endregion

namespace RadLibrary.Logging.Loggers
{
    /// <summary>
    /// Logger that does nothing by itself, but if you pass some loggers in arguments, it'll log in them. Arguments: pass all loggers that you want to log in.
    /// If you want to change settings, it'll change settings of all loggers that you passed in.
    /// </summary>
    public class MultiLogger : Logger
    {
        private Logger[] _loggers;

        /// <inheritdoc />
        public override LoggerSettings Settings
        {
            get => base.Settings;
            set
            {
                if (_loggers == null)
                {
                    base.Settings = value;
                    return;
                }

                foreach (var logger in _loggers)
                {
                    logger.Settings = value;
                }
            }
        }

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