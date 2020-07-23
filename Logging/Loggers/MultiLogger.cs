#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace RadLibrary.Logging.Loggers
{
    /// <summary>
    ///     RadLoggerBase that does nothing by itself, but if you pass some loggers in arguments, it'll log in them. Arguments:
    ///     pass all loggers that you want to log in.
    ///     If you want to change settings, it'll change settings of all loggers that you passed in.
    /// </summary>
    public class MultiLogger : RadLoggerBase
    {
        private IEnumerable<LoggerBase> _loggers;

        /// <inheritdoc />
        public sealed override LoggerSettings Settings
        {
            get => base.Settings;
            set
            {
                base.Settings = value;

                if (_loggers == null) return;

                foreach (var logger in _loggers) logger.Settings = value;
            }
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            var settings = Settings as MultiLoggerSettings;

            if (settings == null || settings.Loggers?.Any() == false)
                throw new ArgumentException("No loggers provided");

            _loggers = settings.Loggers;
        }

        /// <inheritdoc />
        internal override void Log(LogType type, string message, string formatted)
        {
            foreach (var logger in _loggers) logger?.DirectLog(type, formatted);
        }
    }

    /// <summary>
    ///     The multi logger settings
    /// </summary>
    public class MultiLoggerSettings : LoggerSettings
    {
        public IEnumerable<LoggerBase> Loggers;

        public MultiLoggerSettings()
        {
        }

        public MultiLoggerSettings(params LoggerBase[] loggers)
        {
            Loggers = loggers;
        }
    }
}