namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;

    #endregion

    public class CivilStatusService : ICivilStatusService
    {
        #region Constantes

        private const string DataCacheKey = "Nagnoi.CivilStatus";

        #endregion

        #region Miembros Privados

        private readonly ICivilStatusRepository civilStatusRepository = null;

        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructores

        public CivilStatusService()
            : this(
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<ICivilStatusRepository>())
        { }

        internal CivilStatusService(ICacheManager gestorCache, ICivilStatusRepository civilStatusRepository)
        {
            this.cacheManager = gestorCache;
            this.civilStatusRepository = civilStatusRepository;
        }

        #endregion

        public IEnumerable<CivilStatus> GetCivilStatus()
        {
            return this.civilStatusRepository.GetCivilStatus();
        }

        public IEnumerable<CivilStatus> GetCivilStatusAll()
        {
            IEnumerable<CivilStatus> result;

            if (this.cacheManager.IsAdded(DataCacheKey))
            {
                result = this.cacheManager.Get(DataCacheKey) as IEnumerable<CivilStatus>;

                return result.Clone();
            }

            result = this.GetCivilStatus().ToList();
            
            this.cacheManager.Add(DataCacheKey, result);

            return result.Clone();
        }

        public CivilStatus InsertCivilStatus(CivilStatus civilStatus)
        {
            return this.civilStatusRepository.InsertCivilStatus(civilStatus);
        }

        public CivilStatus UpdateCivilStatus(CivilStatus civilStatus)
        {
            return this.civilStatusRepository.UpdateCivilStatus(civilStatus);
        }

        public void DeleteCivilStatus(int civilStatusId)
        {
            this.civilStatusRepository.DeleteCivilStatus(civilStatusId);
        }
    }
}