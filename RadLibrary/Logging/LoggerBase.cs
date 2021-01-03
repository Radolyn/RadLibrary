#region

using System.Linq;
using JetBrains.Annotations;
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
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Trace(string format, params object[] args)
        {
            DirectLog(LogType.Trace, ParseArguments(format, args));
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
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Debug(string format, params object[] args)
        {
            DirectLog(LogType.Debug, ParseArguments(format, args));
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
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Info(string format, params object[] args)
        {
            DirectLog(LogType.Info, ParseArguments(format, args));
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
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Warn(string format, params object[] args)
        {
            DirectLog(LogType.Warn, ParseArguments(format, args));
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
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Error(string format, params object[] args)
        {
            DirectLog(LogType.Error, ParseArguments(format, args));
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
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Fatal(string format, params object[] args)
        {
            DirectLog(LogType.Fatal, ParseArguments(format, args));
        }

        private static string ParseArguments(string format, params object[] args)
        {
            args ??= new object[] {null};
            return args.Length == 0 ? format : string.Format(FormattersStorage.FormatProvider, format, args);
        }

        private static string ParseArguments(params object[] args)
        {
            return args == null ? "null" : string.Format(FormattersStorage.FormatProvider, "{0}", args.ToList());
        }
    }

    public abstract class LoggerBase<T> : LoggerBase
        where T : LoggerSettings
    {
        public new virtual T Settings
        {
            get => (T) base.Settings;
            set => base.Settings = value;
        }
    }
}