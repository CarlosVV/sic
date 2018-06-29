namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class LogRepository : EfRepository<Log>, ILogRepository
    {
        
    }
}