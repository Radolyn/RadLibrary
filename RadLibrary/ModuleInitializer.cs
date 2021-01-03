#if NET5_0

using System.Runtime.CompilerServices;

namespace RadLibrary
{
    public static class RadLibraryInitializer
    {
        [ModuleInitializer]
        internal static void Initialize()
        {
            Utilities.Initialize(false);
        }
    }
}

#endif