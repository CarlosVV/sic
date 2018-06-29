namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References
    
    using Infrastructure.Core.Data;
    using Infrastructure.Core.Dependencies;

    #endregion

    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseContext context = null;

        public EfUnitOfWork()
        {
            context = IoC.Resolve<IDatabaseContext>();
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}