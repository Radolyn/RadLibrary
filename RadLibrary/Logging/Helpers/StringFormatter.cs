#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace RadLibrary.Logging.Helpers
{
    // https://stackoverflow.com/a/36778184
    internal sealed class StringFormatter
    {
        public StringFormatter(string pFormat)
        {
            Format = pFormat;
            Parameters = new Dictionary<string, string>();
        }

        private string Format { get; }

        private Dictionary<string, string> Parameters { get; }

        public void Set(string key, string val)
        {
            if (!Parameters.ContainsKey(key))
                Parameters.Add(key, val);
            else
                Parameters[key] = val;
        }

        public override string ToString()
        {
            return Parameters.Aggregate(Format,
                (current, parameter) => current.Replace(parameter.Key, parameter.Value.ToString()));
        }
    }
}