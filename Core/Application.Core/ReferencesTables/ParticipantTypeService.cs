using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Domain.Core.Repositories;
using Nagnoi.SiC.Domain.Core.Services;
using Nagnoi.SiC.Infrastructure.Core.Data;
using Nagnoi.SiC.Infrastructure.Core.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Application.Core
{
    public class ParticipantTypeService : IParticipantTypeService
    {
        #region Private Members

        private readonly IUnitOfWork unitOfWork = null;

        public readonly IRepository<ParticipantType> repositoryParticipantType = null;

        #endregion


        #region Constructors

        public ParticipantTypeService()
            : this(
                IoC.Resolve<IUnitOfWork>(),
                IoC.Resolve<IRepository<ParticipantType>>())
        { }

        internal ParticipantTypeService(
            IUnitOfWork unitOfWork,
            IRepository<ParticipantType> repositoryParticipantType)
        {
            this.repositoryParticipantType = repositoryParticipantType;
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methdods

        public IEnumerable<ParticipantType> GetAll()
        {
            return this.repositoryParticipantType.GetAll();
        }

        #endregion

    }
}
