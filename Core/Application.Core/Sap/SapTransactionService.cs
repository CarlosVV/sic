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

    public sealed class SapTransactionService : ISapTransactionService {

        #region Private Memebers

        private IRepository<SapTransaction> sapTransaction = null;        
        private readonly IUnitOfWork unitOfWork = null;

        #endregion

        #region Constructors

        public SapTransactionService(): 
            this(IoC.Resolve<IRepository<SapTransaction>>(),            
            IoC.Resolve<IUnitOfWork>()) 
        { }

        public SapTransactionService(IRepository<SapTransaction> sapTransaction,            
            IUnitOfWork unitOfWork)
        {
            this.sapTransaction = sapTransaction;            
            this.unitOfWork = unitOfWork;
        }

        #endregion

        public void CreateSapTransaction(SapTransaction entity) {
            sapTransaction.Insert(entity);
            unitOfWork.SaveChanges();
        }

        public void CreateBulkSapTransaction(IEnumerable<SapTransaction> entities) {
            foreach (var item in entities) {
                sapTransaction.Insert(item);
            }
            unitOfWork.SaveChanges();
        }
    }
}
