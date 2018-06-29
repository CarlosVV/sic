namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("SiC.Transaction")]
    public partial class Transaction : AuditableEntity, ISoftDeletable
    {
        public Transaction()
        {
            TransactionDetails = new HashSet<TransactionDetail>();
            Payments = new HashSet<Payment>();
        }

        public int TransactionId { get; set; }

        public int? CaseDetailId { get; set; }

        [NotMapped]
        public string TransactionNumber
        {
            get
            {
                return TransactionId.ToString().PadLeft(9, '0');
            }
        }
        
        public int? TransactionTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? TransactionAmount { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? TransactionDate { get; set; }

        public int? CaseId_Reference { get; set; }

        [StringLength(15)]
        public string CaseNumber_Reference { get; set; }

        public int? ConceptId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DecisionDate { get; set; }

        [StringLength(25)]
        public string InvoiceNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal? MonthlyInstallment { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? NumberOfWeeks { get; set; }

        [StringLength(250)]
        public string RejectedReason { get; set; }

        public int? AdjustmentReasonId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public string ICCaseNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NotificationDateIC { get; set; }

        [Column(TypeName = "date")]
        public DateTime? HearingDateIC { get; set; }
        
        public bool? Hidden { get; set; }

        public virtual Case CaseReference { get; set; }

        public virtual TransactionType TransactionType { get; set; }

        public virtual CaseDetail CaseDetail { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }

        public virtual Concept Concept { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual AdjustmentReason AdjustmentReason { get; set; }

        public int? SourceId { get; set; }
    }
}