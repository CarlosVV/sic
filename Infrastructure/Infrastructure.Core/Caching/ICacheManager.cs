namespace Nagnoi.SiC.Infrastructure.Core.Caching {
    #region Referencias

    using System;

    #endregion

    /// <summary>
    /// Representa la interface del gestor de cache
    /// </summary>
    public interface ICacheManager {
        #region Methods

        /// <summary>
        /// Obtiene un element del cache
        /// </summary>
        /// <param name="key">Clave del elemento</param>
        /// <returns>El elemento del cache</returns>
        object Get(string key);

        /// <summary>
        /// Agrega un elemento al cache especificando su clave
        /// </summary>
        /// <param name="key">Clave del elemento</param>
        /// <param name="data">Instancia del objeto</param>
        void Add(string key, object data);

        /// <summary>
        /// Agrega un elemento al cache especificando su clave
        /// </summary>
        /// <param name="key">Clave del elemento</param>
        /// <param name="data">Instancia del objeto</param>
        /// <param name="duration">Cache duration</param>
        void Add(string key, object data, TimeSpan duration);

        /// <summary>
        /// Agrega un elemento al cache especificando su clave
        /// </summary>
        /// <param name="key">Clave del elemento</param>
        /// <param name="data">Instancia del objeto</param>
        /// <param name="duration">Duracion dentro de la cache</param>
        /// <param name="cachingTime">Tipo de duracion <see cref="CachingTime"/></param>
        void Add(string key, object data, TimeSpan duration, CachingTime cachingTime);

        /// <summary>
        /// Determina si un elemento del cache ha sido agregado
        /// </summary>
        /// <param name="key">Clave del elemento</param>
        /// <returns>Existe o no</returns>
        bool IsAdded(string key);

        /// <summary>
        /// Quita un elemento del cache especificando su clave
        /// </summary>
        /// <param name="key">Clave del elemento</param>
        void Remove(string key);

        /// <summary>
        /// Quita un elemento del cache por patron
        /// </summary>
        /// <param name="pattern">Patron de busqueda</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Limpia todos los elementos del cache
        /// </summary>
        void Clear();

        #endregion
    }
}
