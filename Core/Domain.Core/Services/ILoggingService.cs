namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Imports

    using System;

    #endregion

    /// <summary>
    /// Logging Service
    /// </summary>
    public interface ILoggingService 
    {
        ///// <summary>
        ///// Searches and selects a paginated content of Log Entries, based on the specified searching Filters and pagination data
        ///// </summary>
        ///// <param name="startPage">the page number to be returned</param>
        ///// <param name="pageSize">the size (number of records) of the page to be returned</param>
        ///// <param name="sortAscending">a item indicating whether or not the sorting is in ascending order</param>
        ///// <param name="totalRows">an out parameter indicating the total number of rows that match the filtering criteria</param>
        ///// <param name="filters">a hashtable containing the filtering criteria</param>
        ///// <param name="logProvider">a LogProviderType enum item indicating from which provider the logs must be obtained (aspnet, Elmah, Log4Net)</param>
        ///// <param name="group">when false, returns the results as they were found; when true, groups repeated errors of the same type occurred within the same minute and returns them as one entry</param>
        ///// <returns>the paginated contents of LogEntry objects matching the filters</returns>
        //IEnumerable<ILogEntry> SelectLogEntriesPaged(int startPage, int pageSize, bool sortAscending, out int totalRows, Hashtable filters, LogProviderType logProvider, bool group);

        ///// <summary>
        ///// Selects the Log Entry with the specified Id from the specified Log Provider
        ///// </summary>
        ///// <param name="logEntryId">the identifier of the log entry</param>
        ///// <param name="logProvider">the log provider</param>
        ///// <returns>the matching LogEntry object</returns>
        //ILogEntry SelectLogEntryByIdAndProvider(string logEntryId, LogProviderType logProvider);
        void LogException(Exception exception);
        void LogError(string message);
        void LogWarningMessage(string message);
        void LogInfoMessage(string message);
    }
}