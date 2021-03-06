﻿namespace RadLibrary.Colors
{
    /// <summary>
    ///     Provides font styles for console
    /// </summary>
    public static class Font
    {
        /// <summary>
        ///     The reset string
        /// </summary>
        public static readonly string Reset = "\x1b[0m";

        /// <summary>
        ///     The bold font string
        /// </summary>
        public static readonly string BoldFont = "\x1b[1m";

        /// <summary>
        ///     The bold font string
        /// </summary>
        public static readonly string FaintFont = "\x1b[2m";

        /// <summary>
        ///     The italic font string
        /// </summary>
        public static readonly string ItalicFont = "\x1b[3m";

        /// <summary>
        ///     The underline font string
        /// </summary>
        public static readonly string UnderlineFont = "\x1b[4m";

        /// <summary>
        ///     The slow blink font string
        /// </summary>
        public static readonly string SlowBlinkFont = "\x1b[5m";

        /// <summary>
        ///     The rapid blink font string
        /// </summary>
        public static readonly string RapidBlinkFont = "\x1b[6m";

        /// <summary>
        ///     The conceal font string
        /// </summary>
        public static readonly string ConcealFont = "\x1b[8m";

        /// <summary>
        ///     The crossed-out font string
        /// </summary>
        public static readonly string CrossedOutFont = "\x1b[9m";

        /// <summary>
        ///     The default font string
        /// </summary>
        public static readonly string DefaultFont = "\x1b[10m";

        /// <summary>
        ///     The fraktur font string
        /// </summary>
        public static readonly string FrakturFont = "\x1b[20m";

        /// <summary>
        ///     The bold off or double underline font string
        /// </summary>
        public static readonly string BoldOffDoubleUnderlineFont = "\x1b[21m";

        /// <summary>
        ///     The normal color or intensity font
        /// </summary>
        public static readonly string NormalColorIntensityFont = "\x1b[22m";

        /// <summary>
        ///     The not italic and not fraktur font
        /// </summary>
        public static readonly string NotItalicNotFrakturFont = "\x1b[23m";

        /// <summary>
        ///     The underline off font
        /// </summary>
        public static readonly string UnderlineOffFont = "\x1b[24m";

        /// <summary>
        ///     The blink off font
        /// </summary>
        public static readonly string BlinkOffFont = "\x1b[25m";

        /// <summary>
        ///     The inverse off font
        /// </summary>
        public static readonly string InverseOffFont = "\x1b[27m";

        /// <summary>
        ///     The reveal font
        /// </summary>
        public static readonly string RevealFont = "\x1b[28m";

        /// <summary>
        ///     The not crossed out font
        /// </summary>
        public static readonly string NotCrossedOutFont = "\x1b[29m";

        /// <summary>
        ///     The framed font
        /// </summary>
        public static readonly string FramedFont = "\x1b[51m";

        /// <summary>
        ///     The encircled font
        /// </summary>
        public static readonly string EncircledFont = "\x1b[52m";

        /// <summary>
        ///     The overlined font
        /// </summary>
        public static readonly string OverlinedFont = "\x1b[53m";

        /// <summary>
        ///     The not framed or circled font
        /// </summary>
        public static readonly string NotFramedOrCircledFont = "\x1b[54m";

        /// <summary>
        ///     The not overlined font
        /// </summary>
        public static readonly string NotOverlinedFont = "\x1b[55m";
    }
}