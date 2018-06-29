namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Data;

    #endregion

    public sealed class CaseService : ICaseService
    {
        #region Private Members

        private readonly ICaseRepository caseRepository = null;

        private readonly ICaseDetailRepository caseDetailRepository = null;

        private readonly IRepository<CaseDetail> caseDetailTransactional = null;
        
        private readonly ITransactionRepository transactionRepository = null;

        private readonly IPaymentRepository paymentRepository = null;

        private readonly IUnitOfWork unitOfWork = null;

        private readonly IRepository<Cancellation> cancellationRepository = null;
        
        #endregion

        #region Constructors

        public CaseService() : this(
            IoC.Resolve<ICaseRepository>(),
            IoC.Resolve<ICaseDetailRepository>(),
            IoC.Resolve<ITransactionRepository>(),
            IoC.Resolve<IPaymentRepository>(),
            IoC.Resolve<IUnitOfWork>(),
            IoC.Resolve<IRepository<CaseDetail>>(),
            IoC.Resolve<IRepository<Cancellation>>())
        { }

        internal CaseService(
            ICaseRepository caseRepository,
            ICaseDetailRepository caseDetailRepository,
            ITransactionRepository transactionRepository,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            IRepository<CaseDetail> caseDetailTransactionalRepository,
            IRepository<Cancellation> cancellationRepository)
        {
            this.caseRepository = caseRepository;
            this.caseDetailRepository = caseDetailRepository;
            this.transactionRepository = transactionRepository;
            this.paymentRepository = paymentRepository;
            this.caseDetailTransactional = caseDetailTransactionalRepository;
            this.unitOfWork = unitOfWork;
            this.cancellationRepository = cancellationRepository;
        }

        #endregion

        #region Public Methods
        
        public IEnumerable<Case> SearchCases(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            return this.caseRepository.SearchCases(caseNumber, injuredName, ssn, dateOfBirth, caseDate, regionId, clinicId, accountEbt);
        }

        public Task<List<Case>> SearchCasesAsync(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            return this.caseRepository.SearchCasesAsync(caseNumber, injuredName, ssn, dateOfBirth, caseDate, regionId, clinicId, accountEbt);
        }
        
        public Case FindCaseByNumber(string caseNumber)
        {
            return this.caseRepository.GetCaseByNumber(caseNumber);
        }

        public Task<Case> FindCaseByNumberAsync(string caseNumber)
        {
            return this.caseRepository.GetCaseByNumberAsync(caseNumber);
        }
        
        public IEnumerable<Case> FindRelatedCasesByCaseNumber(string caseNumber)
        {
            return this.caseRepository.GetRelatedCasesByCaseNumber(caseNumber);
        }

        public IEnumerable<CaseDetail> FindRelatedCasesDetailByCaseNumber(string caseNumber, string caseKey)
        {
            return this.caseDetailRepository.GetRelatedCasesDetailByCaseNumber(caseNumber, caseKey);
        }

        public IEnumerable<Case> FindOtherRelatedCasesByCaseNumber(string caseNumber)
        {
            return this.caseRepository.GetOtherRelatedCasesByCaseNumber(caseNumber);
        }

        public IEnumerable<CaseDetail> FindOtherRelatedCasesDetailByCaseNumber(string caseNumber, string caseKey)
        {
            return this.caseDetailRepository.GetOtherRelatedCasesDetailByCaseNumber(caseNumber, caseKey);
        }

        public IEnumerable<RelatedCasesByCompensationRegion> FindRelatedCasesByCompensationRegion(string caseNumber)
        {
            return caseRepository.GetRelatedCasesByCompensationRegion(caseNumber);
        }
        
        public Task<List<CaseDetail>> SearchCaseDetailsAsync(string caseNumber, string caseKey, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            return this.caseDetailRepository.SearchCaseDetailsAsync(caseNumber, caseKey, injuredName, ssn, dateOfBirth, caseDate, regionId, clinicId, accountEbt);
        }

        public IEnumerable<CaseDetail> SearchCaseDetails(string caseNumber, string caseKey, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            return this.caseDetailRepository.SearchCaseDetails(caseNumber, caseKey, injuredName, ssn, dateOfBirth, caseDate, regionId, clinicId, accountEbt);
        }

        public Task<CaseDetail> FindCaseDetailByIdAsync(int caseDetailId)
        {
            return this.caseDetailRepository.GetByIdAsync(caseDetailId);
        }

        public CaseDetail FindCaseDetailById(int caseDetailId)
        {
            return this.caseDetailRepository.GetById(caseDetailId);
        }

        public IEnumerable<CaseDetail> FindRelatives(int caseId)
        {
            return this.caseDetailRepository.FindRelatives(caseId)
                                            .OrderBy(e => e.Entity.FullName);
        }

        public CaseDetail FindCaseDetailByIdAndKey(int caseId, string key) {            
            return this.caseDetailRepository.FindCaseDetailByIdAndKey(caseId, key);
        }
        
        public IEnumerable<ResumenPagosPorBeneficiario_Result> ResumenPagosPorBeneficiario(string caseNumber)
        {
            return this.caseRepository.ResumenPagosPorBeneficiario(caseNumber);
        }

        public IEnumerable<ResumenPagosPorConcepto_Result> ResumenPagosPorConcepto(string caseNumber)
        {
            return this.caseRepository.ResumenPagosPorConcepto(caseNumber);
        }

        public IEnumerable<BuscarCasos_Result> BuscarCasos(string nombre, string sSN, string eBT, string numeroCaso)
        {
            return this.caseRepository.BuscarCasos(nombre, sSN, eBT, numeroCaso);
        }

        public IEnumerable<InformacionCaso_Result> InformacionCaso(int? caseId)
        {
            return this.caseRepository.InformacionCaso(caseId);
        }

        public bool AddPreexistingCase(string caseNumber, string preexistingCaseNumber)
        {
            return this.caseRepository.AddPreexistingCase(caseNumber, preexistingCaseNumber);
        }

        public bool RemovePreexistingCase(string caseNumber, string preexistingCaseNumber)
        {
            return this.caseRepository.RemovePreexistingCase(caseNumber, preexistingCaseNumber);
        }

        public IEnumerable<CaseDetail> FindCasesInDormantExpunged()
        {
            return this.caseDetailRepository.FindCasesDormantExpunged();
        }

        public CaseDetail FindCaseDetailByNumber(string caseNumber)
        {
            return this.caseDetailRepository.GetCaseByNumber(caseNumber);
        }

        public CaseDetail FindCaseDetailByNumber(string caseNumber, string caseKey = "00")
        {
            return this.caseDetailRepository.GetCaseByNumber(caseNumber, caseKey);
        }

        public IEnumerable<CaseDetail> GetCaseBeneficiaries(string caseNumber) {
            return this.caseDetailRepository.GetCaseBeneficiariesByCaseNumber(caseNumber);
        }

        public void UpdateCaseDetail(CaseDetail caseDetail) {            
            caseDetailTransactional.Update(caseDetail);
            unitOfWork.SaveChanges();
        }

        public IEnumerable<Cancellation> GetAllCancellation() {
            return this.cancellationRepository.GetAll();
        }

        public CaseDetailDemographic GetEntityPriority(int caseDetailId){
            return this.caseDetailRepository.GetEntityPriorityDetail(caseDetailId);
        }

        public CaseDetail FindInjuredDetail(int casedetailId, int entityId){
            return this.caseDetailRepository.GetInjuredDetail(casedetailId, entityId);
        }

        public IEnumerable<Case> FindRelatedCasesUsedInDecision(string caseNumber)
        {
            return this.caseRepository.GetRelatedCasesUsedInDecision(caseNumber);
        }

        #endregion
    }
}