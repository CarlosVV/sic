using System.Collections.Generic;
using Nagnoi.SiC.Domain.Core.Model;

namespace Nagnoi.SiC.Domain.Core.Services
{
    public interface IRelationshipCategoryService
    {
        IEnumerable<RelationshipCategory> GetRelationshipCategories();

        RelationshipCategory InsertRelationshipCategory(RelationshipCategory relationshipType);

        RelationshipCategory UpdateRelationshipCategory(RelationshipCategory relatioshipType);

        void DeleteRelationshipCategory(int relationshipTypeId);
    }
}