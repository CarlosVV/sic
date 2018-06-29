namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Imports

    using System;
    using Nagnoi.SiC.Domain.Core.Model;
    
    #endregion

    public interface IActivityLogTypeRepository : IRepository<ActivityLogType>
    {
        void SaveAll(string xmlActivityLogTypes, string logHostName, DateTime logLastDate);
    }
}