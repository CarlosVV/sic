namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ICompensationRegion
    {
        IEnumerable<CompensationRegion> GetCompensationRegions();

        CompensationRegion CompensationRegion(CompensationRegion compensationRegion);

        TransferType UpdateCompensationRegion(CompensationRegion compensationRegion);

        void DeleteCompensationRegion(int compensationRegion);
	}
}