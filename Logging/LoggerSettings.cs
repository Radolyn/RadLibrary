#region

using System;

#endregion

namespace RadLibrary.Logging
{
    public class LoggerSettings
    {
        /// <summary>
        ///     The logger type (use typeof(Logger))
        /// </summary>
        public Type Logger;

        /// <summary>
        ///     The logger initialization arguments to pass in Logger.Initialize(object[] args)
        /// </summary>
        public object[] InitializationArguments;

        /// <summary>
        ///     The logger name
        /// </summary>
        public string Name;

        /// <summary>
        ///     The log format. Available variables: {time}, {name}, {level}, {message}
        /// </summary>
        public string LogFormat = "[{time} {name} {level}] {message}";

        /// <summary>
        ///     The maximum recursion level. Will return "..." on reaching this value
        /// </summary>
        public int MaxRecursion = 10;

        /// <summary>
        ///     The name max length. Can be set with <see cref="LogManager" /> before creating any loggers
        /// </summary>
        internal static int NameMaxLength = 24;

        /// <summary>
        ///     The time format
        /// </summary>
        public string TimeFormat = "HH:mm:ss:fffff";

        /// <summary>
        ///     Format json-like messages?
        /// </summary>
        public bool FormatJson = true;

        /// <summary>
        ///     The logging level
        /// </summary>
        public LogType LoggingLevel = LogType.Info;

        // todo: optimize
        /// <summary>
        ///     The environment-set logging level
        /// </summary>
        internal static readonly LogType EnvironmentLoggingLevel = (LogType) Enum.Parse(typeof(LogType),
            (Enum.TryParse<LogType>(Environment.GetEnvironmentVariable("LOGGING_LEVEL"), out _)
                ? Environment.GetEnvironmentVariable("LOGGING_LEVEL")
                : "Info") ?? "Info", true);
    }
}