#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RadLibrary.Formatting.Abstractions;

#endregion

namespace RadLibrary.Formatting.Formatters
{
    public class HashSetFormatter : ObjectFormatter<object>
    {
        public override Type Type { get; } = typeof(ISet<>);

        public override string FormatObject(object obj)
        {
            var sb = new StringBuilder("(");

            EnumerableHelper.ParseEnumerable(sb, obj as IEnumerable);

            sb.Append(")");

            return sb.ToString();
        }
    }
}