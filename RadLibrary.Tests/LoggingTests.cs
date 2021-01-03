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

            var logger = GetLoggers();

            logger.Info(s);
        }

        [Fact]
        public void AssertLoggingLevelWorks()
        {
            const string s = "Some cool information";

            var logger = LogManager.GetLogger<FileLogger>("Logger2", new FileLoggerSettings("environment_test.log"));

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

        [Fact]
        public void AssertEverythingPrints()
        {
            var logger = GetLoggers();

            logger.Debug(6);
            logger.Debug("{0}", null);

            logger.Trace(5);
            logger.Debug("{0}", 123);

            logger.Info(4);
            logger.Info("{0} {1}", null, "321");

            logger.Warn(3);
            logger.Warn("{0} {1} {2}", null, "321", 22);

            logger.Error(2);
            logger.Error("{0}", "we at R@d0l^n take our jobs very seriously\ni'm not kidding");

            logger.Fatal(1);
            logger.Fatal("{0}", "{\"color\": \"red\",\"value\": \"#f00\"}");
        }

        [Fact]
        public void AssertSettingsEquals()
        {
            var logger = GetLoggers();

            Assert.True(logger.Settings.Equals(LogManager.GetLogger<MultiLogger>("Logger").Settings));
            Assert.True(logger.Settings.Equals(logger.Settings));
            Assert.True(logger.Settings.Equals((object) logger.Settings));
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.False(logger.Settings.Equals("123"));
            Assert.False(logger.Settings.Equals(null));

            var hash = logger.Settings.GetHashCode();
        }

        [Fact]
        public void AssertFileLoggerCreates()
        {
            var logger = LogManager.GetLogger<FileLogger>("FileLogger",
                new FileLoggerSettings("sadsda11.txt", FileMode.OpenOrCreate));
        }

        [Fact]
        public void AssertMultiLoggerSettings()
        {
            var fileLogger = LogManager.GetLogger<FileLogger>("FileLogger2");
            var consoleLogger = LogManager.GetLogger("FileLogger3");
            var logger =
                LogManager.GetLogger(new MultiLoggerSettings(new List<LoggerBase> {fileLogger, consoleLogger}));

            logger.Settings.Name = "1338";
            logger.Settings.FormatJson = false;
            logger.Settings.LoggingLevel = LogType.Fatal;
            logger.Settings.LogFormat = "[]";

            Assert.Equal(fileLogger.Settings.Name, consoleLogger.Settings.Name);
            Assert.Equal(fileLogger.Settings.FormatJson, consoleLogger.Settings.FormatJson);
            Assert.Equal(fileLogger.Settings.LoggingLevel, consoleLogger.Settings.LoggingLevel);
            Assert.Equal(fileLogger.Settings.LogFormat, consoleLogger.Settings.LogFormat);
        }


        [Fact]
        public void TestAllMethodsToCreateLogger()
        {
            LoggerBase logger;

            logger = LogManager.GetLogger("1");
            logger = LogManager.GetLogger<NullLogger>("2");
            logger = LogManager.GetLogger<NullLogger>("2");
            logger = LogManager.GetLogger(new FileLoggerSettings("asd.txt"));
            logger = LogManager.GetLogger("asdasdsda", new FileLoggerSettings("asd.txt"));

            logger = LogManager.GetMethodLogger();
            logger = LogManager.GetMethodLogger(new FileLoggerSettings("asd2.txt"));
            logger = LogManager.GetMethodLogger<NullLogger>();

            logger = LogManager.GetClassLogger();
            logger = LogManager.GetClassLogger(new FileLoggerSettings("asd3.txt"));
            logger = LogManager.GetClassLogger<NullLogger>();

            Assert.NotNull(LogManager.GetLoggerByName("1"));
            Assert.True(LogManager.GetLoggersByName("2").ToList().Count == 1);
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