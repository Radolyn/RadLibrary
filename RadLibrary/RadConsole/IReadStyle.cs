#region

using System;
using System.Drawing;
using RadLibrary.Colors;

#endregion

namespace RadLibrary.RadConsole
{
    /// <summary>
    ///     User input style
    /// </summary>
    /// <seealso cref="ConsoleRead" />
    /// <seealso cref="RadConsole.Read" />
    public interface IReadStyle : ICloneable
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

        /// <summary>
        ///     Sets prefix
        /// </summary>
        /// <param name="prefix">The prefix</param>
        void SetPrefix(string prefix);
    }
}