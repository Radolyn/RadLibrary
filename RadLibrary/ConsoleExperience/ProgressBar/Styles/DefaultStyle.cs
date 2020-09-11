#region

using System;
using System.Drawing;
using RadLibrary.Colors;

#endregion

namespace RadLibrary.ConsoleExperience.ProgressBar.Styles
{
    public class DefaultStyle : IStyle
    {
        protected readonly int _length;

        protected string _prefix = "";

        protected Color _prefixColor;
        protected string _separator = "|";
        protected Color _separatorColor;
        protected string _suffix = "";
        protected Color _suffixColor;

        public DefaultStyle(int length)
        {
            _length = length;
        }

        /// <summary>
        ///     The prefix
        /// </summary>
        public virtual string Prefix
        {
            get => _prefix.DeColorize();
            set => _prefix = value.Colorize(PrefixColor);
        }

        /// <summary>
        ///     The suffix
        /// </summary>
        public virtual string Suffix
        {
            get => _suffix.DeColorize();
            set => _suffix = value.Colorize(SuffixColor);
        }

        /// <summary>
        ///     The char that will be printed as filled
        /// </summary>
        public virtual string FillChar { get; set; } = "█";

        /// <summary>
        ///     The char that will be printed as unfilled
        /// </summary>
        public virtual string ProgressChar { get; set; } = "-";

        /// <summary>
        ///     The string that will be printed as separator between prefix and percentage
        /// </summary>
        public virtual string SeparatorStr
        {
            get => _separator.DeColorize();
            set => _separator = value.Colorize(SeparatorColor);
        }

        /// <summary>
        ///     The prefix color
        /// </summary>
        public virtual Color PrefixColor
        {
            get => _prefixColor;
            set
            {
                _prefixColor = value;
                _prefix = _prefix.DeColorize().Colorize(_prefixColor);
            }
        }

        /// <summary>
        ///     The suffix color
        /// </summary>
        public virtual Color SuffixColor
        {
            get => _suffixColor;
            set
            {
                _suffixColor = value;
                _suffix = _suffix.DeColorize().Colorize(_suffixColor);
            }
        }

        /// <summary>
        ///     The FillChar color
        /// </summary>
        public virtual Color FillColor { get; set; }

        /// <summary>
        ///     The ProgressChar color
        /// </summary>
        public virtual Color ProgressColor { get; set; }

        /// <summary>
        ///     The percentage color
        /// </summary>
        public virtual Color PercentColor { get; set; }

        /// <summary>
        ///     The SeparatorStr color
        /// </summary>
        public virtual Color SeparatorColor
        {
            get => _separatorColor;
            set
            {
                _separatorColor = value;
                _separator = _separator.DeColorize().Colorize(_separatorColor);
            }
        }

        /// <inheritdoc />
        public virtual string GetProgress(long iteration, ColorfulProgressBar bar)
        {
            var percent = $"{100 * (iteration / (bar.Total + 0.0)):0.00}%".Colorize(PercentColor) + Foreground.Reset;
            var filledLength = _length * iteration / bar.Total;

            var barItself = FillChar.Repeat(filledLength).Colorize(FillColor) +
                            ProgressChar.Repeat(_length - filledLength).Colorize(ProgressColor) + Font.Reset;

            var complete =
                $"{_prefix} {_separator}{barItself}{_separator} {percent} {_suffix}";

            return complete;
        }

        /// <inheritdoc />
        public virtual string GetFinished(ColorfulProgressBar bar)
        {
            return "Done!" + " ".Repeat(Console.WindowWidth - 5);
        }
    }
}