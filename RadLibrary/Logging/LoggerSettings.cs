#region

using System;
using RadLibrary.Formatting;

#endregion

namespace RadLibrary.Logging
{
    public class LoggerSettings : IEquatable<LoggerSettings>
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

        /// <inheritdoc />
        public bool Equals(LoggerSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FormatJson == other.FormatJson &&
                   string.Equals(LogFormat, other.LogFormat, StringComparison.OrdinalIgnoreCase) &&
                   Equals(Logger, other.Logger) && LoggingLevel == other.LoggingLevel &&
                   string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(TimeFormat,
                       other.TimeFormat, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LoggerSettings) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FormatJson.GetHashCode();
                hashCode = (hashCode * 397) ^
                           (LogFormat != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(LogFormat) : 0);
                hashCode = (hashCode * 397) ^ (Logger != null ? Logger.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) LoggingLevel;
                hashCode = (hashCode * 397) ^ (Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode * 397) ^
                           (TimeFormat != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(TimeFormat) : 0);
                return hashCode;
            }
        }

        public static bool operator ==(LoggerSettings left, LoggerSettings right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LoggerSettings left, LoggerSettings right)
        {
            return !Equals(left, right);
        }
    }
}