namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using System;
    using System.Collections.Generic;
    using Model;

    #endregion

    public interface IPaymentRepository : IRepository<Payment>
    {
        Payment GetById(int paymentId);
        
        IEnumerable<Payment> Find(int? caseId, string caseNumber, int? beneficiaryId, int? transferTypeId, int? classId, int? statusId);

        IEnumerable<Payment> Find(int? caseDetailId, int? transactionId, int? conceptId, int? paymentClassId);
        
        IEnumerable<Payment> FindCertifications(string caseNumber, string caseKey);

        IEnumerable<Payment> Find(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt, int? transferTypeId, int? statusId, int? adjustmentStatusId, string adjustmentTypeId);

        IEnumerable<Payment> PaymentQueries(string QueryName);

        IEnumerable<Payment> FindCasesDormantExpunged();

        IEnumerable<Payment> FindApprovals(string EBTNumber, string caseNumber, string entityName, string ssn, DateTime? birthDate, DateTime? filingDate, int? regionId, int? dispensaryId, DateTime? fromIssueDate, DateTime? toIssueDate, int? paymentStatusId, int? ConceptId);

        IEnumerable<Payment> SearchPeremptories(int? caseId, int? caseDetailId, int[] statusIds);

        IEnumerable<Payment> SearchInvestments(int? caseId, int? caseDetailId, int[] statusIds);
    }
}