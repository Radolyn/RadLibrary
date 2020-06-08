#region

using System.Drawing;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>Defines progress bar settings for <see cref="Logger" /></summary>
    public class ProgressBarSettings
    {
        /// <summary>
        ///     The log type
        /// </summary>
        public LogType LogType = LogType.Information;

        /// <summary>
        ///     Total iterations
        /// </summary>
        public int Total = 20;

        /// <summary>
        ///     The prefix
        /// </summary>
        public string Prefix = "";

        /// <summary>
        ///     The suffix
        /// </summary>
        public string Suffix = "";

        /// <summary>
        ///     The progress bar length
        /// </summary>
        public int Length = 50;

        /// <summary>
        ///     The number of symbols after comma
        /// </summary>
        public int Decimals = 1;

        /// <summary>
        ///     The char that will be printed as filled
        /// </summary>
        public string FillChar = "█";

        /// <summary>
        ///     The char that will be printed as unfilled
        /// </summary>
        public string ProgressChar = "-";

        /// <summary>
        ///     The string that will be printed as separator between prefix and percentage
        /// </summary>
        public string SeparatorStr = "|";

        /// <summary>
        ///     The prefix color
        /// </summary>
        public Color PrefixColor;

        /// <summary>
        ///     The suffix color
        /// </summary>
        public Color SuffixColor;

        /// <summary>
        ///     The FillChar color
        /// </summary>
        public Color FillColor;

        /// <summary>
        ///     The ProgressChar color
        /// </summary>
        public Color ProgressColor;

        /// <summary>
        ///     The percentage color
        /// </summary>
        public Color PercentColor;

        /// <summary>
        ///     The SeparatorStr color
        /// </summary>
        public Color SeparatorColor;
    }
}