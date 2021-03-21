﻿#region

using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using RadLibrary.Colors;

#endregion

namespace RadLibrary.RadConsole
{
    public static partial class RadConsole
    {
        #region Console Events

        /// <summary>
        ///     Occurs when the Control modifier key (Ctrl) and either the C console key (C) or the Break key are pressed
        ///     simultaneously (Ctrl+C or Ctrl+Break).
        /// </summary>
        public static event ConsoleCancelEventHandler CancelKeyPress
        {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        #endregion

        #region Console Methods

        /// <summary>
        ///     Plays the sound of a beep through the console speaker.
        /// </summary>
        public static void Beep()
        {
            Console.Beep();
        }

        /// <summary>
        ///     Plays the sound of a beep of a specified frequency and duration through the console speaker.
        /// </summary>
        /// <param name="frequency">The frequency of the beep, ranging from 37 to 32767 hertz.</param>
        /// <param name="duration">The duration of the beep measured in milliseconds.</param>
        public static void Beep(int frequency, int duration)
        {
            Console.Beep(frequency, duration);
        }

        /// <summary>
        ///     Clears the console buffer and corresponding console window of display information.
        /// </summary>
        public static void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        ///     Copies a specified source area of the screen buffer to a specified destination area.
        /// </summary>
        /// <param name="sourceLeft">The leftmost column of the source area.</param>
        /// <param name="sourceTop">The topmost row of the source area.</param>
        /// <param name="sourceWidth">The number of columns in the source area.</param>
        /// <param name="sourceHeight">The number of rows in the source area.</param>
        /// <param name="targetLeft">The leftmost column of the destination area.</param>
        /// <param name="targetTop">The topmost row of the destination area.</param>
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        }

