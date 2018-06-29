namespace Nagnoi.SiC.Infrastructure.Core.Caching {
    #region Referencias

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;

    #endregion

    public sealed class PerRequestCacheManager : ICacheManager {
        #region Propiedades

        /// <summary>
        /// Gets a item indicating whether cache is enabled
        /// </summary>
        public bool IsEnabled {
            get {
                return true;
            }
        }

        #endregion

        #region Metodos Publicos

        /// <summary>
        /// Gets a cache scan
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Returns an instance of cache scan</returns>
        public object Get(string key) {
            var items = this.GetElements();

            if (items == null) {
                return null;
            }

            return items[key];
        }

        /// <summary>
        /// Adds a new cache scan
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="data">Object instance</param>
        public void Add(string key, object data) {
            this.Add(key, data, TimeSpan.Zero);
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="data">Object instance</param>
        /// <param name="duration">Cache duration</param>
        public void Add(string key, object data, TimeSpan duration) {
            if (data == null) {
                return;
            }

            var items = this.GetElements();

            if (items == null) {
                return;
            }

            if (this.IsEnabled) {
                if (items.Contains(key)) {
                    items[key] = data;
                }
                else {
                    items.Add(key, data);
                }
            }
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="data">Object instance</param>
        /// <param name="duration">Time duration</param>
        /// <param name="cachingTime">Cache settings <see cref="CachingTime"/></param>
        public void Add(string key, object data, TimeSpan duration, CachingTime cachingTime) {
            this.Add(key, data, duration);
        }

        /// <summary>
        /// Gets a item indicating whether the item associated 
        /// with the specified key is cached
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Return true or false</returns>
        public bool IsAdded(string key) {
            return this.GetElements().Contains(key);
        }

        /// <summary>
        /// Removes a cache scan by key
        /// </summary>
        /// <param name="key">Cache key</param>
        public void Remove(string key) {
            var items = this.GetElements();

            if (items == null) {
                return;
            }

            items.Remove(key);
        }

        /// <summary>
        /// Removes a cache scan by searching pattern
        /// </summary>
        /// <param name="pattern">Searching pattern</param>
        public void RemoveByPattern(string pattern) {
            var items = this.GetElements();

            if (items == null) {
                return;
            }

            IDictionaryEnumerator enumerator = items.GetEnumerator();

            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var keysToRemove = new List<string>();

            while (enumerator.MoveNext()) {
                if (regex.IsMatch(enumerator.Key.ToString())) {
                    keysToRemove.Add(enumerator.Key.ToString());
                }
            }

            foreach (string key in keysToRemove) {
                items.Remove(key);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear() {
            var items = this.GetElements();
            if (items == null) {
                return;
            }

            var enumerator = items.GetEnumerator();
            var keysToRemove = new List<string>();
            while (enumerator.MoveNext()) {
                keysToRemove.Add(enumerator.Key.ToString());
            }

            foreach (string key in keysToRemove) {
                items.Remove(key);
            }
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Retrieves the list of cached items
        /// </summary>
        /// <returns>Returns a dictionary that contains all cache items</returns>
        private IDictionary GetElements() {
            HttpContext current = HttpContext.Current;
            if (current != null) {
                return current.Items;
            }

            return null;
        }

        #endregion
    }
}
