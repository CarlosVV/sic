namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ITransactionTypeService {
        IEnumerable<TransactionType> GetTransactionTypes();
        TransactionType InsertTransactionType(TransactionType TransactionType);
        TransactionType UpdateTransactionType(TransactionType TransactionType);
        void DeleteTransactionType(int TransactionTypeId);
    }
}
