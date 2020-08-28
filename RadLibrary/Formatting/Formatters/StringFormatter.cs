#region

using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    public class StringFormatter : ObjectFormatter<string>
    {
        /// <inheritdoc />
        public override string FormatObject(string obj)
        {
            if (GetRecursionDeep() == 1)
                return obj;

            return "\"" + obj + "\"";
        }
    }
}