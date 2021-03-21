#region

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

        /// <inheritdoc cref="Console.CancelKeyPress" />
        public static event ConsoleCancelEventHandler CancelKeyPress
        {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        #endregion

        #region Console Methods

        /// <inheritdoc cref="Console.Beep()" />
        public static void Beep()
        {
            Console.Beep();
        }

        /// <inheritdoc cref="Console.Beep(int, int)" />
        public static void Beep(int frequency, int duration)
        {
            Console.Beep(frequency, duration);
        }

        /// <inheritdoc cref="Console.Clear" />
        public static void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc cref="Console.MoveBufferArea(int, int, int, int, int, int)" />
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        }

        /// <inheritdoc cref="Console.MoveBufferArea(int, int, int, int, int, int, char, ConsoleColor, ConsoleColor)" />
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar,
                sourceForeColor, sourceBackColor);
        }

        /// <inheritdoc cref="Console.OpenStandardError()" />
        public static Stream OpenStandardError()
        {
            return Console.OpenStandardError();
        }

        /// <inheritdoc cref="Console.OpenStandardError(int)" />
        public static Stream OpenStandardError(int bufferSize)
        {
            return Console.OpenStandardError(bufferSize);
        }

        /// <inheritdoc cref="Console.OpenStandardInput()" />
        public static Stream OpenStandardInput()
        {
            return Console.OpenStandardInput();
        }

        /// <inheritdoc cref="Console.OpenStandardInput(int)" />
        public static Stream OpenStandardInput(int bufferSize)
        {
            return Console.OpenStandardInput(bufferSize);
        }

        /// <inheritdoc cref="Console.OpenStandardOutput()" />
        public static Stream OpenStandardOutput()
        {
            return Console.OpenStandardOutput();
        }

        /// <inheritdoc cref="Console.OpenStandardOutput(int)" />
        public static Stream OpenStandardOutput(int bufferSize)
        {
            return Console.OpenStandardOutput(bufferSize);
        }

        /// <inheritdoc cref="Console.ResetColor" />
        public static void ResetColor()
        {
            Console.ResetColor();
            Console.Write(Font.Reset);
        }

        /// <inheritdoc cref="Console.SetBufferSize" />
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

        /// <inheritdoc cref="Console.SetError" />
        public static void SetError([NotNull] TextWriter newError)
        {
            Console.SetError(newError);
        }

        /// <inheritdoc cref="Console.SetIn" />
        public static void SetIn([NotNull] TextReader newIn)
        {
            Console.SetIn(newIn);
        }

        /// <inheritdoc cref="Console.SetOut" />
        public static void SetOut(TextWriter newOut)
        {
            Console.SetOut(newOut);
        }

        /// <inheritdoc cref="Console.SetWindowPosition" />
        public static void SetWindowPosition(int left, int top)
        {
            Console.SetWindowPosition(left, top);
        }

        /// <inheritdoc cref="Console.SetWindowSize" />
        public static void SetWindow(int width, int height)
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

        /// <inheritdoc cref="Console.BackgroundColor" />
        public static ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <inheritdoc cref="Console.BufferHeight" />
        public static int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        /// <inheritdoc cref="Console.BufferWidth" />
        public static int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }

        /// <inheritdoc cref="Console.CapsLock" />
        public static bool CapsLock => Console.CapsLock;

        /// <inheritdoc cref="Console.CursorLeft" />
        public static int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        /// <inheritdoc cref="Console.CursorSize" />
        public static int CursorSize
        {
            get => Console.CursorSize;
            set => Console.CursorSize = value;
        }

        /// <inheritdoc cref="Console.CursorTop" />
        public static int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        /// <inheritdoc cref="Console.CursorVisible" />
        public static bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        /// <inheritdoc cref="Console.Error" />
        public static TextWriter Error => Console.Error;

        /// <inheritdoc cref="Console.ForegroundColor" />
        public static ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <inheritdoc cref="Console.In" />
        public static TextReader In => Console.In;

        /// <inheritdoc cref="Console.InputEncoding" />
        public static Encoding InputEncoding
        {
            get => Console.InputEncoding;
            set => Console.InputEncoding = value;
        }

        /// <inheritdoc cref="Console.IsErrorRedirected" />
        public static bool IsErrorRedirected => Console.IsErrorRedirected;

        /// <inheritdoc cref="Console.IsInputRedirected" />
        public static bool IsInputRedirected => Console.IsInputRedirected;

        /// <inheritdoc cref="Console.IsOutputRedirected" />
        public static bool IsOutputRedirected => Console.IsOutputRedirected;

        /// <inheritdoc cref="Console.KeyAvailable" />
        public static bool KeyAvailable => Console.KeyAvailable;

        /// <inheritdoc cref="Console.LargestWindowHeight" />
        public static int LargestWindowHeight => Console.LargestWindowHeight;

        /// <inheritdoc cref="Console.LargestWindowWidth" />
        public static int LargestWindowWidth => Console.LargestWindowWidth;

        /// <inheritdoc cref="Console.NumberLock" />
        public static bool NumberLock => Console.NumberLock;

        /// <inheritdoc cref="Console.Out" />
        public static TextWriter Out => Console.Out;

        /// <inheritdoc cref="Console.OutputEncoding" />
        public static Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => Console.OutputEncoding = value;
        }

        /// <inheritdoc cref="Console.Title" />
        public static string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }

        /// <inheritdoc cref="Console.TreatControlCAsInput" />
        public static bool TreatControlCAsInput
        {
            get => Console.TreatControlCAsInput;
            set => Console.TreatControlCAsInput = value;
        }

        /// <inheritdoc cref="Console.WindowHeight" />
        public static int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        /// <inheritdoc cref="Console.WindowLeft" />
        public static int WindowLeft
        {
            get => Console.WindowLeft;
            set => Console.WindowLeft = value;
        }

        /// <inheritdoc cref="Console.WindowTop" />
        public static int WindowTop
        {
            get => Console.WindowTop;
            set => Console.WindowTop = value;
        }

        /// <inheritdoc cref="Console.WindowWidth" />
        public static int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        #endregion
    }
}