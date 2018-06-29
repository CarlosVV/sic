namespace Nagnoi.SiC.Domain.Core.Services {
    #region Imports

    using System.Collections;

    #endregion

    /// <summary>
    /// Activity Log Service
    /// </summary>
    public interface ISearchLogService 
    {
        /// <summary>
        /// Inserts a new search log entry
        /// </summary>
        /// <param name="searchLogType">SearchLogType enumeration indicating the type of search (e.g. hotels search, room rates search, etc)</param>
        /// <param name="filters">List of filters</param>
        /// <param name="results">The number of matching results returned from the search</param>
        /// <param name="userId">User identifier</param>
        void CreateSearchLog(SearchLogType searchLogType, Hashtable filters, int results, int userId);

        /// <summary>
        /// Inserts a new search log entry
        /// </summary>
        /// <param name="searchLogType">SearchLogType enumeration indicating the type of search (e.g. hotels search, room rates search, etc)</param>
        /// <param name="filters">List of filters</param>
        /// <param name="results">The number of matching results returned from the search</param>
        /// <param name="user">User instance</param>
        void CreateSearchLog(SearchLogType searchLogType, Hashtable filters, int results, User user);
    }
}