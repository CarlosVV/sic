namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Payment.Payment")]
    public partial class PaymentBalance
    {
        public string CaseNumber { get; set; }

        public string CaseKey { get; set; }

        public decimal? Amount { get; set; }

        public int? CaseId { get; set; }

        public int? CaseDetailId { get; set; }

        public int? ConceptId { get; set; }

        public int? ClassId { get; set; }

        public virtual Concept Concept { get; set; }
    }
}