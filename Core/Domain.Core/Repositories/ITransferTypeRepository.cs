namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ITransferTypeRepository : IRepository<TransferType>
    {
		IEnumerable<TransferType> GetTransferTypes();

		TransferType InsertTransferType(TransferType transferType);

		TransferType UpdateTransferType(TransferType transferType);

		void DeleteTransferType(int transferTypeId);
	}
}