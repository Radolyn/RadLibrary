#region

using System;
using System.Collections.Generic;
using RadLibrary.Configuration.Scheme;

#endregion

namespace RadLibrary.Configuration
{
    public delegate void ConfigurationUpdated(IConfigurationManager config);

    /// <summary>
    ///     Allows to create and work with configuration files
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        ///     The configuration manager
        /// </summary>
        private readonly IConfigurationManager _manager;

        private object _configInstance;

        /// <summary>
        ///     Enables or disables hot reload
        /// </summary>
        public bool HotReload
        {
            get => _manager.HotReload;
            set => _manager.HotReload = value;
        }

        /// <summary>
        ///     Returns all parameters (read-only)
        /// </summary>
        public IReadOnlyList<Parameter> Parameters => _manager.GetParameters();

        /// <summary>
        ///     Initializes configuration manager
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="manager">The manager</param>
        private AppConfiguration(string name, IConfigurationManager manager)
        {
            _manager = manager;
            _manager.Setup(name);
            _manager.ConfigurationUpdated += configurationManager =>
            {
                Cast();
                ConfigurationUpdated?.Invoke(configurationManager);
            };
        }

        /// <summary>
        ///     Creates <see cref="AppConfiguration" /> with specified <see cref="IConfigurationManager" />
        /// </summary>
        /// <param name="name">The name</param>
        /// <typeparam name="TManager">The manager</typeparam>
        /// <returns>
        ///     <see cref="AppConfiguration" />
        /// </returns>
        public static AppConfiguration Initialize<TManager>(string name) where TManager : IConfigurationManager, new()
        {
            return new AppConfiguration(name, new TManager());
        }

        /// <summary>
        ///     Creates <see cref="AppConfiguration" /> with specified <see cref="IConfigurationManager" />
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="manager">The manager</param>
        /// <returns>
        ///     <see cref="AppConfiguration" />
        /// </returns>
        public static AppConfiguration Initialize(string name, IConfigurationManager manager)
        {
            return new AppConfiguration(name, manager);
        }

        /// <summary>
        ///     Removes key from configuration
        /// </summary>
        /// <param name="key">The key</param>
        public void RemoveKey(string key)
        {
            _manager.RemoveKey(key);
        }

        /// <summary>
        ///     Gets boolean
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>bool</returns>
        public bool GetBool(string key)
        {
            return _manager.GetBool(key);
        }

        /// <summary>
        ///     Gets integer
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>int</returns>
        public int GetInteger(string key)
        {
            return _manager.GetInteger(key);
        }

        /// <summary>
        ///     Sets bool
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public void SetBool(string key, bool value)
        {
            _manager.SetBool(key, value);
        }

        /// <summary>
        ///     Sets integer
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public void SetInteger(string key, int value)
        {
            _manager.SetInteger(key, value);
        }

        public void SetComment(string key, string comment)
        {
            if (comment != null)
                _manager.SetComment(key, comment);
        }

        /// <summary>
        ///     Ensures configuration
        /// </summary>
        /// <param name="scheme">The scheme</param>
        /// <param name="safe">Throw exception on bad parameter</param>
        /// <exception cref="ArgumentException">Occurs when parameter not found -or- when parameter has invalid type</exception>
        public void EnsureScheme(ConfigurationScheme scheme, bool safe = true)
        {
            scheme.Ensure(this);
        }

        /// <summary>
        ///     Ensures configuration
        /// </summary>
        /// <exception cref="ArgumentException">Occurs when parameter not found -or- when parameter has invalid type</exception>
        public void EnsureScheme(Type scheme, bool safe = true)
        {
            ConfigurationScheme.Ensure(this, scheme, safe);
        }

        /// <summary>
        ///     Saves configuration
        /// </summary>
        public void Save()
        {
            _manager.Save();
        }

        /// <summary>
        ///     Convert app configuration to specified class
        /// </summary>
        /// <typeparam name="T">The class</typeparam>
        /// <returns>Filled instance</returns>
        public T Cast<T>() where T : new()
        {
            _configInstance ??= new T();

            Cast();

            return (T) _configInstance;
        }

        private void Cast()
        {
            if (_configInstance == null)
                return;

            var type = _configInstance.GetType();

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var paramName =
                    Attribute.GetCustomAttribute(field, typeof(SchemeParameterAttribute)) is SchemeParameterAttribute
                        attribute
                        ? attribute.Key ?? Utilities.FirstCharacterToLower(field.Name)
                        : Utilities.FirstCharacterToLower(field.Name);
                field.SetValue(_configInstance, Convert.ChangeType(this[paramName], field.FieldType));
            }
        }

        /// <summary>
        ///     Notices about configuration update
        /// </summary>
        public event ConfigurationUpdated ConfigurationUpdated;

        /// <summary>
        ///     Gets or sets string
        /// </summary>
        /// <param name="key">The key</param>
        public string this[string key]
        {
            get => _manager.GetString(key);

            set => _manager.SetString(key, value);
        }
    }
}