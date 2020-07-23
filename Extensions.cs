#region

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace RadLibrary
{
    public static class Extensions
    {
        /// <summary>
        ///     Returns random item from enumerable
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>Random item</returns>
        public static T RandomItem<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            return array.Length == 0 ? default : array[Utilities.RandomInt(0, array.Length)];
        }

        /// <summary>
        ///     Repeats string specified amount of times
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="times">The count</param>
        /// <returns>The string</returns>
        public static string Repeat(this string s, int times)
        {
            return s.Repeat((long) times);
        }

        /// <summary>
        ///     Repeats string specified amount of times
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="times">The count</param>
        /// <returns>The string</returns>
        public static string Repeat(this string s, long times)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < times; i++) sb.Append(s);

            return sb.ToString();
        }
    }
}