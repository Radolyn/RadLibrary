using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RadLibrary.Configuration
{
    public class FileManager : IConfigurationManager
    {
        private Dictionary<string, string> _config;
        private string _filename;

        public void Setup(string filename)
        {
            _config = new Dictionary<string, string>();
            _filename = filename;
            if (!File.Exists(filename))
                return;
            var text = File.ReadAllLines(filename);
            foreach (var d in text.Select(s => s.Split(new []{'='}, 2)))
            {
                _config.Add(d[0], d[1]);
            }
        }

        public string GetString(string key)
        {
            return _config.ContainsKey(key) ? _config[key] : null;
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
                _config[key] = value;
            else
                _config.Add(key, value);
        }

        public void SetInteger(string key, int value)
        {
            SetString(key, value.ToString());
        }

        public void SetBool(string key, bool value)
        {
            SetString(key, value.ToString());
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
            {
                s.AppendLine(pair.Key + "=" + pair.Value);
            }
            File.WriteAllText(_filename, s.ToString());
        }
    }
}