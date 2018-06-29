namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias
    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;
    #endregion

    public interface IGenderService
    {
        IEnumerable<Gender> GetGender();

        Gender InsertGender(Gender genderType);

        Gender UpdateGender(Gender genderType);

        void DeleteGender(int genderTypeId);
    }
}