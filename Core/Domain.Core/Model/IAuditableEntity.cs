namespace Nagnoi.SiC.Domain.Core.Model
{
    #region References

    using System;

    #endregion

    public interface IAuditableEntity
    {
        DateTime? CreatedDateTime { get; set; }

        string CreatedBy { get; set; }

        DateTime? ModifiedDateTime { get; set; }

        string ModifiedBy { get; set; }
    }
}