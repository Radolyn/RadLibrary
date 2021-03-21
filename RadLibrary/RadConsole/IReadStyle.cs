#region

using System.Drawing;
using RadLibrary.Colors;

#endregion

namespace RadLibrary.RadConsole
{
    public interface IReadStyle
    {
        /// <summary>
        ///     The prefix
        /// </summary>
        public string ColorizedPrefix { get; }

        /// <summary>
        ///     The postfix
        /// </summary>
        public string ColorizedPostfix { get; }

        /// <summary>
        ///     The input decorations
        /// </summary>
        /// <seealso cref="Font" />
        /// <seealso cref="Foreground" />
        /// <seealso cref="Background" />
        public string InputDecorations { get; }

        /// <summary>
        ///     The prediction decorations
        /// </summary>
        /// <seealso cref="Font" />
        /// <seealso cref="Foreground" />
        /// <seealso cref="Background" />
        public string PredictionDecorations { get; }

        /// <summary>
        ///     The input color
        /// </summary>
        public Color InputColor { get; }

        /// <summary>
        ///     The prediction color
        /// </summary>
        public Color PredictionColor { get; }
    }
}