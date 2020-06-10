namespace RadLibrary.Logging
{
    public partial class Logger
    {
        /// <summary>
        ///     The trace message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Trace(params object[] args)
        {
            PrivateLog(LogType.Trace, ParseArguments(args));
        }

        /// <summary>
        ///     The trace message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Trace(string message, params object[] args)
        {
            PrivateLog(LogType.Trace, string.Format(message, args));
        }

        /// <summary>
        ///     The debug message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Debug(params object[] args)
        {
            PrivateLog(LogType.Debug, ParseArguments(args));
        }

        /// <summary>
        ///     The debug message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Debug(string message, params object[] args)
        {
            PrivateLog(LogType.Debug, string.Format(message, args));
        }

        /// <summary>
        ///     The information message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Info(params object[] args)
        {
            PrivateLog(LogType.Info, ParseArguments(args));
        }

        /// <summary>
        ///     The information message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Info(string message, params object[] args)
        {
            PrivateLog(LogType.Info, string.Format(message, args));
        }

        /// <summary>
        ///     The warning message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Warn(params object[] args)
        {
            PrivateLog(LogType.Warn, ParseArguments(args));
        }

        /// <summary>
        ///     The warning message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Warn(string message, params object[] args)
        {
            PrivateLog(LogType.Warn, string.Format(message, args));
        }

        /// <summary>
        ///     The error message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Error(params object[] args)
        {
            PrivateLog(LogType.Error, ParseArguments(args));
        }

        /// <summary>
        ///     The error message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Error(string message, params object[] args)
        {
            PrivateLog(LogType.Error, string.Format(message, args));
        }

        /// <summary>
        ///     The fatal message
        /// </summary>
        /// <param name="args">The arguments</param>
        public void Fatal(params object[] args)
        {
            PrivateLog(LogType.Fatal, ParseArguments(args));
        }

        /// <summary>
        ///     The fatal message
        /// </summary>
        /// <param name="message">The format (will be formatted with string.Format) or message</param>
        /// <param name="args">The arguments to pass in string.Format</param>
        public void Fatal(string message, params object[] args)
        {
            PrivateLog(LogType.Fatal, string.Format(message, args));
        }
    }
}