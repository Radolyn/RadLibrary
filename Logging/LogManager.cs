#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using RadLibrary.Logging.Loggers;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>
    ///     Logger creator and configurator
    /// </summary>
    public static class LogManager
    {
        private static readonly List<Logger> _loggers = new List<Logger>();

        /// <summary>
        ///     Gets or sets max name length
        /// </summary>
        /// <exception cref="Exception">Occurs when trying to set max length after loggers initialization</exception>
        public static int MaxNameLength
        {
            get => LoggerSettings.NameMaxLength;
            set
            {
                if (_loggers.Any())
                    throw new Exception("Cannot change max length, because there's at least one logger initialized");

                LoggerSettings.NameMaxLength = value;
            }
        }

        /// <summary>
        ///     Adds exceptions handler
        /// </summary>
        /// <param name="logger"></param>
        public static void AddExceptionsHandler(Logger logger = null)
        {
            if (logger != null)
                AppDomain.CurrentDomain.UnhandledException += (sender, args) => logger.Error(args.ExceptionObject);
            else
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    GetLogger<MultiLogger>("ExceptionHandler", GetLogger<ConsoleLogger>("ExceptionHandler"),
                            GetLogger<FileLogger>("ExceptionHandler", "crash.txt", FileMode.OpenOrCreate))
                        .Fatal(args.ExceptionObject);
                    Environment.Exit(1);
                };
        }

        /// <summary>
        ///     Creates logger with specified type and name
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TLogger"></typeparam>
        /// <returns></returns>
        public static Logger GetLogger<TLogger>(string name) where TLogger : Logger
        {
            return GetLogger<TLogger>(name, null);
        }

        /// <summary>
        ///     Creates logger with specified type, name and arguments
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TLogger"></typeparam>
        /// <returns></returns>
        public static Logger GetLogger<TLogger>(string name, params object[] args) where TLogger : Logger
        {
            return CreateLogger(new LoggerSettings
            {
                Name = name,
                Logger = typeof(TLogger),
                InitializationArguments = args
            });
        }

        /// <summary>
        ///     Creates logger with the name of calling class
        /// </summary>
        /// <returns>Logger</returns>
        public static Logger GetClassLogger()
        {
            return GetClassLogger<ConsoleLogger>();
        }

        /// <summary>
        ///     Creates logger with the name of calling class and specified arguments
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <returns>Logger</returns>
        public static Logger GetClassLogger(params object[] args)
        {
            return GetClassLogger<ConsoleLogger>(args);
        }

        /// <summary>
        ///     Creates logger with the name of calling class and specified arguments
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <typeparam name="TLogger">Logger type</typeparam>
        /// <returns>Logger</returns>
        public static Logger GetClassLogger<TLogger>(params object[] args) where TLogger : Logger
        {
            // get namespace
            var stack = new StackTrace();
            var current = Assembly.GetExecutingAssembly().ManifestModule;
            var method = stack.GetFrames()?.First(x => x.GetMethod().Module != current).GetMethod();

            return CreateLogger(new LoggerSettings
            {
                Name = method?.DeclaringType?.FullName,
                Logger = typeof(TLogger),
                InitializationArguments = args
            });
        }

        /// <summary>
        ///     Creates logger with the name of calling method
        /// </summary>
        /// <returns>Logger</returns>
        public static Logger GetMethodLogger()
        {
            return GetMethodLogger<ConsoleLogger>();
        }

        /// <summary>
        ///     Creates logger with the name of calling method and specified arguments
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <returns>Logger</returns>
        public static Logger GetMethodLogger(params object[] args)
        {
            return GetMethodLogger<ConsoleLogger>(args);
        }

        /// <summary>
        ///     Creates logger with the name of calling method and specified arguments
        /// </summary>
        /// <param name="args">Logger's arguments</param>
        /// <typeparam name="TLogger">Logger type</typeparam>
        /// <returns>Logger</returns>
        public static Logger GetMethodLogger<TLogger>(params object[] args) where TLogger : Logger
        {
            // get method name
            var stack = new StackTrace();
            var current = Assembly.GetExecutingAssembly().ManifestModule;
            var method = stack.GetFrames()?.First(x => x.GetMethod().Module != current).GetMethod();

            return CreateLogger(new LoggerSettings
            {
                Name = method?.DeclaringType?.FullName + "." + method?.Name,
                Logger = typeof(TLogger),
                InitializationArguments = args
            });
        }

        /// <summary>
        ///     Creates logger with specified settings
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <returns>Logger</returns>
        public static Logger GetLogger(LoggerSettings settings)
        {
            return CreateLogger(settings);
        }

        private static Logger CreateLogger(LoggerSettings settings)
        {
            var predicate = _loggers.Find(x => x.Settings == settings);

            if (predicate != null)
                return predicate;

            VerifySettings(settings);

            var logger = (Logger) Activator.CreateInstance(settings.Logger);
            logger.Settings = settings;

            logger.Initialize(settings.InitializationArguments);

            _loggers.Add(logger);

            return logger;
        }

        private static void VerifySettings(LoggerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings), "Settings cannot be null");

            if (!typeof(Logger).IsAssignableFrom(settings.Logger))
                throw new ArgumentException($"{settings.Logger?.FullName} is not assignable to Logger");
        }
    }
}