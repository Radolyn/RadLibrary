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
        public void Trace([CanBeNull] params object[] args)
        {
            DirectLog(LogType.Trace, ParseArguments(args));
        }

        /// <summary>
        ///     The trace message
        /// </summary>
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Trace([NotNull] string format, [CanBeNull] params object[] args)
        {
            DirectLog(LogType.Trace, ParseArguments(format, args));
        }

        /// <summary>
        ///     The debug message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Debug([CanBeNull] params object[] args)
        {
            DirectLog(LogType.Debug, ParseArguments(args));
        }

        /// <summary>
        ///     The debug message
        /// </summary>
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Debug([NotNull] string format, [CanBeNull] params object[] args)
        {
            DirectLog(LogType.Debug, ParseArguments(format, args));
        }

        /// <summary>
        ///     The information message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Info([CanBeNull] params object[] args)
        {
            DirectLog(LogType.Info, ParseArguments(args));
        }

        /// <summary>
        ///     The information message
        /// </summary>
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Info([NotNull] string format, [CanBeNull] params object[] args)
        {
            DirectLog(LogType.Info, ParseArguments(format, args));
        }

        /// <summary>
        ///     The warning message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Warn([CanBeNull] params object[] args)
        {
            DirectLog(LogType.Warn, ParseArguments(args));
        }

        /// <summary>
        ///     The warning message
        /// </summary>
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Warn([NotNull] string format, [CanBeNull] params object[] args)
        {
            DirectLog(LogType.Warn, ParseArguments(format, args));
        }

        /// <summary>
        ///     The error message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Error([CanBeNull] params object[] args)
        {
            DirectLog(LogType.Error, ParseArguments(args));
        }

        /// <summary>
        ///     The error message
        /// </summary>
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Error([NotNull] string format, [CanBeNull] params object[] args)
        {
            DirectLog(LogType.Error, ParseArguments(format, args));
        }

        /// <summary>
        ///     The fatal message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Fatal([CanBeNull] params object[] args)
        {
            DirectLog(LogType.Fatal, ParseArguments(args));
        }

        /// <summary>
        ///     The fatal message
        /// </summary>
        /// <param name="format">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        [StringFormatMethod("format")]
        public void Fatal([NotNull] string format, [CanBeNull] params object[] args)
        {
            DirectLog(LogType.Fatal, ParseArguments(format, args));
        }

        private static string ParseArguments([NotNull] string format, [CanBeNull] params object[] args)
        {
            args ??= new object[] {null};
            return args.Length == 0 ? format : string.Format(FormattersStorage.FormatProvider, format, args);
        }

        private static string ParseArguments([CanBeNull] params object[] args)
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