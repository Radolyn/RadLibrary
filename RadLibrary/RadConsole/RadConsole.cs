#region

using System;
using System.Text;
using JetBrains.Annotations;
using RadLibrary.Colors;
using RadLibrary.Formatting;

#endregion

namespace RadLibrary.RadConsole
{
    /// <summary>
    ///     Wrapper for <see cref="Console" />. Represents the standard input, output, and error streams for console
    ///     applications.
    /// </summary>
    public static partial class RadConsole
    {
        public static ConsoleRead Read { get; } = new();

        #region RadConsole Methods

        /// <summary>
        ///     Writes the text representation of the specified value or values to the standard output stream.
        /// </summary>
        /// <param name="message">The message</param>
        public static void Write([CanBeNull] string message)
        {
            message = ParseColors(message);

            Console.Write(message);
        }

        /// <summary>
        ///     Writes the specified data, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="message">The message</param>
        public static void WriteLine([CanBeNull] string message)
        {
            Write(message + Font.Reset + Environment.NewLine);
        }

        /// <summary>
        ///     Writes the text representation of the specified value or values to the standard output stream.
        /// </summary>
        /// <param name="format">The format</param>
        /// <param name="args">The objects</param>
        [StringFormatMethod("format")]
        public static void Write([NotNull] string format, [CanBeNull] params object[] args)
        {
            args ??= new object[] {null};

            format = ParseColors(format);

            Console.Write(string.Format(FormattersStorage.FormatProvider, format, args));
        }

        /// <summary>
        ///     Writes the specified data, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="format">The format</param>
        /// <param name="args">The objects</param>
        [StringFormatMethod("format")]
        public static void WriteLine([NotNull] string format, [CanBeNull] params object[] args)
        {
            Write(format + Font.Reset + Environment.NewLine, args);
        }

        /// <summary>
        ///     Writes the text representation of the specified value or values to the standard output stream.
        /// </summary>
        /// <param name="args">The objects</param>
        public static void Write([CanBeNull] params object[] args)
        {
            args ??= new object[] {null};

            for (var i = 0; i < args.Length; i++)
                if (args[i] is not null && args[i] is string)
                    args[i] = ParseColors(args[i].ToString());

            // "{.} "
            var format = new StringBuilder(args.Length * 4);

            for (var i = 0; i < args.Length; i++) format.Append("{" + i + "} ");

            Write(format.ToString(0, format.Length - 1), args);
        }

        /// <summary>
        ///     Writes current line terminator to the standard output stream.
        /// </summary>
        public static void WriteLine()
        {
            Console.WriteLine();
        }

        /// <summary>
        ///     Writes the specified data, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="args">The objects</param>
        public static void WriteLine([CanBeNull] params object[] args)
        {
            args ??= new object[] {null};

            for (var i = 0; i < args.Length; i++)
                if (args[i] is not null && args[i] is string)
                    args[i] = ParseColors(args[i].ToString());

            // "{.} "
            var format = new StringBuilder(args.Length * 4);

            for (var i = 0; i < args.Length; i++) format.Append("{" + i + "} ");

            Write(format.ToString(0, format.Length - 1) + Font.Reset + Environment.NewLine,
                args); // todo: this method is the same as Write except Environment.NewLine
        }

        /// <summary>
        ///     Converts colors in the message to console's colors
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The converted message</returns>
        [CanBeNull]
        public static string ParseColors([CanBeNull] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return message;

            var sb = new StringBuilder(message.Length / 2);
            var colorSb = new StringBuilder(10);

            var colorPart = false;
            var escapeSymbol = false;

            void WriteSb(char ch)
            {
                if (colorPart)
                    colorSb.Append(ch);
                else
                    sb.Append(ch);
            }

            foreach (var ch in message)
            {
                if (!escapeSymbol && ch is '\\')
                {
                    escapeSymbol = true;
                    continue;
                }

                if (escapeSymbol && ch is '[' or ']')
                {
                    WriteSb(ch);
                    escapeSymbol = false;
                    continue;
                }

                if (escapeSymbol)
                {
                    WriteSb('\\');

                    if (ch != '\\')
                    {
                        WriteSb(ch);
                        escapeSymbol = false;
                    }

                    continue;
                }

                switch (ch)
                {
                    case '[':
                        if (colorPart)
                        {
                            sb.Append('[');
                            sb.Append(colorSb);

                            colorSb.Clear();
                        }

                        colorPart = true;
                        break;
                    case ']':
                        colorPart = false;
                        var color = colorSb.ToString();

                        HandleColor(color, sb);

                        colorSb.Clear();
                        break;
                    default:
                        WriteSb(ch);
                        break;
                }
            }

            if (colorSb.Length != 0)
            {
                sb.Append('[');
                sb.Append(colorSb);
            }

            return sb.ToString();
        }

        private static void HandleColor(string color, [NotNull] StringBuilder sb)
        {
            var background = color.StartsWith("b:", StringComparison.OrdinalIgnoreCase);

            if (background)
                color = color.Remove(0, 2).ToLower();

            var s = color switch
            {
                "reset" => background ? Background.Reset : Foreground.Reset,
                "underline" => Font.UnderlineFont,
                "bold" => Font.BoldFont,
                "blink" => Font.SlowBlinkFont,
                "italic" => Font.ItalicFont,
                "framed" => Font.FramedFont,
                // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                "black" => background ? Background.Black : Foreground.Black,
                "blue" => background ? Background.Blue : Foreground.Blue,
                "green" => background ? Background.Green : Foreground.Green,
                "purple" => background ? Background.Purple : Foreground.Purple,
                "red" => background ? Background.Red : Foreground.Red,
                "white" => background ? Background.White : Foreground.White,
                "yellow" => background ? Background.Yellow : Foreground.Yellow,
                // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                "brightblack" => background ? Background.BrightBlack : Foreground.BrightBlack,
                "brightblue" => background ? Background.BrightBlue : Foreground.BrightBlue,
                "brightgreen" => background ? Background.BrightGreen : Foreground.BrightGreen,
                "brightpurple" => background ? Background.BrightPurple : Foreground.BrightPurple,
                "brightred" => background ? Background.BrightRed : Foreground.BrightRed,
                "brightwhite" => background ? Background.BrightWhite : Foreground.BrightWhite,
                "brightyellow" => background ? Background.BrightYellow : Foreground.BrightYellow,
                _ => background // todo: if failed (Exception), just append [string] to sb
                    ? Colorizer.GetBackgroundColorizationString(Colorizer.HexToColor(color))
                    : Colorizer.GetColorizationString(Colorizer.HexToColor(color))
            };

            sb.Append(s);
        }

        #endregion
    }
}