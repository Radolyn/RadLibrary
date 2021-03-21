﻿#region

using System;
using System.IO;
using System.Linq;

#endregion

namespace RadLibrary.RadConsole
{
    /// <summary>
    ///     Default prediction engine. Predicts paths, "yes" or "no" and history
    /// </summary>
    public class DefaultPredictionEngine : IPredictionEngine
    {
        /// <inheritdoc />
        public virtual string Predict(string input)
        {
            switch (input)
            {
                case null:
                case "":
                    return "";
                case "n":
                case "no":
                    return "no";
                case "y":
                case "ye":
                case "yes":
                    return "yes";
                default:
                    return PredictHistory(input) ?? PredictPath(input);
            }
        }

        protected virtual string PredictHistory(string input)
        {
            var history = RadConsole.History.Where(s => s.StartsWith(input, StringComparison.Ordinal));
            return history.FirstOrDefault();
        }

        protected virtual string PredictPath(string input)
        {
            try
            {
                input = input.Replace("\"", "");
                var current = Directory.GetFileSystemEntries(Environment.CurrentDirectory);

                var inCurrent = current
                    .Where(s => Path.GetFileName(s).StartsWith(input, StringComparison.OrdinalIgnoreCase)).ToArray();
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
            catch
            {
                return "";
            }
        }
    }
}