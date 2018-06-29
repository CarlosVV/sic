namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System.Collections.Generic;
    using Domain.Core.Model;

    #endregion

    public interface IModifiedReasonService
    {

        #region ModifiedReason

        IEnumerable<ModifiedReason> GetAll();

        #endregion
    }
}
