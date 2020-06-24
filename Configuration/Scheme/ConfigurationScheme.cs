using System;
using System.Collections.Generic;

namespace RadLibrary.Configuration.Scheme
{
    public class ConfigurationScheme
    {
        private List<SchemeParameter> _scheme = new List<SchemeParameter>();
        
        public void SetComment(string key, string comment)
        {
            var pred = _scheme.Find(p => p.Key == key);
            
            if (pred != null)
                pred.Comment = comment;
            else
                _scheme.Add(new SchemeParameter(key, "", comment, null));
        }

        public void SetDefault(string key, string defaultValue)
        {
            var pred = _scheme.Find(p => p.Key == key);
            
            if (pred != null)
                pred.Value = defaultValue;
            else
                _scheme.Add(new SchemeParameter(key, defaultValue, null, null));
        }

        public void SetType(string key, Type type)
        {
            var pred = _scheme.Find(p => p.Key == key);

            if (pred != null)
                pred.Type = type;
            else
                _scheme.Add(new SchemeParameter(key, "", null, type));
        }

        public ConfigurationScheme AddParameter(SchemeParameter param)
        {
            var pred = _scheme.Find(p => p.Key == param.Key);

            if (pred != null)
                _scheme.Remove(pred);
            
            _scheme.Add(param);

            return this;
        }

        public void Ensure(AppConfiguration config, bool safe = true)
        {
            foreach (var parameter in _scheme)
            {
                var value = config[parameter.Key];
                
                if (value == null)
                    if (safe)
                    {
                        config[parameter.Key] = parameter.Value;
                        value = parameter.Value;
                    }
                    else
                        throw new ArgumentException("Key not found.", parameter.Key);

                try
                {
                    _ = Convert.ChangeType(value, parameter.Type);
                }
                catch
                {
                    if (safe)
                        config[parameter.Key] = parameter.Value;
                    else
                        throw new ArgumentException("Parameter value cannot be converted to " + parameter.Type.FullName, parameter.Key);
                }
                
                config.SetComment(parameter.Key, parameter.Comment);
            }
            
            config.Save();
        }
    }
}