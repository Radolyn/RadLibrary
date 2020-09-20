#region

using System;
using System.IO;
using RadLibrary.Configuration;
using RadLibrary.Configuration.Managers;
using RadLibrary.Configuration.Scheme;
using Xunit;

#endregion

namespace RadLibrary.Tests
{
    public class ConfigurationTests
    {
        public ConfigurationTests()
        {
            var dir = Path.Combine(Path.GetTempPath(), "configTest" + Utilities.RandomInt());

            Directory.CreateDirectory(dir);

            Environment.CurrentDirectory = dir;
        }

        [Fact]
        public void ConfigurationSchemeTest()
        {
            var config = AppConfiguration.Initialize<FileManager>("basicTest" + Utilities.RandomInt());

            config.EnsureScheme(typeof(Config));

            config = AppConfiguration.Initialize<FileManager>("basicTest" + Utilities.RandomInt());

            config.EnsureScheme(Config.GetScheme());

            config = AppConfiguration.Initialize<FileManager>("basicTest" + Utilities.RandomInt());

            ConfigurationScheme.Ensure<Config>(config);

            config.SetInteger("coolInt", 123);

            var cfg = config.Cast<Config>();

            Assert.Null(cfg.CoolString);
        }

        [Fact]
        public void ConfigurationSchemeThrowsException()
        {
            var config = AppConfiguration.Initialize<FileManager>("basicTest" + Utilities.RandomInt());

            Assert.Throws<ArgumentException>(() => config.EnsureScheme(typeof(Config), false));
        }
    }

    public class Config
    {
        [SchemeParameter]
        // ReSharper disable once InconsistentNaming
        public int coolInt;

        [SchemeParameter(Comment = "Really cool string", Key = "someCoolKey")]
        public string CoolString;

        [SchemeParameter(Comment = "sad string")]
        public string NotCoolString = "is that cool?";

        public static ConfigurationScheme GetScheme()
        {
            var scheme = new ConfigurationScheme();

            scheme.AddParameter("someCoolKey", "", "Really cool string", typeof(string))
                .AddParameter(new SchemeParameter("coolInt", 0, "", typeof(int)))
                .AddParameter("notCoolString", "", "sad string", typeof(string));

            return scheme;
        }
    }
}