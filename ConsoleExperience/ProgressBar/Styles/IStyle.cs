namespace RadLibrary.ConsoleExperience.ProgressBar.Styles
{
    public interface IStyle
    {
        /// <summary>
        ///     Get progress bar string
        /// </summary>
        /// <param name="iteration">The iteration</param>
        /// <param name="bar">The progress bar instance</param>
        /// <returns>The progress bar string</returns>
        public string GetProgress(long iteration, ColorfulProgressBar bar);

        /// <summary>
        ///     Get finished progress bar string
        /// </summary>
        /// <param name="bar">The progress bar instance</param>
        /// <returns>The finished progress bar string</returns>
        public string GetFinished(ColorfulProgressBar bar);
    }
}