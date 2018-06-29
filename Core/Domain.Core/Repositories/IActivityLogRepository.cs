namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IActivityLogRepository : IRepository<ActivityLog>
    {
        IEnumerable<ActivityLog> SelectActivityLogByUser(int userId, DateTime from, DateTime to);

        IEnumerable<ActivityLog> Search(Hashtable filters);

        IEnumerable<ActivityLog> SelectPaged();
    }
}