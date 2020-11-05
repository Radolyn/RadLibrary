#region

using System.IO;
using System.Linq;
using RadLibrary.Configuration.Managers.IniManager;
using Xunit;

#endregion

namespace RadLibrary.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void ConfigurationTest()
        {
            const string file = "test1.ini";

            File.WriteAllText(file, "\n\n\r\nsome_key =some value\r\n# some comment\n\nsome_second_key = \"some val\"");

            var config = new IniManager(file);
            config.Load();

            Assert.Equal("some value", config["some_key"].Value);
            Assert.Equal("some val", config["some_second_key"].Value);

            config.Save();

            config["another_key"] = "ok";

            config["another_key"] = new IniSection("another_key", "ok2", null, false);

            config.Save();

            Assert.True(File.ReadAllText(file).Length != 0);
            Assert.True(config.Sections.Count() == 3);
        }
    }
}