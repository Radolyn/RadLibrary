#region

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RadLibrary;
using RadLibrary.Configuration;
using RadLibrary.Configuration.Managers;
using RadLibrary.ConsoleExperience;
using RadLibrary.Logging;

#endregion

namespace Examples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Utilities.Initialize();

            ColorfulConsoleTest();

            //HotReloadTest();
        }

        private static void LoggerTest()
        {
            var settings = new LoggerSettings
            {
                FormatJson = false
            };

            var logger = LogManager.GetClassLogger(settings);

            logger.Info("IEnumerable formatting test:");

            for (var i = 0; i < 100; i++)
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

        private static void ConfigTest()
        {
            var config = AppConfiguration.Initialize<FileManager>("example");

            config["exampleKey"] = "example";
            config.SetComment("exampleKey", "Defines example key.");

            ColorfulConsole.WriteLine(config);
        }

        private static void HotReloadTest()
        {
            var config = AppConfiguration.Initialize<FileManager>("example2");

            config.HotReload = true;

            var instance = config.Cast<Config>();

            config.EnsureScheme(typeof(Config));

            var settings = new LoggerSettings
            {
                FormatJson = false
            };

            var logger = LogManager.GetClassLogger(settings);

            for (var i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);
                logger.Info(config);
                logger.Warn(instance.CoolParam);
            }
        }

        private static void ColorfulConsoleTest()
        {
            ColorfulConsole.WriteLine(new List<string>
            {
                "asdsads",
                "asddad"
            });

            ColorfulConsole.WriteLine(new HashSet<string>
            {
                "asdsads",
                "asddad"
            });

            ColorfulConsole.WriteLine("[fff123]{0}", new HashSet<string>
            {
                "asdsads",
                "asddad"
            }.Select(x => x));
        }
    }
}