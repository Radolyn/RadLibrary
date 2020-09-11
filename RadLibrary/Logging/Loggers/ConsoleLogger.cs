#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using RadLibrary.Colors;

#endregion

namespace RadLibrary.Logging.Loggers
{
    /// <summary>
    ///     Logger that prints logs in console. Arguments: no
    /// </summary>
    public class ConsoleLogger : RadLoggerBase
    {
        /// <inheritdoc />
        public override void Initialize()
        {
            Colorizer.Initialize();
            Console.OutputEncoding = Encoding.UTF8;
        }

        /// <inheritdoc />
        protected override void Log(LogType type, string message, string formatted)
        {
            Console.WriteLine(
                formatted.Colorize(ColorSettings.Colors[type]));
        }
    }

    /// <summary>
    ///     The console colors. Use <see cref="Colorizer" />'s HexToColor function to parse web colors
    /// </summary>
    public static class ColorSettings
    {
        public static readonly Dictionary<LogType, Color> Colors = new Dictionary<LogType, Color>
        {
            {LogType.Trace, Color.Gray},
            {LogType.Debug, Color.DarkGray},
            {LogType.Info, Color.DarkCyan},
            {LogType.Warn, Color.Gold},
            {LogType.Error, Color.Tomato},
            {LogType.Fatal, Color.Red}
        };
    }
}