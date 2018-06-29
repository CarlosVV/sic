namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IProfileRepository : IRepository<Profile>
    {
        Profile GetById(int functionalityId);
    }
}