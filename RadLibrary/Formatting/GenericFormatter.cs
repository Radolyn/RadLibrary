#region

using System;

#endregion

namespace RadLibrary.Formatting
{
    /// <inheritdoc cref="ICustomFormatter" />
    public class GenericFormatter : IFormatProvider, ICustomFormatter
    {
        /// <inheritdoc />
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        /// <inheritdoc />
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return FormattersStorage.GetCustomFormatterResult(arg!);
        }
    }
}