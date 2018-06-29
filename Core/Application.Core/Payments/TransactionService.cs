namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Data;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;

    #endregion

    public sealed class TransactionService : ITransactionService
    {
        #region Constants

        private const string DataCacheKeyAdjustmentReasons = "Nagnoi.AdjustmentReasons";

        #endregion

        #region Private Members

        private readonly ICaseRepository caseRepository = null;

        private readonly ITransactionRepository transactionRepository = null;

        private readonly IPaymentRepository paymentRepository = null;

        private readonly IRepository<AdjustmentReason> adjustmentReasonRepository = null;

        private readonly IUnitOfWork unitOfWork = null;

        private readonly ICacheManager cacheManager = null;
        
        #endregion

        #region Constructors

        public TransactionService() : this(
            IoC.Resolve<ICaseRepository>(),
            IoC.Resolve<ITransactionRepository>(),
            IoC.Resolve<IPaymentRepository>(),
            IoC.Resolve<IRepository<AdjustmentReason>>(),
            IoC.Resolve<IUnitOfWork>(),
            IoC.Resolve<ICacheManager>())
        { }

        internal TransactionService(
            ICaseRepository caseRepository,
            ITransactionRepository transactionRepository,
            IPaymentRepository paymentRepository,
            IRepository<AdjustmentReason> adjustmentReasonRepository,
            IUnitOfWork unitOfWork,
            ICacheManager cacheManager)
        {
            this.caseRepository = caseRepository;
            this.transactionRepository = transactionRepository;
            this.paymentRepository = paymentRepository;
            this.adjustmentReasonRepository = adjustmentReasonRepository;
            this.unitOfWork = unitOfWork;
            this.cacheManager = cacheManager;
        }

        #endregion

        #region Public Methods

        public void CreateTransaction(Transaction transaction)
        {
            this.transactionRepository.Insert(transaction);

            unitOfWork.SaveChanges();
        }
        
        public IEnumerable<Transaction> FindTransactionsByCaseDetailId(int caseDetailId)
        {
            return this.transactionRepository.Find(t => t.CaseDetailId == caseDetailId,
                                                   t => t.TransactionType,
                                                   t => t.Concept,
                                                   t => t.CaseReference);
        }

        public IEnumerable<Transaction> FindTransactionsByCaseId(int caseId)
        {
            return transactionRepository.GetTransactionByCaseId(caseId); 
        }

        public IEnumerable<Transaction> SearchTransactions(int? caseId, int? transactionTypeId, string caseNumber)
        {
            return this.transactionRepository.Find(caseId, transactionTypeId, caseNumber);
        }

        public IEnumerable<Transaction> SearchIppTransactions(int caseDetailId)
        {
            return this.transactionRepository.Find(t => t.CaseDetailId == caseDetailId &&
                                                        t.ConceptId == 3 &&
                                                        new int?[] { 1, 2 }.Contains(t.TransactionTypeId),
                                                   t => t.TransactionType,
                                                   t => t.Concept,
                                                   t => t.CaseReference);
        }
        
        public IEnumerable<Transaction> SearchInvestmentsTransactions(int? caseId, int? caseDetailId)
        {
            IEnumerable<Transaction> transactions = null;

            if (caseId.HasValue) {
                transactions =  this.transactionRepository.Find(t => t.CaseDetail.CaseId == caseId.Value &&
                                                        t.ConceptId == 4 &&
                                                        new int?[] { 3 }.Contains(t.TransactionTypeId),
                                                   t => t.TransactionType,
                                                   t => t.Concept,
                                                   t => t.CaseReference,
                                                   t => t.Payments,
                                                   t => t.CaseDetail,
                                                   t => t.CaseDetail.Entity,
                                                   t => t.CaseDetail.RelationshipType);
            } else if (caseDetailId.HasValue) {
                transactions = this.transactionRepository.Find(t => t.CaseDetailId == caseDetailId.Value &&
                                                        t.ConceptId == 4 &&
                                                        new int?[] { 3 }.Contains(t.TransactionTypeId),
                                                   t => t.TransactionType,
                                                   t => t.Concept,
                                                   t => t.CaseReference,
                                                   t => t.Payments,
                                                   t => t.CaseDetail,
                                                   t => t.CaseDetail.Entity,
                                                   t => t.CaseDetail.RelationshipType);
            }

            return transactions;
        }

        public Transaction GetTransaction(int caseDetailId, string invoiceNumber)
        {
            return this.transactionRepository.GetByCaseId(caseDetailId, invoiceNumber);
        }

        public Transaction GetTransaction(int caseDetailId, string invoiceNumber, int conceptId, int transactionTypeId)
        {
            return this.transactionRepository.GetByCaseId(caseDetailId, invoiceNumber, conceptId, transactionTypeId);
        }

        public void ModifyTransaction(Transaction transaction)
        {
            this.transactionRepository.Update(transaction);

            unitOfWork.SaveChanges();
        }
        
        public Transaction FindTransactionById(int transactionid)
        {
            return this.transactionRepository.GetById(transactionid);
        }

        public decimal? GetBalanceByCase(int? caseDetailId)
        {
            return this.caseRepository.GetBalanceByCase(caseDetailId);
        }

        public PaymentBalance GetBalanceDetailByCase(int? caseDetailId)
        {
            return this.caseRepository.GetBalanceDetailByCase(caseDetailId);
        }

        public decimal? GetTotalAdjudicationByCase(int? caseId)
        {
            return this.caseRepository.GetTotalAdjudicationByCase(caseId);
        }

        public decimal? GetTotalAdjudicationByOtherCases(int? caseId)
        {
            return this.caseRepository.GetTotalAdjudicationByOtherCases(caseId);
        }

        public IEnumerable<Transaction> FindTransactionsToApprove(string ebtNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromTransactionDate,
            DateTime? toTransactionDate, int? transactionTypeId, int? paymentStatusId, int? conceptId = null)
        {
            return this.transactionRepository.FindApprovals(ebtNumber, caseNumber, entityName, ssn, birthDate, filingDate, regionId, dispensaryId, fromTransactionDate, toTransactionDate, transactionTypeId, paymentStatusId, conceptId);
        }

        public void ApprovePendingDiets(int caseDetailId)
        {
            ApproveTransactions(caseDetailId, null, 2, 1);
        }

        public void ApproveTransactions(int caseDetailId, int? transactionId, int? conceptId, int? paymentClassId)
        {
            DateTime workingDate = DateTime.Now;

            var paymentsToApprove = paymentRepository.Find(caseDetailId, transactionId, conceptId, paymentClassId);
            foreach (var paymentToApprove in paymentsToApprove)
            {
                paymentToApprove.ModifiedDateTime = workingDate;
                paymentToApprove.StatusChangeDate = workingDate;
                paymentToApprove.StatusId = 13;

                paymentRepository.Update(paymentToApprove);
            }

            unitOfWork.SaveChanges();
        }

        public void RejectPendingDiets(int caseDetailId, string reason)
        {
            RejectTransactions(caseDetailId, null, reason, 2, 1);
        }

        public void RejectTransactions(int caseDetailId, int? transactionId, string reason, int? conceptId, int? paymentClassId)
        {
            DateTime workingDate = DateTime.Now;

            var paymentsToApprove = paymentRepository.Find(caseDetailId, transactionId, conceptId, paymentClassId);
            foreach (var paymentToApprove in paymentsToApprove)
            {
                paymentToApprove.ModifiedDateTime = workingDate;
                paymentToApprove.StatusChangeDate = workingDate;
                paymentToApprove.StatusId = 12;

                paymentRepository.Update(paymentToApprove);
            }

            if (transactionId.HasValue)
            {
                var transaction = FindTransactionById(transactionId.Value);
                transaction.RejectedReason = reason;
            }
            
            unitOfWork.SaveChanges();
        }

        public void CancelTransactions(int caseDetailId, int? transactionId)
        {
            DateTime workingDate = DateTime.Now;

            var paymentsToApprove = paymentRepository.Find(caseDetailId, transactionId, null, null);
            foreach (var paymentToApprove in paymentsToApprove)
            {
                paymentToApprove.ModifiedDateTime = workingDate;
                paymentToApprove.StatusChangeDate = workingDate;
                paymentToApprove.StatusId = 1;

                paymentRepository.Update(paymentToApprove);
            }

            unitOfWork.SaveChanges();
        }

        public void ReverseTransactions(int caseDetailId, int? transactionId, string reason)
        {
            DateTime workingDate = DateTime.Now;

            var paymentsToApprove = paymentRepository.Find(caseDetailId, transactionId, null, null);
            foreach (var paymentToApprove in paymentsToApprove)
            {
                paymentToApprove.ModifiedDateTime = workingDate;
                paymentToApprove.StatusChangeDate = workingDate;
                paymentToApprove.StatusId = 13;

                paymentRepository.Update(paymentToApprove);
            }

            unitOfWork.SaveChanges();
        }

        public IEnumerable<Transaction> GetTransactionByConceptCasefolder(int caseDetailId, String caseKey, int concept)
        {
            return this.transactionRepository.GetTransactionByConceptAndCasefolder(caseDetailId,caseKey, concept);
        }

        public IEnumerable<Transaction> GetTransactionTypeInversionCase(int caseDetailId, String caseKey)
        {
            return this.transactionRepository.GetTransactionTypeInversionByCase(caseDetailId,caseKey);
        }

        public IEnumerable<Transaction> FindCaseBeneficiariesByCase(int caseDetailId, String caseKey, int caseId)
        {
            return this.transactionRepository.FindCaseBeneficiariesByCaseId(caseDetailId,caseKey,caseId);
        }

        public IEnumerable<AdjustmentReason> GetAllAdjustmentReasons()
        {
            IEnumerable<AdjustmentReason> result;

            if (this.cacheManager.IsAdded(DataCacheKeyAdjustmentReasons))
            {
                Debug.WriteLine("Get Adjustment Reasons from Cache");

                result = this.cacheManager.Get(DataCacheKeyAdjustmentReasons) as IEnumerable<AdjustmentReason>;

                return result.Clone();
            }

            result = this.adjustmentReasonRepository.GetAll().ToList();

            this.cacheManager.Add(DataCacheKeyAdjustmentReasons, result);

            return result.Clone();
        }

        //public IEnumerable<TransactionDetail> GetTransactionDetail(int transactionId)
        //{
        //    return transactionId
        //}

        public IEnumerable<Transaction> SearchTransactionsByCaseIdWithEffect(int caseId, String effect)
        {
            return transactionRepository.GetTransactionByCaseIdWithEffect(caseId,effect);
        }

        public IEnumerable<CompensationRegion2> GetCompensationRegion()
        {
            return transactionRepository.GetCompensationRegion();
        }

        public IEnumerable<TransactionDetail2> InsertTransactionDetail(IEnumerable<TransactionDetail2> trans)
        {
            return transactionRepository.InsertTransactionDetail(trans);
        }

        public IEnumerable<TransactionDetail2> UpdateTransactionDetail(IEnumerable<TransactionDetail2> trans)
        {
            return transactionRepository.UpdateTransactionDetail(trans);
        }

        public IEnumerable<TransactionDetail2> GetTransactionDetailById(int transactionId)
        {
            return transactionRepository.GetTransactionDetail(transactionId);
        }

        public IEnumerable<Transaction> FindBeneficiaryDetail(int caseDetailId, String caseKey, int caseId)
        {
            return transactionRepository.GetBeneficiaryDetail(caseDetailId,caseKey,caseId);
        }

        public void Delete(int transactionId)
        {
            var transaction = transactionRepository.Find(entity => entity.TransactionId == transactionId);
            transactionRepository.Delete(transaction);
        }

        #endregion


        
    }
}