#region

using RadLibrary.Configuration;
using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    public class AppConfigurationFormatter : ObjectFormatter<AppConfiguration>
    {
        /// <inheritdoc />
        public override string FormatObject(AppConfiguration obj)
        {
            return FormattersStorage.GetCustomFormatterResult(obj.Parameters);
        }
    }
}