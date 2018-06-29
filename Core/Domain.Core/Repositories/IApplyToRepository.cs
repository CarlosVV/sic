namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using Domain.Core.Model;

    #endregion

    public interface IApplyToRepository : IRepository<ApplyTo>
    {
        ApplyTo InsertApplyTo(ApplyTo applyTo);

        ApplyTo UpdateApplyTo(ApplyTo applyTo);

        void DeleteApplyTo(int applyToId);
    }
}