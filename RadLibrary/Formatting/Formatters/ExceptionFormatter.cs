#region

using System;
using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    /// <inheritdoc />
    public class ExceptionFormatter : ObjectFormatter<Exception>
    {
        /// <inheritdoc />
        public override string FormatObject(Exception obj)
        {
            return $"{obj.Source}: {obj.GetType()}.\nMessage: {obj.Message}\nStack trace:\n{obj.StackTrace}";
        }
    }
}