namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Core.Helpers;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class RelationshipCategoryRepository : EfRepository<RelationshipCategory>, IRelationshipCategoryRepository
    {
        public IEnumerable<RelationshipCategory> GetRelationshipCategories()
        {
            return this.Context.RelationshipCategories.OrderBy(m => m.RelationshipCategory1);
        }

        public RelationshipCategory InsertRelationshipCategory(RelationshipCategory relationshipCategory)
        {

            relationshipCategory.RelationshipCategoryId = CreateNewId();
            relationshipCategory.CreatedBy = WebHelper.GetUserName();
            relationshipCategory.CreatedDateTime = DateTime.Now;
            this.Context.RelationshipCategories.Add(relationshipCategory);
            this.Context.SaveChanges();
            return relationshipCategory;
        }

        public RelationshipCategory UpdateRelationshipCategory(RelationshipCategory relationshipCategory)
        {
            var relationshipCategoryToUpdate = this.Context.RelationshipCategories.Find(relationshipCategory.RelationshipCategoryId);
            relationshipCategoryToUpdate.RelationshipCategory1 = relationshipCategory.RelationshipCategory1;
            relationshipCategoryToUpdate.Hidden = relationshipCategory.Hidden;
            relationshipCategoryToUpdate.ModifiedBy = WebHelper.GetUserName();
            relationshipCategoryToUpdate.ModifiedDateTime = DateTime.Now;

            this.Context.Entry(relationshipCategoryToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return relationshipCategoryToUpdate;
        }

        public void DeleteRelationshipCategory(int relationshipCategoryId)
        {
            var relationshipCategory = this.Context.RelationshipCategories.Find(relationshipCategoryId);
            this.Context.RelationshipCategories.Remove(relationshipCategory);
            this.Context.SaveChanges();
        }

        private int CreateNewId()
        {
            var relationshipCategoryList = from d in this.Context.RelationshipCategories orderby d.RelationshipCategoryId descending select d;
            return relationshipCategoryList.FirstOrDefault().RelationshipCategoryId + 1;
        }
    }
}