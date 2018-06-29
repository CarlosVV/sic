namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IRepository<TEntity> where TEntity : class
    {
        #region Data Display Methods
        
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties);

        Task<List<TEntity>> GetAllAsync();

        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        IEnumerable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, bool ascending);

        IEnumerable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, bool ascending, params Expression<Func<TEntity, object>>[] navigationProperties);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> criteria);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] navigationProperties);

        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] navigationProperties);

        IEnumerable<TEntity> Find<TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, bool ascending);

        IEnumerable<TEntity> Find<TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, bool ascending, params Expression<Func<TEntity, object>>[] navigationProperties);
        
        TEntity FindOne(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] navigationProperties);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] navigationProperties);

        bool Contains(Expression<Func<TEntity, bool>> filter);

        #endregion

        #region Data Transactional Methods

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> filter);

        void Delete(IEnumerable<TEntity> entities);

        #endregion
    }
}