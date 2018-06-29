namespace Nagnoi.SiC.Application.Audit
{
    #region Imports

    using System.Collections;
    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;
    //using Nagnoi.SiC.Domain.Core.Model.Repository;
    //using Nagnoi.SiC.Domain.Core.Model.Services.Application;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using Nagnoi.SiC.Domain.Core.Services;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    /// <summary>
    /// Log Facade class
    /// </summary>
    public sealed class SearchLogService : ISearchLogService
    {
        #region Private Members

        /// <summary>
        /// WhereGoing Log Repository
        /// </summary>
        private readonly ISearchLogRepository searchLogRepository = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchLogService"/> class
        /// </summary>
        public SearchLogService()
            : this(IoC.Resolve<ISearchLogRepository>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchLogService"/> class
        /// </summary>
        /// <param name="searchLogRepository">the Activity Log Repository layer reference</param>
        internal SearchLogService(ISearchLogRepository searchLogRepository)
        {
            this.searchLogRepository = searchLogRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Inserts a new search log entry
        /// </summary>
        /// <param name="searchLogType">SearchLogType enumeration indicating the type of search (e.g. hotels search, room rates search, etc)</param>
        /// <param name="filters">List of filters</param>
        /// <param name="results">The number of matching results returned from the search</param>
        /// <param name="userId">User identifier</param>
        //public void CreateSearchLog(SearchLogType searchLogType, Hashtable filters, int results, int userId)
        //{
        //    this.CreateSearchLog(searchLogType, filters, results, new User() { UserId = userId });
        //}

        /// <summary>
        /// Inserts a new search log entry
        /// </summary>
        /// <param name="searchLogType">SearchLogType enumeration indicating the type of search (e.g. hotels search, room rates search, etc)</param>
        /// <param name="filters">List of filters</param>
        /// <param name="results">The number of matching results returned from the search</param>
        /// <param name="user">User instance</param>
        //public void CreateSearchLog(SearchLogType searchLogType, Hashtable filters, int results, User user)
        //{
        //    if (filters == null)
        //    {
        //        return;
        //    }

        //    SearchLog searchLog = new SearchLog();

        //    searchLog.SearchTypeId = (int)searchLogType;
        //    searchLog.SessionId = WebSessionHelper.GetSessionId();
        //    searchLog.Results = results;
        //    searchLog.LogHostName = WebHelper.GetServerName();
        //    searchLog.SearchLogDetails = this.BuildSearchLogDetails(filters);

        //    if (user != null &&
        //        !user.ExistsOnlyMaster)
        //    {
        //        searchLog.UserId = user.UserId;
        //    }

        //    this.searchLogRepository.Insert(searchLog);
        //}

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds the search log details
        /// </summary>
        /// <param name="filters">Filters contents</param>
        /// <returns>Returns a contents of search log details</returns>
        private List<SearchLogDetail> BuildSearchLogDetails(Hashtable filters)
        {
            List<SearchLogDetail> searchLogDetails = new List<SearchLogDetail>();

            if (filters == null || filters.Count == 0)
            {
                return searchLogDetails;
            }
            else
            {
                foreach (DictionaryEntry filter in filters)
                {
                    if (filter.Key != null)
                    {
                        SearchLogDetail searchLogDetail = new SearchLogDetail();

                        searchLogDetail.FilterName = filter.Key.ToString();

                        if (filter.Value != null)
                        {
                            searchLogDetail.FilterValue = filter.Value.ToString();
                        }
                        else
                        {
                            searchLogDetail.FilterValue = string.Empty;
                        }

                        searchLogDetails.Add(searchLogDetail);
                    }
                }

                return searchLogDetails;
            }
        }

        #endregion
    }
}