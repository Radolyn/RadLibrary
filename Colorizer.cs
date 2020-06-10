#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

#endregion

namespace RadLibrary
{
    public static class Colorizer
    {
        private static bool _isInitialized;

        private const int StdOutputHandle = -11;
        private const uint EnableVirtualTerminalProcessing = 0x0004;
        private const uint DisableNewlineAutoReturn = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        ///     Initializes colors system
        /// </summary>
        /// <exception cref="Win32Exception">If failed to set color mode</exception>
        public static void Initialize()
        {
            if (!Utilities.IsWindows() || _isInitialized)
                return;

            // todo: support for old terminals ($COLORTERM)

            var iStdOut = GetStdHandle(StdOutputHandle);
            if (!GetConsoleMode(iStdOut, out var outConsoleMode))
                throw new Win32Exception("Failed to get output console mode");

            outConsoleMode |= EnableVirtualTerminalProcessing | DisableNewlineAutoReturn;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
                throw new Win32Exception(
                    $"Failed to set output console mode, error code: {Marshal.GetLastWin32Error()}");

            _isInitialized = true;
        }

        /// <summary>
        ///     Colorizes string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="r">Red color</param>
        /// <param name="g">Green color</param>
        /// <param name="b">Blue color</param>
        /// <returns>Colorized string</returns>
        public static string Colorize(this string str, uint r, uint g, uint b)
        {
            return $"\x1b[38;2;{r};{g};{b}m" + str + "\x1b[0m";
        }

        /// <summary>
        ///     Colorizes string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="color">The color</param>
        /// <returns>Colorized string</returns>
        public static string Colorize(this string str, Color color)
        {
            return color.IsEmpty ? str : Colorize(str, color.R, color.G, color.B);
        }

        /// <summary>
        ///     Colorizes string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="hex">The color in hex</param>
        /// <returns>Colorized string</returns>
        public static string Colorize(this string str, string hex)
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
        public static string ColorizeBackground(this string str, uint r, uint g, uint b)
        {
            return $"\x1b[48;2;{r};{g};{b}m" + str + "\x1b[0m";
        }

        /// <summary>
        ///     Colorizes string's background
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="color">The color</param>
        /// <returns>Colorized string</returns>
        public static string ColorizeBackground(this string str, Color color)
        {
            return ColorizeBackground(str, color.R, color.G, color.B);
        }

        /// <summary>
        ///     Colorizes string's background
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="hex">The color in hex</param>
        /// <returns>Colorized string</returns>
        public static string ColorizeBackground(this string str, string hex)
        {
            return ColorizeBackground(str, HexToColor(hex));
        }

        /// <summary>
        ///     Converts hex color to <see cref="Color" />
        /// </summary>
        /// <param name="hex">Hex string</param>
        /// <returns>
        ///     <see cref="Color" />
        /// </returns>
        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("#", "");

            var r = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            var g = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            var b = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            return Color.FromArgb(r, g, b);
        }
    }
}