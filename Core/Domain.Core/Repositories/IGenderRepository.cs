namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IGenderRepository : IRepository<Gender>
    {
        IEnumerable<Gender> GetGender();

        Gender InsertGender(Gender gender);

        Gender UpdateGender(Gender gender);

        void DeleteGender(int genderId);
    }
}