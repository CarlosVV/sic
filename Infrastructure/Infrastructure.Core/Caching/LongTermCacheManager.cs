namespace Nagnoi.SiC.Infrastructure.Core.Caching {
    #region Referencias

    using System;
    using System.Collections;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Caching;

    #endregion

    public sealed class LongTermCacheManager : ICacheManager {
        #region Miembros Privados

        /// <summary>
        /// Cache field
        /// </summary>
        private readonly Cache cache;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LongTermCacheManager"/> class.
        /// </summary>
        public LongTermCacheManager() {
            HttpContext current = HttpContext.Current;
            if (current == null) {
                this.cache = HttpRuntime.Cache;
            }
            else {
                this.cache = current.Cache;
            }
        }

        #endregion

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
            return this.cache[key];
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

            this.Add(key, data, duration, null, CachingTime.LongTermAbsolute);
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="data">Object instance</param>
        /// <param name="duration">Duration time</param>
        /// <param name="cachingTime">Cache settings <see cref="CachingTime"/></param>
        public void Add(string key, object data, TimeSpan duration, CachingTime cachingTime) {
            this.Add(key, data, duration, null, cachingTime);
        }

        /// <summary>
        /// Gets a item indicating whether the item associated 
        /// with the specified key is cached
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Return true or false</returns>
        public bool IsAdded(string key) {
            return this.cache[key] != null;
        }

        /// <summary>
        /// Removes a cache scan by key
        /// </summary>
        /// <param name="key">Cache key</param>
        public void Remove(string key) {
            this.cache.Remove(key);
        }

        /// <summary>
        /// Removes a cache scan by searching pattern
        /// </summary>
        /// <param name="pattern">Searching pattern</param>
        public void RemoveByPattern(string pattern) {
            IDictionaryEnumerator enumerator = this.cache.GetEnumerator();

            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            while (enumerator.MoveNext()) {
                if (regex.IsMatch(enumerator.Key.ToString())) {
                    this.cache.Remove(enumerator.Key.ToString());
                }
            }
        }

        /// <summary>
        /// Clear all cache content
        /// </summary>
        public void Clear() {
            IDictionaryEnumerator enumerator = this.cache.GetEnumerator();

            while (enumerator.MoveNext()) {
                this.cache.Remove(enumerator.Key.ToString());
            }
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Set a new cache scan
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="data">Object instance</param>
        /// <param name="duration">Duration time</param>
        /// <param name="dependency">Cache dependency</param>
        /// <param name="cachingTime">Caching time</param>
        private void Add(string key, object data, TimeSpan duration, CacheDependency dependency, CachingTime cachingTime) {
            if (data == null) {
                return;
            }

            if (!this.IsEnabled) {
                return;
            }

            DateTime absoluteExpiration;
            TimeSpan slidingExpiration;

            switch (cachingTime) {
                case CachingTime.LongTermAbsolute:

                    absoluteExpiration = Cache.NoAbsoluteExpiration;
                    slidingExpiration = Cache.NoSlidingExpiration;

                    break;
                case CachingTime.LongTermSliding:

                    if (duration > TimeSpan.Zero) {
                        absoluteExpiration = DateTime.UtcNow.Add(duration);
                    }
                    else {
                        absoluteExpiration = DateTime.UtcNow.AddMinutes(20);
                    }

                    slidingExpiration = Cache.NoSlidingExpiration;

                    break;
                case CachingTime.ShortTermSliding:

                    absoluteExpiration = Cache.NoAbsoluteExpiration;

                    if (duration > TimeSpan.Zero) {
                        slidingExpiration = duration;
                    }
                    else {
                        slidingExpiration = new TimeSpan(0, 0, 20, 0);
                    }

                    break;
                default:

                    absoluteExpiration = Cache.NoAbsoluteExpiration;
                    slidingExpiration = Cache.NoSlidingExpiration;

                    break;
            }

            this.cache.Insert(key, data, dependency, absoluteExpiration, slidingExpiration, CacheItemPriority.AboveNormal, null);
        }

        #endregion
    }
}
