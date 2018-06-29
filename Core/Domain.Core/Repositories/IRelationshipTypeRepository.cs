namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IRelationshipTypeRepository : IRepository<RelationshipType>
    {
        IEnumerable<RelationshipType> GetRelationshipTypes();

        RelationshipType InsertRelationshipType(RelationshipType relationshipType);

        RelationshipType UpdateRelationshipType(RelationshipType relationshipType);

        void DeleteRelationshipType(int relationshipTypeId);
    }
}