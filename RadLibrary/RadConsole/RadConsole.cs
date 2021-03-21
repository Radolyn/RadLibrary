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
    public static class RadConsole
    {
        #region Console Events

        /// <summary>
        ///     Occurs when the Control modifier key (Ctrl) and either the C console key (C) or the Break key are pressed
        ///     simultaneously (Ctrl+C or Ctrl+Break).
        /// </summary>
        public static event ConsoleCancelEventHandler CancelKeyPress
        {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        #endregion

        #region Console Methods

        /// <summary>
        ///     Plays the sound of a beep through the console speaker.
        /// </summary>
        public static void Beep()
        {
            Console.Beep();
        }

        /// <summary>
        ///     Plays the sound of a beep of a specified frequency and duration through the console speaker.
        /// </summary>
        /// <param name="frequency">The frequency of the beep, ranging from 37 to 32767 hertz.</param>
        /// <param name="duration">The duration of the beep measured in milliseconds.</param>
        public static void Beep(int frequency, int duration)
        {
            Console.Beep(frequency, duration);
        }

        /// <summary>
        ///     Clears the console buffer and corresponding console window of display information.
        /// </summary>
        public static void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        ///     Copies a specified source area of the screen buffer to a specified destination area.
        /// </summary>
        /// <param name="sourceLeft">The leftmost column of the source area.</param>
        /// <param name="sourceTop">The topmost row of the source area.</param>
        /// <param name="sourceWidth">The number of columns in the source area.</param>
        /// <param name="sourceHeight">The number of rows in the source area.</param>
        /// <param name="targetLeft">The leftmost column of the destination area.</param>
        /// <param name="targetTop">The topmost row of the destination area.</param>
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        }

        /// <summary>
        ///     Copies a specified source area of the screen buffer to a specified destination area.
        /// </summary>
        /// <param name="sourceLeft">The leftmost column of the source area.</param>
        /// <param name="sourceTop">The topmost row of the source area.</param>
        /// <param name="sourceWidth">The number of columns in the source area.</param>
        /// <param name="sourceHeight">The number of rows in the source area.</param>
        /// <param name="targetLeft">The leftmost column of the destination area.</param>
        /// <param name="targetTop">The topmost row of the destination area.</param>
        /// <param name="sourceChar">The character used to fill the source area.</param>
        /// <param name="sourceForeColor">The foreground color used to fill the source area.</param>
        /// <param name="sourceBackColor">The background color used to fill the source area.</param>
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar,
                sourceForeColor, sourceBackColor);
        }

        /// <summary>
        ///     Acquires the standard error stream.
        /// </summary>
        /// <returns>The standard error stream.</returns>
        public static Stream OpenStandardError()
        {
            return Console.OpenStandardError();
        }

        /// <summary>
        ///     Acquires the standard error stream, which is set to a specified buffer size.
        /// </summary>
        /// <param name="bufferSize">The internal stream buffer size.</param>
        /// <returns>The standard error stream.</returns>
        public static Stream OpenStandardError(int bufferSize)
        {
            return Console.OpenStandardError(bufferSize);
        }

        /// <summary>
        ///     Acquires the standard input stream.
        /// </summary>
        /// <returns>The standard input stream.</returns>
        public static Stream OpenStandardInput()
        {
            return Console.OpenStandardInput();
        }

        /// <summary>
        ///     Acquires the standard input stream, which is set to a specified buffer size.
        /// </summary>
        /// <param name="bufferSize">The internal stream buffer size.</param>
        /// <returns>The standard input stream.</returns>
        public static Stream OpenStandardInput(int bufferSize)
        {
            return Console.OpenStandardInput(bufferSize);
        }

        /// <summary>
        ///     Acquires the standard output stream.
        /// </summary>
        /// <returns>The standard output stream.</returns>
        public static Stream OpenStandardOutput()
        {
            return Console.OpenStandardOutput();
        }

        /// <summary>
        ///     Acquires the standard output stream, which is set to a specified buffer size.
        /// </summary>
        /// <param name="bufferSize">The internal stream buffer size.</param>
        /// <returns>The standard output stream.</returns>
        public static Stream OpenStandardOutput(int bufferSize)
        {
            return Console.OpenStandardOutput(bufferSize);
        }

        /// <summary>
        ///     Sets the foreground and background console colors to their defaults.
        /// </summary>
        public static void ResetColor()
        {
            Console.ResetColor();
            Console.Write(Font.Reset);
        }

        /// <summary>
        ///     Sets the height and width of the screen buffer area to the specified values.
        /// </summary>
        /// <param name="width">The width of the buffer area measured in columns.</param>
        /// <param name="height">The height of the buffer area measured in rows.</param>
        public static void SetBufferSize(int width, int height)
        {
            Console.SetBufferSize(width, height);
        }

        /// <summary>
        ///     Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        public static void SetCursorPosition(int left, int top)
        {
            if (BufferHeight < top)
                BufferHeight += top - BufferHeight;

            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        ///     Sets the <see cref="Error" /> property to the specified <see cref="TextWriter" /> object.
        /// </summary>
        /// <param name="newError">A stream that is the new standard error output.</param>
        public static void SetError([NotNull] TextWriter newError)
        {
            Console.SetError(newError);
        }

        /// <summary>
        ///     Sets the <see cref="Error" /> property to the specified <see cref="TextWriter" /> object.
        /// </summary>
        /// <param name="newIn">A stream that is the new standard input.</param>
        public static void SetIn([NotNull] TextReader newIn)
        {
            Console.SetIn(newIn);
        }

        /// <summary>
        ///     Sets the <see cref="Error" /> property to the specified <see cref="TextWriter" /> object.
        /// </summary>
        /// <param name="newOut">A stream that is the new standard output output.</param>
        public static void SetOut(TextWriter newOut)
        {
            Console.SetOut(newOut);
        }

        /// <summary>
        ///     Sets the position of the console window relative to the screen buffer.
        /// </summary>
        /// <param name="left">The column position of the upper left corner of the console window.</param>
        /// <param name="top">The row position of the upper left corner of the console window.</param>
        public static void SetWindowPosition(int left, int top)
        {
            Console.SetWindowPosition(left, top);
        }

        /// <summary>
        ///     Sets the height and width of the console window to the specified values.
        /// </summary>
        /// <param name="width">The width of the console window measured in columns.</param>
        /// <param name="height">The height of the console window measured in rows.</param>
        public static void SetWindowSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
        }

        /// <summary>
        ///     Gets the position of the cursor.
        /// </summary>
        /// <returns>The row and column position of the cursor.</returns>
        public static (int Top, int Left) GetCursorPosition()
        {
            return (Console.CursorTop, Console.CursorLeft);
        }

        #endregion

        #region Console Properties

        /// <summary>
        ///     Gets or sets the background color of the console.
        /// </summary>
        public static ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <summary>
        ///     Gets or sets the height of the buffer area.
        /// </summary>
        public static int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        /// <summary>
        ///     Gets or sets the width of the buffer area.
        /// </summary>
        public static int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }

        /// <summary>
        ///     Gets a value indicating whether the CAPS LOCK keyboard toggle is turned on or turned off.
        /// </summary>
        public static bool CapsLock => Console.CapsLock;

        /// <summary>
        ///     Gets or sets the column position of the cursor within the buffer area.
        /// </summary>
        public static int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        /// <summary>
        ///     Gets or sets the height of the cursor within a character cell.
        /// </summary>
        public static int CursorSize
        {
            get => Console.CursorSize;
            set => Console.CursorSize = value;
        }

        /// <summary>
        ///     Gets or sets the row position of the cursor within the buffer area.
        /// </summary>
        public static int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        public static bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        /// <summary>
        ///     Gets the standard error output stream.
        /// </summary>
        public static TextWriter Error => Console.Error;

        /// <summary>
        ///     Gets or sets the foreground color of the console.
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <summary>
        ///     Gets the standard input stream.
        /// </summary>
        public static TextReader In => Console.In;

        /// <summary>
        ///     Gets or sets the encoding the console uses to read input.
        /// </summary>
        public static Encoding InputEncoding
        {
            get => Console.InputEncoding;
            set => Console.InputEncoding = value;
        }

        /// <summary>
        ///     Gets a value that indicates whether the error output stream has been redirected from the standard error stream.
        /// </summary>
        public static bool IsErrorRedirected => Console.IsErrorRedirected;

        /// <summary>
        ///     Gets a value that indicates whether input has been redirected from the standard input stream.
        /// </summary>
        public static bool IsInputRedirected => Console.IsInputRedirected;

        /// <summary>
        ///     Gets a value that indicates whether output has been redirected from the standard output stream.
        /// </summary>
        public static bool IsOutputRedirected => Console.IsOutputRedirected;

        /// <summary>
        ///     Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        public static bool KeyAvailable => Console.KeyAvailable;

        /// <summary>
        ///     Gets the largest possible number of console window rows, based on the current font and screen resolution.
        /// </summary>
        public static int LargestWindowHeight => Console.LargestWindowHeight;

        /// <summary>
        ///     Gets the largest possible number of console window columns, based on the current font and screen resolution.
        /// </summary>
        public static int LargestWindowWidth => Console.LargestWindowWidth;

        /// <summary>
        ///     Gets a value indicating whether the NUM LOCK keyboard toggle is turned on or turned off.
        /// </summary>
        public static bool NumberLock => Console.NumberLock;

        /// <summary>
        ///     Gets the standard output stream.
        /// </summary>
        public static TextWriter Out => Console.Out;

        /// <summary>
        ///     Gets or sets the encoding the console uses to write output.
        /// </summary>
        public static Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => Console.OutputEncoding = value;
        }

        /// <summary>
        ///     Gets or sets the title to display in the console title bar.
        /// </summary>
        public static string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the combination of the Control modifier key and C console key (Ctrl+C) is
        ///     treated as ordinary input or as an interruption that is handled by the operating system.
        /// </summary>
        public static bool TreatControlCAsInput
        {
            get => Console.TreatControlCAsInput;
            set => Console.TreatControlCAsInput = value;
        }

        /// <summary>
        ///     Gets or sets the height of the console window area.
        /// </summary>
        public static int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        /// <summary>
        ///     Gets or sets the leftmost position of the console window area relative to the screen buffer.
        /// </summary>
        public static int WindowLeft
        {
            get => Console.WindowLeft;
            set => Console.WindowLeft = value;
        }

        /// <summary>
        ///     Gets or sets the top position of the console window area relative to the screen buffer.
        /// </summary>
        public static int WindowTop
        {
            get => Console.WindowTop;
            set => Console.WindowTop = value;
        }

        /// <summary>
        ///     Gets or sets the width of the console window.
        /// </summary>
        public static int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        #endregion

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

            Write(prediction.Colorize(readStyle.PredictionColor));

            SetCursorPosition(startPosition.Left, startPosition.Top);

            var part1 = line.ToString(0, currentPosition);
            Write(part1.Colorize(readStyle.InputColor));

            if (part1.Length != line.Length)
            {
                var part2 = line.ToString(currentPosition, line.Length - currentPosition);

                var currentPos = GetCursorPosition();

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
            Write(message + Environment.NewLine);
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
            Write(format + Environment.NewLine, args);
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

            Write(format.ToString(0, format.Length - 1) + Environment.NewLine,
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

            return sb + Font.Reset;
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