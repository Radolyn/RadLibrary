#region

using System;
using Xunit;
using Console = RadLibrary.RadConsole.RadConsole;

#endregion

namespace RadLibrary.Tests
{
    public class RadConsoleTests
    {
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
    }
}