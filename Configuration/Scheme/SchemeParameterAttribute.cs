#region

using System;

#endregion

namespace RadLibrary.Configuration.Scheme
{
    /// <summary>
    ///     The scheme parameter attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SchemeParameterAttribute : Attribute
    {
        /// <summary>
        ///     The key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     The value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     The comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     Initializes attribute
        /// </summary>
        public SchemeParameterAttribute()
        {
        }

        /// <summary>
        ///     Initializes attribute with specified key
        /// </summary>
        /// <param name="key">The key</param>
        public SchemeParameterAttribute(string key)
        {
            Key = key;
        }
    }
}