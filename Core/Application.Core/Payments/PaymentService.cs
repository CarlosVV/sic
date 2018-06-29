namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Data;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;

    #endregion

    public sealed class PaymentService : IPaymentService
    {
        #region Constants

        private const string DataCacheKeyPaymentStatuses = "Nagnoi.PaymentStatuses";

        private const string DataCacheKeyAdjustmentStatuses = "Nagnoi.AdjustmentStatuses";

        private const string DataCacheKeyConcepts = "Nagnoi.Concepts";

        #endregion

        #region Private Members

        private readonly IThirdPartyScheduleRepository thirdPartyScheduleRepository = null;

        private readonly IPaymentRepository paymentRepository = null;

        private readonly ITransactionRepository transactionRepository = null;

        private readonly IConceptRepository conceptRepository = null;

        private readonly IRepository<Status> statusRepository = null;

        private readonly IRepository<AdjustmentStatus> adjustmentStatusRepository = null;

        private readonly IUnitOfWork unitOfWork = null;

        private readonly ICacheManager cacheManager = null;

        private readonly ITransferTypeService transferTypeService = null;

        private readonly IActivityLogService activityLogService = null;

        #endregion

        #region Constructors

        public PaymentService() : this(
            IoC.Resolve<IThirdPartyScheduleRepository>(),
            IoC.Resolve<IPaymentRepository>(),
            IoC.Resolve<ITransactionRepository>(),
            IoC.Resolve<IConceptRepository>(),
            IoC.Resolve<IRepository<Status>>(),
            IoC.Resolve<IRepository<AdjustmentStatus>>(),
            IoC.Resolve<IUnitOfWork>(),
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<ITransferTypeService>(),
            IoC.Resolve<IActivityLogService>())
        { }

        internal PaymentService(
            IThirdPartyScheduleRepository caseRepository,
            IPaymentRepository paymentRepository,
            ITransactionRepository transactionRepository,
            IConceptRepository conceptRepository,
            IRepository<Status> statusRepository,
            IRepository<AdjustmentStatus> adjustmentStatusRepository,
            IUnitOfWork unitOfWork,
            ICacheManager cacheManager,
            ITransferTypeService transferTypeService,
            IActivityLogService activityLogService)
        {
            this.thirdPartyScheduleRepository = caseRepository;
            this.paymentRepository = paymentRepository;
            this.transactionRepository = transactionRepository;
            this.conceptRepository = conceptRepository;
            this.statusRepository = statusRepository;
            this.adjustmentStatusRepository = adjustmentStatusRepository;
            this.unitOfWork = unitOfWork;
            this.cacheManager = cacheManager;
            this.transferTypeService = transferTypeService;
            this.activityLogService = activityLogService;
        }

        #endregion

        #region Public Methods
        public IEnumerable<ThirdPartySchedule> FindThirdPartyPaymentsToApprove(string EBTNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromTransactionDate,
            DateTime? toTransactionDate, int? transactionTypeId, int? paymentStatusId, int? ConceptId)
        {
            return this.thirdPartyScheduleRepository.FindToApprove(EBTNumber, caseNumber, entityName, ssn,
                birthDate, filingDate, regionId, dispensaryId, fromTransactionDate, toTransactionDate, transactionTypeId, paymentStatusId, ConceptId);
        }
        public IEnumerable<ThirdPartySchedule> FindThirdPartyPayments(int? scheduleId, int? remitterId, int? caseId)
        {
            return this.thirdPartyScheduleRepository.Find(scheduleId, remitterId, caseId);
        }

        public Task<List<ThirdPartySchedule>> FindThirdPartyPaymentsAsync(int? scheduleId, int? remitterId, int? caseId)
        {
            return this.thirdPartyScheduleRepository.FindAsync(scheduleId, remitterId, caseId);
        }

        public ThirdPartySchedule FindThirdPartyPaymentById(int scheduleId)
        {
            return this.thirdPartyScheduleRepository.GetById(scheduleId);
        }

        public void CreateThirdPayment(ThirdPartySchedule entity)
        {           
            this.thirdPartyScheduleRepository.Insert(entity);

            unitOfWork.SaveChanges();

            Dictionary<string, object> dicThirdPartySchedule = new Dictionary<string, object>();
            activityLogService.CreateDictionaryForThirdParty(ref dicThirdPartySchedule, entity);

            XmlHelper.SerializeKeyValuePairs("ThirdPartySchedule", dicThirdPartySchedule);

            activityLogService.CreateActivityLog(new ActivityLog {                
                Comment = XmlHelper.SerializeKeyValuePairs("ThirdPartySchedule", dicThirdPartySchedule),
                ObjectTypeId = (int)AuditObjectType.PaymentThirdPartySchedule,
                ObjectId = entity.ThirdPartyScheduleId.ToString()
            },
            "ThirdPartySchedule.Insert");            
        }

        public void ModifyThirdPayment(ThirdPartySchedule entity)
        {
            this.thirdPartyScheduleRepository.Update(entity);

            unitOfWork.SaveChanges();            
        }

        public IEnumerable<Payment> SearchPayments(int? caseId, string caseNumber, int? beneficiaryId, int? transferTypeId, int? classId, int? statusId)
        {
            return this.paymentRepository.Find(caseId, caseNumber, beneficiaryId, transferTypeId, classId, statusId);
        }

        public IEnumerable<Payment> SearchPeremptoryPayments(int? caseId, int? caseDetailId)
        {
            var statusEmitido = GetAllPaymentStatuses().Where(p => p.StatusCode == PaymentStatusEnum.Emitido.Description()).FirstOrDefault().StatusId;
            var statusMovimiento = GetAllPaymentStatuses().Where(p => p.StatusCode == PaymentStatusEnum.Movimiento.Description()).FirstOrDefault().StatusId;
            var statusPagado = GetAllPaymentStatuses().Where(p => p.StatusCode == PaymentStatusEnum.Pagado.Description()).FirstOrDefault().StatusId;

            return this.paymentRepository.SearchPeremptories(caseId, caseDetailId, new int[] { statusEmitido, statusMovimiento, statusPagado });
        }

        public IEnumerable<Payment> SearchInvestmentPayments(int? caseId, int? caseDetailId)
        {
            var statusEmitido = GetAllPaymentStatuses().Where(p => p.StatusCode == PaymentStatusEnum.Emitido.Description()).FirstOrDefault().StatusId;
            var statusMovimiento = GetAllPaymentStatuses().Where(p => p.StatusCode == PaymentStatusEnum.Movimiento.Description()).FirstOrDefault().StatusId;
            var statusPagado = GetAllPaymentStatuses().Where(p => p.StatusCode == PaymentStatusEnum.Pagado.Description()).FirstOrDefault().StatusId;

            return this.paymentRepository.SearchInvestments(caseId, caseDetailId, new int[] { statusEmitido, statusMovimiento, statusPagado });
        }

        public IEnumerable<Payment> SearchPaymentsAdjustmentEBT(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            var paymentStatus = GetAllPaymentStatuses().Where(p => p.StatusCode == PaymentStatusEnum.Pagado.Description()).FirstOrDefault().StatusId;

            if (transferTypeService.GetTransferTypes().Where(p => p.TransferType1 == PaymentTransferTypeEnum.EBT.Description()).Any())
            {
                var transferType = transferTypeService.GetTransferTypes().Where(p => p.TransferType1 == PaymentTransferTypeEnum.EBT.Description()).FirstOrDefault().TransferTypeId;

                return this.paymentRepository.Find(caseNumber, injuredName, ssn, dateOfBirth, caseDate, regionId, clinicId, accountEbt, transferType, paymentStatus, null, null);
            }

            return null;
        }

        public IEnumerable<Payment> FindAdjustmentsEBTToApprove()
        {
            var transferType = transferTypeService.GetTransferTypes().Where(p => p.TransferType1 == PaymentTransferTypeEnum.EBT.Description()).FirstOrDefault().TransferTypeId;
            var adjustmentStatus = adjustmentStatusRepository.GetAll().Where(p => p.AdjustmentStatus1 == PaymentAdjustmentStatusEnum.Solicitado.Description()).FirstOrDefault().AdjustmentStatusId;

            return this.paymentRepository.Find(null, null, null, null, null, null, null, null, transferType, null, adjustmentStatus, "Crédito");
        }

        public IEnumerable<Payment> FindAdjustmentEBTToDocument()
        {
            if (transferTypeService.GetTransferTypes().Where(p => p.TransferType1 == PaymentTransferTypeEnum.EBT.Description()).Any())
            {
                var transferType = transferTypeService.GetTransferTypes().Where(p => p.TransferType1 == PaymentTransferTypeEnum.EBT.Description()).FirstOrDefault().TransferTypeId;
                var adjustmentStatus = adjustmentStatusRepository.GetAll().Where(p => p.AdjustmentStatus1 == PaymentAdjustmentStatusEnum.Solicitado.Description()).FirstOrDefault().AdjustmentStatusId;
                return this.paymentRepository.Find(null, null, null, null, null, null, null, null, transferType, null, adjustmentStatus, "Débito");
            }
            return null;
        }

        public Payment FindPaymentById(int paymentId)
        {
            return this.paymentRepository.GetById(paymentId);
        }

        public void CreatePayment(Payment entity)
        {
            this.paymentRepository.Insert(entity);

            unitOfWork.SaveChanges();
        }

        public void ModifyPayment(Payment entity)
        {
            this.paymentRepository.Update(entity);

            unitOfWork.SaveChanges();
        }

        public IEnumerable<Status> GetAllPaymentStatuses()
        {
            IEnumerable<Status> result;

            if (this.cacheManager.IsAdded(DataCacheKeyPaymentStatuses))
            {
                Debug.WriteLine("Get Payment Status from Cache");

                result = this.cacheManager.Get(DataCacheKeyPaymentStatuses) as IEnumerable<Status>;

                return result.Clone();
            }

            result = this.statusRepository.GetAll().Where(p => p.Hidden.Value == false).ToList();

            this.cacheManager.Add(DataCacheKeyPaymentStatuses, result);

            return result.Clone();
        }

        public IEnumerable<AdjustmentStatus> GetAllAdjustmentStatuses()
        {
            IEnumerable<AdjustmentStatus> result;

            if (this.cacheManager.IsAdded(DataCacheKeyAdjustmentStatuses))
            {
                Debug.WriteLine("Get Adjustment Status from Cache");

                result = this.cacheManager.Get(DataCacheKeyAdjustmentStatuses) as IEnumerable<AdjustmentStatus>;

                return result.Clone();
            }

            result = this.adjustmentStatusRepository.GetAll().ToList();

            this.cacheManager.Add(DataCacheKeyAdjustmentStatuses, result);

            return result.Clone();
        }

        public IEnumerable<Payment> FindPaymentCertificationsByCaseNumber(string caseNumber, string caseKey)
        {
            return this.paymentRepository.FindCertifications(caseNumber, caseKey);
        }

        public void CreatePaymentCertification(Payment payment, Transaction transaction, bool isDiet, bool transactionExist)
        {
            if (isDiet)
            {
                this.paymentRepository.Insert(payment);
            }
            else
            {
                if (transactionExist)
                {
                    this.paymentRepository.Insert(payment);
                    this.transactionRepository.Update(transaction);
                }
                else
                {
                    this.paymentRepository.Insert(payment);
                    this.transactionRepository.Insert(transaction);
                }
            }

            unitOfWork.SaveChanges();
        }

        public IEnumerable<Concept> GetAllConcepts()
        {
            IEnumerable<Concept> result;

            if (this.cacheManager.IsAdded(DataCacheKeyConcepts))
            {
                Debug.WriteLine("Get Payment Concepts from Cache");

                result = this.cacheManager.Get(DataCacheKeyConcepts) as IEnumerable<Concept>;

                return result.Clone();
            }

            result = this.conceptRepository.GetAll().ToList();

            this.cacheManager.Add(DataCacheKeyConcepts, result);

            return result.Clone();
        }

        public IEnumerable<Payment> PaymentQuery(string QueryName)
        {
            return this.paymentRepository.PaymentQueries(QueryName);
        }

        public IEnumerable<Payment> FindCasesInDormantExpunged()
        {
            return this.paymentRepository.FindCasesDormantExpunged();
        }

        public IEnumerable<Payment> FindPaymentsToApprove(string EBTNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromIssueDate,
            DateTime? toIssueDate, int? paymentStatusId, int? ConceptId)
        {
            return paymentRepository.FindApprovals(EBTNumber, caseNumber, entityName, ssn, birthDate, filingDate, regionId, dispensaryId, fromIssueDate, toIssueDate, paymentStatusId, ConceptId);
        }

        public void Delete(int paymentId) {
            var paymentToDelete = paymentRepository.FindOne(payment => payment.PaymentId == paymentId);
            paymentRepository.Delete(paymentToDelete);
            unitOfWork.SaveChanges();
        }

        public IEnumerable<Payment> FindPaymentsToSAP() {

            return paymentRepository.Find(x => x.StatusId == (int)PaymentStatusEnum.SimeraProcesado || 
                                            (x.StatusId == (int)PaymentStatusEnum.Aprobado && 
                                            (x.Transaction.TransactionTypeId == (int) TransactionTypeEnum.AdjudicacionAdicional ||
                                            x.Transaction.TransactionTypeId == (int) TransactionTypeEnum.AdjudicacionInicial)) && x.Amount > 0,
                                            t => t.Transaction,
                                            t => t.Remitter,
                                            t => t.Remitter.Addresses,                                            
                                            t => t.Remitter.Addresses.Select(d => d.AddressType),
                                            t => t.Remitter.Addresses.Select(d => d.State),
                                            t => t.Remitter.Addresses.Select(d => d.Country),
                                            t => t.Remitter.Addresses.Select(d => d.City),
                                            t => t.Remitter.ParticipantStatus,
                                            t => t.Remitter.ParticipantType,
                                            t => t.CaseDetail,
                                            t => t.CaseDetail.Entity,
                                            t => t.CaseDetail.Entity.Addresses,
                                            t => t.CaseDetail.Entity.Addresses.Select(d => d.AddressType),
                                            t => t.CaseDetail.Entity.Addresses.Select(d => d.State),                                            
                                            t => t.CaseDetail.Entity.ParticipantType,
                                            t => t.CaseDetail.Entity.ParticipantStatus,
                                            t => t.CaseDetail.Case                                            
                                            );
        }

        #endregion    
   
    }
}