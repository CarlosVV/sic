namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IRelationshipTypeService
    {
        IEnumerable<RelationshipType> GetRelationshipTypes();

        RelationshipType InsertRelationshipType(RelationshipType relationshipType);

        RelationshipType UpdateRelationshipType(RelationshipType relatioshipType);

        void DeleteRelationshipType(int relationshipTypeId);
    }
}