namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Core.Repositories;
    using Infrastructure.Core.Dependencies;

    #endregion

    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Private Members

        private readonly IDatabaseContext context = null;

        private IDbSet<TEntity> entities;

        #endregion

        #region Constructor

        public EfRepository()
        {
            this.context = IoC.Resolve<IDatabaseContext>();
        }

        #endregion

        #region Properties

        protected IDatabaseContext Context
        {
            get { return context; }
        }

        protected virtual IDbSet<TEntity> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = context.Set<TEntity>();
                }

                return entities;
            }
        }

        protected virtual IQueryable<TEntity> EntitiesNoTracking
        {
            get
            {
                return Entities.AsNoTracking();
            }
        }

        #endregion

        #region Data Display Methods
        
        public virtual IEnumerable<TEntity> GetAll()
        {
            return EntitiesNoTracking.AsNoTracking()
                                     .AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> query = Entities.AsQueryable();

            foreach (var property in navigationProperties)
            {
                query = query.Include(property);
            }
            
            return query.AsNoTracking()
                        .AsEnumerable();
        }

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return EntitiesNoTracking.ToListAsync();
        }

        public virtual Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return EntitiesNoTracking.ToListAsync(cancellationToken);
        }

        public virtual IEnumerable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, bool ascending)
        {
            if (ascending)
            {
                return Entities.AsQueryable()
                               .OrderBy(orderBy)
                               .AsEnumerable();
            }
            else
            {
                return Entities.AsQueryable()
                               .OrderByDescending(orderBy)
                               .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, bool ascending, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> query = Entities.AsQueryable();

            foreach (var property in navigationProperties)
            {
                query = query.Include(property);
            }

            if (ascending)
            {
                return query.OrderBy(orderBy)
                            .AsEnumerable();
            }
            else
            {
                return query.OrderByDescending(orderBy)
                            .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> criteria)
        {
            return EntitiesNoTracking.Where(criteria);
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> query = Entities.AsQueryable();

            foreach (var property in navigationProperties)
            {
                query = query.Include(property);
            }

            query = query.AsNoTracking()
                         .Where(criteria);

            return query;
        }

        public virtual Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> query = Entities.AsQueryable();

            foreach (var property in navigationProperties)
            {
                query = query.Include(property);
            }

            return query.AsNoTracking()
                        .Where(criteria)
                        .ToListAsync();
        }

        public virtual IEnumerable<TEntity> Find<TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, bool ascending)
        {
            if (ascending)
            {
                return Entities.AsQueryable()
                               .Where(criteria)
                               .OrderBy(orderBy)
                               .AsEnumerable();
            }
            else
            {
                return Entities.AsQueryable()
                               .Where(criteria)
                               .OrderByDescending(orderBy)
                               .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> Find<TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, bool ascending, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> query = Entities.AsQueryable();

            foreach (var property in navigationProperties)
            {
                query = query.Include(property);
            }

            if (ascending)
            {
                return query.Where(criteria)
                            .OrderBy(orderBy)
                            .AsEnumerable();
            }
            else
            {
                return query.Where(criteria)
                            .OrderByDescending(orderBy)
                            .AsEnumerable();
            }
        }
        
       public virtual TEntity 
            FindOne(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> query = Entities.AsQueryable();

            foreach (var property in navigationProperties)
            {
                query = query.Include(property);
            }

            return query.FirstOrDefault(criteria);
        }      

        public virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> query = Entities.AsQueryable();

            foreach (var property in navigationProperties)
            {
                query = query.Include(property);
            }

            return query.FirstOrDefaultAsync(criteria);
        }

        public virtual bool Contains(Expression<Func<TEntity, bool>> criteria)
        {
            return Entities.Any(criteria);
        }
        
        #endregion

        #region Data Transactional Methods

        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

           
            Entities.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Entities.Remove(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> filter)
        {
            var entitiesToDelete = Entities.Where(filter);
            foreach (var entityToDelete in entitiesToDelete)
            {
                Delete(entityToDelete);
            }
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }
            
            foreach (var entityToDelete in entities)
            {
                Delete(entityToDelete);
            }
        }

        #endregion
    }
}