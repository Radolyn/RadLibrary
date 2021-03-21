#region

using System.Drawing;

#endregion

namespace RadLibrary.RadConsole
{
    public interface IReadStyle
    {
        public string ColorizedPrefix { get; }
        public string ColorizedPostfix { get; }

        public string InputDecorations { get; }
        public string PredictionDecorations { get; }

        public Color InputColor { get; }
        public Color PredictionColor { get; }
    }
}