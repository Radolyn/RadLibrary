namespace RadLibrary.ConsoleExperience.PredictionEngine
{
    public interface IPredictionEngine
    {
        /// <summary>
        ///     Predict user's input
        /// </summary>
        /// <param name="input">The input</param>
        /// <returns>Prediction</returns>
        string Predict(string input);
    }
}