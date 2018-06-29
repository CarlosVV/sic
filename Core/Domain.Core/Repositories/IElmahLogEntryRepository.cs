using System;

namespace Nagnoi.SiC.Domain.Core.Repositories
{
    public interface IElmahLogEntryRepository
    {
        Model.ILogEntry SelectById(Guid guid);
    }
}