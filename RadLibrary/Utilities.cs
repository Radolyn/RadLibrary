#region

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using RadLibrary.Colors;
using RadLibrary.Formatting;

#endregion

namespace RadLibrary
{
    public static class Utilities
    {
        private const int StdOutputHandle = -11;

        private static Random _random;
        // Windows console coloring support

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int AllocConsole();

        /// <summary>
        ///     Initializes Colorizer and FormattersStorage
        /// </summary>
        public static void Initialize()
        {
            Colorizer.Initialize();
            FormattersStorage.AddDefault();
        }

        /// <summary>
        ///     Infinite loop (use with caution!)
        /// </summary>
        public static void InfiniteWait()
        {
            SpinWait.SpinUntil(() => false);
        }

        /// <summary>
        ///     Allocate console with specified encoding. Works on Windows only
        /// </summary>
        /// <param name="encoding">The encoding</param>
        /// <param name="clearConsole">If set to true, console will be cleared after allocation</param>
        public static void AllocateConsole(Encoding encoding = null, bool clearConsole = true)
        {
            if (!IsWindows() || Debugger.IsAttached)
                return;

            encoding ??= Encoding.UTF8;

            AllocConsole();

            var stdHandle = GetStdHandle(StdOutputHandle);
            var fileHandle = new SafeFileHandle(stdHandle, true);
            var fs = new FileStream(fileHandle, FileAccess.Write);
            var output = new StreamWriter(fs, encoding) {AutoFlush = true};

            Console.SetOut(output);

            if (clearConsole)
                Console.Clear();
        }

        /// <summary>
        ///     Checks whether the current system is Windows or not
        /// </summary>
        /// <returns>true if Windows, otherwise false</returns>
        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        ///     Prevents user from running more than 1 instance of program
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="action">Action that will be invoked if there's already one instance running</param>
        public static void OnlyOneInstance(string name, Action action = null)
        {
            var mutex = new Mutex(true, name);

            if (mutex.WaitOne(TimeSpan.Zero, true)) return;

            action?.Invoke();
            Environment.Exit(1);
        }

        /// <summary>
        ///     Returns string with lowered first letter
        /// </summary>
        /// <param name="str">The string</param>
        /// <returns>String with lowered first letter</returns>
        public static string FirstCharacterToLower(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
                return str;

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        /// <summary>
        ///     Returns random integer
        /// </summary>
        /// <param name="start">The minimal value</param>
        /// <param name="end">The maximal value</param>
        /// <returns>Random integer</returns>
        public static int RandomInt(int start = int.MinValue, int end = int.MaxValue)
        {
            _random ??= new Random();

            return _random.Next(start, end);
        }
    }
}