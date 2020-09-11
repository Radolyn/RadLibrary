#region

using System;
using System.Diagnostics;
using System.Linq;

#endregion

namespace RadLibrary.Formatting.Abstractions
{
    /// <summary>
    ///     Represents a formatter for object that controls recursion
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    public abstract class ObjectFormatter<T> : IObjectFormatter where T : class
    {
        public virtual int Priority { get; } = 1;

        /// <inheritdoc />
        public virtual Type Type { get; } = typeof(T);

        /// <inheritdoc />
        public virtual string Format(object obj)
        {
            return GetRecursionDeep() >= FormattersStorage.MaxRecursion ? "..." : FormatObject((obj as T)!);
        }

        /// <summary>
        ///     Formats specified object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The formatted string</returns>
        public abstract string FormatObject(T obj);

        /// <summary>
        ///     Returns the recursion deep
        /// </summary>
        /// <returns>The recursion deep</returns>
        public static int GetRecursionDeep()
        {
            var stack = new StackTrace();

            var frames = stack.GetFrames();

            var count = frames!.Count(x => x?.GetMethod()?.Name == nameof(FormatObject));

            return count;
        }
    }
}