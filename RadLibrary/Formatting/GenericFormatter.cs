#region

using System;

#endregion

namespace RadLibrary.Formatting
{
    /// <inheritdoc cref="ICustomFormatter" />
    public class GenericFormatter : IFormatProvider, ICustomFormatter
    {
        /// <inheritdoc />
        public virtual string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return FormattersStorage.GetCustomFormatterResult(arg!);
        }

        /// <inheritdoc />
        public virtual object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }
}