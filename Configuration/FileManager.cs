#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

#endregion

namespace RadLibrary.Configuration
{
    public class FileManager : IConfigurationManager
    {
        private Dictionary<string, Parameter> _config;
        private string _filename;

        private DateTime _lastUpdate;

        public event ConfigurationUpdated ConfigurationUpdated;

        private bool _hotReload;
        private FileSystemWatcher _watcher;

        public bool HotReload
        {
            get => _hotReload;
            set
            {
                _hotReload = value;
                _watcher?.Dispose();

                if (!value) return;

                _watcher = new FileSystemWatcher {NotifyFilter = NotifyFilters.LastWrite, Filter = _filename};

                var path = Path.GetDirectoryName(_filename);

                if (string.IsNullOrEmpty(path))
                    path = AppDomain.CurrentDomain.BaseDirectory;

                _watcher.Path = path;

                _watcher.Changed += (sender, args) =>
                {
                    if (!NeedToReload())
                        return;

                    Thread.Sleep(5);
                    ReloadConfiguration();
                    ConfigurationUpdated?.Invoke();
                };
                _watcher.Error += (sender, args) => HotReload = false;

                _watcher.EnableRaisingEvents = true;
            }
        }

        public void Setup(string filename)
        {
            _config = new Dictionary<string, Parameter>();
            _filename = filename + ".conf";

            ReloadConfiguration();
        }

        public Dictionary<string, Parameter> GetParameters()
        {
            // clone
            return _config.ToDictionary(entry => entry.Key,
                entry => entry.Value);
        }

        private void ReloadConfiguration()
        {
            if (!NeedToReload())
                return;

            if (!File.Exists(_filename))
                File.Create(_filename).Close();

            _config = new Dictionary<string, Parameter>();

            var text = File.ReadAllLines(_filename);

            // comment builder
            var sb = new StringBuilder();

            foreach (var s in text)
            {
                if (s.StartsWith("#"))
                {
                    sb.Append(s);
                    continue;
                }

                var split = s.Split(new[] {'='}, 2);

                if (split.Length == 0 || string.IsNullOrEmpty(split[0]))
                    continue;

                if (split.Length == 1)
                    split = new[] {split[0], ""};

                if (!_config.ContainsKey(split[0]))
                    _config.Add(split[0], new Parameter(split[1], sb.ToString()));
                else
                    _config[split[0]] = new Parameter(split[1], sb.ToString());

                sb.Clear();
            }

            _lastUpdate = DateTime.Now;
        }

        public string GetString(string key)
        {
            return _config.ContainsKey(key) ? _config[key].Value : null;
        }

        public int GetInteger(string key)
        {
            return int.Parse(GetString(key) ?? "0");
        }

        public bool GetBool(string key)
        {
            return bool.Parse(GetString(key) ?? "0");
        }

        public void SetString(string key, string value)
        {
            if (_config.ContainsKey(key))
                _config[key].Value = value;
            else
                _config.Add(key, new Parameter(value, null));
        }

        public void SetInteger(string key, int value)
        {
            SetString(key, value.ToString());
        }

        public void SetBool(string key, bool value)
        {
            SetString(key, value.ToString());
        }

        public void SetComment(string key, string comment)
        {
            comment = "# " + comment.Replace("\r\n", "# ").Replace("\n", "# ");
            if (_config.ContainsKey(key))
                _config[key].Comment = comment;
            else
                _config.Add(key, new Parameter("", comment));
        }

        public void RemoveKey(string key)
        {
            if (_config.ContainsKey(key))
                _config.Remove(key);
        }

        public void Save()
        {
            var s = new StringBuilder();
            foreach (var pair in _config)
                if (pair.Value.Comment == "")
                {
                    s.Append(pair.Key + "=" + pair.Value.Value);
                }
                else
                {
                    s.AppendLine(pair.Value.Comment);
                    s.AppendLine(pair.Key + "=" + pair.Value.Value + "\n");
                }

            File.WriteAllText(_filename, s.ToString());
        }

        private bool NeedToReload()
        {
            // prevent from double notify (VS Code, etc.)
            return !((DateTime.Now - _lastUpdate).TotalSeconds < 0.8);
        }
    }
}