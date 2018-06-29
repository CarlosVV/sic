namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System.Collections.Generic;
    using Model;

    #endregion

    public interface IEntityService
    {
        void CreateEntity(Entity entity);

        void ModifyEntity(Entity entity);

        Entity GetById(int entityId);

        IEnumerable<Entity> GetByCaseNumber(string caseNumber);

        IEnumerable<Entity> GetBySourceId(int sourceId);

        IEnumerable<Entity> GetAllCourts();

        void Delete(int? nullable);
    }
}