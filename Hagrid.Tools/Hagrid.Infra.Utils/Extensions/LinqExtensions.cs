using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// Linq Extensions
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Distinct By
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> RemoveItem<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (TSource element in source)
            {
                if (!predicate(element))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, params TSource[] items)
        {
            if (source.IsNull())
                source = new TSource[] { };

            if (!items.IsNull())
                return source.Concat(items);

            return source;
        }
    }
}
