namespace Nagnoi.SiC.Application.Core {

    #region Imports

    using System.Collections.Generic;
    using System.Linq;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;
    using Nagnoi.SiC.Domain.Core.Services;
    using Nagnoi.SiC.Infrastructure.Core.Caching;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using Nagnoi.SiC.Infrastructure.Core.Data;
    #endregion

    public sealed class SimeraTransactionService : ISimeraTransactionService {

        #region Private Memebers

        private IRepository<SimeraTransaction> simeraTransaction = null;
        private IRepository<Transaction> sicTransaction = null;
        private readonly IUnitOfWork unitOfWork = null;

        #endregion

        #region Constructors

        public SimeraTransactionService(): 
            this(IoC.Resolve<IRepository<SimeraTransaction>>(),
            IoC.Resolve<IRepository<Transaction>>(), 
            IoC.Resolve<IUnitOfWork>()) 
        { }

        public SimeraTransactionService(IRepository<SimeraTransaction> simeraTransaction,
            IRepository<Transaction> sicTransaction, 
            IUnitOfWork unitOfWork)
        {
            this.simeraTransaction = simeraTransaction;
            this.sicTransaction = sicTransaction;
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public void InsertSimeraTransaction(SimeraTransaction transaction) {
            simeraTransaction.Insert(transaction);
            unitOfWork.SaveChanges();
        }

        public void InsertTransactions(SimeraTransaction simeratransaction, Transaction sictransaction)
        {
            sicTransaction.Insert(sictransaction);
            //Update TransactionId on Simera.Transaction from Sic.Transaction
            simeratransaction.TransactionId = sictransaction.TransactionId;
            simeraTransaction.Insert(simeratransaction);
            //Save all entities in the same DatabaseTransactionScope
            unitOfWork.SaveChanges();
        }

        #endregion
    }
}
