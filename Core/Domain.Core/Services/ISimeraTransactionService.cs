namespace Nagnoi.SiC.Domain.Core.Services {
    
    #region Imports

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ISimeraTransactionService {
        void InsertSimeraTransaction(SimeraTransaction transaction);
        void InsertTransactions(SimeraTransaction simeratransaction, Transaction sictransaction);
    }
}
