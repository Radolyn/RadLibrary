#region

using System;
using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    internal class NullFormatter : IObjectFormatter
    {
        public int Priority { get; } = 0;

        /// <inheritdoc />
        public Type Type { get; } = typeof(object);

        /// <inheritdoc />
        public string Format(object obj)
        {
            return "null";
        }
    }
}