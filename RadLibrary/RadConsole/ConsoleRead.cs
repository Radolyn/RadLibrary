#region

using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using RadLibrary.Colors;
using RadLibrary.RadConsole.Engines;
using static RadLibrary.RadConsole.RadConsole;

#endregion

// todo: use ConsoleRead.ReadStyle instead of ReadStyle

namespace RadLibrary.RadConsole
{
    /// <summary>
    ///     Provides methods for reading user input
    /// </summary>
    public sealed class ConsoleRead
    {
        private readonly BooleanPredictionEngine _booleanPredictionEngine = new();

        private readonly List<string> _inputHistory = new() {"1", "2"};

        /// <summary>
        ///     Used in <see cref="Boolean()" />
        /// </summary>
        public readonly HashSet<string> FalseBooleans = new()
        {
            "false",
            "0",
            "no",
            "n",
            "not",
            "nope",
            "нет"
        };

        /// <summary>
        ///     Used in <see cref="Boolean()" />
        /// </summary>
        public readonly HashSet<string> TrueBooleans = new()
        {
            "true",
            "1",
            "yes",
            "ye",
            "y",
            "sure",
            "да"
        };

        internal ConsoleRead()
        {
        }

        /// <summary>
        ///     Gets or sets style for <see cref="Line()" />
        /// </summary>
        public IReadStyle ReadStyle { get; set; } = new ReadStyle();

        /// <summary>
        ///     Gets or sets prediction engine for <see cref="Line()" />
        /// </summary>
        public IPredictionEngine PredictionEngine { get; set; } = new DefaultPredictionEngine();

        /// <summary>
        ///     Gets input history
        /// </summary>
        [NotNull]
        public IEnumerable<string> History => _inputHistory.AsReadOnly();

        /// <summary>
        ///     Reads the next integer from the standard input stream with specified read style.
        /// </summary>
        /// <param name="readStyle">The read style</param>
        /// <returns>The next integer from the input stream</returns>
        public int Integer([NotNull] IReadStyle readStyle)
        {
            int res;
            while (!int.TryParse(Line(readStyle, false), out res))
            {
            }

            return res;
        }

        /// <summary>
        ///     Reads the next integer from the standard input stream with default read style.
        /// </summary>
        /// <param name="prefix">The prefix</param>
        /// <returns>The next integer from the input stream</returns>
        public int Integer(string prefix)
        {
            var readStyle = (IReadStyle) ReadStyle.Clone();
            readStyle.SetPrefix(prefix);
            return Integer(readStyle);
        }

        /// <summary>
        ///     Reads the next integer from the standard input stream with default read style.
        /// </summary>
        /// <returns>The next integer from the input stream</returns>
        public int Integer()
        {
            return Integer(ReadStyle);
        }

        /// <summary>
        ///     Reads the next boolean from the standard input stream with specified read style.
        /// </summary>
        /// <param name="readStyle">The read style</param>
        /// <returns>The next boolean from the input stream</returns>
        public bool Boolean([NotNull] IReadStyle readStyle)
        {
            while (true)
            {
                var s = Line(readStyle, _booleanPredictionEngine);

                var lower = s.ToLower();

                if (TrueBooleans.Contains(lower))
                    return true;
                if (FalseBooleans.Contains(lower))
                    return false;
            }
        }

        /// <summary>
        ///     Reads the next boolean from the standard input stream with default read style.
        /// </summary>
        /// <param name="prefix">The prefix</param>
        /// <returns>The next boolean from the input stream</returns>
        public bool Boolean(string prefix)
        {
            var readStyle = (IReadStyle) ReadStyle.Clone();
            readStyle.SetPrefix(prefix);
            return Boolean(readStyle);
        }

        /// <summary>
        ///     Reads the next boolean from the standard input stream with default read style.
        /// </summary>
        /// <returns>The next boolean from the input stream</returns>
        public bool Boolean()
        {
            return Boolean(ReadStyle);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with default read style and prediction engine.
        /// </summary>
        /// <returns>The next line of characters from the input stream</returns>
        public string Line()
        {
            return Line(ReadStyle, PredictionEngine);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with specified read style and default prediction
        ///     engine.
        /// </summary>
        /// <param name="readStyle">The read style</param>
        /// <param name="usePredictionEngine">Use default prediction engine or not</param>
        /// <returns>The next line of characters from the input stream</returns>
        public string Line([NotNull] IReadStyle readStyle, bool usePredictionEngine = true)
        {
            return Line(readStyle, usePredictionEngine ? PredictionEngine : null);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with default read style and specified prediction
        ///     engine.
        /// </summary>
        /// <param name="predictionEngine">The prediction engine</param>
        /// <returns>The next line of characters from the input stream</returns>
        public string Line([CanBeNull] IPredictionEngine predictionEngine)
        {
            return Line(ReadStyle, predictionEngine);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with specified prefix and default prediction
        ///     engine.
        /// </summary>
        /// <param name="prefix">The prefix</param>
        /// <param name="usePredictionEngine">Use default prediction engine or not</param>
        /// <returns>The next line of characters from the input stream</returns>
        public string Line([CanBeNull] string prefix, bool usePredictionEngine = true)
        {
            var readStyle = (IReadStyle) ReadStyle.Clone();
            readStyle.SetPrefix(prefix);
            return Line(readStyle, usePredictionEngine ? PredictionEngine : null);
        }

        /// <summary>
        ///     Reads the next line of characters from the standard input stream with specified read style and prediction engine.
        /// </summary>
        /// <param name="readStyle">The read style</param>
        /// <param name="predictionEngine">The prediction engine</param>
        /// <returns>The next line of characters from the input stream</returns>
        public string Line([NotNull] IReadStyle readStyle, [CanBeNull] IPredictionEngine predictionEngine)
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
            _inputHistory.Add(res);

            return res;
        }

        private void ProcessArrows(ConsoleKeyInfo key, ref int currentPosition, ref int currentHistory,
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
                if (currentHistory == _inputHistory.Count - 1)
                    return;

                ++currentHistory;
                SetLine(_inputHistory[currentHistory], ref currentPosition);
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
                SetLine(_inputHistory[currentHistory], ref currentPosition);
            }
        }

        private void ProcessPrediction(string prediction, ref int currentPosition, StringBuilder line)
        {
            line.Clear();
            line.Append(prediction);

            currentPosition = line.Length;
        }

        private void ProcessBackspace(StringBuilder line, ref int currentPosition)
        {
            if (currentPosition == 0)
                return;

            line.Remove(currentPosition - 1, 1);
            --currentPosition;
        }

        private void UpdateScreen((int Top, int Left) startPosition, IReadStyle readStyle, int currentPosition,
            int biggestInput,
            string prediction, StringBuilder line)
        {
            CursorVisible = false;

            SetCursorPosition(startPosition.Left, startPosition.Top);

            Write(" ".Repeat(biggestInput));

            SetCursorPosition(startPosition.Left, startPosition.Top);

            if (prediction != null)
            {
                Write(readStyle.PredictionDecorations);
                Write(prediction.Colorize(readStyle.PredictionColor));
            }

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
    }
}