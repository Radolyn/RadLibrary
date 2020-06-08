#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using RadLibrary.Configuration;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>Defines the logger</summary>
    public partial class Logger
    {
        /// <summary>
        ///     Locks console for writing (may cause delays).
        /// </summary>
        private static readonly object ConsoleWriterLock = new object();

        /// <summary>
        ///     Locks input
        /// </summary>
        private static bool _inputInProgress;

        private static readonly LogType _environmentLogType = (LogType) Enum.Parse(typeof(LogType),
            Environment.GetEnvironmentVariable("LOGGING_LEVEL") ?? "Verbose");

        /// <summary>Initializes a new instance of the <see cref="Logger" /> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="settings">The settings.</param>
        public Logger(string name, LoggerSettings settings, int loggerThread = 0)
        {
            Colorizer.Initialize();
            Console.CursorVisible = false;

            // fix custom consoles behaviour (Terminus, etc.) 
            Console.CancelKeyPress += (sender, args) =>
            {
                Console.CursorVisible = true;
                Console.ResetColor();
                Console.WriteLine("".ResetColorAfter());
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
            {
                Console.CursorVisible = true;
                Console.ResetColor();
                Console.WriteLine("".ResetColorAfter());
            };

            Name = name;
            Settings = settings;
            LoggerThread = loggerThread;
        }

        /// <summary>Gets or sets the settings.</summary>
        /// <value>The settings.</value>
        public LoggerSettings Settings { get; set; }

        /// <summary>
        ///     Logger thread
        /// </summary>
        public int LoggerThread { get; }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>Logs the specified type.</summary>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArgumentOutOfRangeException">type is null</exception>
        public void Log(LogType type, params object[] args)
        {
            if (type < Settings.LogLevel || type < _environmentLogType)
                return;

            if (args?.Length == 0)
                args = new object[] {"<empty object>"};

            var s = new StringBuilder();

            var prefix = GetPrefix(type);

            var str = args[0]?.ToString();

            if (Settings.StringFormatRegex.IsMatch(str) && args.Length != 0)
            {
                var handled = args.Skip(1).Select(arg => HandleArgument(arg)).ToArray();

                // nah it can't
                // ReSharper disable once CoVariantArrayConversion
                str = string.Format(str, handled);

                s.Append(str);
            }
            else
            {
                foreach (var handledArgument in args.Select(arg => HandleArgument(arg)))
                {
                    s.Append(handledArgument);
                    s.Append(" ");
                }
            }

            var message = s.ToString();

            if (Settings.FormatJsonLike && Settings.JsonRegex.IsMatch(message))
                message = FormatJson(message);

            lock (ConsoleWriterLock)
            {
                void Print()
                {
                    message = message.Replace("\r\n", "\n");
                    
                    if (message.Contains("\n"))
                    {
                        var messages = message.Split('\n');
                        _lastMessageSize = CountSize(messages);
                        foreach (var msg in messages)
                            Console.WriteLine((prefix + msg).Colorize(GetColor(type)).ResetColorAfter());
                    }
                    else
                    {
                        _lastMessageSize = CountSize(prefix + message);
                        Console.WriteLine((prefix + message).Colorize(GetColor(type)).ResetColorAfter());
                    }
                }

                if (_progressBarInProgress)
                {
                    Console.Write("\r" + string.Concat(Enumerable.Repeat(" ", _lastIteration.Length)) + "\r");
                    Console.SetCursorPosition(0, Console.BufferHeight - 3);
                }

                if (_inputInProgress)
                {
                    _inputReRenderingNeeded = true;

                    SpinWait.SpinUntil(() => !_inputReRenderingNeeded);

                    Print();

                    _inputReRenderingFinished = true;

                    SpinWait.SpinUntil(() => !_inputReRenderingFinished);
                }
                else
                {
                    Print();
                }

                if (_progressBarInProgress)
                {
                    _lastMessageSize = CountSize(_lastIteration);
                    Console.Write(_lastIteration);
                }
            }
        }

        /// <summary>
        ///     Returns prefix.
        /// </summary>
        /// <param name="type">The logger type.</param>
        /// <param name="rewrite">The rewrite.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string GetPrefix(LogType type, bool rewrite = false)
        {
            var date = DateTime.Now.ToString(Settings.TimeFormat);

            var str = string.Format(Settings.LoggerPrefix + " ", date, Name, type.ToString(), LoggerThread)
                .Colorize(GetColor(type));

            return rewrite ? "\r" + str : str;
        }

        private static int CountSize(string msg)
        {
            return msg.Length / Console.BufferWidth + 1;
        }

        private static int CountSize(IEnumerable<string> messages)
        {
            return messages.Sum(CountSize);
        }

        /// <summary>
        ///     Gets color
        /// </summary>
        /// <param name="type">The logger type.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private Color GetColor(LogType type)
        {
            switch (type)
            {
                case LogType.Verbose:
                    return Settings.VerboseColor;
                case LogType.Information:
                    return Settings.InformationColor;
                case LogType.Warning:
                    return Settings.WarningColor;
                case LogType.Error:
                    return Settings.ErrorColor;
                case LogType.Success:
                    return Settings.SuccessColor;
                case LogType.Exception:
                    return Settings.ExceptionColor;
                case LogType.Deprecation:
                    return Settings.DeprecatedColor;
                case LogType.Input:
                    return Settings.InputColor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        ///     Formats output
        /// </summary>
        /// <param name="json">The output</param>
        /// <returns>The formatted output</returns>
        private static string FormatJson(string json)
        {
            // https://stackoverflow.com/questions/4580397/json-formatter-in-c answer by Vince Panuccio (big thanks! :d)
            const string indentString = "  ";
            var indentation = 0;
            var quoteCount = 0;
            var result =
                from ch in json
                let quotes = ch == '"' ? quoteCount++ : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0
                    ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indentString, indentation))
                    : null
                let openChar = ch == '{' || ch == '['
                    ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indentString, ++indentation))
                    : ch.ToString()
                let closeChar = ch == '}' || ch == ']'
                    ? Environment.NewLine + string.Concat(Enumerable.Repeat(indentString, --indentation)) + ch
                    : ch.ToString()
                select lineBreak ?? (openChar.Length > 1
                    ? openChar
                    : closeChar);

            return string.Concat(result);
        }

        /// <summary>Handles the argument.</summary>
        /// <param name="arg">The argument.</param>
        /// <param name="recursion">The recursion.</param>
        /// <returns>Returns formatted string</returns>
        private string HandleArgument(object arg, int recursion = 0)
        {
            if (arg == null)
                return "null";

            if (recursion >= Settings.RecursionLimit)
                if (Settings.ErrorOnRecursionLimit)
                    throw new StackOverflowException();
                else
                    return "...";

            ++recursion;

            switch (arg)
            {
                case DateTime date:
                    return date.ToString(Settings.TimeFormat);
                // Python styled list output
                case IList list:
                {
                    var str = list.Cast<object>().Aggregate("[",
                        (current, item) => current + HandleArgument(item, recursion) + ", ");

                    return str.Remove(str.Length - 2) + "]";
                }
                // Python styled dictionary output
                case IDictionary dictionary:
                {
                    var str = dictionary.Cast<object>().Aggregate("{",
                        (current, item) => current + HandleArgument(item, recursion) + ", ");

                    return str.Remove(str.Length - 2) + "}";
                }
                case DictionaryEntry pair:
                    return HandleArgument(pair.Key, recursion) + ": " + HandleArgument(pair.Value, recursion);
                case AppConfiguration configuration:
                    return HandleArgument(configuration.Parameters, recursion);
                default:
                    return arg.ToString();
            }
        }
    }
}