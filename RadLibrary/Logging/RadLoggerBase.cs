#region

using System;
using System.Linq;
using System.Text.RegularExpressions;
using RadLibrary.Logging.Helpers;

#endregion

namespace RadLibrary.Logging
{
    public abstract class RadLoggerBase : LoggerBase
    {
        private readonly object _formatterLock = new object();
        private readonly Regex _jsonRegex = new Regex(@"(({)|(\[)).*\s*:\s*.*((})|(\]))", RegexOptions.Compiled);

        private StringFormatter _formatter;

        /// <summary>
        ///     The log action
        /// </summary>
        /// <param name="type">Log type</param>
        /// <param name="message">The original message</param>
        /// <param name="formatted">The formatted message (see <see cref="LoggerBase.Settings" />)</param>
        protected abstract void Log(LogType type, string message, string formatted);

        /// <summary>
        ///     The log action
        /// </summary>
        /// <param name="type">Log type</param>
        /// <param name="message">The message</param>
        public override void DirectLog(LogType type, string message)
        {
            if (LoggerSettings.EnvironmentLoggingLevel <= type || Settings.LoggingLevel <= type)
                Log(type, message, Format(type, message));
        }

        private string Format(LogType type, string message)
        {
            if (message == null)
                message = "null";

            if (_formatter == null)
                lock (_formatterLock)
                {
                    _formatter = new StringFormatter(Settings.LogFormat);
                    _formatter.Set("{name}", Normalize(Settings.Name, LoggerSettings.NameMaxLength));
                }

            message = message.Replace("\r\n", "\n");

            if (_jsonRegex.IsMatch(message) && Settings.FormatJson)
                message = FormatJson(message);

            string s;

            lock (_formatterLock)
            {
                _formatter.Set("{level}", Normalize(type.ToString(), 5));
                _formatter.Set("{time}", DateTime.Now.ToString(Settings.TimeFormat));
                _formatter.Set("{message}", message);

                s = _formatter.ToString();
            }

            if (!s.Contains('\n'))
                return s;

            var padding = " ".Repeat(s.IndexOf(message, StringComparison.Ordinal));

            var messages = s.Split('\n').Aggregate((current, item) =>
                current + Environment.NewLine + padding + item);

            return messages;
        }

        private static string FormatJson(string json)
        {
            // https://stackoverflow.com/a/57100143
            var indentation = 0;
            var quoteCount = 0;
            var escapeCount = 0;
            const string indent = "  ";

            var result =
                from ch in json ?? string.Empty
                let escaped = (ch == '\\' ? escapeCount++ : escapeCount > 0 ? escapeCount-- : escapeCount) > 0
                let quotes = ch == '"' && !escaped ? quoteCount++ : quoteCount
                let unquoted = quotes % 2 == 0
                let colon = ch == ':' && unquoted ? ": " : null
                let noSpace = char.IsWhiteSpace(ch) && unquoted ? string.Empty : null
                let lineBreak = ch == ',' && unquoted
                    ? ch + Environment.NewLine + indent.Repeat(indentation)
                    : null
                let openChar = (ch == '{' || ch == '[') && unquoted
                    ? ch + Environment.NewLine + indent.Repeat(++indentation)
                    : ch.ToString()
                let closeChar = (ch == '}' || ch == ']') && unquoted
                    ? Environment.NewLine + indent.Repeat(--indentation) + ch
                    : ch.ToString()
                select colon ?? noSpace ?? lineBreak ?? (
                    openChar.Length > 1 ? openChar : closeChar
                );

            return string.Concat(result);
        }

        private static string Normalize(string name, int length)
        {
            var s = string.Format("{0, " + length + "}", name);
            return s.Length <= length ? s : ".." + s.Substring(s.Length - length + 2);
        }
    }
}