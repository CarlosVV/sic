namespace Nagnoi.SiC.Domain.Core.Repositories
{
    public interface IAspNetLogEntryRepository
    {
        Model.ILogEntry SelectById(string logEntryId);
    }
}