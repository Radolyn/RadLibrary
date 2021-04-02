#if NET5_0
#region

using System;
using System.Runtime.CompilerServices;

#endregion

namespace RadLibrary
{
    public static class RadLibraryInitializer
    {
        [ModuleInitializer]
        internal static void Initialize()
        {
            try
            {
                RadUtilities.Initialize(false);
            }
            catch
            {
                Console.WriteLine("Failed to initialize RadLibrary");
            }
        }
    }
}

#endif