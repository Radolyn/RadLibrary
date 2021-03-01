#region

using System.Drawing;
using JetBrains.Annotations;

#endregion

namespace RadLibrary.RadConsole
{
    public class ReadStyle
    {
        public string DefaultValue = null;
        public Color InputColor = Color.Azure;
        [CanBeNull] public string Postfix = "<<<";
        public Color PostfixColor = Color.WhiteSmoke;
        public Color PredictionColor = Color.Goldenrod;
        [CanBeNull] public string Prefix = ">>>";
        public Color PrefixColor = Color.WhiteSmoke;
        public bool UnderlineInput = false;
        public bool UnderlinePrediction = false;
    }
}