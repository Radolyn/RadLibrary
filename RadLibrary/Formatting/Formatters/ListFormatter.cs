#nullable enable

#region

using System.Collections;
using System.Text;
using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    public class ListFormatter : ObjectFormatter<IList>
    {
        /// <inheritdoc />
        public override string FormatObject(IList obj)
        {
            var sb = new StringBuilder("[");

            EnumerableHelper.ParseEnumerable(sb, obj);

            sb.Append("]");

            return sb.ToString();
        }
    }
}