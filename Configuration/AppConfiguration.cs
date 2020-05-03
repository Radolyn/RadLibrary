namespace RadLibrary.Configuration
{
    public delegate void ConfigurationUpdated();

    /// <summary>
    ///     Allows to create and work with configuration files
    /// </summary>
    /// <typeparam name="T">The configuration manager</typeparam>
    public class AppConfiguration<T> where T : IConfigurationManager, new()
    {
        /// <summary>
        ///     The configuration manager
        /// </summary>
        private readonly IConfigurationManager _manager;

        /// <summary>
        ///     Initializes configuration manager
        /// </summary>
        /// <param name="name">The name</param>
        public AppConfiguration(string name)
        {
            _manager = new T();
            _manager.Setup(name);
            _manager.ConfigurationUpdated += () => ConfigurationUpdated?.Invoke();
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

        /// <summary>
        ///     Saves configuration
        /// </summary>
        public void Save()
        {
            _manager.Save();
        }

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