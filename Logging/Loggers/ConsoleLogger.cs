#region

using System;
using System.Drawing;

#endregion

namespace RadLibrary.Logging.Loggers
{
    public class ConsoleLogger : Logger
    {
        public static readonly object ConsoleLocker = new object();

        /// <inheritdoc />
        public override void Initialize(params object[] args)
        {
            Colorizer.Initialize();
        }

        /// <inheritdoc />
        public override void Log(LogType type, string message, string formatted)
        {
            lock (ConsoleLocker)
            {
                Console.WriteLine(
                    formatted.Colorize((Color) typeof(ColorsSettings).GetField(type.ToString()).GetValue(null)));
            }
        }
    }

    /// <summary>
    ///     The console colors. Use <see cref="Colorizer" />'s HexToColor function to parse web colors
    /// </summary>
    public static class ColorsSettings
    {
        public static Color Trace = Color.Gray;
        public static Color Debug = Color.DarkGray;
        public static Color Info = Color.DarkCyan;
        public static Color Warn = Color.Gold;
        public static Color Error = Color.Tomato;
        public static Color Fatal = Color.Red;
    }
}