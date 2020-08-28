#region

using System;
using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    public class DefaultFormatter : IObjectFormatter
    {
        public int Priority { get; } = -5;

        /// <inheritdoc />
        public Type Type { get; } = typeof(object);

        /// <inheritdoc />
        public string Format(object obj)
        {
            return obj.ToString() ?? obj.GetType().Name;
        }
    }
}