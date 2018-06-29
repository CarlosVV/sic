namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Payment.Concept")]
    public class Concept : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Concept()
        {
            MonthlyConcepts = new HashSet<MonthlyConcept>();
            Payments = new HashSet<Payment>();
            Cases = new HashSet<Case>();
            Transactions = new HashSet<Transaction>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConceptId { get; set; }

        [StringLength(2)]
        public string ConceptCode { get; set; }

        [Column("Concept")]
        [StringLength(50)]
        public string Concept1 { get; set; }

        [StringLength(50)]
        public string ConceptType { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonthlyConcept> MonthlyConcepts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual bool HasDeathBenefit { get; set; }

        [StringLength(2)]
        public string DocumentType { get; set; }
        [StringLength(10)]
        public string ReserveJournalEntryDebit { get; set; }
        [StringLength(10)]
        public string GLAccountDebit { get; set; }
        [StringLength(10)]
        public string GLAccountCredit { get; set; }
        [StringLength(24)]
        public string CommitmentItem { get; set; }
        [StringLength(10)]
        public string FundCurrentYear { get; set; }
        [StringLength(10)]
        public string FundPreviousYear { get; set; }
        [StringLength(16)]
        public string FunctionalArea { get; set; }

        public object Clone()
        {
            return new Concept()
            {
                ConceptId = this.ConceptId,
                Concept1 = this.Concept1,
                ConceptCode = this.ConceptCode,
                ConceptType = this.ConceptType,
                Hidden = this.Hidden,
                ModifiedBy = this.ModifiedBy,
                ModifiedDateTime = this.ModifiedDateTime,
                CreatedBy = this.CreatedBy,
                CreatedDateTime = this.CreatedDateTime,
                HasDeathBenefit = this.HasDeathBenefit,
                DocumentType = this.DocumentType,
                ReserveJournalEntryDebit = this.ReserveJournalEntryDebit,
                GLAccountCredit = this.GLAccountCredit,
                GLAccountDebit = this.GLAccountDebit,
                CommitmentItem = this.CommitmentItem,
                FundCurrentYear = this.FundCurrentYear,
                FundPreviousYear = this.FundPreviousYear,
                FunctionalArea = this.FunctionalArea
            };
        }
    }
}