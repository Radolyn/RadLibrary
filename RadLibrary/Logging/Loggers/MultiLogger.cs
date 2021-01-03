#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace RadLibrary.Logging.Loggers
{
    /// <summary>
    ///     Logger that does nothing by itself, but if you pass some loggers in arguments, it'll log in them. Arguments:
    ///     pass all loggers that you want to log in.
    ///     If you want to change settings, it'll change settings of all loggers that you passed in.
    /// </summary>
    public class MultiLogger : LoggerBase<MultiLoggerSettings>
    {
        private IEnumerable<LoggerBase> _loggers;

        /// <inheritdoc />
        public override void DirectLog(LogType type, string message)
        {
            foreach (var logger in _loggers) logger?.DirectLog(type, message);
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            if (Settings.Loggers?.Any() == false)
                throw new ArgumentException("No loggers provided");

            _loggers = Settings.Loggers;
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

        public MultiLoggerSettings(IEnumerable<LoggerBase> loggers)
        {
            Loggers = loggers;
        }

        /// <inheritdoc />
        public override bool FormatJson
        {
            get => base.FormatJson;
            set
            {
                base.FormatJson = value;

                if (Loggers == null) return;
                foreach (var logger in Loggers) logger.Settings.FormatJson = value;
            }
        }

        /// <inheritdoc />
        public override string LogFormat
        {
            get => base.LogFormat;
            set
            {
                base.LogFormat = value;

                if (Loggers == null) return;
                foreach (var logger in Loggers) logger.Settings.LogFormat = value;
            }
        }

        /// <inheritdoc />
        public override Type Logger { get; set; } = typeof(MultiLogger);

        /// <inheritdoc />
        public override LogType LoggingLevel
        {
            get => base.LoggingLevel;
            set
            {
                base.LoggingLevel = value;

                if (Loggers == null) return;
                foreach (var logger in Loggers) logger.Settings.LoggingLevel = value;
            }
        }

        /// <inheritdoc />
        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;

                if (Loggers == null) return;
                foreach (var logger in Loggers) logger.Settings.Name = value;
            }
        }
    }
}