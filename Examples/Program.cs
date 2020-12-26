#region

using System;
using System.Collections.Generic;
using RadLibrary;
using RadLibrary.Logging;
using RadLibrary.RadConsole;

#endregion

namespace Examples
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Utilities.Initialize();

            // RadConsole.ReadLine(new ReadStyle
            // {
            //     Prefix = "Input your name:",
            //     Postfix = "<<< Good name!",
            //     UnderlineInput = true
            // }, new DefaultPredictionEngine());
            // var s = RadConsole.ReadLine(new ReadStyle
            // {
            //     Prefix = "Input your name:",
            //     Postfix = "<<< Good name!"
            // }, new DefaultPredictionEngine());

            RadConsole.Write("[fffaaa][b:00ffaa]msg[b:reset][framed]asdsa");

            // Console.WriteLine(s);
            Console.ReadLine();
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

        // private static void ColorfulConsoleTest()
        // {
        //     ColorfulConsole.WriteLine(new List<string>
        //     {
        //         "asdsads",
        //         "asddad"
        //     });
        //
        //     ColorfulConsole.WriteLine(new HashSet<string>
        //     {
        //         "asdsads",
        //         "asddad"
        //     });
        //
        //     ColorfulConsole.WriteLine("[fff123]{0}", new HashSet<string>
        //     {
        //         "asdsads",
        //         "asddad"
        //     }.Select(x => x));
        // }
    }
}