        /// <summary>
        ///     Copies a specified source area of the screen buffer to a specified destination area.
        /// </summary>
        /// <param name="sourceLeft">The leftmost column of the source area.</param>
        /// <param name="sourceTop">The topmost row of the source area.</param>
        /// <param name="sourceWidth">The number of columns in the source area.</param>
        /// <param name="sourceHeight">The number of rows in the source area.</param>
        /// <param name="targetLeft">The leftmost column of the destination area.</param>
        /// <param name="targetTop">The topmost row of the destination area.</param>
        /// <param name="sourceChar">The character used to fill the source area.</param>
        /// <param name="sourceForeColor">The foreground color used to fill the source area.</param>
        /// <param name="sourceBackColor">The background color used to fill the source area.</param>
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar,
                sourceForeColor, sourceBackColor);
        }

        /// <summary>
        ///     Acquires the standard error stream.
        /// </summary>
        /// <returns>The standard error stream.</returns>
        public static Stream OpenStandardError()
        {
            return Console.OpenStandardError();
        }

        /// <summary>
        ///     Acquires the standard error stream, which is set to a specified buffer size.
        /// </summary>
        /// <param name="bufferSize">The internal stream buffer size.</param>
        /// <returns>The standard error stream.</returns>
        public static Stream OpenStandardError(int bufferSize)
        {
            return Console.OpenStandardError(bufferSize);
        }

        /// <summary>
        ///     Acquires the standard input stream.
        /// </summary>
        /// <returns>The standard input stream.</returns>
        public static Stream OpenStandardInput()
        {
            return Console.OpenStandardInput();
        }

        /// <summary>
        ///     Acquires the standard input stream, which is set to a specified buffer size.
        /// </summary>
        /// <param name="bufferSize">The internal stream buffer size.</param>
        /// <returns>The standard input stream.</returns>
        public static Stream OpenStandardInput(int bufferSize)
        {
            return Console.OpenStandardInput(bufferSize);
        }

        /// <summary>
        ///     Acquires the standard output stream.
        /// </summary>
        /// <returns>The standard output stream.</returns>
        public static Stream OpenStandardOutput()
        {
            return Console.OpenStandardOutput();
        }

        /// <summary>
        ///     Acquires the standard output stream, which is set to a specified buffer size.
        /// </summary>
        /// <param name="bufferSize">The internal stream buffer size.</param>
        /// <returns>The standard output stream.</returns>
        public static Stream OpenStandardOutput(int bufferSize)
        {
            return Console.OpenStandardOutput(bufferSize);
        }

        /// <summary>
        ///     Sets the foreground and background console colors to their defaults.
        /// </summary>
        public static void ResetColor()
        {
            Console.ResetColor();
            Console.Write(Font.Reset);
        }

        /// <summary>
        ///     Sets the height and width of the screen buffer area to the specified values.
        /// </summary>
        /// <param name="width">The width of the buffer area measured in columns.</param>
        /// <param name="height">The height of the buffer area measured in rows.</param>
        public static void SetBufferSize(int width, int height)
        {
            Console.SetBufferSize(width, height);
        }

        /// <summary>
        ///     Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        public static void SetCursorPosition(int left, int top)
        {
            if (BufferHeight < top)
                BufferHeight += top - BufferHeight;

            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        ///     Sets the <see cref="Error" /> property to the specified <see cref="TextWriter" /> object.
        /// </summary>
        /// <param name="newError">A stream that is the new standard error output.</param>
        public static void SetError([NotNull] TextWriter newError)
        {
            Console.SetError(newError);
        }

        /// <summary>
        ///     Sets the <see cref="Error" /> property to the specified <see cref="TextWriter" /> object.
        /// </summary>
        /// <param name="newIn">A stream that is the new standard input.</param>
        public static void SetIn([NotNull] TextReader newIn)
        {
            Console.SetIn(newIn);
        }

        /// <summary>
        ///     Sets the <see cref="Error" /> property to the specified <see cref="TextWriter" /> object.
        /// </summary>
        /// <param name="newOut">A stream that is the new standard output output.</param>
        public static void SetOut(TextWriter newOut)
        {
            Console.SetOut(newOut);
        }

        /// <summary>
        ///     Sets the position of the console window relative to the screen buffer.
        /// </summary>
        /// <param name="left">The column position of the upper left corner of the console window.</param>
        /// <param name="top">The row position of the upper left corner of the console window.</param>
        public static void SetWindowPosition(int left, int top)
        {
            Console.SetWindowPosition(left, top);
        }

        /// <summary>
        ///     Sets the height and width of the console window to the specified values.
        /// </summary>
        /// <param name="width">The width of the console window measured in columns.</param>
        /// <param name="height">The height of the console window measured in rows.</param>
        public static void SetWindowSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
        }

        /// <summary>
        ///     Gets the position of the cursor.
        /// </summary>
        /// <returns>The row and column position of the cursor.</returns>
        public static (int Top, int Left) GetCursorPosition()
        {
            return (Console.CursorTop, Console.CursorLeft);
        }

        #endregion

        #region Console Properties

        /// <summary>
        ///     Gets or sets the background color of the console.
        /// </summary>
        public static ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <summary>
        ///     Gets or sets the height of the buffer area.
        /// </summary>
        public static int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        /// <summary>
        ///     Gets or sets the width of the buffer area.
        /// </summary>
        public static int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }

        /// <summary>
        ///     Gets a value indicating whether the CAPS LOCK keyboard toggle is turned on or turned off.
        /// </summary>
        public static bool CapsLock => Console.CapsLock;

        /// <summary>
        ///     Gets or sets the column position of the cursor within the buffer area.
        /// </summary>
        public static int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        /// <summary>
        ///     Gets or sets the height of the cursor within a character cell.
        /// </summary>
        public static int CursorSize
        {
            get => Console.CursorSize;
            set => Console.CursorSize = value;
        }

        /// <summary>
        ///     Gets or sets the row position of the cursor within the buffer area.
        /// </summary>
        public static int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        public static bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        /// <summary>
        ///     Gets the standard error output stream.
        /// </summary>
        public static TextWriter Error => Console.Error;

        /// <summary>
        ///     Gets or sets the foreground color of the console.
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <summary>
        ///     Gets the standard input stream.
        /// </summary>
        public static TextReader In => Console.In;

        /// <summary>
        ///     Gets or sets the encoding the console uses to read input.
        /// </summary>
        public static Encoding InputEncoding
        {
            get => Console.InputEncoding;
            set => Console.InputEncoding = value;
        }

        /// <summary>
        ///     Gets a value that indicates whether the error output stream has been redirected from the standard error stream.
        /// </summary>
        public static bool IsErrorRedirected => Console.IsErrorRedirected;

        /// <summary>
        ///     Gets a value that indicates whether input has been redirected from the standard input stream.
        /// </summary>
        public static bool IsInputRedirected => Console.IsInputRedirected;

        /// <summary>
        ///     Gets a value that indicates whether output has been redirected from the standard output stream.
        /// </summary>
        public static bool IsOutputRedirected => Console.IsOutputRedirected;

        /// <summary>
        ///     Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        public static bool KeyAvailable => Console.KeyAvailable;

        /// <summary>
        ///     Gets the largest possible number of console window rows, based on the current font and screen resolution.
        /// </summary>
        public static int LargestWindowHeight => Console.LargestWindowHeight;

        /// <summary>
        ///     Gets the largest possible number of console window columns, based on the current font and screen resolution.
        /// </summary>
        public static int LargestWindowWidth => Console.LargestWindowWidth;

        /// <summary>
        ///     Gets a value indicating whether the NUM LOCK keyboard toggle is turned on or turned off.
        /// </summary>
        public static bool NumberLock => Console.NumberLock;

        /// <summary>
        ///     Gets the standard output stream.
        /// </summary>
        public static TextWriter Out => Console.Out;

        /// <summary>
        ///     Gets or sets the encoding the console uses to write output.
        /// </summary>
        public static Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => Console.OutputEncoding = value;
        }

        /// <summary>
        ///     Gets or sets the title to display in the console title bar.
        /// </summary>
        public static string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the combination of the Control modifier key and C console key (Ctrl+C) is
        ///     treated as ordinary input or as an interruption that is handled by the operating system.
        /// </summary>
        public static bool TreatControlCAsInput
        {
            get => Console.TreatControlCAsInput;
            set => Console.TreatControlCAsInput = value;
        }

        /// <summary>
        ///     Gets or sets the height of the console window area.
        /// </summary>
        public static int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        /// <summary>
        ///     Gets or sets the leftmost position of the console window area relative to the screen buffer.
        /// </summary>
        public static int WindowLeft
        {
            get => Console.WindowLeft;
            set => Console.WindowLeft = value;
        }

        /// <summary>
        ///     Gets or sets the top position of the console window area relative to the screen buffer.
        /// </summary>
        public static int WindowTop
        {
            get => Console.WindowTop;
            set => Console.WindowTop = value;
        }

        /// <summary>
        ///     Gets or sets the width of the console window.
        /// </summary>
        public static int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        #endregion
    }
}