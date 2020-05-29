﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>Defines utils for <see cref="Logger" /></summary>
    public static class LoggerUtils
    {
        /// <summary>List that contains all loggers</summary>
        private static readonly List<Logger> Loggers = new List<Logger>();

        /// <summary>The exception logger</summary>
        private static Logger _exceptionLogger;

        /// <summary>Gets the logger or creates if not exists.</summary>
        /// <param name="name">The name.</param>
        /// <returns>Returns <see cref="Logger" /></returns>
        public static Logger GetLogger(string name)
        {
            if (Loggers.Any(logger1 => logger1.Name == name))
                return Loggers.Single(logger1 => logger1.Name == name);
            if (name.Length < 3)
                throw new ArgumentException("Name can't be less than 4 symbols", name);

            var logger = new Logger(name, new LoggerSettings());
            Loggers.Add(logger);

            return logger;
        }

        /// <summary>Prints the system information.</summary>
        /// <param name="verbose">Set to true if you want to know more</param>
        public static void PrintSystemInformation(bool verbose = false)
        {
            var logger = GetLogger("RadLibrary");

            var libVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var buildDate = new DateTime(2000, 1, 1)
                .AddDays(libVersion.Build).AddSeconds(libVersion.Revision * 2);

            logger.Verbose("RadLibrary version:", libVersion, buildDate, Environment.Is64BitProcess ? "x64" : "x32");
            logger.Verbose("Running on:", RuntimeInformation.OSDescription,
                Environment.Is64BitOperatingSystem ? "x64" : "x32");

            if (verbose) logger.Verbose("Environment variables:", Environment.GetEnvironmentVariables());
        }

        /// <summary>Registers the exception handler.</summary>
        /// <param name="name">The name of logger.</param>
        public static void RegisterExceptionHandler(string name = "RadLibrary")
        {
            var logger = GetLogger(name);

            logger.Verbose("Registering exception handler...");

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            _exceptionLogger = logger;
        }

        /// <summary>Prints the exception and kills process.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs" /> instance containing the event data.</param>
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                _exceptionLogger?.Exception(ex);

            Console.ResetColor();
            Environment.Exit(-1);
        }

        /// <summary>Converts input to bool.</summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultResult">Defines default result if input == "".</param>
        /// <returns></returns>
        public static bool InputToBool(string input, bool defaultResult = true)
        {
            input = input.ToLower();

            var yes = new[] {"yes", "y", "д", "да"};

            return input == "" ? defaultResult : yes.Contains(input);
        }
    }
}