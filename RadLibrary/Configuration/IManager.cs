#region

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

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
        [ItemNotNull]
        IEnumerable<T> Sections { get; }

        /// <summary>
        ///     Get or set section by its key
        ///     <example>config["key"] = "value";</example>
        /// </summary>
        /// <param name="key">The key</param>
        [NotNull]
        T this[[NotNull] string key] { get; set; }

        /// <summary>
        ///     Get section by key
        /// </summary>
        /// <param name="section">The key</param>
        /// <returns>The section</returns>
        T GetSection([NotNull] string section);

        /// <summary>
        ///     Set section by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="section">The section</param>
        void SetSection([NotNull] string key, [NotNull] T section);

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
        void EnsureScheme([NotNull] Type type);
    }
}