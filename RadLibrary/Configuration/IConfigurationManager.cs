#region

using System.Collections.Generic;

#endregion

namespace RadLibrary.Configuration
{
    public interface IConfigurationManager
    {
        /// <summary>
        ///     Gets or sets necessity in hot reload
        /// </summary>
        bool HotReload { get; set; }

        /// <summary>
        ///     Event that invokes on configuration update
        /// </summary>
        event ConfigurationUpdated ConfigurationUpdated;

        /// <summary>
        ///     Setups manager
        /// </summary>
        /// <param name="name">The name</param>
        void Setup(string name);

        /// <summary>
        ///     Returns COPY of all parameters
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<Parameter> GetParameters();

        /// <summary>
        ///     Gets string from configuration
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>Value</returns>
        string GetString(string key);

        /// <summary>
        ///     Gets integer from configuration
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>Value</returns>
        int GetInteger(string key);

        /// <summary>
        ///     Gets bool from configuration
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>Value</returns>
        bool GetBool(string key);

        /// <summary>
        ///     Sets string by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">Value</param>
        void SetString(string key, string value);

        /// <summary>
        ///     Sets integer by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">Value</param>
        void SetInteger(string key, int value);

        /// <summary>
        ///     Sets bool by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">Value</param>
        void SetBool(string key, bool value);

        /// <summary>
        ///     Sets comment for key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="comment">The comment</param>
        void SetComment(string key, string comment);

        /// <summary>
        ///     Removes parameter from configuration by key
        /// </summary>
        /// <param name="key">The key</param>
        void RemoveKey(string key);

        /// <summary>
        ///     Saves configuration
        /// </summary>
        void Save();
    }
}