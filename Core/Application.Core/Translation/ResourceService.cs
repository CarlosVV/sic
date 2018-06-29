namespace Nagnoi.SiC.Application.Core
{
    #region Referencias

    using System.Collections.Generic;
    using System.Linq;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;
    using Nagnoi.SiC.Domain.Core.Services;
    using Nagnoi.SiC.Infrastructure.Core.Caching;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    #endregion

    public sealed class ResourceService : IResourceService
    {
        #region Constantes

        private const string DataCacheKey = "Nagnoi.Resources";

        #endregion

        #region Miembros Privados

        private readonly IResourceRepository resourceRepository = null;

        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructores

        public ResourceService()
            : this(
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<IResourceRepository>()
            )
        { }

        internal ResourceService(
            ICacheManager cacheManager,
            IResourceRepository resourceRepository)
        {
            this.cacheManager = cacheManager;
            this.resourceRepository = resourceRepository;
        }

        #endregion
        
        public string GetResourceString(string resourceName)
        {
            var result = from item in this.GetResources()
                         where item.ResourceName == resourceName
                         select item;
            if (result != null)
            {
                var entity = result.FirstOrDefault();
                if (entity != null)
                {
                    return entity.ResourceValue;
                }
            }
            return resourceName;
        }

        public IEnumerable<ResourcesString> GetResources()
        {
            IEnumerable<ResourcesString> result;

            if (this.cacheManager.IsAdded(DataCacheKey))
            {
                result = this.cacheManager.Get(DataCacheKey) as IEnumerable<ResourcesString>;
                
                return result.Clone();
            }

            result = this.resourceRepository.GetAll().ToList();
            
            this.cacheManager.Add(DataCacheKey, result);

            return result.Clone();
        }
    }
}