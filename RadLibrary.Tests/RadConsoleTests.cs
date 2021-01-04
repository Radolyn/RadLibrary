#region

using System;
using System.Collections.Generic;
using System.Linq;
using RadLibrary.Colors;
using RadLibrary.Formatting;
using RadLibrary.RadConsole;
using Xunit;
using Xunit.Abstractions;
using Console = RadLibrary.RadConsole.RadConsole;

#endregion

namespace RadLibrary.Tests
{
    public class RadConsoleTests
    {
        public RadConsoleTests(ITestOutputHelper output)
        {
            Output = output;
        }

        public ITestOutputHelper Output { get; }

        [Fact]
        public void AssertConsolePrints()
        {
            Console.WriteLine("[ffaa22]Some text.[reset]\nNow I'm default!");
        }

        [Fact]
        public void AssertConsoleThrows()
        {
            Assert.Throws<FormatException>(() => Console.WriteLine("[gggggg][lol]Some text"));
        }

        [Fact]
        public void AssertConsoleProxies()
        {
            // actually, we can't test such a things because we can't allocate real console even for a 1 test
            // so just check the output of this test to see exceptions

            var properties = typeof(Console).GetProperties();

            foreach (var property in properties)
                try
                {
                    var val = property.GetValue(null);

                    if (property.CanWrite)
                        property.GetValue(val);
                }
                catch (Exception ex)
                {
                    Output.WriteLine(FormattersStorage.GetCustomFormatterResult(ex) + Environment.NewLine.Repeat(2));
                }
        }

        [Fact]
        public void WriteToConsoleTest()
        {
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

            Console.ResetColor();
        }

        [Fact]
        public void EscapeTest()
        {
            var s = Console.ParseColors("\\[\\\\]");
            var s2 = Console.ParseColors(@"C:\Windows\System32\drivers\etc\hosts");

            Assert.Equal("[\\]", s.DeColorize());
            Assert.Equal(@"C:\Windows\System32\drivers\etc\hosts", s2.DeColorize());
        }

        [Fact]
        public void PredictionTest()
        {
            var engine = new DefaultPredictionEngine();

            var nothingPrediction = engine.Predict("");
            var yesPrediction = engine.Predict("y");
            var noPrediction = engine.Predict("n");
            var historyPrediction = engine.Predict("history"); // we can't really use it in test
            var pathPrediction = engine.Predict("C:\\Win");

            Assert.Equal("", nothingPrediction);
            Assert.Equal("yes", yesPrediction);
            Assert.Equal("no", noPrediction);
            Assert.Equal("", historyPrediction);
            Assert.Equal("C:\\Windows", pathPrediction);
        }
    }
}