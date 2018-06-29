namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System.Collections.Generic;
    using Domain.Core.Model;

    #endregion

    public interface IApplyToService
    {
        IEnumerable<ApplyTo> GetApplyTosAll();

        ApplyTo InsertApplyTo(ApplyTo applyTo);

        ApplyTo UpdateApplyTo(ApplyTo applyTo);

        void DeleteApplyTo(int applyToId);
    }
}