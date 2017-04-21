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
    }
}