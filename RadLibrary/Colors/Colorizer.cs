#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

#endregion

namespace RadLibrary.Colors
{
    public static class Colorizer
    {
        private const int StdOutputHandle = -11;
        private const uint EnableVirtualTerminalProcessing = 0x0004;
        private const uint DisableNewlineAutoReturn = 0x0008;
        private static bool _isInitialized;

        private static readonly Regex ColorsRegex =
            new("(\x1b\\[\\d{2};2;\\d{1,3};\\d{1,3};\\d{1,3}m)|(\x1b\\[\\d{1,3}m)", RegexOptions.Compiled);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        /// <summary>
        ///     Initializes colors system
        /// </summary>
        /// <exception cref="Win32Exception">If failed to set color mode</exception>
        public static void Initialize()
        {
            if (_isInitialized || !Utilities.IsWindows || Console.IsOutputRedirected)
            {
                _isInitialized = true;
                return;
            }

            // todo: support for old terminals ($COLORTERM)

            var iStdOut = Utilities.GetStdHandle(StdOutputHandle);
            if (!GetConsoleMode(iStdOut, out var outConsoleMode))
                throw new Win32Exception(
                    $"Failed to get output console mode, error code: {Marshal.GetLastWin32Error()}");

            outConsoleMode |= EnableVirtualTerminalProcessing;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
                throw new Win32Exception(
                    $"Failed to set output console mode, error code: {Marshal.GetLastWin32Error()}");

            AppDomain.CurrentDomain.ProcessExit += (sender, args) => Console.Write(Font.Reset);
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => Console.Write(Font.Reset);

            _isInitialized = true;
        }

        /// <summary>
        ///     Gets colorization string
        /// </summary>
        /// <param name="r">The red color value</param>
        /// <param name="g">The green color value</param>
        /// <param name="b">The blue color value</param>
        /// <returns>Prefix for string colorization</returns>
        [NotNull]
        public static string GetColorizationString(uint r, uint g, uint b)
        {
            return _isInitialized ? $"\x1b[38;2;{r};{g};{b}m" : "";
        }

        /// <summary>
        ///     Gets colorization string
        /// </summary>
        /// <param name="color">The color</param>
        /// <returns>Prefix for string colorization</returns>
        [NotNull]
        public static string GetColorizationString(Color color)
        {
            return GetColorizationString(color.R, color.G, color.B);
        }

        /// <summary>
        ///     Gets background colorization string
        /// </summary>
        /// <param name="r">The red color value</param>
        /// <param name="g">The green color value</param>
        /// <param name="b">The blue color value</param>
        /// <returns>Prefix for string background colorization</returns>
        [NotNull]
        public static string GetBackgroundColorizationString(uint r, uint g, uint b)
        {
            return _isInitialized ? $"\x1b[48;2;{r};{g};{b}m" : "";
        }

        /// <summary>
        ///     Gets background colorization string
        /// </summary>
        /// <param name="color">The color</param>
        /// <returns>Prefix for string background colorization</returns>
        [NotNull]
        public static string GetBackgroundColorizationString(Color color)
        {
            return GetBackgroundColorizationString(color.R, color.G, color.B);
        }

        /// <summary>
        ///     Colorizes string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="r">Red color</param>
        /// <param name="g">Green color</param>
        /// <param name="b">Blue color</param>
        /// <returns>Colorized string</returns>
        [NotNull]
        public static string Colorize([NotNull] this string str, uint r, uint g, uint b)
        {
            var colorized = GetColorizationString(r, g, b) + str;
            return !str.EndsWith(Font.Reset, StringComparison.Ordinal) ? colorized + Font.Reset : colorized;
        }

        /// <summary>
        ///     Colorizes string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="color">The color</param>
        /// <returns>Colorized string</returns>
        [NotNull]
        public static string Colorize([NotNull] this string str, Color color)
        {
            return color.IsEmpty ? str : Colorize(str, color.R, color.G, color.B);
        }

        /// <summary>
        ///     Colorizes string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="hex">The color in hex</param>
        /// <returns>Colorized string</returns>
        [NotNull]
        public static string Colorize([NotNull] this string str, [NotNull] string hex)
        {
            return Colorize(str, HexToColor(hex));
        }

        /// <summary>
        ///     Colorizes string's background
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="r">Red color</param>
        /// <param name="g">Green color</param>
        /// <param name="b">Blue color</param>
        /// <returns>Colorized string</returns>
        [NotNull]
        public static string ColorizeBackground([NotNull] this string str, uint r, uint g, uint b)
        {
            var colorized = GetBackgroundColorizationString(r, g, b) + str;
            return !str.EndsWith(Font.Reset, StringComparison.Ordinal) ? colorized + Font.Reset : colorized;
        }

        /// <summary>
        ///     Colorizes string's background
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="color">The color</param>
        /// <returns>Colorized string</returns>
        [NotNull]
        public static string ColorizeBackground([NotNull] this string str, Color color)
        {
            return ColorizeBackground(str, color.R, color.G, color.B);
        }

        /// <summary>
        ///     Colorizes string's background
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="hex">The color in hex</param>
        /// <returns>Colorized string</returns>
        [NotNull]
        public static string ColorizeBackground([NotNull] this string str, [NotNull] string hex)
        {
            return ColorizeBackground(str, HexToColor(hex));
        }

        /// <summary>
        ///     Removes all colorization marks from string
        /// </summary>
        /// <param name="s">The string</param>
        /// <returns>De colorized string</returns>
        [NotNull]
        public static string DeColorize([NotNull] this string s)
        {
            return ColorsRegex.Replace(s, "");
        }

        /// <summary>
        ///     Converts hex color to <see cref="Color" />
        /// </summary>
        /// <param name="hex">Hex string</param>
        /// <returns>
        ///     <see cref="Color" />
        /// </returns>
        public static Color HexToColor([NotNull] string hex)
        {
            hex = hex.Replace("#", "");

            return Color.FromArgb(int.Parse(hex, NumberStyles.AllowHexSpecifier));
        }
    }
}