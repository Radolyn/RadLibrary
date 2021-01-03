#region

using System.Collections.Generic;
using System.Drawing;
using RadLibrary.Colors;
using Xunit;

#endregion

namespace RadLibrary.Tests
{
    public class ColorizerTests
    {
        public ColorizerTests()
        {
            Colorizer.Initialize();
        }

        [Fact]
        public void ColorizeTest()
        {
            var hexStrings = new Dictionary<string, string>
            {
                {"32a852", "50, 168, 82"},
                {"#2b6dbd", "43, 109, 189"}
            };

            foreach (var (hex, rgb) in hexStrings)
            {
                var colorized = "test".Colorize(hex);
                var colorized2 = "test".ColorizeBackground(hex);
                var colorized3 = Colorizer.GetBackgroundColorizationString(Colorizer.HexToColor(hex)) + "test";

                foreach (var rgbColor in rgb.Split(", "))
                {
                    Assert.Contains(rgbColor, colorized);
                    Assert.Contains(rgbColor, colorized2);
                    Assert.Contains(rgbColor, colorized3);
                }
            }
        }

        [Fact]
        public void DeColorizerTest()
        {
            const string s = "Some cool string";

            var colorized = s.Colorize(Color.Aquamarine).ColorizeBackground(Color.Chocolate);

            Assert.Equal(s, colorized.DeColorize());
        }
    }
}