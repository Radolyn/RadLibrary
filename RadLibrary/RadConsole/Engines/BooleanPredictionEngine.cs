#region

using JetBrains.Annotations;

#endregion

namespace RadLibrary.RadConsole.Engines
{
    public class BooleanPredictionEngine : IPredictionEngine
    {
        /// <inheritdoc />
        [NotNull]
        public string Predict(string input)
        {
            if (input == null)
                return "";

            var lower = input.ToLower();

            foreach (var val in RadConsole.Read.TrueBooleans)
                if (val.StartsWith(lower))
                    return val;

            foreach (var val in RadConsole.Read.FalseBooleans)
                if (val.StartsWith(lower))
                    return val;

            return "";
        }
    }
}