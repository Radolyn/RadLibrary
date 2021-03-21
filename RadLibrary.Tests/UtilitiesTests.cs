#region

using System.Collections.Generic;
using RadLibrary.Configuration.Managers.IniManager;
using Xunit;

#endregion

namespace RadLibrary.Tests
{
    public class UtilitiesTests
    {
        [Fact]
        public void RepeatTest()
        {
            const string s = "R@d0l^И";

            var s2 = s.Repeat(2);
            var s3 = s.Repeat(2);
            var s4 = "".Repeat(2);

            Assert.Equal("R@d0l^ИR@d0l^И", s2);
            Assert.Equal(s2, s3);

            Assert.Equal("", s4);
        }

        [Fact]
        public void FirstCharacterToLowerTest()
        {
            const string s = "R@d0l^И";

            var s2 = s.FirstCharacterToLower();
            var s3 = s.FirstCharacterToLower();
            var s4 = "".FirstCharacterToLower();

            Assert.Equal("r@d0l^И", s2);
            Assert.Equal(s2, s3);
            Assert.Equal("", s4);
        }

        [Fact]
        public void RandomItemTest()
        {
            var list = new List<string>
            {
                "R@d0l^И",
                "r@dLlBr@r^"
            };

            var res = list.RandomItem();

            Assert.NotNull(res);
            Assert.Contains(list, x => x == res);
        }

        [Fact]
        public void GetDefaultTest()
        {
            var res1 = (int) typeof(int).GetDefault();
            var res2 = (string) typeof(string).GetDefault();
            var res3 = (IniManager) typeof(IniManager).GetDefault();

            Assert.Equal(default, res1);
            Assert.Equal(default, res2);
            Assert.Equal(default, res3);
        }

        [Fact]
        public void InitializeTest()
        {
            // ReSharper disable once RedundantArgumentDefaultValue
            RadUtilities.Initialize(true);
        }

        [Fact]
        public void AllocateConsoleTest()
        {
            RadUtilities.AllocateConsole();
        }

        [Fact]
        public void RandomTest()
        {
            RadUtilities.RandomBool();
            RadUtilities.RandomBool();
            RadUtilities.RandomBool();

            RadUtilities.RandomInt();
            RadUtilities.RandomInt();
            RadUtilities.RandomInt();
        }
    }
}