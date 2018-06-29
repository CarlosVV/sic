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

    public sealed class TransactionTypeRepository : EfRepository<TransactionType>, ITransactionTypeRepository
    {
        public IEnumerable<TransactionType> GetTransactionTypes()
        {
            return this.Context.TransactionTypes.OrderBy(m => m.TransactionTypeId);
        }

        public TransactionType InsertTransactionType(TransactionType transactionType)
        {
            transactionType.TransactionTypeId = CreateNewId();
            transactionType.CreatedBy = WebHelper.GetUserName();
            transactionType.CreatedDateTime = DateTime.Now;
            this.Context.TransactionTypes.Add(transactionType);
            this.Context.SaveChanges();
            return transactionType;
        }

        public TransactionType UpdateTransactionType(TransactionType transactionType)
        {
            var transactionTypeToUpdate = this.Context.TransactionTypes.Find(transactionType.TransactionTypeId);
            transactionTypeToUpdate.TransactionType1 = transactionType.TransactionType1;
            transactionTypeToUpdate.Hidden = transactionType.Hidden;
            transactionTypeToUpdate.ModifiedBy = WebHelper.GetUserName();
            transactionTypeToUpdate.ModifiedDateTime = DateTime.Now;
            this.Context.Entry(transactionTypeToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return transactionTypeToUpdate;
        }

        public void DeleteTransactionType(int transactionTypeId)
        {
            var transactionType = this.Context.TransactionTypes.Find(transactionTypeId);
            this.Context.TransactionTypes.Remove(transactionType);
            this.Context.SaveChanges();
        }

        private int CreateNewId()
        {
            var transactionTypeList = from d in this.Context.TransactionTypes orderby d.TransactionTypeId descending select d;
            return transactionTypeList.FirstOrDefault().TransactionTypeId + 1;
        }
    }
}