namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;


    #endregion

    /// <summary>
    /// Activity Log Service
    /// </summary>
    public interface IActivityLogService 
    {     
        /// <summary>
        /// Inserts a new Activity Log entry
        /// </summary>
        /// <param name="activityLog">the ActivityLog object containing the data to be logged</param>
        /// <param name="systemKeyword">a string that identifies the type of activity</param>
        void CreateActivityLog(ActivityLog activityLog, string systemKeyword);

        /// <summary>
        /// Gets all activity log types
        /// </summary>
        /// <returns>Returns the contents of activity log types</returns>
        IEnumerable<ActivityLogType> GetAllActivityTypes();

        /// <summary>
        /// Gets the activity type by its identifier
        /// </summary>
        /// <param name="activityLogTypeId">Activity Log Type identifier</param>
        /// <returns>Returns the activity log type</returns>
        ActivityLogType FindActivityTypeById(int activityLogTypeId);

        /// <summary>
        /// Finds an activity log type by system keyword
        /// </summary>
        /// <param name="systemKeyword">System keyword</param>
        /// <returns>Returns the activity log type</returns>
        ActivityLogType FindActivityLogTypeBySystemKeyword(string systemKeyword);

        /// <summary>
        /// Selects all the activity logs for a specific user between range dates
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="from">Earliest UTC date of the logs to be retrieved</param>
        /// <param name="to">Later UTC date of the logs to be retrieved</param>
        /// <returns>Returns a contents of activity logs</returns>
        IEnumerable<ActivityLog> SelectActivityLogsByUserId(int userId, DateTime from, DateTime to);

   
        /// <summary>
        /// Saves the Activity Log entry into the cached session summary
        /// </summary>
        /// <param name="activityLog">the ActivityLog object containing the data to be logged</param>
        void SaveActivityLogToCache(ActivityLog activityLog);

        /// <summary>
        /// Searches a set of activity logs
        /// </summary>
        /// <param name="filters">Filter criteria</param>
        /// <returns>Returns a contents of activity logs</returns>
        IEnumerable<ActivityLog> SearchActivityLogs(Hashtable filters);

        /// <summary>
        /// Searches and selects paginated ActivityLog entries, based on the specified searching filters and pagination data
        /// </summary>
        /// <param name="startPage">the page number to be returned</param>
        /// <param name="pageSize">the size (number of records) of the page to be returned</param>
        /// <param name="sortAscending">a item indicating whether or not the sorting is in ascending order</param>
        /// <param name="totalRows">an out parameter indicating the total number of rows that match the filtering criteria</param>
        /// <param name="filters">a hashtable containing the filtering criteria</param>
        /// <returns>Returns a paginated list of activity log entries</returns>
        IEnumerable<ActivityLog> SelectActivityLogPaged(int startPage, int pageSize, bool sortAscending, out int totalRows, Hashtable filters);

        /// <summary>
        /// Searches and selects paginated ActivityLogType entries, based on the specified searching filters and pagination data
        /// </summary>
        /// <param name="startPage">the page number to be returned</param>
        /// <param name="pageSize">the size (number of records) of the page to be returned</param>
        /// <param name="sortAscending">a item indicating whether or not the sorting is in ascending order</param>
        /// <param name="totalRows">an out parameter indicating the total number of rows that match the filtering criteria</param>
        /// <param name="filters">a hashtable containing the filtering criteria</param>
        /// <returns>Returns a paginated list of activity log type entries</returns>
        IEnumerable<ActivityLogType> SelectActivityLogTypesPaged(int startPage, int pageSize, bool sortAscending, out int totalRows, Hashtable filters);

        /// <summary>
        /// Saves changes in a list of activity log types
        /// </summary>
        /// <param name="activityLogTypes">a list of ActivityLogType objects</param>
        void SaveActivityLogTypes(IList<ActivityLogType> activityLogTypes);

        /// <summary>
        /// Creates dictionary for ThirdPartySchedule
        /// </summary>
        /// <param name="dicThirdParty"></param>
        /// <param name="objThirdPartySchedule"></param>
        void CreateDictionaryForThirdParty(ref Dictionary<string, object> dicThirdParty, ThirdPartySchedule objThirdPartySchedule);
    }
}