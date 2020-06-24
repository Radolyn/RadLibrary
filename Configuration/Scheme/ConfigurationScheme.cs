#region

using System;
using System.Collections.Generic;

#endregion

namespace RadLibrary.Configuration.Scheme
{
    /// <summary>
    ///     Config verification
    /// </summary>
    public class ConfigurationScheme
    {
        private readonly List<SchemeParameter> _scheme = new List<SchemeParameter>();

        /// <summary>
        ///     Sets comment
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="comment">The comment</param>
        public void SetComment(string key, string comment)
        {
            var pred = _scheme.Find(p => p.Key == key);

            if (pred != null)
                pred.Comment = comment;
            else
                _scheme.Add(new SchemeParameter(key, "", comment, null));
        }

        /// <summary>
        ///     Sets default value
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value</param>
        public void SetDefault(string key, string defaultValue)
        {
            var pred = _scheme.Find(p => p.Key == key);

            if (pred != null)
                pred.Value = defaultValue;
            else
                _scheme.Add(new SchemeParameter(key, defaultValue, null, null));
        }

        /// <summary>
        ///     Sets type
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="type">The type</param>
        public void SetType(string key, Type type)
        {
            var pred = _scheme.Find(p => p.Key == key);

            if (pred != null)
                pred.Type = type;
            else
                _scheme.Add(new SchemeParameter(key, "", null, type));
        }

        /// <summary>
        ///     Adds parameter
        /// </summary>
        /// <param name="param">The parameter</param>
        /// <returns>This configuration scheme</returns>
        public ConfigurationScheme AddParameter(SchemeParameter param)
        {
            var pred = _scheme.Find(p => p.Key == param.Key);

            if (pred != null)
                _scheme.Remove(pred);

            _scheme.Add(param);

            return this;
        }

        /// <summary>
        ///     Ensures configuration
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="safe">Throw exception on bad parameter</param>
        /// <exception cref="ArgumentException">Occurs when parameter not found -or- when parameter has invalid type</exception>
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
                    {
                        throw new ArgumentException("Key not found.", parameter.Key);
                    }

                if (parameter.Type != null)
                    try
                    {
                        _ = Convert.ChangeType(value, parameter.Type);
                    }
                    catch
                    {
                        if (safe)
                            config[parameter.Key] = parameter.Value;
                        else
                            throw new ArgumentException(
                                "Parameter value cannot be converted to " + parameter.Type.FullName,
                                parameter.Key);
                    }

                config.SetComment(parameter.Key, parameter.Comment);
            }

            config.Save();
        }
    }
}