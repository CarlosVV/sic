namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using System;
    using System.Collections.Generic;
    using Domain.Core.Model;

    #endregion

    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> Find(int? caseId, int? transactionTypeId, string caseNumber);

        Transaction GetById(int transactionId);

        Transaction GetByCaseId(int caseDetailId, string invoiceNumber);

        Transaction GetByCaseId(int caseDetailId, string invoiceNumber, int conceptId, int transactionTypeId);

        IEnumerable<Transaction> FindApprovals(string EBTNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromTransactionDate,
            DateTime? toTransactionDate, int? transactionTypeId, int? paymentStatusId, int? ConceptId);

        IEnumerable<Transaction> GetTransactionByConceptAndCasefolder(int CaseDetailId, String CaseKey, int concept);

        IEnumerable<Transaction> GetTransactionTypeInversionByCase(int CaseDetailId, String CaseKey);

        IEnumerable<Transaction> FindCaseBeneficiariesByCaseId(int CaseDetailId, String CaseKey, int CaseId);

        IEnumerable<Transaction> GetTransactionByCaseId(int caseid);

        IEnumerable<Transaction> GetTransactionByCaseIdWithEffect(int caseid, string Effect);

        IEnumerable<CompensationRegion2> GetCompensationRegion();

        IEnumerable<TransactionDetail2> InsertTransactionDetail(IEnumerable<TransactionDetail2> trans);

        IEnumerable<TransactionDetail2> UpdateTransactionDetail(IEnumerable<TransactionDetail2> trans);

        IEnumerable<TransactionDetail2> GetTransactionDetail(int TransactionId);

        IEnumerable<Transaction> GetBeneficiaryDetail(int CaseDetailId, String CaseKey, int CaseId);
    }
}