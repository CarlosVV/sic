namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Core.Helpers;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public class TransferTypeRepository : EfRepository<TransferType>, ITransferTypeRepository
    {
        public IEnumerable<TransferType> GetTransferTypes()
        {
            return this.Context.TransferTypes.OrderBy(m => m.TransferTypeId);
        }

        public TransferType InsertTransferType(TransferType transferType)
        {
            transferType.TransferTypeId = CreateNewId();
            transferType.CreatedBy = WebHelper.GetUserName();
            transferType.CreatedDateTime = DateTime.Now;
            this.Context.TransferTypes.Add(transferType);
            this.Context.SaveChanges();
            return transferType;
        }

        public TransferType UpdateTransferType(TransferType transferType)
        {
            var transferTypeToUpdate = this.Context.TransferTypes.Find(transferType.TransferTypeId);
            transferTypeToUpdate.TransferType1 = transferType.TransferType1;
            transferTypeToUpdate.Hidden = transferType.Hidden;
            transferTypeToUpdate.ModifiedBy = WebHelper.GetUserName();
            transferTypeToUpdate.ModifiedDateTime = DateTime.Now;
            this.Context.Entry(transferTypeToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return transferTypeToUpdate;
        }

        public void DeleteTransferType(int transferTypeId)
        {
            var transferType = this.Context.TransferTypes.Find(transferTypeId);
            this.Context.TransferTypes.Remove(transferType);
            this.Context.SaveChanges();
        }

        private int CreateNewId()
        {
			var transferTypeList = from d in this.Context.TransferTypes orderby d.TransferTypeId descending select d;
			return transferTypeList.FirstOrDefault().TransferTypeId + 1;
		}
    }
}