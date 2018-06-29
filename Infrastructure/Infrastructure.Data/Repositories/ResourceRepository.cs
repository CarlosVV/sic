namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class ResourceRepository : EfRepository<ResourcesString>, IResourceRepository
    {
        public IEnumerable<ResourcesString> GetResourceString(string resourceName)
        {
            return Find(r => r.ResourceName.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
        }
    }
}