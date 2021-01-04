#region

using JetBrains.Annotations;

#endregion

namespace RadLibrary.RadConsole
{
    public interface IPredictionEngine
    {
        public string Predict([CanBeNull] string input);
    }
}