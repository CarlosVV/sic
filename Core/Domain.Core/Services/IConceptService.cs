namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System.Collections.Generic;
    using Model;

    #endregion

    public interface IConceptService
    {
        IEnumerable<Concept> GetAllConcepts();

        Concept InsertConcept(Concept concept);

        Concept UpdateConcept(Concept concept);

        void DeleteConcept(int conceptId);

        MonthlyConcept GetByConceptAndYear(string conceptName, string conceptType, int year);

        Concept GetConceptByCode(string ConceptCode);
    }
}