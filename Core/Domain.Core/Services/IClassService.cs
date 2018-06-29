namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IClassService
    {
        IEnumerable<Class> GetClasses();

        Class InsertClass(Class _class);

        Class UpdateClass(Class _class);

        void DeleteClass(int classId);
    }
}