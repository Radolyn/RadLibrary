#region

using System.Collections;
using System.Text;

#endregion

namespace RadLibrary.Formatting
{
    internal static class EnumerableHelper
    {
        public static void ParseEnumerable(StringBuilder sb, IEnumerable enumerable)
        {
            foreach (var o in enumerable)
            {
                var customFormatter = FormattersStorage.GetCustomFormatter(o);

                sb.Append(customFormatter.Format(o!));

                sb.Append(", ");
            }

            if (sb.Length != 1)
                sb.Remove(sb.Length - 2, 2);
        }
    }
}