#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RadLibrary.Logging.InputPredictionEngine;

#endregion

namespace RadLibrary.Logging
{
    public partial class Logger
    {
        public readonly List<string> InputHistory = new List<string> {""};

        private static bool _inputReRenderingNeeded;
        private static bool _inputReRenderingFinished;

        private static int _lastMessageSize;

        private static readonly object _inputWriterLock = new object();

        /// <summary>Gets the input with predictions.</summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="type">The type.</param>
        /// <param name="engine">The prediction engine.</param>
        /// <returns>Returns user's input</returns>
        public string GetInput(string prefix = null, LogType type = LogType.Input,
            IPredictionEngine engine = null)
        {
            SpinWait.SpinUntil(() => !_inputInProgress);

            _inputInProgress = true;

            prefix = prefix == null ? ">>> " : prefix + " >>> ";
            prefix = prefix.Colorize(Settings.InputColor);

            ++Console.BufferHeight;

            Console.Write(GetPrefix(type));
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

                lock (_inputWriterLock)
                {
                    Console.CursorVisible = false;

                    Rewrite();

                    Console.SetCursorPosition(0, startTop);

                    Console.Write(GetPrefix(type, true));
                    Console.Write(prefix);

                    Console.Write(prediction.Colorize(Settings.PredictionColor));

                    Console.SetCursorPosition(startLeft, startTop);

                    Console.Write(s.Colorize(Settings.InputColor));

                    Console.SetCursorPosition(currentLeft, currentTop);

                    Console.CursorVisible = true;
                }
            }

            void Rewrite()
            {
                Console.SetCursorPosition(0, startTop);
                Console.Write(string.Concat(Enumerable.Repeat(" ", longestStr + startLeft)).Colorize(GetColor(type)));
                Console.SetCursorPosition(currentLeft, currentTop);
            }

            void Finish()
            {
                Update();

                var (left, top) = GetEndPosition();

                Console.SetCursorPosition(left, top);

                Console.Write(" <<<".Colorize(GetColor(type)) + Environment.NewLine);
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
                while (true)
                {
                    SpinWait.SpinUntil(() => _inputReRenderingNeeded);

                    lock (_inputWriterLock)
                    {
                        Console.SetCursorPosition(0, startTop);
                        Console.Write(
                            string.Concat(Enumerable.Repeat(" ", longestStr + startLeft)).Colorize(GetColor(type)));
                        Console.SetCursorPosition(0, startTop);

                        _inputReRenderingNeeded = false;

                        SpinWait.SpinUntil(() => _inputReRenderingFinished);

                        startTop += _lastMessageSize;
                        currentTop += _lastMessageSize;

                        Update(engine?.Predict(sb.ToString(), this));

                        _inputReRenderingFinished = false;
                    }
                }
            }, cancellationToken.Token);

            Update();

            while (true)
            {
                var prediction = engine?.Predict(sb.ToString(), this);

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

        /// <summary>Gets the input.</summary>
        /// <param name="type">The type.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>Returns user's input</returns>
        public string GetInputSimple(LogType type = LogType.Warning, string prefix = "")
        {
            SpinWait.SpinUntil(() => !_inputInProgress);

            _inputInProgress = true;

            lock (ConsoleWriterLock)
            {
                Console.CursorVisible = true;

                var logPrefix = GetPrefix(type);

                Console.Write(logPrefix + prefix + ">>> ");

                var stop = false;

                var leftStart = Console.CursorLeft;
                var topStart = Console.CursorTop;

                Task.Run(() =>
                {
                    while (!stop)
                    {
                        var left = Console.CursorLeft;
                        var top = Console.CursorTop;
                        Console.SetCursorPosition(leftStart, topStart);
                        Console.Write(GetPrefix(type, true).Colorize(GetColor(type)));
                        Console.SetCursorPosition(left, top);
                        Thread.Sleep(2000);
                    }
                });

                var input = Console.ReadLine() ?? "";

                stop = true;

                var leftEnd = (leftStart + input.Length) % Console.WindowWidth;
                var topEnd = (leftStart + input.Length) / Console.WindowWidth;

                Console.SetCursorPosition(leftEnd, topEnd + topStart);

                Console.Write(" <<<".Colorize(GetColor(type)) + Environment.NewLine);

                Console.CursorVisible = false;

                _inputInProgress = false;

                return input;
            }
        }
    }
}