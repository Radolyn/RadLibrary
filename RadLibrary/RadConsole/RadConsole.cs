#region

using System;
using System.Collections.Generic;
using System.IO;
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
        #region RadConsole Methods

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with default read style and prediction engine.
        /// </summary>
        /// <returns>The next line of characters from the input stream</returns>
        public static string ReadLine()
        {
            return ReadLine(ReadStyle, DefaultPredictionEngine);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with specified read style and default prediction
        ///     engine.
        /// </summary>
        /// <param name="readStyle">The read style</param>
        /// <returns>The next line of characters from the input stream</returns>
        public static string ReadLine([NotNull] IReadStyle readStyle)
        {
            return ReadLine(readStyle, DefaultPredictionEngine);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with default read style and specified prediction
        ///     engine.
        /// </summary>
        /// <param name="predictionEngine">The prediction engine</param>
        /// <returns>The next line of characters from the input stream</returns>
        public static string ReadLine([CanBeNull] IPredictionEngine predictionEngine)
        {
            return ReadLine(ReadStyle, predictionEngine);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with specified prefix and default prediction
        ///     engine.
        /// </summary>
        /// <param name="prefix">The prefix</param>
        /// <param name="usePredictionEngine">Use default prediction engine or not</param>
        /// <returns>The next line of characters from the input stream</returns>
        public static string ReadLine([CanBeNull] string prefix, bool usePredictionEngine = true)
        {
            return ReadLine(new ReadStyle
            {
                Prefix = prefix
            }, usePredictionEngine ? DefaultPredictionEngine : null);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with specified read style and prediction engine.
        /// </summary>
        /// <param name="readStyle">The read style</param>
        /// <param name="predictionEngine">The prediction engine</param>
        /// <returns>The next line of characters from the input stream</returns>
        public static string ReadLine([NotNull] IReadStyle readStyle, [CanBeNull] IPredictionEngine predictionEngine)
        {
            // print prefix
            Write(readStyle.ColorizedPrefix);

            var line = new StringBuilder();
            var startPosition = GetCursorPosition();

            var currentPosition = 0;
            var biggestInput = 0;

            var currentHistory = -1;
            var savedInputBeforeHistory = "";

            var prediction = "";

            var stop = false;

            while (!stop)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    // if key is arrow
                    case >= ConsoleKey.LeftArrow and <= ConsoleKey.DownArrow:
                        ProcessArrows(key, ref currentPosition, ref currentHistory, ref savedInputBeforeHistory, line);
                        break;
                    case ConsoleKey.Tab:
                        ProcessPrediction(prediction, ref currentPosition, line);
                        break;
                    case ConsoleKey.Backspace:
                        ProcessBackspace(line, ref currentPosition);
                        break;
                    case ConsoleKey.Enter:
                        stop = true;
                        break;
                    default:
                        line.Insert(currentPosition, key.KeyChar);
                        ++currentPosition;
                        break;
                }

                prediction = predictionEngine?.Predict(line.ToString());

                // todo: optimize biggest input

                if (biggestInput < prediction?.Length)
                    biggestInput = prediction.Length;

                if (biggestInput < line.Length)
                    biggestInput = line.Length;

                UpdateScreen(startPosition, readStyle, currentPosition, biggestInput, prediction, line);
            }

            UpdateScreen(startPosition, readStyle, line.Length, biggestInput, prediction, line);

            Write(readStyle.ColorizedPostfix);

            WriteLine();

            var res = line.ToString();
            InputHistory.Add(res);

            return res;
        }

        private static void ProcessArrows(ConsoleKeyInfo key, ref int currentPosition, ref int currentHistory,
            ref string savedInputBeforeHistory,
            StringBuilder line)
        {
            if (key.Key is ConsoleKey.LeftArrow && currentPosition != 0)
                --currentPosition;
            if (key.Key is ConsoleKey.RightArrow && currentPosition != line.Length)
                ++currentPosition;

            void SetLine(string s, ref int currentPosition)
            {
                line.Clear();
                line.Append(s);
                currentPosition = s.Length;
            }

            if (key.Key is ConsoleKey.UpArrow)
            {
                if (currentHistory == -1) savedInputBeforeHistory = line.ToString();
                if (currentHistory == InputHistory.Count - 1)
                    return;

                ++currentHistory;
                SetLine(InputHistory[currentHistory], ref currentPosition);
            }

            if (key.Key is ConsoleKey.DownArrow)
            {
                if (currentHistory == 0)
                {
                    SetLine(savedInputBeforeHistory, ref currentPosition);
                    --currentHistory;
                    return;
                }

                if (currentHistory == -1)
                    return;

                --currentHistory;
                SetLine(InputHistory[currentHistory], ref currentPosition);
            }
        }

        private static void ProcessPrediction(string prediction, ref int currentPosition, StringBuilder line)
        {
            line.Clear();
            line.Append(prediction);

            currentPosition = line.Length;
        }

        private static void ProcessBackspace(StringBuilder line, ref int currentPosition)
        {
            if (currentPosition == 0)
                return;

            line.Remove(currentPosition - 1, 1);
            --currentPosition;
        }

        private static void UpdateScreen((int Top, int Left) startPosition, IReadStyle readStyle, int currentPosition,
            int biggestInput,
            string prediction, StringBuilder line)
        {
            CursorVisible = false;

            SetCursorPosition(startPosition.Left, startPosition.Top);

            Write(" ".Repeat(biggestInput));

            SetCursorPosition(startPosition.Left, startPosition.Top);

            Write(readStyle.PredictionDecorations);
            Write(prediction.Colorize(readStyle.PredictionColor));

            SetCursorPosition(startPosition.Left, startPosition.Top);

            var part1 = line.ToString(0, currentPosition);

            Write(readStyle.InputDecorations);
            Write(part1.Colorize(readStyle.InputColor));

            if (part1.Length != line.Length)
            {
                var part2 = line.ToString(currentPosition, line.Length - currentPosition);

                var currentPos = GetCursorPosition();

                Write(readStyle.InputDecorations);
                Write(part2.Colorize(readStyle.InputColor));

                SetCursorPosition(currentPos.Left, currentPos.Top);
            }

            CursorVisible = true;
        }

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
            var background = color.StartsWith("b:", StringComparison.Ordinal);

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

        #region RadConsole Properties

        /// <summary>
        ///     Gets or sets style for <see cref="ReadLine()" />
        /// </summary>
        public static IReadStyle ReadStyle { get; set; } = new ReadStyle();

        /// <summary>
        ///     Gets or sets prediction engine for <see cref="ReadLine()" />
        /// </summary>
        public static IPredictionEngine PredictionEngine { get; set; }

        /// <summary>
        ///     Gets input history
        /// </summary>
        [NotNull]
        public static IEnumerable<string> History => InputHistory.AsReadOnly();

        private static readonly List<string> InputHistory = new() {"1", "2"};
        private static readonly DefaultPredictionEngine DefaultPredictionEngine = new();

        #endregion
    }
}