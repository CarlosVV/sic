namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias
    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ITransferTypeService { 
	   IEnumerable<TransferType> GetTransferTypes();
	   TransferType InsertTransferType(TransferType transferType);
	   TransferType UpdateTransferType(TransferType transferType);
	   void DeleteTransferType(int transferTypeId);
	} 
}