namespace Nagnoi.SiC.Application.Core
{
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

    public class ModifiedReasonService : IModifiedReasonService
    {

        #region Private Members

        private readonly IUnitOfWork unitOfWork = null;

        public readonly IRepository<ModifiedReason> repositoryModifiedReason = null;

        #endregion

        #region Constructors

        public ModifiedReasonService()
            : this(
                IoC.Resolve<IUnitOfWork>(),
                IoC.Resolve<IRepository<ModifiedReason>>())
        { }

        internal ModifiedReasonService(
            IUnitOfWork unitOfWork,
            IRepository<ModifiedReason> repositoryModifiedReason)
        {
            this.repositoryModifiedReason = repositoryModifiedReason;
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methdods

        public IEnumerable<ModifiedReason> GetAll()
        {
            return this.repositoryModifiedReason.GetAll();
        }

        #endregion
    }
}
