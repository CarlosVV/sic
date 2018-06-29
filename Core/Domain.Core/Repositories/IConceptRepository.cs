namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using Model;

    #endregion

    public interface IConceptRepository : IRepository<Concept>
    {
        Concept InsertConcept(Concept Concept);

        Concept UpdateConcept(Concept Concept);

        void DeleteConcept(int ConceptId);
    }
}