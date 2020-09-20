#region

using System;

#endregion

namespace RadLibrary.Formatting
{
    /// <inheritdoc cref="ICustomFormatter" />
    public sealed class GenericFormatter : IFormatProvider, ICustomFormatter
    {
        /// <inheritdoc />
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return FormattersStorage.GetCustomFormatterResult(arg!);
        }

        /// <inheritdoc />
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }
}