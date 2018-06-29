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

    public sealed class RelationshipTypeRepository : EfRepository<RelationshipType>, IRelationshipTypeRepository 
    {
        public IEnumerable<RelationshipType> GetRelationshipTypes()
        {
            return this.Context.RelationshipTypes.OrderBy(m => m.RelationshipTypeId);
        }
                                
        public RelationshipType InsertRelationshipType(RelationshipType relationshipType)
        {

            relationshipType.RelationshipTypeId = CreateNewId();
            relationshipType.CreatedBy = WebHelper.GetUserName();
            relationshipType.CreatedDateTime = DateTime.Now;
            this.Context.RelationshipTypes.Add(relationshipType);
            this.Context.SaveChanges();
            return relationshipType;
        }

        public RelationshipType UpdateRelationshipType(RelationshipType relationshipType)
        {
            var relationshipTypeToUpdate = this.Context.RelationshipTypes.Find(relationshipType.RelationshipTypeId);
            relationshipTypeToUpdate.RelationshipType1 = relationshipType.RelationshipType1;
            relationshipTypeToUpdate.Handicapped = relationshipType.Handicapped;
            relationshipTypeToUpdate.Hidden = relationshipType.Hidden;
            relationshipTypeToUpdate.ModifiedBy = WebHelper.GetUserName();
            relationshipTypeToUpdate.ModifiedDateTime = DateTime.Now;
            relationshipTypeToUpdate.RelationshipTypeCode = relationshipType.RelationshipTypeCode;
            relationshipTypeToUpdate.SchoolCertification = relationshipType.SchoolCertification;
            relationshipTypeToUpdate.VitalData = relationshipType.VitalData;
            relationshipTypeToUpdate.WidowCertification = relationshipType.WidowCertification;
            relationshipTypeToUpdate.WithChildren = relationshipType.WithChildren;

            this.Context.Entry(relationshipTypeToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return relationshipTypeToUpdate;
        }

        public void DeleteRelationshipType(int relationshipTypeId)
        {
            var relationshipType = this.Context.RelationshipTypes.Find(relationshipTypeId);
            this.Context.RelationshipTypes.Remove(relationshipType);
            this.Context.SaveChanges();
        }

        private int CreateNewId()
        {
            var relationshipTypeList = from d in this.Context.RelationshipTypes orderby d.RelationshipTypeId descending select d;
            return relationshipTypeList.FirstOrDefault().RelationshipTypeId + 1;
        }
    }
}