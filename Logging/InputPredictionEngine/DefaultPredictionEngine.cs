#region

using System;
using System.IO;
using System.Linq;

#endregion

namespace RadLibrary.Logging.InputPredictionEngine
{
    public class DefaultPredictionEngine : IPredictionEngine
    {
        public string Predict(string input)
        {
            switch (input)
            {
                case "":
                    return "";
                case "n":
                    return "no";
                case "ye":
                case "y":
                    return "yes";
                default:
                    return PredictPath(input);
            }
        }

        private string PredictPath(string input)
        {
            input = input.Replace("\"", "");
            var current = Directory.GetFileSystemEntries(Environment.CurrentDirectory);

            var inCurrent = current.Where(s => Path.GetFileName(s).Contains(input)).ToArray();
            if (inCurrent.Length != 0)
                return Path.GetFileName(inCurrent[0]);

            var dir = input.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var path = string.Concat(dir.Take(dir.Length - 1).Select(s => s + Path.DirectorySeparatorChar));

            if (!Directory.Exists(path))
                return "";

            var entries = Directory.GetFileSystemEntries(path + Path.DirectorySeparatorChar, dir.Last() + "*");

            return entries.Length == 0
                ? ""
                : entries[0].Replace(Path.DirectorySeparatorChar + Path.DirectorySeparatorChar.ToString(),
                    Path.DirectorySeparatorChar.ToString());
        }
    }
}