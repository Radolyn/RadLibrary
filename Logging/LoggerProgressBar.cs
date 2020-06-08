#region

using System;
using System.Linq;

#endregion

namespace RadLibrary.Logging
{
    public partial class Logger
    {
        private static bool _progressBarInProgress;

        private static string _lastIteration;

        /// <summary>
        ///     Prints progress bar in console
        /// </summary>
        /// <param name="iteration">The iteration</param>
        /// <param name="settings">The progress bar settings</param>
        public void ProgressBar(int iteration, ProgressBarSettings settings)
        {
            // todo: animation support
            // todo: integrate with input function
            // SpinWait.SpinUntil(() => !_progressBarInProgress && !_inputInProgress);

            _progressBarInProgress = true;

            var prefix = settings.Prefix.Colorize(settings.PrefixColor).ResetColorAfter();
            var suffix = settings.Suffix.Colorize(settings.SuffixColor).ResetColorAfter();
            var fillChar = settings.FillChar.Colorize(settings.FillColor).ResetColorAfter();
            var progressChar = settings.ProgressChar.Colorize(settings.ProgressColor).ResetColorAfter();
            var separatorStr = settings.SeparatorStr.Colorize(settings.SeparatorColor).ResetColorAfter();

            var percent = string
                .Format("{0:0." + string.Concat(Enumerable.Repeat("0", settings.Decimals)) + "}%",
                    100 * (iteration / (settings.Total + 0.0))).Colorize(settings.PercentColor).ResetColorAfter();

            var filledLength = settings.Length * iteration / settings.Total;
            var bar = string.Concat(Enumerable.Repeat(fillChar, filledLength)) +
                      string.Concat(Enumerable.Repeat(progressChar, settings.Length - filledLength));

            var complete =
                $"{prefix} {separatorStr}{bar}{separatorStr} {percent} {suffix}";

            var outPrefix = GetPrefix(settings.LogType, true);

            var final = outPrefix + complete;

            if (iteration == settings.Total) _progressBarInProgress = false;

            lock (ConsoleWriterLock)
            {
                if (iteration == settings.Total)
                {
                    _progressBarInProgress = false;
                    Console.WriteLine(final);
                }
                else
                {
                    Console.Write(final);
                }
            }


            _lastIteration = final;
        }
    }
}