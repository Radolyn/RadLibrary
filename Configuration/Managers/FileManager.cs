#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

#endregion

namespace RadLibrary.Configuration.Managers
{
    public class FileManager : IConfigurationManager
    {
        private List<Parameter> _config;
        private string _filename;

        private DateTime _lastUpdate;

        /// <inheritdoc />
        public event ConfigurationUpdated ConfigurationUpdated;

        private bool _hotReload;
        private FileSystemWatcher _watcher;

        /// <inheritdoc />
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
                    ConfigurationUpdated?.Invoke(this);
                };

                _watcher.Error += (sender, args) => HotReload = false;

                _watcher.EnableRaisingEvents = true;
            }
        }

        /// <inheritdoc />
        public void Setup(string filename)
        {
            _config = new List<Parameter>();
            _filename = filename + ".conf";

            ReloadConfiguration();
        }

        /// <inheritdoc />
        public IReadOnlyList<Parameter> GetParameters()
        {
            return _config;
        }

        private void ReloadConfiguration()
        {
            if (!NeedToReload())
                return;

            if (!File.Exists(_filename))
                File.Create(_filename).Close();

            _config = new List<Parameter>();

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

                var pred = _config.Find(p => p.Key == split[0]);

                if (pred == null)
                    _config.Add(new Parameter(split[0], split[1], sb.ToString()));
                else
                    throw new ArgumentException("Duplicated parameter", split[0]);

                sb.Clear();
            }

            _lastUpdate = DateTime.Now;
        }

        /// <inheritdoc />
        public string GetString(string key)
        {
            var param = _config.Find(p => p.Key == key);
            return param?.Value;
        }

        /// <inheritdoc />
        public int GetInteger(string key)
        {
            return int.Parse(GetString(key) ?? "0");
        }

        /// <inheritdoc />
        public bool GetBool(string key)
        {
            return bool.Parse(GetString(key) ?? "false");
        }

        /// <inheritdoc />
        public void SetString(string key, string value)
        {
            var pred = _config.Find(p => p.Key == key);

            if (pred != null)
                pred.Value = value;
            else
                _config.Add(new Parameter(key, value, null));
        }

        /// <inheritdoc />
        public void SetInteger(string key, int value)
        {
            SetString(key, value.ToString());
        }

        /// <inheritdoc />
        public void SetBool(string key, bool value)
        {
            SetString(key, value.ToString());
        }

        /// <inheritdoc />
        public void SetComment(string key, string comment)
        {
            comment = "# " + comment.Replace("\r\n", "\n# ").Replace("\n", "\n# ");
            var pred = _config.Find(p => p.Key == key);

            if (pred != null)
                pred.Comment = comment;
            else
                _config.Add(new Parameter(key, "", comment));
        }

        /// <inheritdoc />
        public void RemoveKey(string key)
        {
            var pred = _config.Find(p => p.Key == key);
            if (pred != null)
                _config.Remove(pred);
        }

        /// <inheritdoc />
        public void Save()
        {
            var s = new StringBuilder();
            foreach (var param in _config)
                if (param.Comment == "")
                {
                    s.Append(param.Key + "=" + param.Value + Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    s.AppendLine(param.Comment);
                    s.AppendLine(param.Key + "=" + param.Value + Environment.NewLine);
                }

            File.WriteAllText(_filename, s.ToString());
        }

        private bool NeedToReload()
        {
            // prevent from double notify (VS Code, etc.)
            return (DateTime.Now - _lastUpdate).TotalSeconds >= 0.8;
        }
    }
}