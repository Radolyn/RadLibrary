#region

using System;

#endregion

namespace RadLibrary.Configuration.Scheme
{
    /// <summary>
    ///     The scheme parameter
    /// </summary>
    public class SchemeParameter : Parameter
    {
        /// <summary>
        ///     Initializes class
        /// </summary>
        public SchemeParameter()
        {
        }

        /// <summary>
        ///     Initializes class with specified key, default value, comment & type
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The default value</param>
        /// <param name="comment">The comment</param>
        /// <param name="type">The type</param>
        public SchemeParameter(string key, object value, string comment, Type type)
        {
            Key = key;
            Value = value.ToString();
            Comment = comment;
            Type = type;
        }

        /// <summary>
        ///     The type
        /// </summary>
        public Type Type { get; internal set; }
    }
}