#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadLibrary.Formatting;

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
        
        /// <summary>
        /// Formats string
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="dictionary">The dictionary</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string s, IDictionary<string, object> dictionary)
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
                        var res = string.Format("{0" + (sbAlignment.Length == 0 ? "" : "," + sbAlignment) + (sbFormat.Length == 0 ? "" : ":" + sbFormat) + "}", param);
                        
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
        
        enum CurrentPart : byte
        {
            Out,
            Parameter,
            Alignment,
            Format
        }
    }
}