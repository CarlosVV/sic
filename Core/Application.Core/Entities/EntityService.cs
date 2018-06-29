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
    using Nagnoi.SiC.Infrastructure.Core.Data;

    #endregion

    public sealed class EntityService : IEntityService
    {
        #region Constants

        private const string DataCacheKeyCourts = "Nagnoi.Courts";

        #endregion

        #region Private Members

        private readonly IEntityRepository entityRepository = null;

        private readonly ICacheManager cacheManager = null;

        private readonly IUnitOfWork unitOfWork = null;

        #endregion

        #region Constructors

        public EntityService() : this(
            IoC.Resolve<IEntityRepository>(),
            IoC.Resolve<IUnitOfWork>(),
            IoC.Resolve<ICacheManager>())
        { }

        internal EntityService(
            IEntityRepository entityRepository,
            IUnitOfWork unitOfWork,
            ICacheManager cacheManager)
        {
            this.entityRepository = entityRepository;
            this.unitOfWork = unitOfWork;
            this.cacheManager = cacheManager;
        }

        #endregion

        #region Public Methods

        public void CreateEntity(Entity entity)
        {
            this.entityRepository.Insert(entity);
            unitOfWork.SaveChanges();
        }

        public void ModifyEntity(Entity entity)
        {
            this.entityRepository.Update(entity);
            unitOfWork.SaveChanges();
        }

        public Entity GetById(int entityId)
        {
            return this.entityRepository.FindOne(e => e.EntityId == entityId);
        }

        public IEnumerable<Entity> GetByCaseNumber(string caseNumber)
        {
            return this.entityRepository.Find(x => x.CaseNumber == caseNumber);
        }

        public IEnumerable<Entity> GetBySourceId(int sourceId)
        {
            return this.entityRepository.Find(x => x.SourceId == sourceId);
        }

        public IEnumerable<Entity> GetAllCourts()
        {
            IEnumerable<Entity> result;

            if (this.cacheManager.IsAdded(DataCacheKeyCourts))
            {
                result = this.cacheManager.Get(DataCacheKeyCourts) as IEnumerable<Entity>;

                return result.Clone();
            }

            result = this.entityRepository.FindBySourceId(13).ToList();

            this.cacheManager.Add(DataCacheKeyCourts, result);

            return result.Clone();

        }

        public void Delete(int? entityId)
        {
            var entityToDelete = entityRepository.FindOne(entity => entity.EntityId == entityId.Value);
            entityRepository.Delete(entityToDelete);
        }

        #endregion
    }
}