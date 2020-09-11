#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RadLibrary.Formatting.Abstractions;
using RadLibrary.Formatting.Formatters;

#endregion

namespace RadLibrary.Formatting
{
    /// <summary>
    ///     The static storage for formatters
    /// </summary>
    public static class FormattersStorage
    {
        private static readonly List<IObjectFormatter> Formatters = new List<IObjectFormatter>();

        private static readonly ConcurrentDictionary<Type, IObjectFormatter> CachedFormatters =
            new ConcurrentDictionary<Type, IObjectFormatter>();

        private static readonly IObjectFormatter NullFormatter = new NullFormatter();

        /// <summary>
        ///     Pass this as first parameter in string.Format to apply formatting
        /// </summary>
        public static readonly IFormatProvider FormatProvider = new GenericFormatter();

        /// <summary>
        ///     The maximum recursion level. Will return "..." on reaching this value
        /// </summary>
        public static int MaxRecursion = 10;

        /// <summary>
        ///     The all registered in the storage formatters
        /// </summary>
        public static IEnumerable<IObjectFormatter> RegisteredFormatters => Formatters.Skip(0);

        /// <summary>
        ///     Adds default formatters
        /// </summary>
        public static void AddDefault()
        {
            AddFormatters(typeof(GenericFormatter).Assembly);
        }

        /// <summary>
        ///     Adds T formatter
        /// </summary>
        /// <typeparam name="T">The <see cref="IObjectFormatter" /></typeparam>
        public static void AddFormatter<T>() where T : IObjectFormatter, new()
        {
            AddFormatter(new T());
        }

        /// <summary>
        ///     Adds formatter
        /// </summary>
        /// <param name="formatter">The formatter instance</param>
        public static void AddFormatter(IObjectFormatter formatter)
        {
            if (!Formatters.Contains(formatter))
                Formatters.Add(formatter);
        }

        /// <summary>
        ///     Adds all available formatters from specified assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        public static void AddFormatters(Assembly assembly)
        {
            var type = typeof(IObjectFormatter);

            var formatters = assembly.ExportedTypes.Where(x => type.IsAssignableFrom(x) && !x.IsAbstract);

            foreach (var formatter in formatters) AddFormatter((IObjectFormatter) Activator.CreateInstance(formatter));
        }

        /// <summary>
        ///     Returns formatter for specified object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The formatter</returns>
        public static IObjectFormatter GetCustomFormatter(object obj)
        {
            if (obj == null)
                return NullFormatter;

            var type = obj.GetType();

            if (CachedFormatters.ContainsKey(type))
                return CachedFormatters[type];

            var pred = Formatters.Where(x =>
            {
                if (!x.Type.IsGenericType || !type.IsGenericType)
                    return x.Type.IsAssignableFrom(type);

                var generic1 = x.Type.GetGenericTypeDefinition();
                var generic2 = type.GetGenericTypeDefinition();

                return generic1 == generic2 ||
                       (generic2 as TypeInfo)!.ImplementedInterfaces.Any(y => y.GUID == generic1.GUID);
            }).ToList();

            if (pred.Count == 1)
                return pred[0];

            var theMostPriority = pred.Max(x => x.Priority);

            var res = pred.First(x => x.Priority == theMostPriority);

            CachedFormatters.TryAdd(type, res);

            return res;
        }

        // /// <summary>
        // ///     Returns formatter for specified type
        // /// </summary>
        // /// <typeparam name="T">The type</typeparam>
        // /// <returns>The formatter</returns>
        // public static IObjectFormatter GetCustomFormatter<T>() where T : class
        // {
        //     var type = typeof(T);
        //
        //     var pred = Formatters.FirstOrDefault(x => x.Type.IsAssignableFrom(type));
        //
        //     return pred ?? DefaultFormatter;
        // }

        /// <summary>
        ///     Returns a result from formatter for specified object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The formatted string</returns>
        public static string GetCustomFormatterResult(object obj)
        {
            var formatter = GetCustomFormatter(obj);
            return formatter.Format(obj);
        }
    }
}