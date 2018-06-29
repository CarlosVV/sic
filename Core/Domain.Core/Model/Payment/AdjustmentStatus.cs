namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Payment.AdjustmentStatus")]
    public partial class AdjustmentStatus : AuditableEntity, ISoftDeletable, ICloneable
    {
        public AdjustmentStatus()
        {
            Payments = new HashSet<Payment>();
        }

        public int AdjustmentStatusId { get; set; }

        [Column("AdjustmentStatus")]
        public string AdjustmentStatus1 { get; set; }

        public bool? Hidden { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public object Clone()
        {
            return new AdjustmentStatus
            {
                AdjustmentStatusId = this.AdjustmentStatusId,
                AdjustmentStatus1 = this.AdjustmentStatus1,
                Hidden = this.Hidden,
                ModifiedBy = this.ModifiedBy,
                ModifiedDateTime = this.ModifiedDateTime,
                CreatedBy = this.CreatedBy,
                CreatedDateTime = this.CreatedDateTime
            };
        }
    }
}