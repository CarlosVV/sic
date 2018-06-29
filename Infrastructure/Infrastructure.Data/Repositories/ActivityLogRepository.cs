namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class ActivityLogRepository : EfRepository<ActivityLog>, IActivityLogRepository
    {
        public IEnumerable<ActivityLog> SelectActivityLogByUser(int userId, DateTime from, DateTime to)
        {
            //TODO: Filter by UserId and DateTime
            var query = from item in this.Context.ActivityLogs select item;
            return query;
        }

        public IEnumerable<ActivityLog> Search(Hashtable filters)
        {
            //TODO: Filter by filters: ANY COLUMN and VALUE
            var query = from item in this.Context.ActivityLogs select item;
            return query;
        }
        
        public IEnumerable<ActivityLog> SelectPaged()
        {
            var items = from item in this.Context.ActivityLogs select item;
            return items;
        }
    }
}