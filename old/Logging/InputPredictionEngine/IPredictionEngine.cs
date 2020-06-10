namespace RadLibrary.Logging.InputPredictionEngine
{
    public interface IPredictionEngine
    {
        /// <summary>
        ///     Predict user's input
        /// </summary>
        /// <param name="input">The input</param>
        /// <param name="logger">The logger</param>
        /// <returns>Prediction</returns>
        string Predict(string input, Logger logger);
    }
}