namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using Model;

    #endregion

    public interface IClassRepository : IRepository<Class>
    {
        Class InsertClass(Class _class);

        Class UpdateClass(Class _class);

        void DeleteClass(int classId);
    }
}