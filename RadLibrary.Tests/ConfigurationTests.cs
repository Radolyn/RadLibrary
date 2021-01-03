#region

using System;
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
            const string file = "test3.1.ini";

            File.WriteAllText(file, "key1 = false\nkey2 = 1338\n");

            var config = new IniManager(file);
            config.Load();

            config.EnsureScheme(typeof(Config));

            Assert.Equal(ulong.MaxValue, config["count"].ValueAs<ulong>());
            Assert.Equal(default, config["port"].ValueAs<int>());
            Assert.Equal("127.0.0.1", config["ip"].Value);
        }

        [Fact]
        public void SchemePartialTest()
        {
            const string file = "test3.2.ini";

            File.WriteAllText(file, "port = 1332\nproperty = -20\nkey2 = 1338\n");

            var config = new IniManager(file);
            config.Load();

            config.EnsureScheme(typeof(Config));

            Assert.Equal(ulong.MaxValue, config["count"].ValueAs<ulong>());
            Assert.Equal(1332, config["port"].ValueAs<int>());
            Assert.Equal(default, config["property"].ValueAs<ushort>());
            Assert.Equal("127.0.0.1", config["ip"].Value);
        }

        [Fact]
        public void SchemeNullTest()
        {
            const string file = "test4.ini";

            File.WriteAllText(file, "key1 = false\nkey2 = 1338\n");

            var config = new IniManager(file);
            config.Load();

            Assert.Throws<ArgumentNullException>(() => config.EnsureScheme(null));
        }

        [Fact]
        public void SchemeInvalidTest()
        {
            const string file = "test5.ini";

            File.WriteAllText(file, "key1 = false\nkey2 = 1338\n");

            var config = new IniManager(file);
            config.Load();

            Assert.Throws<ArgumentException>(() => config.EnsureScheme(typeof(InvalidConfig)));
        }

        [Fact]
        public void DuplicateTest()
        {
            const string file = "test6.ini";

            File.WriteAllText(file, "key1 = false\nkey2 = 1338\nkey2 = 228");

            var config = new IniManager(file, false);
            Assert.Throws<ArgumentException>(() => config.Load());
        }

        [Fact]
        public void IniSectionTest()
        {
            const string file = "test7.ini";

            File.WriteAllText(file, "key1 = false\nkey2 = 1338\n");

            var config = new IniManager(file);
            config.Load();

            config["key1"].SetValue("1122");
            config["key1"].SetValue(321);

            Assert.Throws<NotSupportedException>(() => config["key1"].SetValue(config["key2"]));
            Assert.Throws<NotSupportedException>(() => config["key1"]["key3"]);

            Assert.Equal(config["key1"].Value, config["key1"].ToString());

            // implicit operators test

            IniSection section1 = "aaaa";
            IniSection section2 = true;
            IniSection section3 = 321;
        }

        public class InvalidConfig
        {
            public InvalidConfig(object arg)
            {
            }
        }

        public class Config
        {
            [SchemeSection] public string Ip = "127.0.0.1";

            [SchemeSection("port")] public ushort Port { get; set; }

            [SchemeSection("property", Comment = "asd")]
            public ushort SomeProperty { get; set; }

            [SchemeSection] public bool Connect { get; set; }

            [SchemeSection] public ulong Count { get; set; } = ulong.MaxValue;

            public void SomeMethod()
            {
                // *does something*
            }
        }

        public struct TestStruct
        {
        }
    }
}