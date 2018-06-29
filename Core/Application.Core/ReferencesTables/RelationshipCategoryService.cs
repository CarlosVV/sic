using System.Collections.Generic;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Domain.Core.Repositories;
using Nagnoi.SiC.Domain.Core.Services;
using Nagnoi.SiC.Infrastructure.Core.Dependencies;

namespace Nagnoi.SiC.Application.Core
{
    public class RelationshipCategoryService : IRelationshipCategoryService
    {
        #region Private Members

        private readonly IRelationshipCategoryRepository relationshipCategoryRepository = null;

        #endregion

        #region Constructors

        public RelationshipCategoryService()
            : this(IoC.Resolve<IRelationshipCategoryRepository>())
        { }

        internal RelationshipCategoryService(IRelationshipCategoryRepository relationshipCategoryRepository)
        {
            this.relationshipCategoryRepository = relationshipCategoryRepository;
        }

        #endregion

        public IEnumerable<RelationshipCategory> GetRelationshipCategories()
        {
            return this.relationshipCategoryRepository.GetRelationshipCategories();
        }

        public RelationshipCategory InsertRelationshipCategory(RelationshipCategory relationshipCategory)
        {
            return this.relationshipCategoryRepository.InsertRelationshipCategory(relationshipCategory);
        }

        public RelationshipCategory UpdateRelationshipCategory(RelationshipCategory relationshipCategory)
        {
            return this.relationshipCategoryRepository.UpdateRelationshipCategory(relationshipCategory);
        }

        public void DeleteRelationshipCategory(int relationshipCategoryId)
        {
            this.relationshipCategoryRepository.DeleteRelationshipCategory(relationshipCategoryId);
        }
    }
}
