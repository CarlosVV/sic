namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Payment.Payment")]
    public partial class Payment : AuditableEntity
    {
        public int PaymentId { get; set; }

        public int? TransactionId { get; set; }

        public int? CheckBk { get; set; }

        [StringLength(2)]
        public string CaseKey { get; set; }

        [StringLength(11)]
        public string CaseNumber { get; set; }

        [StringLength(9)]
        public string TransactionNum { get; set; }

        [Column(TypeName = "money")]
        public decimal? BaseAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? Discount { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        public short? PaymentDay { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public DateTime? StatusChangeDate { get; set; }

        public int? CaseId { get; set; }

        public int? CaseDetailId { get; set; }

        public int? ClinicId { get; set; }

        public int? RegionId { get; set; }

        public int? EntityId_RemitTo { get; set; }

        public int? KeyRiskIndicatorId { get; set; }

        public int? ConceptId { get; set; }

        public int? ClassId { get; set; }

        public int? StatusId { get; set; }

        public int? TransferTypeId { get; set; }

        public int? AdjustmentStatusId { get; set; }

        public string AdjustmentType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AdjustmentRequestedDate { get; set; }

        public string AdjustmentRequestedBy { get; set; }

        public string AdjustmentCompletedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AdjustmentCompletedDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdjustmentAmount { get; set; }

        public string Comments { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public int? ThirdPartyScheduleId { get; set; }
        [NotMapped]
        public bool? Hidden { get; set; }
        public virtual ThirdPartySchedule ThirdPartySchedule { get; set; }

        public virtual Entity Remitter { get; set; }

        public virtual Clinic Clinic { get; set; }

        public virtual Region Region { get; set; }

        public virtual Class Class { get; set; }

        public virtual Concept Concept { get; set; }

        public virtual Case Case { get; set; }

        public virtual CaseDetail CaseDetail { get; set; }

        public virtual KeyRiskIndicator KeyRiskIndicator { get; set; }

        public virtual Status Status { get; set; }

        public virtual TransferType TransferType { get; set; }

        public virtual Transaction Transaction { get; set; }

        public virtual AdjustmentStatus AdjustmentStatus { get; set; }

    }
}