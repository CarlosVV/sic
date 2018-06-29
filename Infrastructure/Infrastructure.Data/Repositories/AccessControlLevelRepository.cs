namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Core.Helpers;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class AccessControlLevelRepository : EfRepository<AccessControlLevel>, IAccessControlLevelRepository
    {
        public override IEnumerable<AccessControlLevel> GetAll()
        {
            return EntitiesNoTracking
                           .Include(a => a.Functionality)
                           .Include(a => a.Profile);
        }

        public IEnumerable<AccessControlLevel> GetAll(int functionalityId, int profileId, bool allow)
        {
            return EntitiesNoTracking
                           .Where(a => a.FunctionalityId == functionalityId && a.ProfileId == profileId && a.Allow == allow)
                           .Include(a => a.Functionality)
                           .Include(a => a.Profile);
        }

        public IEnumerable<AccessControlLevel> FindByProfileName(string profileName)
        {
            return EntitiesNoTracking
                           .Where(a => a.Profile.ProfileName.Equals(profileName, StringComparison.OrdinalIgnoreCase))
                           .Include(a => a.Functionality)
                           .Include(a => a.Profile);
        }

        public IEnumerable<AccessControlLevel> FindByProfileName(string[] profileNames)
        {
            return EntitiesNoTracking
                           .Where(a => profileNames.Any(p => p.Contains(a.Profile.ProfileName)))
                           .Include(a => a.Functionality)
                           .Include(a => a.Profile);
        }

        public AccessControlLevel GetById(int accessControlLevelId)
        {
            var query = from item in this.Context.AccessControlLevels
                        where item.AccessControlLevelId == accessControlLevelId
                        select item;
            return query.FirstOrDefault();
        }

        public new void Delete(AccessControlLevel accessControlLevel)
        {
            var objectToDelete = this.Context.AccessControlLevels.Find(accessControlLevel.AccessControlLevelId);
            this.Context.Entry(objectToDelete).State = EntityState.Deleted;
            this.Context.SaveChanges();
        }

        public new void Update(AccessControlLevel accessControlLevel)
        {
            accessControlLevel.ModifiedBy = WebHelper.GetUserName();
            accessControlLevel.ModifiedDateTime = DateTime.UtcNow;
            this.Context.Entry(accessControlLevel).State = EntityState.Modified;
            this.Context.SaveChanges();
        }

        public new int Insert(AccessControlLevel accessControlLevel)
        {
            accessControlLevel.CreatedBy = WebHelper.GetUserName();
            accessControlLevel.CreatedDateTime = DateTime.UtcNow;
            this.Context.AccessControlLevels.Add(accessControlLevel);
            this.Context.SaveChanges();
            return accessControlLevel.AccessControlLevelId;
        }
    }
}