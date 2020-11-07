#region

using System;
using System.Collections.Generic;

#endregion

namespace RadLibrary.Configuration
{
    /// <summary>
    ///     Defines basic configuration manager methods
    /// </summary>
    /// <typeparam name="T">The configuration section</typeparam>
    public interface IManager<T> where T : IConfigurationSection
    {
        /// <summary>
        ///     All root sections
        /// </summary>
        IEnumerable<T> Sections { get; }

        /// <summary>
        ///     Get or set section by its key
        ///     <example>config["key"] = "value";</example>
        /// </summary>
        /// <param name="key">The key</param>
        T this[string key] { get; set; }

        /// <summary>
        ///     Get section by key
        /// </summary>
        /// <param name="section">The key</param>
        /// <returns>The section</returns>
        T GetSection(string section);

        /// <summary>
        ///     Set section by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="section">The section</param>
        void SetSection(string key, T section);

        /// <summary>
        ///     Load configuration in memory
        /// </summary>
        void Load();

        /// <summary>
        ///     Save configuration
        /// </summary>
        void Save();

        /// <summary>
        ///     Ensure config scheme
        /// </summary>
        /// <param name="type">The config scheme</param>
        void EnsureScheme(Type type);
    }
}