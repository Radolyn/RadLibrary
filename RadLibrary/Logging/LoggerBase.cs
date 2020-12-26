#region

using System.Linq;
using RadLibrary.Formatting;

#endregion

namespace RadLibrary.Logging
{
    public abstract class LoggerBase
    {
        /// <summary>
        ///     Gets or sets settings
        /// </summary>
        public virtual LoggerSettings Settings { get; set; }

        public abstract void DirectLog(LogType type, string message);

        /// <summary>
        ///     Initializes logger
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        ///     The trace message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Trace(params object[] args)
        {
            DirectLog(LogType.Trace, ParseArguments(args));
        }

        /// <summary>
        ///     The trace message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Trace(string message, params object[] args)
        {
            DirectLog(LogType.Trace,
                args.Length == 0 ? message : string.Format(FormattersStorage.FormatProvider, message, args));
        }

        /// <summary>
        ///     The debug message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Debug(params object[] args)
        {
            DirectLog(LogType.Debug, ParseArguments(args));
        }

        /// <summary>
        ///     The debug message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Debug(string message, params object[] args)
        {
            DirectLog(LogType.Debug,
                args.Length == 0 ? message : string.Format(FormattersStorage.FormatProvider, message, args));
        }

        /// <summary>
        ///     The information message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Info(params object[] args)
        {
            DirectLog(LogType.Info, ParseArguments(args));
        }

        /// <summary>
        ///     The information message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Info(string message, params object[] args)
        {
            DirectLog(LogType.Info,
                args.Length == 0 ? message : string.Format(FormattersStorage.FormatProvider, message, args));
        }

        /// <summary>
        ///     The warning message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Warn(params object[] args)
        {
            DirectLog(LogType.Warn, ParseArguments(args));
        }

        /// <summary>
        ///     The warning message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Warn(string message, params object[] args)
        {
            DirectLog(LogType.Warn,
                args.Length == 0 ? message : string.Format(FormattersStorage.FormatProvider, message, args));
        }

        /// <summary>
        ///     The error message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Error(params object[] args)
        {
            DirectLog(LogType.Error, ParseArguments(args));
        }

        /// <summary>
        ///     The error message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Error(string message, params object[] args)
        {
            DirectLog(LogType.Error,
                args.Length == 0 ? message : string.Format(FormattersStorage.FormatProvider, message, args));
        }

        /// <summary>
        ///     The fatal message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Fatal(params object[] args)
        {
            DirectLog(LogType.Fatal, ParseArguments(args));
        }

        /// <summary>
        ///     The fatal message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Fatal(string message, params object[] args)
        {
            DirectLog(LogType.Fatal,
                args.Length == 0 ? message : string.Format(FormattersStorage.FormatProvider, message, args));
        }

        public static string ParseArguments(params object[] args)
        {
            return args == null ? "null" : string.Format(FormattersStorage.FormatProvider, "{0}", args.ToList());
        }
    }

    public abstract class LoggerBase<T> : LoggerBase
        where T : LoggerSettings
    {
        public new T Settings
        {
            get => (T) base.Settings;
            set => base.Settings = value;
        }
    }
}