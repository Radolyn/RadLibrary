#region

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using RadLibrary.Colors;
using RadLibrary.Formatting;
using RadLibrary.Logging;

#endregion

namespace RadLibrary.ConsoleExperience
{
    public static partial class ColorfulConsole
    {
        private static readonly Regex Regex = new Regex(@"\[(#?[0-9A-Fa-f]{1,6})\](.*?)(?=\[#?[0-9A-Fa-f]{1,6}\]|$)",
            RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        ///     Writes message to console
        /// </summary>
        /// <param name="message">The message</param>
        public static void Write(string message)
        {
            var messages = InternalColorizer(message);

            if (!_inputInProgress)
            {
                lock (Console.Out)
                {
                    Console.Write(messages);
                }
            }
            else
            {
                _inputReRenderingNeeded = true;

                SpinWait.SpinUntil(() => !_inputReRenderingNeeded);

                lock (Console.Out)
                {
                    Console.Write(messages);
                }

                _inputReRenderingFinished = true;

                SpinWait.SpinUntil(() => !_inputReRenderingFinished);
            }
        }

        /// <summary>
        ///     Writes message with new line to console
        /// </summary>
        /// <param name="message">The message</param>
        public static void WriteLine(params object[] args)
        {
            // todo: fix x2 new line
            Write(LoggerBase.ParseArguments(args) + Environment.NewLine);
        }

        /// <summary>
        ///     Writes message with new line to console
        /// </summary>
        /// <param name="format">The message</param>
        /// <param name="args">The arguments</param>
        public static void WriteLine(string format, params object[] args)
        {
            Write(string.Format(FormattersStorage.FormatProvider, format, args) + Environment.NewLine);
        }

        /// <summary>
        ///     Writes message at specified place in console buffer, then returns cursor back
        /// </summary>
        /// <param name="message"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        public static void WriteAt(string message, int top, int left = 0)
        {
            var msg = InternalColorizer(message);

            var savePositionLeft = Console.CursorLeft;
            var savePositionTop = Console.CursorTop;
            var saveCursorVisibility = Console.CursorVisible;

            lock (Console.Out)
            {
                Console.CursorVisible = false;

                Console.SetCursorPosition(left, top);

                Console.WriteLine(msg);

                Console.SetCursorPosition(savePositionLeft, savePositionTop);

                Console.CursorVisible = saveCursorVisibility;
            }
        }

        private static string InternalColorizer(string message)
        {
            var messages = Regex.Matches(message);

            var sb = new StringBuilder();

            if (messages.Count != 0)
            {
                foreach (Match msg in messages)
                    sb.Append(msg.Groups[2].Captures[0].Value.Colorize(msg.Groups[1].Captures[0].Value));

                _lastMessageSize = CountSize(Regex.Replace(message, ""));
            }
            else
            {
                sb.Append(message);

                _lastMessageSize = CountSize(message);
            }

            return sb.ToString();
        }

        private static int CountSize(string msg)
        {
            return msg.Length / Console.BufferWidth + 1;
        }
    }
}