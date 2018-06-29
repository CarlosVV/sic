namespace Nagnoi.SiC.Application.Core
{
    #region Referencias

    using System.Collections.Generic;
    using System.Linq;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;
    using Nagnoi.SiC.Domain.Core.Services;
    using Nagnoi.SiC.Infrastructure.Core.Caching;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    #endregion

    public sealed class SimeraBeneficiaryService : ISimeraBeneficiaryService
    {
        #region Private Members
        ISimeraBeneficiaryRepository repository = null;
        #endregion 

        #region Constructors

        public SimeraBeneficiaryService() : this(
            IoC.Resolve<ISimeraBeneficiaryRepository>())
        { }

        internal SimeraBeneficiaryService(
            ISimeraBeneficiaryRepository repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Public Methods

        public SimeraBeneficiary InsertSimeraBeneficiary(SimeraBeneficiary simeraBeneficiary)
        {
            return this.repository.InsertSimeraBeneficiary(simeraBeneficiary);
        }

        public void DeleteSimeraBeneficiary(SimeraBeneficiary simeraBeneficiary)
        {
            SimeraBeneficiary entity = this.repository.Find(x => x.CaseNumber == simeraBeneficiary.CaseNumber 
                && x.CaseKey == simeraBeneficiary.CaseKey).First();
            this.repository.DeleteSimeraBeneficiary(entity);
        }

        public void DeleteCaseBeneficiaries(string caseNumber)
        {
            this.repository.DeleteCaseBeneficiaries(caseNumber);
        }
        
        #endregion
    }
}
