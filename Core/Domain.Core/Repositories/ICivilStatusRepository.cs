namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ICivilStatusRepository : IRepository<CivilStatus>
    {
        IEnumerable<CivilStatus> GetCivilStatus();

        CivilStatus InsertCivilStatus(CivilStatus civilStatus);

        CivilStatus UpdateCivilStatus(CivilStatus civilStatus);

        void DeleteCivilStatus(int civilStatusId);
    }
}