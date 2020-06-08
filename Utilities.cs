#region

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

#endregion

namespace RadLibrary
{
    public static class Utilities
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int AllocConsole();

        private const int StdOutputHandle = -11;

        /// <summary>
        ///     Infinite loop
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
        public static void AllocateConsole(Encoding encoding, bool clearConsole = true)
        {
            if (!IsWindows())
                return;

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
        /// <returns>Windows or not</returns>
        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
    }
}