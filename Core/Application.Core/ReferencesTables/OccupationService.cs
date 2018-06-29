namespace Nagnoi.SiC.Application.Core{
    #region References

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;
using Nagnoi.SiC.Infrastructure.Core.Data;

    #endregion

    public class OccupationService : IOccupationService {

        #region Private Members

        private readonly IUnitOfWork unitOfWork = null;

        public readonly IRepository<Occupation> repositoryOccupation = null;

        #endregion

        #region Constructors

        public OccupationService() : this(      
            IoC.Resolve<IUnitOfWork>(),
            IoC.Resolve<IRepository<Occupation>>())
        { }

        internal OccupationService(            
            IUnitOfWork unitOfWork,
            IRepository<Occupation> repositoryOccupation)
        {
            this.repositoryOccupation = repositoryOccupation;            
            this.unitOfWork = unitOfWork;            
        }

        #endregion

        #region Public Methdods

        public IEnumerable<Occupation> GetAll() {
            return this.repositoryOccupation.GetAll();
        }

        #endregion
    }
}
