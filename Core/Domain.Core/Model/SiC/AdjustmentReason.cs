namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("SiC.AdjustmentReason")]
    public class AdjustmentReason : AuditableEntity, ISoftDeletable, ICloneable
    {
        #region Constructor

        public AdjustmentReason()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        #endregion

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdjustmentReasonId { get; set; }

        [Column("AdjustmentReason")]
        [StringLength(50)]
        public string AdjustmentReason1 { get; set; }

        public bool? Hidden { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public object Clone()
        {
            var clonedAdjustmentReason = new AdjustmentReason();
            clonedAdjustmentReason.AdjustmentReasonId = this.AdjustmentReasonId;
            clonedAdjustmentReason.AdjustmentReason1 = this.AdjustmentReason1;
            clonedAdjustmentReason.Hidden = this.Hidden;
            clonedAdjustmentReason.CreatedBy = this.CreatedBy;
            clonedAdjustmentReason.CreatedDateTime = this.CreatedDateTime;
            clonedAdjustmentReason.ModifiedBy = this.ModifiedBy;
            clonedAdjustmentReason.ModifiedDateTime = this.ModifiedDateTime;

            return clonedAdjustmentReason;
        }
    }
}