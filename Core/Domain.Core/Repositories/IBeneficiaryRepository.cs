namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IBeneficiaryRepository : IRepository<Beneficiary>
    {
        IEnumerable<Beneficiary> GetBeneficiaries();
        Beneficiary InsertBeneficiary(Beneficiary beneficiary);
        Beneficiary UpdateBeneficiary(Beneficiary beneficiary);
        void DeleteBeneficiary(int beneficiaryId);
    }
}