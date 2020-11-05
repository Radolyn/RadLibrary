#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var loggers = GetLoggers();

            var logger = LogManager.GetLogger<MultiLogger>("MultiLogger", new MultiLoggerSettings(loggers));

            logger.Info(s);

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (FileLogger loggerBase in loggers)
            {
                loggerBase.Dispose();

                var logText = File.ReadAllText((loggerBase.Settings as FileLoggerSettings)?.FileName);

                Assert.Contains(s, logText);
            }
        }

        [Fact]
        public void AssertLoggingLevelWorks()
        {
            const string s = "Some cool information";

            var loggers = GetLoggers();

            var logger = LogManager.GetLogger<MultiLogger>("MultiLogger", new MultiLoggerSettings(loggers));

            logger.Settings.LoggingLevel = LogType.Trace;

            logger.Trace(s);

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (FileLogger loggerBase in loggers)
            {
                loggerBase.Dispose();

                var logText = File.ReadAllText((loggerBase.Settings as FileLoggerSettings)?.FileName);

                Assert.DoesNotContain(s, logText);
            }
        }

        private static List<LoggerBase> GetLoggers()
        {
            var fileNames = new List<string>
            {
                "testLog" + Utilities.RandomInt(), "testLog" + Utilities.RandomInt(), "testLog" + Utilities.RandomInt(),
                "testLog" + Utilities.RandomInt(), "testLog" + Utilities.RandomInt()
            };

            var fileLoggers = fileNames.Select(x =>
                LogManager.GetLogger<FileLogger>("TestLogger", new FileLoggerSettings(x)));

            return fileLoggers.ToList();
        }
    }
}