#region

using System.Collections.Generic;
using System.Linq;
using RadLibrary.Configuration.Managers.IniManager;
using RadLibrary.Configuration.Scheme;
using RadLibrary.Formatting;
using RadLibrary.Logging;
using RadLibrary.RadConsole;
using Console = RadLibrary.RadConsole.RadConsole;

#endregion

namespace Examples
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // Don't forget to call this if you're not on .NET 5
            // RadUtilities.Initialize();

            // Loggers();
            RadConsole();
            // Configs();

            Console.WriteLine(@"\[\]");

            var readStyle = new ReadStyle {UnderlinePrediction = true};

            var s = Console.Read.Line(readStyle);
            var b = Console.Read.Boolean("Are you sure?");
            var i = Console.Read.Integer(readStyle);

            Console.WriteLine("\"{0}\" {1} {2}", s, b, i);
            
            Console.Read.Line();
            Console.Read.Line();
            Console.Read.Line();
        }

        private static void Configs()
        {
            var manager = new IniManager("config.ini");

            manager.EnsureScheme(typeof(Config));

            Console.WriteLine(manager["password"].Value);
            Console.WriteLine(manager["someField"].Value);
            Console.WriteLine(manager["someProperty"].Value);
        }

        private static void Loggers()
        {
            var settings = new LoggerSettings
            {
                FormatJson = false
            };

            var logger = LogManager.GetClassLogger(settings);

            logger.Info("IEnumerable formatting test:");

            for (var i = 0; i < 5; i++)
            {
                var list = new List<string>
                {
                    "cool string" + i,
                    "another cool string"
                };

                var dict = new Dictionary<string, bool>
                {
                    {"Go to the party?" + i, true},
                    {"Go to the school?", false}
                };

                var set = new HashSet<string>
                {
                    "cool string" + i,
                    "another cool string"
                };

                logger.Info(list, dict, set);
            }
        }

        private static void RadConsole()
        {
            Console.WriteLine("[ffaa22]Some text.[reset]\nNow I'm default!\n[red]And... Now red!\n{0}",
                "Some arg text");

            Console.WriteLine(new List<string>
            {
                "asdsads",
                "asddad"
            });

            Console.WriteLine(new HashSet<string>
            {
                "asdsads",
                "asddad"
            });

            var s = string.Format(FormattersStorage.FormatProvider, "{0}", new HashSet<string>
            {
                "asdsads",
                "asddad"
            });

            Console.WriteLine("[fff123]{0}", new HashSet<string>
            {
                "asdsads",
                "asddad"
            }.Select(x => x));

            Console.WriteLine("[fff123]{0}", null);

            Console.WriteLine("[#fffff]{CoolPrefix} [aaaff]Some colorful text");

            Console.WriteLine("[00ffcc]{CoolPrefix} [#ffff66]Some colorful text");

            object obj = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            Console.WriteLine(obj);

            Console.WriteLine("123");

            Console.WriteLine("[red]123");
            Console.WriteLine("[f00]123");
            Console.WriteLine("[fff000]123");
            Console.WriteLine("[underline][bold][blink][italic][framed]123");
            Console.WriteLine("123");

            var colors = "black blue green purple red white yellow".Split(' ');

            var foreground = colors.Aggregate("", (current, item) => current + $"[{item}]");
            Console.WriteLine(foreground + "test str");

            var background = colors.Aggregate("", (current, item) => current + $"[b:{item}]");
            Console.WriteLine(background + "test str");

            var brightForeground = colors.Aggregate("", (current, item) => current + $"[bright{item}]");
            Console.WriteLine(brightForeground + "test str");

            var brightBackground = colors.Aggregate("", (current, item) => current + $"[b:bright{item}]");
            Console.WriteLine(brightBackground + "test str");

            Console.Write(foreground + "test str");

            Console.WriteLine(foreground + "{0}", "111");

            var list = new List<string>
            {
                "yes?",
                "no!"
            };

            Console.Write(list);

            Console.WriteLine(list);
        }

        private class Config
        {
            [SchemeSection] public string SomeField = "Field";

            [SchemeSection("password")] public string SomePassword;

            [SchemeSection] public string SomeProperty = "Property";
        }
    }
}