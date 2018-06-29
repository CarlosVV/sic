namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System;
    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class BeneficiaryRepository : EfRepository<Beneficiary>, IBeneficiaryRepository
    {
        public IEnumerable<Beneficiary> GetBeneficiaries()
        {
            throw new NotImplementedException();
        }

        public Beneficiary InsertBeneficiary(Beneficiary beneficiary)
        {
            throw new NotImplementedException();
        }

        public Beneficiary UpdateBeneficiary(Beneficiary beneficiary)
        {
            throw new NotImplementedException();
        }

        public void DeleteBeneficiary(int beneficiaryId)
        {
            throw new NotImplementedException();
        }
    }
}