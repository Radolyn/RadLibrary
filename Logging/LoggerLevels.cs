#region

using System;

#endregion

namespace RadLibrary.Logging
{
    public partial class Logger
    {
        /// <summary>Alias for <see cref="Log" /> (Verbose)</summary>
        /// <param name="args">The arguments.</param>
        public void Verbose(params object[] args)
        {
            Log(LogType.Verbose, args);
        }

        /// <summary>Alias for <see cref="Log" /> (Information)</summary>
        /// <param name="args">The arguments.</param>
        public void Info(params object[] args)
        {
            Log(LogType.Information, args);
        }

        /// <summary>Alias for <see cref="Log" /> (Warning)</summary>
        /// <param name="args">The arguments.</param>
        public void Warn(params object[] args)
        {
            Log(LogType.Warning, args);
        }

        /// <summary>Alias for <see cref="Log" /> (Error)</summary>
        /// <param name="args">The arguments.</param>
        public void Error(params object[] args)
        {
            Log(LogType.Error, args);
        }

        /// <summary>Alias for <see cref="Log" /> (Success)</summary>
        /// <param name="args">The arguments.</param>
        public void Success(params object[] args)
        {
            Log(LogType.Success, args);
        }

        /// <summary>Logs the specified exception</summary>
        /// <param name="ex">The exception</param>
        /// <exception cref="FormatException"><see cref="LoggerSettings.ExceptionString" /> doesn't contains {0}</exception>
        public void Exception(Exception ex)
        {
            if (!Settings.StringFormatRegex.IsMatch(Settings.ExceptionString))
                throw new FormatException();
            Log(LogType.Exception, Settings.ExceptionString, ex.GetType(), ex.StackTrace, ex.Message);
        }

        /// <summary>Logs the deprecated part of code</summary>
        /// <param name="old">Deprecation object</param>
        /// <param name="replacement">Replacement</param>
        /// <exception cref="FormatException"><see cref="LoggerSettings.ExceptionString" /> doesn't contains {0}</exception>
        public void Deprecated(object old, object replacement)
        {
            if (!Settings.DeprecatedString.Contains("{0}"))
                throw new FormatException();
            Log(LogType.Deprecation, Settings.DeprecatedString, old, replacement);
        }
    }
}