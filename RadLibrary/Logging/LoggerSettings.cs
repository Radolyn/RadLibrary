﻿#region

using System;

#endregion

namespace RadLibrary.Logging
{
    public class LoggerSettings : IEquatable<LoggerSettings>
    {
        static LoggerSettings()
        {
            var env = Environment.GetEnvironmentVariable("LOGGING_LEVEL");

            if (string.IsNullOrWhiteSpace(env))
                return;

            var success = Enum.TryParse<LogType>(env, out var res);

            if (!success)
                return;

            EnvironmentLoggingLevel = res;
        }

        public LoggerSettings()
        {
        }

        public LoggerSettings(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     The environment-set logging level
        /// </summary>
        internal static LogType EnvironmentLoggingLevel { get; }

        /// <summary>
        ///     Format json-like messages?
        /// </summary>
        public virtual bool FormatJson { get; set; } = true;

        /// <summary>
        ///     The log format. Available variables: {Time}, {Name}, {Level}, {Message}.
        ///     Supports all string.Format features (alignment, formatting)
        /// </summary>
        public virtual string LogFormat { get; set; } = "[{Time:HH\\:mm\\:ss\\:fffff} {Name,20} {Level,5}] {Message}";

        /// <summary>
        ///     The logger type (use typeof(RadLoggerBase))
        /// </summary>
        public virtual Type Logger { get; set; }

        /// <summary>
        ///     The minimal logging level
        /// </summary>
        public virtual LogType LoggingLevel { get; set; } = LogType.Info;

        /// <summary>
        ///     The logger name
        /// </summary>
        public virtual string Name { get; set; }

        /// <inheritdoc />
        public bool Equals(LoggerSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FormatJson == other.FormatJson && LogFormat == other.LogFormat && Logger == other.Logger &&
                   LoggingLevel == other.LoggingLevel && Name == other.Name;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
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