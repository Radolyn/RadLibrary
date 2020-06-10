namespace RadLibrary.Logging
{
    public interface ILoggerExtension
    {
        /// <summary>
        ///     Custom log
        /// </summary>
        /// <param name="message">The original message</param>
        /// <param name="formatted">The formatted message</param>
        /// <param name="settings">The logger settings</param>
        void Log(string message, string formatted, LoggerSettings settings);
    }
}