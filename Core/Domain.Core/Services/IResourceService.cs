namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IResourceService
    {
        string GetResourceString(string resourceName);

        IEnumerable<ResourcesString> GetResources();
    }
}