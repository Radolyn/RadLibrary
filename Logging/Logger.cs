#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RadLibrary.Logging.InputPredictionEngine;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>Defines the logger</summary>
    public class Logger
    {
        /// <summary>Initializes a new instance of the <see cref="Logger" /> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="settings">The settings.</param>
        public Logger(string name, LoggerSettings settings)
        {
            Console.CursorVisible = false;

            // fix custom consoles behaviour (Terminus, etc.) 
            Console.CancelKeyPress += (sender, args) =>
            {
                Console.CursorVisible = true;
                Console.ForegroundColor = ConsoleColor.Gray;
            };

            Name = name;
            Settings = settings;
        }

        /// <summary>
        ///     Locks console for writing (may cause delays).
        /// </summary>
        private static readonly object ConsoleWriterLock = new object();

        /// <summary>Gets or sets the settings.</summary>
        /// <value>The settings.</value>
        public LoggerSettings Settings { get; set; }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        ///     The custom argument handler delegate
        /// </summary>
        /// <param name="obj">The object.</param>
        public delegate object CustomHandlerDelegate(object obj);

        /// <summary>
        ///     The custom handlers dictionary
        /// </summary>
        public readonly Dictionary<Type, CustomHandlerDelegate> CustomHandlers =
            new Dictionary<Type, CustomHandlerDelegate>();

        public readonly List<string> InputHistory = new List<string> {""};

        /// <summary>Logs the specified type.</summary>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArgumentOutOfRangeException">type is null</exception>
        public void Log(LogType type, params object[] args)
        {
            if (type < Settings.LogLevel)
                return;

            if (args?.Length == 0)
                args = new object[] {"<empty object>"};

            var s = new StringBuilder();

            var prefix = GetPrefix(type);

            lock (ConsoleWriterLock)
            {
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

                if (Settings.FormatJsonLike && message.Contains('{') && message.Contains('['))
                    message = FormatJson(message);

                SetColor(type);

                if (message.Contains("\n"))
                {
                    var messages = message.Split('\n');
                    foreach (var msg in messages) Console.WriteLine(prefix + msg);
                }
                else
                {
                    Console.WriteLine(prefix + message);
                }

                Console.ResetColor();
            }
        }

        /// <summary>
        ///     Returns prefix.
        /// </summary>
        /// <param name="type">The logger type.</param>
        /// <param name="rewrite">The rewrite.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal string GetPrefix(LogType type, bool rewrite = false)
        {
            var date = DateTime.Now.ToString(Settings.TimeFormat);

            var str = string.Format(Settings.LoggerPrefix + " ", date, Name, type.ToString());

            return rewrite ? "\r" + str : str;
        }

        /// <summary>
        ///     Sets color in console.
        /// </summary>
        /// <param name="type">The logger type.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal void SetColor(LogType type)
        {
            switch (type)
            {
                case LogType.Verbose:
                    Console.ForegroundColor = Settings.VerboseColor;
                    break;
                case LogType.Information:
                    Console.ForegroundColor = Settings.InformationColor;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = Settings.WarningColor;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = Settings.ErrorColor;
                    break;
                case LogType.Success:
                    Console.ForegroundColor = Settings.SuccessColor;
                    break;
                case LogType.Exception:
                    Console.ForegroundColor = Settings.ExceptionColor;
                    break;
                case LogType.Deprecation:
                    Console.ForegroundColor = Settings.DeprecatedColor;
                    break;
                case LogType.Input:
                    Console.ForegroundColor = Settings.InputColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        ///     Formats output
        /// </summary>
        /// <param name="json">The output</param>
        /// <returns>The formatted output</returns>
        internal static string FormatJson(string json)
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

            recursion++;

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
                default:
                    return CustomHandlers.ContainsKey(arg.GetType())
                        ? HandleArgument(CustomHandlers[arg.GetType()].Invoke(arg), recursion)
                        : arg.ToString();
            }
        }

        /// <summary>Alias for <see cref="Log" /> (Verbose)</summary>
        /// <param name="args">The arguments.</param>
        public void Verbose(params object[] args)
        {
            Log(LogType.Verbose, args);
        }

        /// <summary>Alias for <see cref="Log" /> (Information)</summary>
        /// <param name="args">The arguments.</param>
        public void Info(params object[] args)
        {
            Log(LogType.Information, args);
        }

        /// <summary>Alias for <see cref="Log" /> (Warning)</summary>
        /// <param name="args">The arguments.</param>
        public void Warn(params object[] args)
        {
            Log(LogType.Warning, args);
        }

        /// <summary>Alias for <see cref="Log" /> (Error)</summary>
        /// <param name="args">The arguments.</param>
        public void Error(params object[] args)
        {
            Log(LogType.Error, args);
        }

        /// <summary>Alias for <see cref="Log" /> (Success)</summary>
        /// <param name="args">The arguments.</param>
        public void Success(params object[] args)
        {
            Log(LogType.Success, args);
        }

        /// <summary>Logs the specified exception</summary>
        /// <param name="ex">The exception</param>
        /// <exception cref="FormatException"><see cref="LoggerSettings.ExceptionString" /> doesn't contains {0}</exception>
        public void Exception(Exception ex)
        {
            if (!Settings.StringFormatRegex.IsMatch(Settings.ExceptionString))
                throw new FormatException();
            Log(LogType.Exception, Settings.ExceptionString, ex.GetType(), ex.StackTrace, ex.Message);
        }

        /// <summary>Logs the deprecated part of code</summary>
        /// <param name="old">Deprecation object</param>
        /// <param name="replacement">Replacement</param>
        /// <exception cref="FormatException"><see cref="LoggerSettings.ExceptionString" /> doesn't contains {0}</exception>
        public void Deprecated(object old, object replacement)
        {
            if (!Settings.DeprecatedString.Contains("{0}"))
                throw new FormatException();
            Log(LogType.Deprecation, Settings.DeprecatedString, old, replacement);
        }

        /// <summary>Gets the input.</summary>
        /// <param name="type">The type.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>Returns user's input</returns>
        public string GetInputSimple(LogType type = LogType.Warning, string prefix = "")
        {
            lock (ConsoleWriterLock)
            {
                Console.CursorVisible = true;

                var logPrefix = GetPrefix(type);
                SetColor(type);
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
                        SetColor(type);
                        Console.SetCursorPosition(leftStart, topStart);
                        Console.Write(GetPrefix(type, true));
                        Console.SetCursorPosition(left, top);
                        Thread.Sleep(2000);
                    }
                });

                var input = Console.ReadLine() ?? "";

                stop = true;

                var leftEnd = (leftStart + input.Length) % Console.WindowWidth;
                var topEnd = (leftStart + input.Length) / Console.WindowWidth;

                Console.SetCursorPosition(leftEnd, topEnd + topStart);

                SetColor(type);
                Console.Write(" <<<" + Environment.NewLine);

                Console.CursorVisible = false;

                return input;
            }
        }
    }
}