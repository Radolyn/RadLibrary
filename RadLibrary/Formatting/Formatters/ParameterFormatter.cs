#region

using RadLibrary.Configuration;
using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    public class ParameterFormatter : ObjectFormatter<Parameter>
    {
        /// <inheritdoc />
        public override string FormatObject(Parameter obj)
        {
            return $"\"{obj.Key}\": \"{obj.Value}\"" + (string.IsNullOrEmpty(obj.Comment) ? "" : $" ({obj.Comment})");
        }
    }
}