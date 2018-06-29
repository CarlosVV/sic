namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using Model;

    #endregion

    public interface ISimeraBeneficiaryRepository : IRepository<SimeraBeneficiary>
    {
        SimeraBeneficiary InsertSimeraBeneficiary(SimeraBeneficiary entity);

        void DeleteSimeraBeneficiary(SimeraBeneficiary entity);

        void DeleteCaseBeneficiaries(string caseNumber);
    }
}