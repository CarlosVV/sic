namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ISimeraBeneficiaryService
    {
        SimeraBeneficiary InsertSimeraBeneficiary(SimeraBeneficiary simeraBeneficiary);

        void DeleteSimeraBeneficiary(SimeraBeneficiary simeraBeneficiary);

        void DeleteCaseBeneficiaries(string caseNumber);
    }
}