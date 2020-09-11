#region

using System;
using RadLibrary.Formatting;

#endregion

namespace RadLibrary.Logging
{
    public class LoggerSettings
    {
        /// <summary>
        ///     The name max length. Can be set with <see cref="LogManager" /> before creating any loggers
        /// </summary>
        internal static int NameMaxLength = 24;

        // todo: optimize
        /// <summary>
        ///     The environment-set logging level
        /// </summary>
        internal static readonly LogType EnvironmentLoggingLevel = (LogType) Enum.Parse(typeof(LogType),
            (Enum.TryParse<LogType>(Environment.GetEnvironmentVariable("LOGGING_LEVEL"), out _)
                ? Environment.GetEnvironmentVariable("LOGGING_LEVEL")
                : "Info") ?? "Info", true);

        /// <summary>
        ///     Format json-like messages?
        /// </summary>
        public bool FormatJson = true;

        /// <summary>
        ///     The log format. Available variables: {time}, {name}, {level}, {message}
        /// </summary>
        public string LogFormat = "[{time} {name} {level}] {message}";

        /// <summary>
        ///     The logger type (use typeof(RadLoggerBase))
        /// </summary>
        public Type Logger;

        /// <summary>
        ///     The logging level
        /// </summary>
        public LogType LoggingLevel = LogType.Info;

        /// <summary>
        ///     The logger name
        /// </summary>
        public string Name;

        /// <summary>
        ///     The time format
        /// </summary>
        public string TimeFormat = "HH:mm:ss:fffff";

        /// <summary>
        ///     The maximum recursion level. Will return "..." on reaching this value
        /// </summary>
        public static int MaxRecursion
        {
            get => FormattersStorage.MaxRecursion;
            set => FormattersStorage.MaxRecursion = value;
        }
    }
}