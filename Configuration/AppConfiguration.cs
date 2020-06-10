﻿#region

using System.Collections.Generic;

#endregion

namespace RadLibrary.Configuration
{
    public delegate void ConfigurationUpdated();

    /// <summary>
    ///     Allows to create and work with configuration files
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        ///     The configuration manager
        /// </summary>
        private readonly IConfigurationManager _manager;

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
        public Dictionary<string, Parameter> Parameters => _manager.GetParameters();

        /// <summary>
        ///     Initializes configuration manager
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="manager">The manager</param>
        private AppConfiguration(string name, IConfigurationManager manager)
        {
            _manager = manager;
            _manager.Setup(name);
            _manager.ConfigurationUpdated += () => ConfigurationUpdated?.Invoke();
        }

        public static AppConfiguration Initialize<TManager>(string name) where TManager : IConfigurationManager, new()
        {
            return new AppConfiguration(name, new TManager());
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
            _manager.SetComment(key, comment);
        }

        /// <summary>
        ///     Saves configuration
        /// </summary>
        public void Save()
        {
            _manager.Save();
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