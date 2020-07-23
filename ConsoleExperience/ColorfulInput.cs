#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RadLibrary.Colors;
using RadLibrary.ConsoleExperience.PredictionEngine;

// ReSharper disable AccessToModifiedClosure

#endregion

namespace RadLibrary.ConsoleExperience
{
    public static partial class ColorfulConsole
    {
        /// <summary>
        ///     List that contains all inputs from current logger
        /// </summary>
        public static readonly List<string> InputHistory = new List<string> {""};

        private static bool _inputInProgress;

        private static bool _inputReRenderingNeeded;
        private static bool _inputReRenderingFinished;

        private static int _lastMessageSize;

        private static readonly object InputWriterLock = new object();

        /// <summary>Gets the input with predictions</summary>
        /// <param name="settings">The settings</param>
        /// <returns>Returns user's input</returns>
        public static string GetInput(ColorfulInputSettings settings)
        {
            SpinWait.SpinUntil(() => !_inputInProgress);

            _inputInProgress = true;

            var arrowsColorized = ">>> ".Colorize(settings.ArrowsForegroundColor)
                .ColorizeBackground(settings.ArrowsBackgroundColor);

            var prefix = settings.Prefix == null ? arrowsColorized : settings.Prefix + " " + arrowsColorized;
            prefix += Colorizer.GetColorizationString(settings.InputForegroundColor);
            prefix += Colorizer.GetBackgroundColorizationString(settings.InputBackgroundColor);

            ++Console.BufferHeight;

            Console.Write(prefix);

            var startTop = Console.CursorTop;
            var startLeft = Console.CursorLeft;

            var currentTop = Console.CursorTop;
            var currentLeft = Console.CursorLeft;

            var sb = new StringBuilder();

            var history = InputHistory.Count;

            var longestStr = 0;

            void Update(string prediction = "")
            {
                var s = sb.ToString();

                if (prediction?.Length > longestStr)
                    longestStr = prediction.Length;
                if (sb.Length > longestStr)
                    longestStr = sb.Length;

                lock (InputWriterLock)
                {
                    Console.CursorVisible = false;

                    Rewrite();

                    Console.SetCursorPosition(0, startTop);

                    Console.Write("\r" + prefix);

                    Console.Write(prediction?.Colorize(settings.PredictionForegroundColor)
                        .ColorizeBackground(settings.PredictionBackgroundColor));

                    Console.SetCursorPosition(startLeft, startTop);

                    Console.Write(s.Colorize(settings.InputForegroundColor)
                        .ColorizeBackground(settings.InputBackgroundColor));

                    Console.SetCursorPosition(currentLeft, currentTop);

                    Console.CursorVisible = true;
                }
            }

            void Rewrite()
            {
                Console.SetCursorPosition(0, startTop);
                Console.Write(string.Concat(Enumerable.Repeat(" ", longestStr + startLeft)));
                Console.SetCursorPosition(currentLeft, currentTop);
            }

            void Finish()
            {
                Update();

                var (left, top) = GetEndPosition();

                Console.SetCursorPosition(left, top);

                Console.Write(" <<<".Colorize(settings.ArrowsForegroundColor)
                    .ColorizeBackground(settings.ArrowsBackgroundColor) + Environment.NewLine);
            }

            Tuple<int, int> GetEndPosition()
            {
                var l = sb.Length;

                var leftEnd = (startLeft + l) % Console.BufferWidth;
                var topEnd = (startLeft + l) / Console.BufferWidth;

                return new Tuple<int, int>(leftEnd, topEnd + startTop);
            }

            int GetCurrentPosition()
            {
                if (currentTop == startTop)
                    return currentLeft - startLeft - 1;

                var fullyFilled = (currentTop - startTop - 1) * Console.BufferWidth;

                var l = Console.BufferWidth - startLeft - 1;

                return l + fullyFilled + currentLeft;
            }

            void SetEndPosition()
            {
                var (left, top) = GetEndPosition();

                if (Console.BufferHeight < top)
                    Console.BufferHeight += top - Console.BufferHeight + 1;

                currentLeft = left;
                currentTop = top;
            }

            void GoLeft()
            {
                if (currentLeft == startLeft && currentTop == startTop)
                    return;

                if (currentLeft == 0)
                {
                    --currentTop;
                    currentLeft = Console.BufferWidth - 1;
                }
                else
                {
                    --currentLeft;
                }
            }

            void GoRight(bool check = true)
            {
                if (check)
                {
                    var (left, top) = GetEndPosition();

                    if (left == currentLeft && top == currentTop)
                        return;
                }

                if (currentLeft == Console.BufferWidth - 1)
                {
                    // fix custom consoles behaviour (Terminus, Windows Terminal, etc.) 
                    ++Console.BufferHeight;
                    currentLeft = 0;
                    ++currentTop;
                }
                else
                {
                    ++currentLeft;
                }
            }

            var cancellationToken = new CancellationTokenSource();

            // re-render
            Task.Run(() =>
            {
                while (cancellationToken.IsCancellationRequested)
                {
                    SpinWait.SpinUntil(() => _inputReRenderingNeeded);

                    lock (InputWriterLock)
                    {
                        Console.SetCursorPosition(0, startTop);
                        Console.Write(
                            string.Concat(Enumerable.Repeat(" ", longestStr + startLeft)));
                        Console.SetCursorPosition(0, startTop);

                        _inputReRenderingNeeded = false;

                        SpinWait.SpinUntil(() => _inputReRenderingFinished);

                        startTop += _lastMessageSize;
                        currentTop += _lastMessageSize;

                        Update(settings.Engine?.Predict(sb.ToString()));

                        _inputReRenderingFinished = false;
                    }
                }
            }, cancellationToken.Token);

            Update();

            while (true)
            {
                var prediction = settings.Engine?.Predict(sb.ToString());

                Update(prediction);

                var ch = Console.ReadKey(true);

                if (ch.KeyChar == -1) break;
                if (ch.Key >= ConsoleKey.NumPad0 && ch.Key <= ConsoleKey.NumPad9
                    || ch.Key == ConsoleKey.Clear || ch.Key == ConsoleKey.Insert
                    || ch.Key >= ConsoleKey.PageUp && ch.Key <= ConsoleKey.Home) continue;

                switch (ch.Key)
                {
                    case ConsoleKey.Backspace:
                    {
                        if (currentLeft == startLeft)
                            continue;

                        sb.Remove(GetCurrentPosition(), 1);

                        GoLeft();

                        continue;
                    }
                    case ConsoleKey.LeftArrow:
                        GoLeft();

                        break;
                    case ConsoleKey.RightArrow:
                        GoRight();

                        break;
                    case ConsoleKey.UpArrow:
                        if (history == 0) break;

                        history--;

                        sb.Clear();
                        sb.Append(InputHistory[history]);

                        Console.SetCursorPosition(startLeft, startTop);
                        if (InputHistory[history].Length > longestStr)
                            longestStr = InputHistory[history].Length;

                        SetEndPosition();

                        break;
                    case ConsoleKey.DownArrow:
                        if (history == InputHistory.Count - 1) break;

                        history++;

                        sb.Clear();
                        sb.Append(InputHistory[history]);

                        if (InputHistory[history].Length > longestStr)
                            longestStr = InputHistory[history].Length;

                        SetEndPosition();

                        break;
                    case ConsoleKey.Tab:
                        if (sb.Length >= prediction?.Length)
                            break;
                        sb.Clear();
                        sb.Append(prediction);

                        SetEndPosition();

                        break;
                    default:
                        if (ch.KeyChar == '\r' || ch.KeyChar == '\n')
                            // return
                            goto ret;

                        GoRight(false);

                        if (sb.Length == 0)
                            sb.Append(ch.KeyChar);
                        else
                            sb.Insert(GetCurrentPosition(), ch.KeyChar);
                        continue;
                }
            }

            ret:
            Finish();

            var res = sb.Length > 0 ? sb.ToString() : "";
            InputHistory.Add(res);

            _inputInProgress = false;
            cancellationToken.Cancel();

            return res;
        }

