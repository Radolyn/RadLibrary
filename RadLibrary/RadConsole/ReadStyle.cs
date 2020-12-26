#region

using System.Drawing;

#endregion

namespace RadLibrary.RadConsole
{
    public class ReadStyle
    {
        public Color InputColor = Color.Azure;
        public string Postfix = "<<<";
        public Color PostfixColor = Color.WhiteSmoke;
        public Color PredictionColor = Color.Goldenrod;
        public string Prefix = ">>>";
        public Color PrefixColor = Color.WhiteSmoke;
        public bool UnderlineInput = false;
        public bool UnderlinePrediction = false;
    }
}