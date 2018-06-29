namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Imports

    using System.Collections.Generic;
    using Domain.Core.Model;

    #endregion

    /// <summary>
    /// Access Control Level Repository
    /// </summary>
    public interface IAccessControlLevelRepository : IRepository<AccessControlLevel>
    {
        IEnumerable<AccessControlLevel> GetAll(int functionalityId, int profileId, bool allow);

        IEnumerable<AccessControlLevel> FindByProfileName(string profileName);

        IEnumerable<AccessControlLevel> FindByProfileName(string[] profileNames);

        AccessControlLevel GetById(int accessControlLevelId);
    }
}