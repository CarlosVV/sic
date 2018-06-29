namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using System.Collections.Generic;
    using Model;

    #endregion

    public interface IEntityRepository : IRepository<Entity>
    {
        IEnumerable<Entity> FindBySourceId(int sourceId);
    }
}