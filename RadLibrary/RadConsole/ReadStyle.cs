#region

using System.Drawing;
using JetBrains.Annotations;
using RadLibrary.Colors;

#endregion

namespace RadLibrary.RadConsole
{
    public class ReadStyle : IReadStyle
    {
        public virtual string DefaultValue { get; set; } = null;
        [CanBeNull] public virtual string Postfix { get; set; } = "<<<";
        public virtual Color PostfixColor { get; set; } = Color.WhiteSmoke;
        [CanBeNull] public virtual string Prefix { get; set; } = ">>>";
        public virtual Color PrefixColor { get; set; } = Color.WhiteSmoke;
        public virtual bool UnderlineInput { get; set; } = false;
        public virtual bool UnderlinePrediction { get; set; } = false;
        public virtual Color InputColor { get; set; } = Color.Azure;
        public virtual Color PredictionColor { get; set; } = Color.Goldenrod;

        public string ColorizedPrefix => Prefix?.Colorize(PrefixColor) + " ";
        public string ColorizedPostfix => " " + Postfix?.Colorize(PostfixColor);
    }
}