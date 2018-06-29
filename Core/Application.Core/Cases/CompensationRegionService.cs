namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;

    #endregion

    public sealed class CompensationRegionService : ICompensationRegionService
    {
        #region Private Members

        private const string CompensationRegionsCacheKey = "Nagnoi.SiC.CompensationRegions.All";

        private readonly IRepository<CompensationRegion> compensationRegionRepository = null;

        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructors

        public CompensationRegionService() : this(
            IoC.Resolve<IRepository<CompensationRegion>>(),
            IoC.Resolve<ICacheManager>())
        { }

        internal CompensationRegionService(
            IRepository<CompensationRegion> compensationRegionRepository,
            ICacheManager cacheManager)
        {
            this.compensationRegionRepository = compensationRegionRepository;
            this.cacheManager = cacheManager;
        }

        #endregion

        #region Public Methods

        public IEnumerable<CompensationRegion> GetAllCompensationRegions()
        {
            IEnumerable<CompensationRegion> result;

            if (this.cacheManager.IsAdded(CompensationRegionsCacheKey))
            {
                Debug.WriteLine("Get Compensation Regions from Cache");

                result = this.cacheManager.Get(CompensationRegionsCacheKey) as IEnumerable<CompensationRegion>;

                return result.Clone();
            }

            result = this.compensationRegionRepository.GetAll().ToList();

            this.cacheManager.Add(CompensationRegionsCacheKey, result);

            return result.Clone();
        }

        public IEnumerable<CompensationRegion> GetCompensationRegionsGroupedByRegion()
        {
            var compensationRegions = this.GetAllCompensationRegions();

            return compensationRegions.DistinctBy(c => c.Region);
        }

        #endregion
    }
}