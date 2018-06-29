namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Payment.MonthlyConcept")]
    public partial class MonthlyConcept
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MonthlyConceptId { get; set; }

        public int ConceptId { get; set; }

        [Column(TypeName = "money")]
        public decimal? MonthlyPayment { get; set; }

        [Column(TypeName = "money")]
        public decimal? Maximum { get; set; }

        [Column(TypeName = "money")]
        public decimal? Advance { get; set; }

        public int? Year { get; set; }

        public int? RelationshipTypeId { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public virtual Concept Concept { get; set; }
    }
}