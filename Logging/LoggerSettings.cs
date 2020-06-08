#region

using System;
using System.Drawing;
using System.Text.RegularExpressions;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>Defines settings for <see cref="Logger" /></summary>
    public class LoggerSettings
    {
        /// <summary>The verbose message color</summary>
        public Color VerboseColor = Color.DarkGray;

        /// <summary>The information message color</summary>
        public Color InformationColor = Colorizer.HexToColor("#2196F3");

        /// <summary>The warning message color</summary>
        public Color WarningColor = Colorizer.HexToColor("#FFEB3B");

        /// <summary>The error message color</summary>
        public Color ErrorColor = Color.LightCoral;

        /// <summary>The success message color</summary>
        public Color SuccessColor = Color.SeaGreen;

        /// <summary>The exception message color</summary>
        public Color ExceptionColor = Colorizer.HexToColor("#FF3D00");

        /// <summary>The deprecated message color</summary>
        public Color DeprecatedColor = Color.Orange;

        /// <summary>The input message color</summary>
        public Color InputColor = Colorizer.HexToColor("#FDD835");

        /// <summary>The prediction message color</summary>
        public Color PredictionColor = Color.Orange;

        /// <summary>
        ///     <para>The exception string.</para>
        ///     <para>Must contain {0}, {1} and {2} (<see cref="Exception" /> type, stack trace, message)</para>
        /// </summary>
        public string ExceptionString = "{0}. Stack trace:\n{1}.\nMessage: {2}";

        /// <summary>
        ///     <para>The deprecated string.</para>
        ///     <para>Must contain {0} and {1} (what's deprecated, replacement)</para>
        /// </summary>
        public string DeprecatedString = "{0} is deprecated, use {1} instead.";

        /// <summary>
        ///     <para>The logger prefix</para>
        ///     <para>Must contain {0}, {1}, {2} and {3} (date, <see cref="Logger" /> name, log type, thread)</para>
        /// </summary>
        public string LoggerPrefix = "[{0, 13:c} {1, 14:c} {2, 12:c} TH-{3, 1:0}]";

        /// <summary>The log level</summary>
        public LogType LogLevel = LogType.Verbose;

        /// <summary>The time format</summary>
        public string TimeFormat = "HH:mm:ss:ffff";

        /// <summary>
        ///     The argument recursion limit.
        /// </summary>
        public int RecursionLimit = 10;

        /// <summary>
        ///     The error upon reaching recursion limit.
        /// </summary>
        public bool ErrorOnRecursionLimit = false;

        /// <summary>
        ///     Format json like output
        /// </summary>
        public bool FormatJsonLike = true;

        /// <summary>
        ///     Regex that string need to match for json-like formatting
        ///     Use @"(?&lt;=\"")([^\s,].*?)(?=\"")|null" to format json only
        /// </summary>
        public Regex JsonRegex = new Regex(@"{.*\s*:\s*.*}");

        /// <summary>
        ///     Regex that string need to match for string.Format
        /// </summary>
        public Regex StringFormatRegex = new Regex(@"{(\d+\s*(()|(\d)|(,\s*\d+)|(:\s*\w+)|(,\s*\d+\s*:\s*\w+)))}");
    }
}