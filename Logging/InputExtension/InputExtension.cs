using System;
using System.Linq;
using System.Text;
using RadLibrary.Logging.InputPredictionEngine;

namespace RadLibrary.Logging.InputExtension
{
    public static class InputExtension
    {
        /// <summary>Gets the input with predictions.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="type">The type.</param>
        /// <param name="engine">The prediction engine.</param>
        /// <returns>Returns user's input</returns>
        public static string GetInput(this Logger logger, string prefix = null, LogType type = LogType.Input,
            IPredictionEngine engine = null)
        {
            prefix = prefix == null ? ">>> " : prefix + " >>> ";

            Console.Write(logger.GetPrefix(type));
            Console.Write(prefix);

            var startTop = Console.CursorTop;
            var startLeft = Console.CursorLeft;

            var currentTop = Console.CursorTop;
            var currentLeft = Console.CursorLeft;

            var sb = new StringBuilder();

            var history = logger.InputHistory.Count;

            var longestStr = 0;

            void Update(string prediction = "")
            {
                var s = sb.ToString();

                if (prediction?.Length > longestStr)
                    longestStr = prediction.Length;
                if (sb.Length > longestStr)
                    longestStr = sb.Length;

                Console.CursorVisible = false;

                Rewrite();

                Console.SetCursorPosition(0, startTop);

                Console.Write(logger.GetPrefix(type, true));
                Console.Write(prefix);

                Console.ForegroundColor = logger.Settings.PredictionColor;
                Console.Write(prediction);

                Console.SetCursorPosition(startLeft, startTop);

                Console.ForegroundColor = logger.Settings.InputColor;

                Console.Write(s);

                Console.SetCursorPosition(currentLeft, currentTop);

                Console.CursorVisible = true;
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

                var pos = GetEndPosition();

                Console.SetCursorPosition(pos.Item1, pos.Item2);

                logger.SetColor(type);
                Console.Write(" <<<" + Environment.NewLine);
            }

            Tuple<int, int> GetEndPosition()
            {
                var s = sb.Length;

                var leftEnd = (startLeft + s) % Console.BufferWidth;
                var topEnd = (startLeft + s) / Console.BufferWidth;

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

                currentLeft = left;
                currentTop = top;
            }

            void GoLeft()
            {
                if (currentLeft == startLeft && currentTop == startTop)
                    return;

                if (currentLeft == 0)
                {
                    currentTop -= 1;
                    currentLeft = Console.BufferWidth - 1;
                }
                else
                {
                    --currentLeft;
                }
            }

            void GoRight(bool check = true)
            {
                if (currentLeft == sb.Length + startLeft && check)
                    return;
                if (currentLeft == Console.BufferWidth - 1)
                {
                    // fix custom consoles behaviour (Terminus, etc.) 
                    ++Console.BufferHeight;
                    currentTop += 1;
                    currentLeft = 0;
                }
                else
                {
                    currentLeft = Console.CursorLeft + 1;
                    currentTop = Console.CursorTop;
                }
            }

            Update();

            while (true)
            {
                var prediction = engine?.Predict(sb.ToString(), logger);

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
                        sb.Append(logger.InputHistory[history]);

                        Console.SetCursorPosition(startLeft, startTop);
                        if (logger.InputHistory[history].Length > longestStr)
                            longestStr = logger.InputHistory[history].Length;

                        SetEndPosition();

                        break;
                    case ConsoleKey.DownArrow:
                        if (history == logger.InputHistory.Count - 1) break;

                        history++;

                        sb.Clear();
                        sb.Append(logger.InputHistory[history]);

                        if (logger.InputHistory[history].Length > longestStr)
                            longestStr = logger.InputHistory[history].Length;

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
            logger.InputHistory.Add(res);

            return res;
        }
    }
}