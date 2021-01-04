#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using RadLibrary.Logging.Loggers;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>
    ///     Logger creator and configurator
    /// </summary>
    public static class LogManager
    {
        private static readonly List<LoggerBase> Loggers = new();

        /// <summary>
        ///     Adds exceptions handler to app
        /// </summary>
        /// <param name="logger">The custom logger</param>
        public static void AddExceptionsHandler(LoggerBase logger = null)
        {
            if (logger != null)
                AppDomain.CurrentDomain.UnhandledException += (sender, args) => logger.Error(args.ExceptionObject);
            else
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    GetLogger<MultiLogger>("ExceptionHandler", new MultiLoggerSettings(
                            GetLogger<ConsoleLogger>("ExceptionHandler"),
                            GetLogger<FileLogger>("ExceptionHandler",
                                new FileLoggerSettings("crash.txt", FileMode.OpenOrCreate))))
                        .Fatal(args.ExceptionObject);
                    Environment.Exit(1);
                };
        }

        /// <summary>
        ///     Creates console logger with specified name
        /// </summary>
        /// <param name="name">The logger name</param>
        /// <returns>The console logger</returns>
        public static LoggerBase GetLogger(string name)
        {
            return GetLogger<ConsoleLogger>(name);
        }

        /// <summary>
        ///     Creates console logger with specified name
        /// </summary>
        /// <param name="name">The logger name</param>
        /// <param name="args">Logger's arguments</param>
        /// <returns>The console logger</returns>
        public static LoggerBase GetLogger(string name, LoggerSettings args)
        {
            return GetLogger<ConsoleLogger>(name, args);
        }

        /// <summary>
        ///     Creates logger with specified type and name
        /// </summary>
        /// <param name="name">The logger name</param>
        /// <typeparam name="TLogger">Logger type</typeparam>
        /// <returns>The T logger</returns>
        public static LoggerBase GetLogger<TLogger>(string name) where TLogger : LoggerBase
        {
            return GetLogger<TLogger>(name, null);
        }

        /// <summary>
        ///     Creates logger with specified type, name and arguments
        /// </summary>
        /// <param name="name">The logger name</param>
        /// <param name="args">Logger's arguments</param>
        /// <typeparam name="TLogger">Logger type</typeparam>
        /// <returns>The T logger</returns>
        public static LoggerBase GetLogger<TLogger>(string name, LoggerSettings args) where TLogger : LoggerBase
        {
            args ??= GetSettingsInstance(typeof(TLogger));

            args.Name = name;
            args.Logger = typeof(TLogger);

            return CreateLogger(args);
        }

        /// <summary>
        ///     Creates logger with the name of calling class
        /// </summary>
        /// <returns>The console logger</returns>
        public static LoggerBase GetClassLogger()
        {
            return GetClassLogger<ConsoleLogger>();
        }

        /// <summary>
        ///     Creates logger with the name of calling class and specified arguments
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <returns>The console logger</returns>
        public static LoggerBase GetClassLogger(LoggerSettings args)
        {
            return GetClassLogger<ConsoleLogger>(args);
        }

        /// <summary>
        ///     Creates logger with the name of calling class and specified arguments
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <typeparam name="TLogger">Logger type</typeparam>
        /// <returns>The T logger</returns>
        public static LoggerBase GetClassLogger<TLogger>(LoggerSettings args = null) where TLogger : LoggerBase
        {
            args ??= GetSettingsInstance(typeof(TLogger));

            // get namespace
            var method = GetPreviousFrame().GetMethod();

            args.Name = method?.DeclaringType?.FullName;
            args.Logger ??= typeof(TLogger);

            return CreateLogger(args);
        }

        /// <summary>
        ///     Creates logger with the name of calling method
        /// </summary>
        /// <returns>The console logger</returns>
        public static LoggerBase GetMethodLogger()
        {
            return GetMethodLogger<ConsoleLogger>();
        }

        /// <summary>
        ///     Creates logger with the name of calling method and specified arguments
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <returns>The console logger</returns>
        public static LoggerBase GetMethodLogger(LoggerSettings args)
        {
            return GetMethodLogger<ConsoleLogger>(args);
        }

        /// <summary>
        ///     Creates logger with the name of calling method and specified arguments
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <typeparam name="TLogger">Logger type</typeparam>
        /// <returns>The T logger</returns>
        public static LoggerBase GetMethodLogger<TLogger>(LoggerSettings args = null) where TLogger : LoggerBase
        {
            args ??= GetSettingsInstance(typeof(TLogger));

            // get method name
            var method = GetPreviousFrame().GetMethod();

            args.Name = method?.DeclaringType?.FullName + "." + method?.Name;
            args.Logger ??= typeof(TLogger);

            return CreateLogger(args);
        }

        /// <summary>
        ///     Creates logger with specified settings
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <returns>The logger</returns>
        public static LoggerBase GetLogger(LoggerSettings args)
        {
            return CreateLogger(args);
        }

        /// <summary>
        ///     Returns loggers by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The loggers</returns>
        public static IEnumerable<LoggerBase> GetLoggersByName(string name)
        {
            return Loggers.FindAll(x => x.Settings.Name == name);
        }

        /// <summary>
        ///     Returns logger by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The logger</returns>
        public static LoggerBase GetLoggerByName(string name)
        {
            return Loggers.Find(x => x.Settings.Name == name);
        }

        private static LoggerBase CreateLogger(LoggerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings), "Settings cannot be null");

            var predicate = Loggers.Find(x => x.Settings.Equals(settings));

            if (predicate != null)
                return predicate;

            var logger = (LoggerBase) Activator.CreateInstance(settings.Logger);
            logger!.Settings = settings;

            logger.Initialize();

            Loggers.Add(logger);

            return logger;
        }

        private static LoggerSettings GetSettingsInstance(Type type)
        {
            if (!typeof(LoggerBase).IsAssignableFrom(type))
                throw new ArgumentException($"{type.FullName} is not assignable to LoggerBase");

            if (type?.BaseType?.IsGenericType != true)
                return new LoggerSettings();

            return (LoggerSettings) Activator.CreateInstance(type.BaseType.GetGenericArguments()[0]);
        }

        private static StackFrame GetPreviousFrame()
        {
            var stack = new StackTrace();
            var current = typeof(LogType).Module;
            var frame = stack.GetFrames()?.First(x => x.GetMethod().Module != current);

            return frame;
        }
    }
}