namespace Nagnoi.SiC.Domain.Core.Repositories
{
    public interface ILog4NetLogEntryRepository
    {
        Model.ILogEntry SelectById(int idAsInt);
    }
}