#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace RadLibrary
{
    public static class RadExtensions
    {
        /// <summary>
        ///     Returns random item from enumerable
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>Random item</returns>
        public static T RandomItem<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as List<T> ?? enumerable.ToList();
            return array.Count == 0 ? default : array[Utilities.RandomInt(0, array.Count)];
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

        /// <summary>
        ///     Removes specified substring from string
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="value">The substring</param>
        /// <returns>The string</returns>
        public static string Remove(this string s, string value)
        {
            return s.Replace(value, "");
        }

        /// <summary>
        ///     Returns string with lowered first letter
        /// </summary>
        /// <param name="s">The string</param>
        /// <returns>String with lowered first letter</returns>
        public static string FirstCharacterToLower(this string s)
        {
            if (string.IsNullOrEmpty(s) || char.IsLower(s, 0))
                return s;

            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }

        /// <summary>
        ///     Returns default value for specified type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The default value</returns>
        public static object GetDefault(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}