#region

using System.Collections;
using System.Text;
using RadLibrary.Formatting.Abstractions;

#endregion

#pragma warning disable 8605

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    public class DictionaryFormatter : ObjectFormatter<IDictionary>
    {
        /// <inheritdoc />
        public override string FormatObject(IDictionary obj)
        {
            var sb = new StringBuilder("{");

            foreach (DictionaryEntry pair in obj)
            {
                var keyCustomFormatter = FormattersStorage.GetCustomFormatter(pair.Key);
                sb.Append(keyCustomFormatter.Format(pair.Key!));

                sb.Append(": ");

                var valueCustomFormatter = FormattersStorage.GetCustomFormatter(pair.Value);
                sb.Append(valueCustomFormatter.Format(pair.Value!));

                sb.Append(", ");
            }

            if (sb.Length != 1)
                sb.Remove(sb.Length - 2, 2);

            sb.Append("}");

            return sb.ToString();
        }
    }
}