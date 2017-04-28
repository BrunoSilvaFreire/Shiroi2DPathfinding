using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vexe.Runtime.Extensions;

namespace Shiroi.Unity.Pathfinding2D.Util {
    public static class CollectionsUtil {
        public static T GetOrPut<T>(this ICollection<T> collection, Func<T, bool> predicate, Func<T> instantiator) {
            foreach (var obj in collection) {
                if (predicate(obj)) {
                    return obj;
                }
            }
            var newObj = instantiator();
            collection.Add(newObj);
            return newObj;
        }

        public static bool HasSingle(this ICollection collection) {
            return collection.Count == 1;
        }

        public static List<T> GetAllOrPut<T>(this ICollection<T> collection, Func<T, bool> predicate,
            Func<T> instantiator) {
            var list = collection.Where(predicate).ToList();
            if (list.IsEmpty()) {
                var newObj = instantiator();
                collection.Add(newObj);
                list.Add(newObj);
            }
            return list;
        }
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }
    }
}