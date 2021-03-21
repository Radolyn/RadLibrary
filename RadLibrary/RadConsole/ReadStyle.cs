#region

using System.Drawing;
using JetBrains.Annotations;
using RadLibrary.Colors;

#endregion

namespace RadLibrary.RadConsole
{
    public class ReadStyle : IReadStyle
    {
        /// <summary>
        ///     The postfix
        /// </summary>
        [CanBeNull]
        public virtual string Postfix { get; set; } = "<<<";

        /// <summary>
        ///     The postfix color
        /// </summary>
        public virtual Color PostfixColor { get; set; } = Color.WhiteSmoke;

        /// <summary>
        ///     The prefix
        /// </summary>
        [CanBeNull]
        public virtual string Prefix { get; set; } = ">>>";

        /// <summary>
        ///     The prefix color
        /// </summary>
        public virtual Color PrefixColor { get; set; } = Color.WhiteSmoke;

        public virtual bool UnderlineInput { get; set; } = false;
        public virtual bool UnderlinePrediction { get; set; } = false;

        /// <inheritdoc />
        public virtual Color InputColor { get; set; } = Color.Azure;

        /// <inheritdoc />
        public virtual Color PredictionColor { get; set; } = Color.Goldenrod;

        /// <inheritdoc />
        public string ColorizedPrefix => Prefix?.Colorize(PrefixColor) + " ";

        /// <inheritdoc />
        public string ColorizedPostfix => " " + Postfix?.Colorize(PostfixColor);

        /// <inheritdoc />
        public string InputDecorations => UnderlineInput ? Font.UnderlineOffFont : "";

        /// <inheritdoc />
        public string PredictionDecorations => UnderlinePrediction ? Font.UnderlineFont : "";
    }
}