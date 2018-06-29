namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Payment.ThirdPartySchedule")]
    public partial class ThirdPartySchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThirdPartySchedule()
        {
            Payments = new HashSet<Payment>();
        }
        public int ThirdPartyScheduleId { get; set; }

        public int? CaseId { get; set; }

        public int? CaseDetailId { get; set; }

        public int? EntityId_RemitTo { get; set; }

        [StringLength(30)]
        public string ClaimNumber { get; set; }

        [StringLength(30)]
        public string OrderIdentifier { get; set; }

        public bool? TerminationFlag { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [StringLength(30)]
        public string TerminationOrderNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal? SinglePaymentAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? FirstInstallmentAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? SecondInstallmentAmount { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [Column(TypeName = "money")]
        public decimal? OrderAmount { get; set; }

        public DateTime? TerminationDate { get; set; }

        public virtual Entity Remitter { get; set; }

        public virtual Case Case { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public virtual CaseDetail CaseDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}