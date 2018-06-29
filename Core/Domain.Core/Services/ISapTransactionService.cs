namespace Nagnoi.SiC.Domain.Core.Services {

    #region References

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;

    #endregion

    public interface ISapTransactionService {

        void CreateSapTransaction(SapTransaction entity);

        void CreateBulkSapTransaction(IEnumerable<SapTransaction> entities);
    }
}