        /// <summary>Gets the input</summary>
        /// <returns>Returns user's input</returns>
        public static string GetInput()
        {
            return GetInput(new ColorfulInputSettings());
        }

        /// <summary>Gets the input with predictions</summary>
        /// <param name="engine">The prediction engine</param>
        /// <returns>Returns user's input</returns>
        public static string GetInput(IPredictionEngine engine)
        {
            return GetInput(new ColorfulInputSettings
            {
                Engine = engine
            });
        }

        /// <summary>Gets the input with prefix</summary>
        /// <param name="prefix">The prefix</param>
        /// <returns>Returns user's input</returns>
        public static string GetInput(string prefix)
        {
            return GetInput(new ColorfulInputSettings
            {
                Prefix = prefix
            });
        }

        /// <summary>Gets the input with prefix</summary>
        /// <param name="prefix">The prefix</param>
        /// <param name="engine">The prediction engine</param>
        /// <returns>Returns user's input</returns>
        public static string GetInput(string prefix, IPredictionEngine engine)
        {
            return GetInput(new ColorfulInputSettings
            {
                Prefix = prefix,
                Engine = engine
            });
        }

        /// <summary>Gets the input with prefix</summary>
        /// <param name="prefix">The prefix</param>
        /// <param name="inputForegroundColor">The input text color</param>
        /// <returns>Returns user's input</returns>
        public static string GetInput(string prefix, Color inputForegroundColor)
        {
            return GetInput(new ColorfulInputSettings
            {
                Prefix = prefix,
                InputForegroundColor = inputForegroundColor
            });
        }

        /// <summary>Gets the input with prefix</summary>
        /// <param name="prefix">The prefix</param>
        /// <param name="engine">The prediction engine</param>
        /// <param name="inputForegroundColor">The input text color</param>
        /// <returns>Returns user's input</returns>
        public static string GetInput(string prefix, IPredictionEngine engine, Color inputForegroundColor)
        {
            return GetInput(new ColorfulInputSettings
            {
                Prefix = prefix,
                Engine = engine,
                InputForegroundColor = inputForegroundColor
            });
        }
    }
}