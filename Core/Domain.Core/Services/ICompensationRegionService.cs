namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System.Collections.Generic;
    using Domain.Core.Model;

    #endregion

    public interface ICompensationRegionService
    {
        IEnumerable<CompensationRegion> GetAllCompensationRegions();

        IEnumerable<CompensationRegion> GetCompensationRegionsGroupedByRegion();
    }
}