namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;

    #endregion

    public interface IPaymentService
    {
        IEnumerable<ThirdPartySchedule> FindThirdPartyPaymentsToApprove(string EBTNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromTransactionDate,
            DateTime? toTransactionDate, int? transactionTypeId, int? paymentStatusId, int? ConceptId);

        IEnumerable<ThirdPartySchedule> FindThirdPartyPayments(int? scheduleId, int? remitterId, int? caseId);

        Task<List<ThirdPartySchedule>> FindThirdPartyPaymentsAsync(int? scheduleId, int? remitterId, int? caseId);

        ThirdPartySchedule FindThirdPartyPaymentById(int scheduleId);

        void CreateThirdPayment(ThirdPartySchedule entity);

        void ModifyThirdPayment(ThirdPartySchedule entity);

        void CreatePayment(Payment entity);

        void ModifyPayment(Payment entity);

        IEnumerable<Payment> SearchPayments(int? caseId, string caseNumber, int? beneficiaryId, int? transferTypeId, int? classId, int? statusId);

        IEnumerable<Payment> SearchPeremptoryPayments(int? caseId, int? caseDetailId);

        IEnumerable<Payment> SearchInvestmentPayments(int? caseId, int? caseDetailId);

        IEnumerable<Payment> SearchPaymentsAdjustmentEBT(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        IEnumerable<Payment> FindAdjustmentsEBTToApprove();

        IEnumerable<Payment> FindAdjustmentEBTToDocument();

        Payment FindPaymentById(int paymentId);

        IEnumerable<Payment> FindPaymentCertificationsByCaseNumber(string caseNumber, string caseKey);

        IEnumerable<Status> GetAllPaymentStatuses();

        IEnumerable<AdjustmentStatus> GetAllAdjustmentStatuses();

        IEnumerable<Concept> GetAllConcepts();

        void CreatePaymentCertification(Payment payment, Transaction transaction, bool IsDiet, bool transactionExist);

        IEnumerable<Payment> PaymentQuery(string QueryName);

        IEnumerable<Payment> FindCasesInDormantExpunged();

        IEnumerable<Payment> FindPaymentsToApprove(string EBTNumber, string caseNumber, string entityName, string ssn, DateTime? birthDate, DateTime? filingDate, int? regionId, int? dispensaryId, DateTime? fromIssueDate, DateTime? toIssueDate, int? paymentStatusId, int? ConceptId = null);

        void Delete(int p);

        IEnumerable<Payment> FindPaymentsToSAP();

        //void CreatePaymentInvestment(IEnumerable<Payment> payments, Transaction transaction, IEnumerable<Entity> entity);
    }
}