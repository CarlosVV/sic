namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IRelationshipCategoryRepository : IRepository<RelationshipCategory>
    {
        IEnumerable<RelationshipCategory> GetRelationshipCategories();

        RelationshipCategory InsertRelationshipCategory(RelationshipCategory relationshipCategory);

        RelationshipCategory UpdateRelationshipCategory(RelationshipCategory relationshipCategory);

        void DeleteRelationshipCategory(int relationshipCategoryId);
    }
}