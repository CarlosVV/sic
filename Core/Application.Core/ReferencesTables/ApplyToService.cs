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

    public sealed class ApplyToService : IApplyToService
    {
        #region Constantes

        private const string DataCacheKey = "Nagnoi.ApplyTo";

        #endregion

        #region Miembros Privados

        private readonly IApplyToRepository applyToRepository = null;

        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructores

        public ApplyToService()
            : this(
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<IApplyToRepository>())
        { }

        internal ApplyToService(
            ICacheManager cacheManager,
            IApplyToRepository applyToRepository)
        {
            this.cacheManager = cacheManager;
            this.applyToRepository = applyToRepository;
        }

        #endregion

        #region Public Methods

        public IEnumerable<ApplyTo> GetApplyTosAll()
        {
            IEnumerable<ApplyTo> result;

            if (this.cacheManager.IsAdded(DataCacheKey))
            {
                result = this.cacheManager.Get(DataCacheKey) as IEnumerable<ApplyTo>;

                return result.Clone();
            }

            result = this.applyToRepository.GetAll().ToList();
            
            this.cacheManager.Add(DataCacheKey, result);

            return result.Clone();
        }

        public ApplyTo InsertApplyTo(ApplyTo applyTo)
        {
            return this.applyToRepository.InsertApplyTo(applyTo);
        }

        public ApplyTo UpdateApplyTo(ApplyTo applyTo)
        {
            return this.applyToRepository.UpdateApplyTo(applyTo);
        }

        public void DeleteApplyTo(int applyToId)
        {
            this.applyToRepository.DeleteApplyTo(applyToId);
        }

        #endregion
    }
}