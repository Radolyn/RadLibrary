#region

using System;
using RadLibrary.ConsoleExperience.ProgressBar.Styles;

#endregion

namespace RadLibrary.ConsoleExperience.ProgressBar
{
    public class ColorfulProgressBar
    {
        public ColorfulProgressBar(int total) : this(Console.CursorLeft, Console.CursorLeft, total)
        {
        }

        public ColorfulProgressBar(int left, int top, int total)
        {
            Left = left;
            Top = top;
            Total = total;
        }

        public int Left { get; }
        public int Top { get; }
        public long Total { get; set; }

        public IStyle Style { get; set; } = new DefaultStyle(50);

        /// <summary>
        ///     Prints progress bar
        /// </summary>
        /// <param name="iteration">The iteration</param>
        public void Update(long iteration)
        {
            var s = iteration < Total ? Style.GetProgress(iteration, this) : Style.GetFinished(this);

            ColorfulConsole.WriteAt(s, Top, Left);
        }

        /// <summary>
        ///     Prints finished progress bar string
        /// </summary>
        public void Finish()
        {
            Update(Total);
        }
    }
}