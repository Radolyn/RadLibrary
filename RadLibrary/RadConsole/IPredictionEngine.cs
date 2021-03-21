#region

using JetBrains.Annotations;

#endregion

namespace RadLibrary.RadConsole
{
    /// <summary>
    ///     Prediction engine uses input string to predict user's input.
    /// </summary>
    public interface IPredictionEngine
    {
        /// <summary>
        ///     Predict string based on input.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The prediction</returns>
        public string Predict([CanBeNull] string input);
    }
}