namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Data.Entity;
    using System.Linq;
    using Core.Helpers;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class ConceptRepository : EfRepository<Concept>, IConceptRepository
    {
        public Concept InsertConcept(Concept Concept)
        {
            Concept.ConceptId = CreateNewId();           
            Concept.CreatedBy = WebHelper.GetUserName();
            Concept.CreatedDateTime = DateTime.Now;
            Concept.ModifiedBy = WebHelper.GetUserName();
            Concept.ModifiedDateTime = DateTime.Now;
            this.Context.Concepts.Add(Concept);
            this.Context.SaveChanges();
            return Concept;
        }

        public Concept UpdateConcept(Concept Concept)
        {
            var ConceptUpdate = this.Context.Concepts.Find(Concept.ConceptId);
            ConceptUpdate.Concept1 = Concept.Concept1;
            ConceptUpdate.ConceptCode = Concept.ConceptCode;
            ConceptUpdate.ConceptType = Concept.ConceptType;
            ConceptUpdate.Hidden = Concept.Hidden;
            ConceptUpdate.ModifiedBy = WebHelper.GetUserName();
            ConceptUpdate.ModifiedDateTime = DateTime.Now;
            this.Context.Entry(ConceptUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return ConceptUpdate;
        }

        public void DeleteConcept(int ConceptId)
        {
            var concept = this.Context.Concepts.Find(ConceptId);
            this.Context.Concepts.Remove(concept);
            this.Context.SaveChanges();
        }

        private int CreateNewId()
        {
            var ConceptList = from d in this.Context.Concepts orderby d.ConceptId descending select d;
            return ConceptList.FirstOrDefault().ConceptId + 1;
        }
    }
}