#region

using JetBrains.Annotations;

#endregion

namespace RadLibrary.Configuration
{
    /// <summary>
    ///     Defines basic configuration section methods
    /// </summary>
    public interface IConfigurationSection
    {
        /// <summary>
        ///     Get child section by key
        /// </summary>
        /// <param name="key">The key</param>
        [NotNull]
        public IConfigurationSection this[[NotNull] string key] { get; }

        /// <summary>
        ///     The key
        /// </summary>
        [NotNull]
        public string Key { get; }

        /// <summary>
        ///     The value
        /// </summary>
        [CanBeNull]
        public string Value { get; }

        /// <summary>
        ///     The comment (null if none or unsupported)
        /// </summary>
        [CanBeNull]
        public string Comment { get; set; }

        /// <summary>
        ///     Sets value of this section
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of value</typeparam>
        public void SetValue<T>([CanBeNull] T value);

        TU ValueAs<TU>() where TU : new();
    }
}