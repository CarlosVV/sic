namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IBeneficiaryService
    {
        IEnumerable<Beneficiary> GetBeneficiaries();

        IEnumerable<Beneficiary> GetBeneficiariesAll();

        Beneficiary InsertBeneficiary(Beneficiary beneficiary);

        Beneficiary UpdateBeneficiary(Beneficiary beneficiary);

        void DeleteBeneficiary(int beneficiaryId);
    }
}