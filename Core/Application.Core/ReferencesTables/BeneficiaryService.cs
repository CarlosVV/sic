namespace Nagnoi.SiC.Application.Core
{
    #region Referencias

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;

    #endregion

    public class BeneficiaryService : IBeneficiaryService
    {
        #region Constantes

        private const string DataCacheKey = "Nagnoi.Beneficiary";

        #endregion

        #region Miembros Privados

        private readonly IBeneficiaryRepository beneficiaryRepository = null;

        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructores

        public BeneficiaryService()
            : this(
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<IBeneficiaryRepository>())
        { }

        internal BeneficiaryService(
            ICacheManager cacheManager,
            IBeneficiaryRepository beneficiaryRepository)
        {
            this.cacheManager = cacheManager;
            this.beneficiaryRepository = beneficiaryRepository;
        }

        #endregion

        public IEnumerable<Beneficiary> GetBeneficiaries()
        {
            return this.beneficiaryRepository.GetBeneficiaries();
        }

        public IEnumerable<Beneficiary> GetBeneficiariesAll()
        {
            IEnumerable<Beneficiary> result;

            if (this.cacheManager.IsAdded(DataCacheKey))
            {
                result = this.cacheManager.Get(DataCacheKey) as IEnumerable<Beneficiary>;

                return result.Clone();
            }

            result = this.GetBeneficiaries().ToList();
            
            this.cacheManager.Add(DataCacheKey, result);

            return result.Clone();
        }

        public Beneficiary InsertBeneficiary(Beneficiary beneficiary)
        {
            return this.beneficiaryRepository.InsertBeneficiary(beneficiary);
        }

        public Beneficiary UpdateBeneficiary(Beneficiary beneficiary)
        {
            return this.beneficiaryRepository.UpdateBeneficiary(beneficiary);
        }

        public void DeleteBeneficiary(int beneficiaryId)
        {
            this.beneficiaryRepository.DeleteBeneficiary(beneficiaryId);
        }
    }
}