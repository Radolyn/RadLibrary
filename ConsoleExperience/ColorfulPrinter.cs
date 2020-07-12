#region

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#endregion

namespace RadLibrary.ConsoleExperience
{
    public static partial class ColorfulConsole
    {
        private static readonly Regex Regex = new Regex(@"\[(#?[0-9A-Fa-f]{1,6})\](.*?)(?=\[#?[0-9A-Fa-f]{1,6}\]|$)",
            RegexOptions.Compiled);

        /// <summary>
        ///     Writes line to console
        /// </summary>
        /// <param name="message">The message</param>
        public static void WriteLine(string message)
        {
            var messages = Regex.Matches(message);

            var sb = new StringBuilder();

            foreach (Match msg in messages)
                sb.Append(msg.Groups[2].Captures[0].Value.Colorize(msg.Groups[1].Captures[0].Value));

            _lastMessageSize = CountSize(Regex.Replace(message, ""));

            if (!_inputInProgress)
            {
                Console.WriteLine(sb.ToString());
            }
            else
            {
                _inputReRenderingNeeded = true;

                SpinWait.SpinUntil(() => !_inputReRenderingNeeded);

                Console.WriteLine(sb.ToString());

                _inputReRenderingFinished = true;

                SpinWait.SpinUntil(() => !_inputReRenderingFinished);
            }
        }

        private static int CountSize(string msg)
        {
            return msg.Length / Console.BufferWidth + 1;
        }
    }
}