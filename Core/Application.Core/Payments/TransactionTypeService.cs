namespace Nagnoi.SiC.Application.Core
{
    #region Referencias

    using System.Collections.Generic;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Dependencies;

    #endregion

    public class TransactionTypeService : ITransactionTypeService
    {
        #region Miembros Privados

        private readonly ITransactionTypeRepository transactionTypeRepository = null;

        #endregion

        #region Constructores

        public TransactionTypeService() : this(
            IoC.Resolve<ITransactionTypeRepository>())
        { }

        internal TransactionTypeService(ITransactionTypeRepository transactionTypeRepository)
        {
            this.transactionTypeRepository = transactionTypeRepository;
        }

        #endregion

        public IEnumerable<TransactionType> GetTransactionTypes()
        {
            return this.transactionTypeRepository.GetTransactionTypes();
        }

        public TransactionType InsertTransactionType(TransactionType transactionType)
        {
            return this.transactionTypeRepository.InsertTransactionType(transactionType);
        }

        public TransactionType UpdateTransactionType(TransactionType transactionType)
        {
            return this.transactionTypeRepository.UpdateTransactionType(transactionType);
        }

        public void DeleteTransactionType(int transactionTypeId)
        {
            this.transactionTypeRepository.DeleteTransactionType(transactionTypeId);
        }
    }
}