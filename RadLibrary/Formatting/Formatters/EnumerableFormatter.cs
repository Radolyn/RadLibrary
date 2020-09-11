#region

using System.Collections;
using System.Text;
using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    public class EnumerableFormatter : ObjectFormatter<IEnumerable>
    {
        public override int Priority { get; } = 0;

        /// <inheritdoc />
        public override string FormatObject(IEnumerable obj)
        {
            var sb = new StringBuilder("<");

            EnumerableHelper.ParseEnumerable(sb, obj);

            sb.Append(">");

            return sb.ToString();
        }
    }
}