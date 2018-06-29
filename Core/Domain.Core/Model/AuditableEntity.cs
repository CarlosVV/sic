namespace Nagnoi.SiC.Domain.Core.Model
{
    #region References

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    [Serializable]
    public abstract class AuditableEntity : IAuditableEntity
    {
        [Column(TypeName = "datetime")]
        public virtual DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public virtual string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ModifiedDateTime { get; set; }

        [StringLength(150)]
        public virtual string ModifiedBy { get; set; }
    }
}