#region

using System;
using System.Collections.Generic;
using System.Linq;
using RadLibrary.Formatting;
using RadLibrary.Formatting.Formatters;
using Xunit;

#endregion

namespace RadLibrary.Tests
{
    public class FormattingTests
    {
        [Fact]
        public void GetCustomFormatterResultTest()
        {
            FormattersStorage.AddDefault();
            FormattersStorage.AddFormatter<EnumerableFormatter>();

            FormattersStorage.MaxRecursion = 2;

            var res = FormattersStorage.GetCustomFormatterResult(new List<object>
            {
                "sadasdsda",
                123,
                new List<int>
                {
                    1,
                    2
                }
            });

            Assert.True(FormattersStorage.RegisteredFormatters.Any());
            Assert.False(string.IsNullOrWhiteSpace(res));
        }

        [Fact]
        public void AllFormattersTest()
        {
            var obj = new List<object>
            {
                "123",
                123,
                (ushort) 2,
                new HashSet<int>
                {
                    1,
                    2,
                    3,
                    4
                },
                new Dictionary<string, string>
                {
                    {"at Radolyn we take", "our jobs very seriously"},
                    {"yes", null} // haha, suppress null warning with '!', wtf
                },
                null,
                null,
                new List<ushort>
                {
                    120,
                    1
                }.AsQueryable(),
                new ArgumentException("oh it works!")
            };

            var res = FormattersStorage.GetCustomFormatterResult(obj);

            Assert.False(string.IsNullOrWhiteSpace(res));
        }
    }
}