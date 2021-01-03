﻿#region

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

        private static ConsoleEventDelegate _handler;

        /// <summary>
        ///     Checks whether the current system is Windows or not
        /// </summary>
        /// <returns>true if Windows, otherwise false</returns>
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        /// <summary>
        ///     Initializes Colorizer and FormattersStorage
        /// </summary>
        /// <param name="registerColorResetEvent">Do we need to reset colors on CTRL + C event?</param>
        public static void Initialize(bool registerColorResetEvent = true)
        {
            Colorizer.Initialize();
            FormattersStorage.AddDefault();

            if (!registerColorResetEvent || !IsWindows) return;

            _handler = eventType =>
            {
                if (eventType == 0 || eventType == 2) Console.Write(Font.Reset + Foreground.Reset + Background.Reset);
                return false;
            };
            SetConsoleCtrlHandler(_handler, true);
        }

        /// <summary>
        ///     Allocate console with specified encoding. Works on Windows only
        /// </summary>
        /// <param name="encoding">The encoding</param>
        /// <param name="clearConsole">If set to true, console will be cleared after allocation</param>
        public static void AllocateConsole(Encoding encoding = null, bool clearConsole = true)
        {
            if (!IsWindows || Debugger.IsAttached)
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

        private delegate bool ConsoleEventDelegate(int eventType);
    }
}