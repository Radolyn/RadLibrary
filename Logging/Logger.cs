#region

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RadLibrary.Configuration;
using RadLibrary.Logging.Helpers;

#endregion

namespace RadLibrary.Logging
{
    public abstract partial class Logger
    {
        /// <summary>
        ///     Gets or sets settings
        /// </summary>
        public virtual LoggerSettings Settings { get; set; }

        private readonly Regex _jsonRegex = new Regex(@"(({)|(\[)).*\s*:\s*.*((})|(\]))");

        private StringFormatter _formatter;
        private readonly object _formatterLock = new object();

        /// <summary>
        ///     Initializes logger
        /// </summary>
        /// <param name="args">Arguments</param>
        public abstract void Initialize(params object[] args);

        /// <summary>
        ///     The log action
        /// </summary>
        /// <param name="type">Log type</param>
        /// <param name="message">The original message</param>
        /// <param name="formatted">The formatted message (see <see cref="Settings" />)</param>
        public abstract void Log(LogType type, string message, string formatted);

        private void PrivateLog(LogType type, string message)
        {
            if (LoggerSettings.EnvironmentLoggingLevel <= type || Settings.LoggingLevel <= type)
                Log(type, message, Format(type, message));
        }

        private string Format(LogType type, string message)
        {
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

            var padding = string.Concat(Enumerable.Repeat(" ", s.IndexOf(message, StringComparison.Ordinal)));

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
                    ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indent, indentation))
                    : null
                let openChar = (ch == '{' || ch == '[') && unquoted
                    ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indent, ++indentation))
                    : ch.ToString()
                let closeChar = (ch == '}' || ch == ']') && unquoted
                    ? Environment.NewLine + string.Concat(Enumerable.Repeat(indent, --indentation)) + ch
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

        private string ParseArguments(params object[] args)
        {
            if (args == null)
                return "null";

            var sb = new StringBuilder();

            foreach (var arg in args) sb.Append(ArgumentToString(arg) + " ");

            return sb.ToString();
        }

        private string ArgumentToString(object arg, int iteration = 0)
        {
            ++iteration;

            if (arg == null)
                return "null";

            if (iteration >= Settings.MaxRecursion)
                return "...";

            switch (arg)
            {
                case string s when iteration > 1:
                    return $"\"{s}\"";
                case DateTime date:
                    return date.ToString(Settings.TimeFormat);
                // Python styled dictionary output
                case IDictionary dictionary:
                {
                    var sb = new StringBuilder();

                    sb.Append("{");

                    foreach (DictionaryEntry entry in dictionary) sb.Append(ArgumentToString(entry, iteration) + ", ");

                    var str = sb.ToString();

                    str = str.Remove(str.Length - 2);

                    return str + "}";
                }
                // Python styled list output
                case IEnumerable list:
                {
                    var str = list.Cast<object>().Aggregate("[",
                        (current, item) => current + ArgumentToString(item, iteration) + ", ");

                    return str.Remove(str.Length - 2) + "]";
                }
                case DictionaryEntry pair:
                    return ArgumentToString(pair.Key, iteration) + ": " + ArgumentToString(pair.Value, iteration);
                case Exception exception:
                    return $"{exception.Source}: {exception.GetType()}.\nMessage: {exception.Message}\nStack trace:\n{exception.StackTrace}";
                case AppConfiguration configuration:
                    return ArgumentToString(configuration.Parameters, iteration);
                case Parameter parameter:
                    return $"[\"value\": \"{parameter.Value}\", \"comment\": \"{parameter.Comment.Replace("# ", "")}\"]";
                default:
                    return arg.ToString();
            }
        }
    }
}