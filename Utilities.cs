#region

using System.Threading;

#endregion

namespace RadLibrary
{
    public static class Utilities
    {
        /// <summary>
        ///     Infinite loop
        /// </summary>
        public static void InfiniteWait()
        {
            SpinWait.SpinUntil(() => false);
        }
    }
}