#region

using System;
using System.Drawing;
using System.Text;

#endregion

namespace RadLibrary.Logging.Loggers
{
    /// <summary>
    ///     Logger that prints logs in console. Arguments: no
    /// </summary>
    public class ConsoleLogger : Logger
    {
        public static readonly object ConsoleLocker = new object();

        /// <inheritdoc />
        public override void Initialize()
        {
            Colorizer.Initialize();
            Console.OutputEncoding = Encoding.UTF8;
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