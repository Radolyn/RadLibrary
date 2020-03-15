﻿#region

using System;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>Defines settings for <see cref="Logger"/></summary>
    public class LoggerSettings
    {
        /// <summary>The verbose message color</summary>
        public ConsoleColor VerboseColor = ConsoleColor.DarkGray;

        /// <summary>The information message color</summary>
        public ConsoleColor InformationColor = ConsoleColor.Cyan;

        /// <summary>The warning message color</summary>
        public ConsoleColor WarningColor = ConsoleColor.Yellow;

        /// <summary>The error message color</summary>
        public ConsoleColor ErrorColor = ConsoleColor.Red;

        /// <summary>The exception message color</summary>
        public ConsoleColor ExceptionColor = ConsoleColor.DarkRed;

        /// <summary>The deprecated message color</summary>
        public ConsoleColor DeprecatedColor = ConsoleColor.DarkYellow;

        /// <summary>
        ///   <para>The exception string.</para>
        ///   <para>Must contain {0} and {1} (<see cref="Exception"/> type, <see cref="Logger"/> name)</para>
        /// </summary>
        public string ExceptionString = "{0} in: {1}";

        /// <summary>
        ///   <para>The deprecated string.</para>
        ///   <para>Must contain {0} and {1} (what's deprecated, replacement)</para>
        /// </summary>
        public string DeprecatedString = "{0} is deprecated, use {1} instead.";

        /// <summary>
        ///   <para>The logger prefix</para>
        ///   <para>Must contain {0}, {1} and {2} (date, <see cref="Logger"/> name, log type)</para>
        /// </summary>
        public string LoggerPrefix = "[{0, 13:c} {1, 16:c} {2, 12:c}]";

        /// <summary>The log level</summary>
        public LogType LogLevel = LogType.Verbose;

        /// <summary>The time format</summary>
        public string TimeFormat = "HH:mm:ss:ffff";
    }
}