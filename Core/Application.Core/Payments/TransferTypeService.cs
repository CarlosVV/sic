namespace Nagnoi.SiC.Application.Core
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;
    using Nagnoi.SiC.Domain.Core.Services;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;

    #endregion

    public class TransferTypeService : ITransferTypeService 
    {
        #region Miembros Privados

        private readonly ITransferTypeRepository transferTypeRepository = null;

        #endregion

        #region Constructores

        public TransferTypeService()
            : this(IoC.Resolve<ITransferTypeRepository>())
        { }

        internal TransferTypeService(ITransferTypeRepository transferTypeRepository)
        {
            this.transferTypeRepository = transferTypeRepository;
        }

        #endregion
        public IEnumerable<TransferType> GetTransferTypes() {
            return this.transferTypeRepository.GetTransferTypes();
        }

        public TransferType InsertTransferType(TransferType transferType) {
            return this.transferTypeRepository.InsertTransferType(transferType);
        }

        public TransferType UpdateTransferType(TransferType transferType) {
            return this.transferTypeRepository.UpdateTransferType(transferType);
        }

        public void DeleteTransferType(int transferTypeId) {
            this.transferTypeRepository.DeleteTransferType(transferTypeId);
        }
    }
}
 