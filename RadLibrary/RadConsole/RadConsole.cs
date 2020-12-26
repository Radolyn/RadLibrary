#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using RadLibrary.Colors;
using RadLibrary.Formatting;

#endregion

namespace RadLibrary.RadConsole
{
    public static class RadConsole
    {
        #region Console Events

        public static event ConsoleCancelEventHandler CancelKeyPress
        {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        #endregion

        #region Console Methods

        public static void Beep()
        {
            Console.Beep();
        }

        public static void Beep(int frequency, int duration)
        {
            Console.Beep(frequency, duration);
        }

        public static void Clear()
        {
            Console.Clear();
        }

        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        }

        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar,
                sourceForeColor, sourceBackColor);
        }

        public static Stream OpenStandardError()
        {
            return Console.OpenStandardError();
        }

        public static Stream OpenStandardError(int bufferSize)
        {
            return Console.OpenStandardError(bufferSize);
        }

        public static Stream OpenStandardInput()
        {
            return Console.OpenStandardInput();
        }

        public static Stream OpenStandardInput(int bufferSize)
        {
            return Console.OpenStandardInput(bufferSize);
        }

        public static Stream OpenStandardOutput()
        {
            return Console.OpenStandardOutput();
        }

        public static Stream OpenStandardOutput(int bufferSize)
        {
            return Console.OpenStandardOutput(bufferSize);
        }

        public static void ResetColor()
        {
            Console.ResetColor();
            Console.Write(Font.Reset);
        }

        public static void SetBufferSize(int width, int height)
        {
            Console.SetBufferSize(width, height);
        }

        public static void SetCursorPosition(int left, int top)
        {
            if (BufferHeight < top)
                BufferHeight += top - BufferHeight;

            Console.SetCursorPosition(left, top);
        }

        public static void SetError(TextWriter newError)
        {
            Console.SetError(newError);
        }

        public static void SetIn(TextReader newIn)
        {
            Console.SetIn(newIn);
        }

        public static void SetOut(TextWriter newOut)
        {
            Console.SetOut(newOut);
        }

        public static void SetWindowPosition(int left, int top)
        {
            Console.SetWindowPosition(left, top);
        }

        public static void SetWindowSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
        }

        public static (int, int) GetCursorPosition()
        {
            return (Console.CursorTop, Console.CursorLeft);
        }

        #endregion

        #region Console Properties

        public static ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public static int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        public static int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }

        public static bool CapsLock => Console.CapsLock;

        public static int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        public static int CursorSize
        {
            get => Console.CursorSize;
            set => Console.CursorSize = value;
        }

        public static int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        public static bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        public static TextWriter Error => Console.Error;

        public static ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public static TextReader In => Console.In;

        public static Encoding InputEncoding
        {
            get => Console.InputEncoding;
            set => Console.InputEncoding = value;
        }

        public static bool IsErrorRedirected => Console.IsErrorRedirected;

        public static bool IsInputRedirected => Console.IsInputRedirected;

        public static bool IsOutputRedirected => Console.IsOutputRedirected;

        public static bool KeyAvailable => Console.KeyAvailable;

        public static int LargestWindowHeight => Console.LargestWindowHeight;

        public static int LargestWindowWidth => Console.LargestWindowWidth;

        public static bool NumberLock => Console.NumberLock;

        public static TextWriter Out => Console.Out;

        public static Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => Console.OutputEncoding = value;
        }

        public static string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }

        public static bool TreatControlCAsInput
        {
            get => Console.TreatControlCAsInput;
            set => Console.TreatControlCAsInput = value;
        }

        public static int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        public static int WindowLeft
        {
            get => Console.WindowLeft;
            set => Console.WindowLeft = value;
        }

        public static int WindowTop
        {
            get => Console.WindowTop;
            set => Console.WindowTop = value;
        }

        public static int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        #endregion

        #region RadConsole Methods

        public static string ReadLine()
        {
            return ReadLine(ReadStyle, DefaultPredictionEngine);
        }

        public static string ReadLine(ReadStyle readStyle)
        {
            return ReadLine(readStyle, DefaultPredictionEngine);
        }

        public static string ReadLine(IPredictionEngine predictionEngine)
        {
            return ReadLine(ReadStyle, predictionEngine);
        }

        public static string ReadLine(string prefix, bool usePredictionEngine = true)
        {
            return ReadLine(new ReadStyle
            {
                Prefix = prefix
            }, usePredictionEngine ? DefaultPredictionEngine : null);
        }

        public static string ReadLine(ReadStyle readStyle, IPredictionEngine predictionEngine)
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

        public static void Write(string message)
        {
            message = ParseColors(message);

            Console.Write(message);
        }

        public static void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }

        public static void Write(string format, params object[] args)
        {
            format = ParseColors(format);

            Console.Write(string.Format(FormattersStorage.FormatProvider, format, args));
        }

        public static void WriteLine(string message, params object[] args)
        {
            Write(message + Environment.NewLine, args);
        }

        public static string ParseColors(string message)
        {
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

                if (escapeSymbol)
                {
                    WriteSb(ch);
                    escapeSymbol = false;
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

            return sb.ToString();
        }

        private static void HandleColor(string color, StringBuilder sb)
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
                default:
                    sb.Append(background
                        ? Colorizer.GetBackgroundColorizationString(Colorizer.HexToColor(color))
                        : Colorizer.GetColorizationString(Colorizer.HexToColor(color)));
                    break;
            }
        }

        #endregion

        #region RadConsole Properties

        public static ReadStyle ReadStyle { get; set; } = new();
        public static IPredictionEngine PredictionEngine { get; set; }
        public static ReadOnlyCollection<string> History => InputHistory.AsReadOnly();

        private static readonly List<string> InputHistory = new();
        private static readonly DefaultPredictionEngine DefaultPredictionEngine = new();

        #endregion
    }
}