#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace RadLibrary.Logging.Helpers
{
    // https://stackoverflow.com/a/36778184
    internal sealed class StringFormatter
    {
        private string Format { get; }

        private Dictionary<string, string> Parameters { get; }

        public StringFormatter(string pFormat)
        {
            Format = pFormat;
            Parameters = new Dictionary<string, string>();
        }

        public void Add(string key, string val)
        {
            Parameters.Add(key, val);
        }

        public override string ToString()
        {
            return Parameters.Aggregate(Format,
                (current, parameter) => current.Replace(parameter.Key, parameter.Value.ToString()));
        }
    }
}