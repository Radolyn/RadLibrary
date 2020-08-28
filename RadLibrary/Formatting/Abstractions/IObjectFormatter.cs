#region

using System;

#endregion

namespace RadLibrary.Formatting.Abstractions
{
    /// <summary>
    ///     Represents a formatter for object
    /// </summary>
    public interface IObjectFormatter
    {
        public int Priority { get; }

        /// <summary>
        ///     The type (interface, class) that can handle this formatter instance
        /// </summary>
        Type Type { get; }

        /// <summary>
        ///     Returns formatted string
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The formatted string</returns>
        string Format(object obj);
    }
}