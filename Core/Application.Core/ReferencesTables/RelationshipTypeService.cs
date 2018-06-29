using System.Collections.Generic;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Domain.Core.Repositories;
using Nagnoi.SiC.Domain.Core.Services;
using Nagnoi.SiC.Infrastructure.Core.Dependencies;

namespace Nagnoi.SiC.Application.Core
{
    public class RelationshipTypeService : IRelationshipTypeService
    {
        #region Private Members

        private readonly IRelationshipTypeRepository relationshipTypeRepository = null;

        #endregion

        #region Constructors
               
        public RelationshipTypeService() 
            : this(IoC.Resolve<IRelationshipTypeRepository>())
        { }

        internal RelationshipTypeService(IRelationshipTypeRepository relationshipTypeRepository)
        {
            this.relationshipTypeRepository = relationshipTypeRepository;
        }

        #endregion

        public IEnumerable<RelationshipType> GetRelationshipTypes()
        {
            return this.relationshipTypeRepository.GetRelationshipTypes();
        }

        public RelationshipType InsertRelationshipType(RelationshipType relationshipType)
        {
            return this.relationshipTypeRepository.InsertRelationshipType(relationshipType);
        }

        public RelationshipType UpdateRelationshipType(RelationshipType relationshipType)
        {
            return this.relationshipTypeRepository.UpdateRelationshipType(relationshipType);
        }

        public void DeleteRelationshipType(int relationshipTypeId)
        {
            this.relationshipTypeRepository.DeleteRelationshipType(relationshipTypeId);
        }


  
    }
}
