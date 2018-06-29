namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    #endregion

    /// <summary>
    /// Enumerable extension methods
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Public Methods

        /// <summary>
        /// Returns distinct elements from a sequence using function selector
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <param name="source">Source elements</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>Returns distinct elements</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Returns distinct elements from a sequence using equality comparer
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <param name="source">Source elements</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="comparer">Equality comparer</param>
        /// <returns>Returns distinct elements</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return DistinctByImpl(source, keySelector, comparer);
        }

        /// <summary>
        /// Finds the index of the first scan matching an expression in an enumerable.
        /// </summary>
        /// <param name="items">The enumerable to search.</param>
        /// <param name="predicate">The expression to test the items against.</param>
        /// <typeparam name="T">The generic type parameter</typeparam>
        /// <returns>The index of the first matching scan, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    return retVal;
                }

                retVal++;
            }

            return -1;
        }

        /// <summary>
        /// Finds the index of the first occurrence of an scan in an enumerable.
        /// </summary>
        /// <param name="items">The enumerable to search.</param>
        /// <param name="scan">The scan to find.</param>
        /// <typeparam name="T">The generic type parameter</typeparam>
        /// <returns>The index of the first matching scan, or -1 if the scan was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item)
        {
            return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i));
        }

        /// <summary>
        /// Performs an action for each scan into a collection
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="items">Collection items</param>
        /// <param name="action">Action function</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// Verifies whether the enumerable is null or empty
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="source">Source elements</param>
        /// <returns>Returns true or false</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source.IsNull())
            {
                return true;
            }

            return !source.Any();
        }

        /// <summary>
        /// Verifies whether the enumerable is empty
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="source">Source elements</param>
        /// <returns>Returns true or false</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> elementsToClone) where T : ICloneable
        {
            return elementsToClone.Select(item => (T)item.Clone());
        }
        
        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.ICollection.
        /// </summary>
        /// <typeparam name="T">Generic list</typeparam>
        /// <param name="collection">The collection whose elements should be added</param>
        /// <param name="items">Enumerable items</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Converts a specialized collection to enumerable of pairs
        /// </summary>
        /// <param name="collection">Specialized collection</param>
        /// <returns>Returns a list of pairs</returns>
        public static IEnumerable<KeyValuePair<string, string>> ToPairs(this NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            return collection.Cast<string>().Select(key => new KeyValuePair<string, string>(key, collection[key]));
        }

        /// <summary>
        /// Converts a specialized collection to lookup interface
        /// </summary>
        /// <param name="collection">Specialized collection</param>
        /// <returns>Returns a list of lookup values</returns>
        public static ILookup<string, string> ToLookup(this NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            var pairs = from key in collection.Cast<string>()
                        from value in collection.GetValues(key)
                        select new
                        {
                            key,
                            value
                        };

            return pairs.ToLookup(pair => pair.key, pair => pair.value);
        }

        /// <summary>
        /// Converts a dictionary to specialized collection
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dictionary">Dictionary instance</param>
        /// <returns>Returns a list of specialized values</returns>
        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            NameValueCollection result = new NameValueCollection();

            foreach (var item in dictionary)
            {
                string value = null;
                if (item.Value != null)
                {
                    value = item.Value.ToString();
                }

                result.Add(item.Key.ToString(), value);
            }

            return result;
        }

        /// <summary>
        /// Gets the value for a specific key in a NameValueCollection or returns a default value if not found
        /// </summary>
        /// <param name="collection">a NameValueCollection instance</param>
        /// <param name="key">the key to be searched</param>
        /// <param name="defaultValue">the default values to be returned if no value found</param>
        /// <returns>the value as string</returns>
        public static string GetValueForKey(this NameValueCollection collection, string key, string defaultValue)
        {
            if (collection.IsNull())
            {
                return defaultValue;
            }

            if (collection[key].IsNullOrEmpty())
            {
                return defaultValue;
            }

            return collection[key];
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns distinct elements from a sequence using equality comparer
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <param name="source">Source elements</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="comparer">Equality comparer</param>
        /// <returns>Returns distinct elements</returns>
        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #endregion
    }
}