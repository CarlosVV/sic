namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ITransactionTypeRepository : IRepository<TransactionType>
    {
        IEnumerable<TransactionType> GetTransactionTypes();

        TransactionType InsertTransactionType(TransactionType TransactionType);

        TransactionType UpdateTransactionType(TransactionType TransactionType);

        void DeleteTransactionType(int TransactionTypeId);
    }
}