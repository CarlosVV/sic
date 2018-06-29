namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System;
    using System.Collections.Generic;
    using Domain.Core.Model;

    #endregion

    public interface ITransactionService
    {
        void CreateTransaction(Transaction transaction);
        
        IEnumerable<Transaction> FindTransactionsByCaseDetailId(int caseDetailId);

        IEnumerable<Transaction> FindTransactionsByCaseId(int caseId);

        IEnumerable<Transaction> SearchTransactions(int? caseId, int? transactionTypeId, string caseNumber);

        IEnumerable<Transaction> SearchIppTransactions(int caseDetailId);
        
        Transaction GetTransaction(int caseDetailId, string invoiceNumber);

        Transaction GetTransaction(int caseDetailId, string invoiceNumber, int conceptId, int transactionTypeId);
        
        void ModifyTransaction(Transaction transaction);

        decimal? GetBalanceByCase(int? caseDetailId);

        PaymentBalance GetBalanceDetailByCase(int? caseDetailId);

        decimal? GetTotalAdjudicationByCase(int? caseId);

        decimal? GetTotalAdjudicationByOtherCases(int? caseId);
        
        Transaction FindTransactionById(int transactionid);

        IEnumerable<Transaction> FindTransactionsToApprove(string ebtNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromTransactionDate,
            DateTime? toTransactionDate, int? transactionTypeId, int? paymentStatusId, int? conceptId = null);

        void ApprovePendingDiets(int caseDetailId);

        void ApproveTransactions(int caseDetailId, int? transactionId, int? conceptId, int? paymentClassId);

        void CancelTransactions(int caseDetailId, int? transactionId);

        void RejectPendingDiets(int caseDetailId, string reason);

        void RejectTransactions(int caseDetailId, int? transactionId, string reason, int? conceptId, int? paymentClassId);

        IEnumerable<Transaction> GetTransactionByConceptCasefolder(int caseDetailId, String caseKey, int concept);

        IEnumerable<Transaction> GetTransactionTypeInversionCase(int caseDetailId, String caseKey);

        IEnumerable<Transaction> FindCaseBeneficiariesByCase(int caseDetailId, String caseKey, int caseId);

        void ReverseTransactions(int caseDetailId, int? transactionId, string reason);

        IEnumerable<AdjustmentReason> GetAllAdjustmentReasons();

        IEnumerable<Transaction> SearchTransactionsByCaseIdWithEffect(int caseId, String effect);

        IEnumerable<CompensationRegion2> GetCompensationRegion();

        IEnumerable<TransactionDetail2> InsertTransactionDetail(IEnumerable<TransactionDetail2> trans);

        IEnumerable<TransactionDetail2> UpdateTransactionDetail(IEnumerable<TransactionDetail2> trans);

        IEnumerable<TransactionDetail2> GetTransactionDetailById(int transactionId);

        IEnumerable<Transaction> FindBeneficiaryDetail(int caseDetailId, String caseKey, int caseId);

        IEnumerable<Transaction> SearchInvestmentsTransactions(int? caseId, int? caseDetailId);

        void Delete(int transactionId);
    }
}