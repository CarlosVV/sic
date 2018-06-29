namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ICivilStatusService
    {
        IEnumerable<CivilStatus> GetCivilStatus();

        IEnumerable<CivilStatus> GetCivilStatusAll();

        CivilStatus InsertCivilStatus(CivilStatus civilStatus);

        CivilStatus UpdateCivilStatus(CivilStatus civilStatus);

        void DeleteCivilStatus(int civilStatusId);
    }
}