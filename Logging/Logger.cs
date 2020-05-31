#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

#endregion

namespace RadLibrary.Logging
{
    /// <summary>
    ///     The custom argument handler delegate
    /// </summary>
    /// <param name="obj">The object.</param>
    public delegate object CustomHandlerDelegate(object obj);

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

        /// <summary>
        ///     The custom handlers dictionary
        /// </summary>
        public readonly Dictionary<Type, CustomHandlerDelegate> CustomHandlers =
            new Dictionary<Type, CustomHandlerDelegate>();

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

                if (message.Contains("\n"))
                {
                    var messages = message.Split('\n');
                    foreach (var msg in messages)
                        Console.WriteLine((prefix + msg).Colorize(GetColor(type)).ResetColorAfter());
                }
                else
                {
                    Console.WriteLine((prefix + message).Colorize(GetColor(type)).ResetColorAfter());
                }
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

            var str = string.Format(Settings.LoggerPrefix + " ", date, Name, type.ToString(), LoggerThread)
                .Colorize(GetColor(type));

            return rewrite ? "\r" + str : str;
        }

        /// <summary>
        ///     Gets color
        /// </summary>
        /// <param name="type">The logger type.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal Color GetColor(LogType type)
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
    }
}