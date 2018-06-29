using Nagnoi.SiC.Domain.Core.Model;
using System.Collections.Generic;

namespace Nagnoi.SiC.Domain.Core.Services
{
    public interface IParticipantTypeService
    {

        #region ModifiedReason

        IEnumerable<ParticipantType> GetAll();

        #endregion
    }
}