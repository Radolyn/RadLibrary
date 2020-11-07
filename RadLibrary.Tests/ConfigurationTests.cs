#region

using System.IO;
using System.Linq;
using RadLibrary.Configuration.Managers.IniManager;
using RadLibrary.Configuration.Scheme;
using Xunit;

#endregion

namespace RadLibrary.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void IniTest()
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

        [Fact]
        public void TypeConversionsTest()
        {
            const string file = "test2.ini";

            File.WriteAllText(file, "key1 = false\nkey2 = 1338\n");

            var config = new IniManager(file);
            config.Load();

            var key1 = config["key1"].ValueAs<bool>();
            var key2 = config["key2"].ValueAs<int>();

            Assert.False(key1);
            Assert.Equal(1338, key2);
        }

        [Fact]
        public void SchemeTest()
        {
            const string file = "test3.ini";

            File.WriteAllText(file, "key1 = false\nkey2 = 1338\n");

            var config = new IniManager(file);
            config.Load();

            config.EnsureScheme(typeof(Config));

            Assert.Equal(ulong.MaxValue, config["count"].ValueAs<ulong>());
            Assert.Equal(default, config["port"].ValueAs<int>());
            Assert.Equal("127.0.0.1", config["ip"].Value);

            ;
        }

        public class Config
        {
            [SchemeSection] public string Ip = "127.0.0.1";

            [SchemeSection] public int Port { get; set; }

            [SchemeSection] public bool Connect { get; set; }

            [SchemeSection] public ulong Count { get; set; } = ulong.MaxValue;
        }
    }
}