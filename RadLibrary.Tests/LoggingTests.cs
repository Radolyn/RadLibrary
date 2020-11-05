#region

using System.IO;
using RadLibrary.Logging;
using RadLibrary.Logging.Loggers;
using Xunit;

#endregion

namespace RadLibrary.Tests
{
    public sealed class LoggingTests
    {
        [Fact]
        public void AssertLoggerCreates()
        {
            var logger = LogManager.GetLogger("TestLogger");

            Assert.NotNull(logger);

            Assert.Equal("TestLogger", logger.Settings.Name);
        }

        [Fact]
        public void AssertLoggerPrints()
        {
            const string s = "Some cool information";

            var fileName = "testLog" + Utilities.RandomInt();

            var logger = LogManager.GetLogger<FileLogger>("TestLogger", new FileLoggerSettings(fileName));

            logger.Info(s);
        }

        [Fact]
        public void AssertMultiLoggerWorks()
        {
            const string s = "Some cool information";

            var logger = GetLoggers();

            logger.Info(s);
        }

        [Fact]
        public void AssertLoggingLevelWorks()
        {
            const string s = "Some cool information";

            var logger = LogManager.GetLogger<FileLogger>("Logger", new FileLoggerSettings("environment_test.log"));

            logger.Settings.LoggingLevel = LogType.Error;

            logger.Trace(s);

            ((FileLogger) logger).Dispose();

            var logText = File.ReadAllText((logger.Settings as FileLoggerSettings)?.FileName);

            Assert.DoesNotContain(s, logText);
        }

        [Fact]
        public void AssertLoggerFound()
        {
            var logger1 = LogManager.GetLogger("LoggerFound");
            var logger2 = LogManager.GetLoggerByName("LoggerFound");
            var logger3 = LogManager.GetLogger("LoggerFound");

            Assert.Equal(logger1, logger2);
            Assert.Equal(logger1, logger3);

            var logger4 = LogManager.GetLogger<NullLogger>("LoggerFound");

            Assert.NotEqual(logger1, logger4);
        }

        private static LoggerBase GetLoggers()
        {
            var consoleLogger = LogManager.GetLogger("Logger");
            var nullLogger = LogManager.GetLogger<NullLogger>("Logger");
            var fileLogger =
                LogManager.GetLogger<FileLogger>("Logger", new FileLoggerSettings("log" + Utilities.RandomInt()));

            return LogManager.GetLogger<MultiLogger>("Logger",
                new MultiLoggerSettings(consoleLogger, nullLogger, fileLogger));
        }
    }
}