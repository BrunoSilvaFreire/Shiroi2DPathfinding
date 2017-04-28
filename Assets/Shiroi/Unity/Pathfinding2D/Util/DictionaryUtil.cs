using System;
using System.Collections.Generic;
using Vexe.Runtime.Types.Others;

namespace Shiroi.Unity.Pathfinding2D.Util {
    public static class DictionaryUtil {
        public static T GetOrPut<T, K>(this Dictionary<K, T> dictionary, K key, Func<T> creator) {
            if (dictionary.ContainsKey(key)) {
                return dictionary[key];
            }
            var value = creator();
            dictionary[key] = value;
            return value;
        }
        public static T GetOrPut<T, K>(this Dictionary<K, T> dictionary, K key, T value) {
            if (dictionary.ContainsKey(key)) {
                return dictionary[key];
            }
            dictionary[key] = value;
            return value;
        }
        public static T GetOrPut<T, K>(this SerializableDictionary<K, T> dictionary, K key, Func<T> creator) {
            if (dictionary.ContainsKey(key)) {
                return dictionary[key];
            }
            var value = creator();
            dictionary[key] = value;
            return value;
        }
    }
}