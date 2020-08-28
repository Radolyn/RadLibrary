#region

using System.Collections.Generic;
using System.Threading;
using RadLibrary;
using RadLibrary.Configuration;
using RadLibrary.Configuration.Managers;
using RadLibrary.Logging;

#endregion

namespace Examples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Utilities.Initialize();

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

            var config = AppConfiguration.Initialize<FileManager>("example");

            config["exampleKey"] = "example";
            config.SetComment("exampleKey", "Defines example key.");

            logger.Warn(config);

            HotReloadTest();
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
    }
}