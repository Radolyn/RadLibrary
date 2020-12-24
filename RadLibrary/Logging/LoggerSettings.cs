#region

using System;
using RadLibrary.Formatting;

#endregion

namespace RadLibrary.Logging
{
    public class LoggerSettings : IEquatable<LoggerSettings>
    {
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
        ///     The log format. Available variables: {Time}, {Name}, {Level}, {Message}.
        ///     Supports all string.Format features (alignment, formatting)
        /// </summary>
        public string LogFormat = "[{Time:HH\\:mm\\:ss\\:fffff} {Name,20} {Level,5}] {Message}";

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
        ///     The maximum recursion level. Will return "..." on reaching this value
        /// </summary>
        public static int MaxRecursion
        {
            get => FormattersStorage.MaxRecursion;
            set => FormattersStorage.MaxRecursion = value;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LoggerSettings) obj);
        }

        public static bool operator ==(LoggerSettings left, LoggerSettings right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LoggerSettings left, LoggerSettings right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public bool Equals(LoggerSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FormatJson == other.FormatJson && LogFormat == other.LogFormat && Equals(Logger, other.Logger) && LoggingLevel == other.LoggingLevel && Name == other.Name;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FormatJson.GetHashCode();
                hashCode = (hashCode * 397) ^ (LogFormat != null ? LogFormat.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Logger != null ? Logger.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) LoggingLevel;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}