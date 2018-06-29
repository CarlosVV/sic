namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class EntityRepository : EfRepository<Entity>, IEntityRepository
    {
        public IEnumerable<Entity> FindBySourceId(int sourceId)
        {
            return EntitiesNoTracking.Include(e => e.Addresses)
                                     .Include(e => e.Addresses.Select(a => a.City))
                                     .Include(e => e.Addresses.Select(a => a.AddressType))
                                     .Include(e => e.Addresses.Select(a => a.Country))
                                     .Include(e => e.Addresses.Select(a => a.State))
                                     .Where(e => e.SourceId == sourceId);
        }

        public override Entity FindOne(Expression<Func<Entity, bool>> criteria, params Expression<Func<Entity, object>>[] navigationProperties)
        {
            var query = Entities.AsQueryable();

            foreach (var property in navigationProperties)
            {
                query = query.Include(property);
            }

            return query.FirstOrDefault(criteria);
        }

        public override void Insert(Entity entity)
        {
            int nextEntityId = EntitiesNoTracking.OrderByDescending(e => e.EntityId)
                                                 .FirstOrDefault()
                                                 .EntityId + 1;
            entity.EntityId = nextEntityId;

            base.Insert(entity);
        }
    }
}