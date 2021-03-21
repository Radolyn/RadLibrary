#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

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
        public static T RandomItem<T>([NotNull] this IEnumerable<T> enumerable)
        {
            var collection = enumerable is ICollection<T> coll ? coll : enumerable.ToList();

            return collection.Count == 0
                ? default
                : collection.ElementAt(RadUtilities.RandomInt(0, collection.Count));
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
        [NotNull]
        public static string Remove([NotNull] this string s, [NotNull] string value)
        {
            return s.Replace(value, "");
        }

        /// <summary>
        ///     Returns string with lowered first letter
        /// </summary>
        /// <param name="s">The string</param>
        /// <returns>String with lowered first letter</returns>
        [CanBeNull]
        public static string FirstCharacterToLower([CanBeNull] this string s)
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
        [CanBeNull]
        public static object GetDefault([NotNull] this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        ///     Formats string
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="dictionary">The dictionary</param>
        /// <returns>The formatted string</returns>
        [NotNull]
        public static string FormatWith([NotNull] this string s, [NotNull] IDictionary<string, object> dictionary)
        {
            // remainder: 'out ... {parameter[,alignment][:format]}'
            var sbOut = new StringBuilder(s.Length);
            var sbParameter = new StringBuilder(8);
            var sbAlignment = new StringBuilder();
            var sbFormat = new StringBuilder();

            var formatPart = CurrentPart.Out;
            var escapeSymbol = false;

            void Write(char ch)
            {
                switch (formatPart)
                {
                    case CurrentPart.Out:
                        sbOut.Append(ch);
                        break;
                    case CurrentPart.Parameter:
                        sbParameter.Append(ch);
                        break;
                    case CurrentPart.Alignment:
                        sbAlignment.Append(ch);
                        break;
                    case CurrentPart.Format:
                        sbFormat.Append(ch);
                        break;
                }
            }

            foreach (var ch in s)
            {
                if (!escapeSymbol && ch == '\\')
                {
                    escapeSymbol = true;
                    continue;
                }

                if (escapeSymbol)
                {
                    Write(ch);
                    escapeSymbol = false;
                    continue;
                }

                switch (ch)
                {
                    case '{':
                        formatPart = CurrentPart.Parameter;
                        continue;
                    case '}':
                        var param = dictionary[sbParameter.ToString()];
                        var res = string.Format(
                            "{0" + (sbAlignment.Length == 0 ? "" : "," + sbAlignment) +
                            (sbFormat.Length == 0 ? "" : ":" + sbFormat) + "}", param);

                        sbOut.Append(res);

                        sbParameter.Clear();
                        sbAlignment.Clear();
                        sbFormat.Clear();

                        formatPart = CurrentPart.Out;
                        continue;
                    case ',' when formatPart is CurrentPart.Parameter:
                        formatPart = CurrentPart.Alignment;
                        continue;
                    case ':' when formatPart is CurrentPart.Parameter || formatPart is CurrentPart.Alignment:
                        formatPart = CurrentPart.Format;
                        continue;
                    default:
                        Write(ch);
                        continue;
                }
            }

            return sbOut.ToString();
        }

        /// <summary>
        ///     Returns a value indicating whether a specified substring occurs within this <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder" /></param>
        /// <param name="value">The string to seek</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c></returns>
        public static bool Contains(this StringBuilder sb, string value)
        {
            if (sb.Length == 0 || string.IsNullOrEmpty(value))
                return false;

            if (sb.Length < value.Length)
                return false;

            var foundLength = 0;

            for (var i = 0; i < sb.Length; i++)
            {
                if (sb[i] == value[foundLength])
                    foundLength++;
                else
                    foundLength = 0;

                if (foundLength == value.Length)
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Determines whether the beginning of this <see cref="StringBuilder" /> instance matches the specified string.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder" /></param>
        /// <param name="value">The string to seek</param>
        /// <returns><c>true</c> if starts with specified string; otherwise <c>false</c></returns>
        public static bool StartsWith(this StringBuilder sb, string value)
        {
            if (sb.Length == 0 || string.IsNullOrEmpty(value))
                return false;

            if (sb.Length < value.Length)
                return false;

            var foundLength = 0;

            for (var i = 0; i < sb.Length; i++)
            {
                if (sb[i] == value[i])
                    ++foundLength;
                else
                    return false;

                if (foundLength == value.Length)
                    return true;
            }

            return false;
        }

        private enum CurrentPart : byte
        {
            Out,
            Parameter,
            Alignment,
            Format
        }
    }
}