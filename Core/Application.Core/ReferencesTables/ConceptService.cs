namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Exceptions;
    using Infrastructure.Core.Helpers;

    #endregion

    public sealed class ConceptService : IConceptService
    {
        #region Constants

        private const string DataCacheKey = "Nagnoi.Concept";

        #endregion       

        #region Private Methods

        private readonly IConceptRepository conceptRepository = null;
        
        private readonly IRepository<MonthlyConcept> monthlyConceptRepository = null;

        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructors

        public ConceptService()
            : this(
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<IConceptRepository>(),
            IoC.Resolve<IRepository<MonthlyConcept>>()) { }

        internal ConceptService(
            ICacheManager gestorCache,
            IConceptRepository ConceptRepository,
            IRepository<MonthlyConcept> monthlyconceptRepository)
        {
            this.cacheManager = gestorCache; 
            this.conceptRepository = ConceptRepository;
            this.monthlyConceptRepository = monthlyconceptRepository;
        }

        #endregion

        #region Public Methods

        public IEnumerable<Concept> GetAllConcepts()
        {
            IEnumerable<Concept> result;

            if (this.cacheManager.IsAdded(DataCacheKey)) {
                result = this.cacheManager.Get(DataCacheKey) as IEnumerable<Concept>;

                return result.Clone();
            }

            result = this.conceptRepository.GetAll().OrderBy(c => c.ConceptId).ToList();

            this.cacheManager.Add(DataCacheKey, result);

            return result.Clone();
        }

        public Concept GetConceptByCode(string ConceptCode) {
            return this.GetAllConcepts().Where(p => p.ConceptCode == ConceptCode).FirstOrDefault();
        }

        public Concept InsertConcept(Concept concept)
        {
            return this.conceptRepository.InsertConcept(concept);
        }

        public Concept UpdateConcept(Concept concept)
        {
            return this.conceptRepository.UpdateConcept(concept);
        }

        public void DeleteConcept(int conceptId)
        {
            this.conceptRepository.DeleteConcept(conceptId);
        }

        public MonthlyConcept GetByConceptAndYear(string conceptName, string conceptType, int year)
        {
            var concept = this.GetAllConcepts().Where(p => p.Concept1.Equals(conceptName, StringComparison.OrdinalIgnoreCase) &&
                                                           p.ConceptType.Equals(conceptType, StringComparison.OrdinalIgnoreCase))
                                               .FirstOrDefault();
            if (concept.IsNull())
            {
                throw new BusinessException("Payment Concept no encontrado");
            }
            
            return monthlyConceptRepository.FindOne(p => p.ConceptId == concept.ConceptId && p.Year <= year);
        }
        
        #endregion
    }
}