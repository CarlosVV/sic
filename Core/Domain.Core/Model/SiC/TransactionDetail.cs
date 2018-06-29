namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("SiC.TransactionDetail")]
    public partial class TransactionDetail : AuditableEntity, ISoftDeletable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionDetailId { get; set; }

        public int? TransactionId { get; set; }

        public int? CompensationRegionId { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? Percent { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        public bool? Hidden { get; set; }
        
        public Transaction Transaction { get; set; }

        public CompensationRegion CompensationRegion { get; set; }
    }
}