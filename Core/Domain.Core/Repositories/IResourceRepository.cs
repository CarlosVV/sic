namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IResourceRepository : IRepository<ResourcesString>
    {
        IEnumerable<ResourcesString> GetResourceString(string resourceName);
    }
}