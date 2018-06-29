namespace Nagnoi.SiC.Infrastructure.Core.Data
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}