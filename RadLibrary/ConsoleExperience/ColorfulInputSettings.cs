#region

using System.Drawing;
using RadLibrary.ConsoleExperience.PredictionEngine;

#endregion

namespace RadLibrary.ConsoleExperience
{
    public sealed class ColorfulInputSettings
    {
        /// <summary>
        ///     The arrows background color
        /// </summary>
        public Color ArrowsBackgroundColor = Color.Black;

        /// <summary>
        ///     The arrows foreground color
        /// </summary>
        public Color ArrowsForegroundColor = Color.Aquamarine;

        /// <summary>
        ///     The prediction engine
        /// </summary>
        public IPredictionEngine Engine;

        /// <summary>
        ///     The input text background color
        /// </summary>
        public Color InputBackgroundColor = Color.Black;

        /// <summary>
        ///     The input text foreground color
        /// </summary>
        public Color InputForegroundColor = Color.White;

        /// <summary>
        ///     The prediction text background color
        /// </summary>
        public Color PredictionBackgroundColor = Color.Black;

        /// <summary>
        ///     The prediction text foreground color
        /// </summary>
        public Color PredictionForegroundColor = Color.Goldenrod;

        /// <summary>
        ///     The prefix
        /// </summary>
        public string Prefix;
    }
}