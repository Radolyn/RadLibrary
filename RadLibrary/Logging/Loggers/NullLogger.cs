namespace RadLibrary.Logging.Loggers
{
    /// <summary>
    ///     Logger that does nothing
    /// </summary>
    public sealed class NullLogger : LoggerBase
    {
        /// <inheritdoc />
        public override void DirectLog(LogType type, string message)
        {
        }

        /// <inheritdoc />
        public override void Initialize()
        {
        }
    }
}