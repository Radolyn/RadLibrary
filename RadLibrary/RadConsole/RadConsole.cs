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
        public static (int, int) GetCursorPosition()
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
        public static string ReadLine([NotNull] ReadStyle readStyle)
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
        public static string ReadLine([NotNull] ReadStyle readStyle, [CanBeNull] IPredictionEngine predictionEngine)
        {
            var sb = new StringBuilder();

            Console.Write(readStyle.Prefix + " ");

            var (startTop, startLeft) = GetCursorPosition();

            var startOffset = 0;

            var savedInput = "";
            var savedInputCounter = 0;
            var previousSize = 0;

            string pred;

            while (true)
            {
                var input = Console.ReadKey();
                pred = predictionEngine?.Predict(sb.ToString()) ?? "";

                if (input.Key == ConsoleKey.Enter)
                    break;

                previousSize = sb.Length;

                switch (input.Key)
                {
                    // auto-completion
                    case ConsoleKey.Tab:

                        if (string.IsNullOrEmpty(pred) || sb.Length > pred.Length || sb.ToString() == pred)
                            break;

                        sb.Clear();
                        sb.Append(pred);

                        startOffset = pred.Length;

                        break;
                    // navigation
                    case ConsoleKey.LeftArrow:

                        if (startOffset > 0 && sb.Length >= startOffset)
                            --startOffset;

                        break;
                    case ConsoleKey.RightArrow:

                        if (sb.Length > startOffset)
                            ++startOffset;

                        break;
                    case ConsoleKey.PageUp:

                        startOffset = 0;

                        break;
                    case ConsoleKey.PageDown:

                        startOffset = sb.Length;

                        break;
                    // editing
                    case ConsoleKey.Backspace:

                        if (startOffset == 0)
                            break;

                        sb.Remove(startOffset - 1, 1);
                        --startOffset;

                        break;
                    // history
                    case ConsoleKey.UpArrow:

                        if (InputHistory.Count <= savedInputCounter)
                            break;

                        if (string.IsNullOrEmpty(savedInput))
                            savedInput = sb.ToString();

                        previousSize = sb.Length;

                        sb.Clear();
                        sb.Append(InputHistory[savedInputCounter++]);

                        startOffset = sb.Length;

                        break;
                    case ConsoleKey.DownArrow:

                        if (savedInputCounter == 0)
                            break;

                        if (savedInputCounter == 1)
                        {
                            previousSize = sb.Length;

                            sb.Clear();
                            sb.Append(savedInput);

                            --savedInputCounter;

                            startOffset = sb.Length;

                            break;
                        }

                        sb.Clear();
                        sb.Append(InputHistory[savedInputCounter--]);

                        startOffset = sb.Length;

                        break;
                    default:

                        sb.Insert(startOffset, input.KeyChar);
                        ++startOffset;

                        break;
                }

                ReDraw(sb, pred, startOffset, startLeft, startTop, previousSize, readStyle);
            }

            if (pred.Length > sb.Length)
                previousSize = pred.Length;

            startOffset = sb.Length;
            ReDraw(sb, "", startOffset, startLeft, startTop, previousSize, readStyle);

            Console.WriteLine(" " + readStyle.Postfix);

            var s = sb.ToString();
            InputHistory.Add(s);

            return s;
        }

        private static void ReDraw(StringBuilder sb, string pred, int startOffset, int startLeft, int startTop,
            int previousSize, ReadStyle readStyle)
        {
            CursorVisible = false;

            // prediction
            SetCursorPosition(startLeft, startTop);

            if (readStyle.UnderlinePrediction)
                Console.Write(Font.UnderlineFont);

            Console.Write(pred.Colorize(readStyle.PredictionColor));


            // input
            SetCursorPosition(startLeft, startTop);

            if (readStyle.UnderlineInput)
                Console.Write(Font.UnderlineFont);

            Console.Write(sb.ToString().Colorize(readStyle.InputColor) +
                          (pred.Length > previousSize ? "" : " ".Repeat(previousSize)));

            SetCursorPosition(startLeft, startTop);


            // $$$ean$$$ $$$merti
            var leftEnd = (startLeft + startOffset) % Console.BufferWidth;
            var topEnd = (startLeft + startOffset) / Console.BufferWidth + startTop;

            SetCursorPosition(leftEnd, topEnd);

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
                if (!escapeSymbol && ch == '\\')
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

            return sb + Font.Reset;
        }

        private static void HandleColor(string color, [NotNull] StringBuilder sb)
        {
            var background = color.StartsWith("b:", StringComparison.Ordinal);

            if (background)
                color = color.Remove(0, 2).ToLower();

            switch (color)
            {
                case "reset": // reset
                    sb.Append(background
                        ? Background.Reset
                        : Foreground.Reset);
                    break;
                case "underline": // underline
                    sb.Append(Font.UnderlineFont);
                    break;
                case "bold": // bold
                    sb.Append(Font.BoldFont);
                    break;
                case "blink": // blink
                    sb.Append(Font.SlowBlinkFont);
                    break;
                case "italic": // italic
                    sb.Append(Font.ItalicFont);
                    break;
                case "framed": // framed
                    sb.Append(Font.FramedFont);
                    break;
                // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                case "black":
                    sb.Append(background
                        ? Background.Black
                        : Foreground.Black);
                    break;
                case "blue":
                    sb.Append(background
                        ? Background.Blue
                        : Foreground.Blue);
                    break;
                case "green":
                    sb.Append(background
                        ? Background.Green
                        : Foreground.Green);
                    break;
                case "purple":
                    sb.Append(background
                        ? Background.Purple
                        : Foreground.Purple);
                    break;
                case "red":
                    sb.Append(background
                        ? Background.Red
                        : Foreground.Red);
                    break;
                case "white":
                    sb.Append(background
                        ? Background.White
                        : Foreground.White);
                    break;
                case "yellow":
                    sb.Append(background
                        ? Background.Yellow
                        : Foreground.Yellow);
                    break;
                // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                case "brightblack":
                    sb.Append(background
                        ? Background.BrightBlack
                        : Foreground.BrightBlack);
                    break;
                case "brightblue":
                    sb.Append(background
                        ? Background.BrightBlue
                        : Foreground.BrightBlue);
                    break;
                case "brightgreen":
                    sb.Append(background
                        ? Background.BrightGreen
                        : Foreground.BrightGreen);
                    break;
                case "brightpurple":
                    sb.Append(background
                        ? Background.BrightPurple
                        : Foreground.BrightPurple);
                    break;
                case "brightred":
                    sb.Append(background
                        ? Background.BrightRed
                        : Foreground.BrightRed);
                    break;
                case "brightwhite":
                    sb.Append(background
                        ? Background.BrightWhite
                        : Foreground.BrightWhite);
                    break;
                case "brightyellow":
                    sb.Append(background
                        ? Background.BrightYellow
                        : Foreground.BrightYellow);
                    break;
                // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                default:
                    sb.Append(background
                        ? Colorizer.GetBackgroundColorizationString(Colorizer.HexToColor(color))
                        : Colorizer.GetColorizationString(Colorizer.HexToColor(color)));
                    break;
            }
        }

        #endregion

        #region RadConsole Properties

        /// <summary>
        ///     Gets or sets style for <see cref="ReadLine()" />
        /// </summary>
        public static ReadStyle ReadStyle { get; set; } = new();

        /// <summary>
        ///     Gets or sets prediction engine for <see cref="ReadLine()" />
        /// </summary>
        public static IPredictionEngine PredictionEngine { get; set; }

        /// <summary>
        ///     Gets input history
        /// </summary>
        [NotNull]
        public static IEnumerable<string> History => InputHistory.AsReadOnly();

        private static readonly List<string> InputHistory = new();
        private static readonly DefaultPredictionEngine DefaultPredictionEngine = new();

        #endregion
    }
}