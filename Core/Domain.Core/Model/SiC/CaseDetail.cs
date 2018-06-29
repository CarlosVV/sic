namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Serializable]
    [Table("SiC.CaseDetail")]
    public class CaseDetail
    {
        public CaseDetail()
        {
            Transactions = new HashSet<Transaction>();
            Payments = new HashSet<Payment>();
            ThirdPartySchedules = new HashSet<ThirdPartySchedule>();
        }


        public int CaseDetailId { get; set; }
        
        public int? CaseId { get; set; }

        [StringLength(11)]
        public string CaseNumber { get; set; }

        [StringLength(2)]
        public string CaseKey { get; set; }

        [StringLength(9)]
        public string EBTAccount { get; set; }

        public int? CaseFolderId { get; set; }

        [StringLength(10)]
        public string CaseFolder { get; set; }

        public int? DietBk { get; set; }

        public int? IncaBK { get; set; }

        public int? CheckCaseBK { get; set; }

        public int? EntityId { get; set; }

        public int? RelationshipTypeId { get; set; }

        [StringLength(25)]
        public string OtherRelationshipType { get; set; }

        public int? EntityId_LegalGuardian { get; set; }

        public int? EntityId_Lawyer { get; set; }

        public int? EntityId_Sif { get; set; }

        public int? EntityId_Sic { get; set; }

        public int? EntityId_Diet { get; set; }

        public int? EntityId_Inca { get; set; }

        public int? EntityId_Inca_Beneficiary { get; set; }

        public int? EntityId_Inca_LegalGuardian { get; set; }

        public int? EntityId_Check { get; set; }
        
        public int? TransferTypeId { get; set; }

        public int? CancellationId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CancellationDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? DeductedMonthly { get; set; }
        
        [Column(TypeName = "money")]
        public decimal? LegacyAmountPaid_Diet { get; set; }

        [Column(TypeName = "money")]
        public decimal? LegacyAmountPaid_Inca { get; set; }

        [Column(TypeName = "money")]
        public decimal? MonthlyInstallment { get; set; }

        [Column(TypeName = "money")]
        public decimal? Reserve { get; set; }

        public bool? Hidden { get; set; }

        [Column(TypeName = "money")]
        public decimal? EBTBalance { get; set; }

        public string EBTStatus { get; set; }

        public DateTime? ProcessDate { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public virtual Case Case { get; set; }

        public virtual Entity Entity { get; set; }

        public virtual Entity EntityInca { get; set; }

        public virtual Entity EntityCheck { get; set; }

        public virtual Entity EntityDiet { get; set; }

        public virtual Entity EntityLawyer { get; set; }

        public virtual Entity EntityLegalGuardian { get; set; }

        public virtual Entity EntitySif { get; set; }

        public virtual Entity EntitySic { get; set; }

        public virtual TransferType TransferType { get; set; }

        public virtual Cancellation Cancellation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        public virtual RelationshipType RelationshipType { get; set; }

        public virtual ICollection<ThirdPartySchedule> ThirdPartySchedules { get; set; }

        public DateTime? RestartDate { get; set; }

        public string ActiveOnOff { get; set; }

        public string ActiveIdent { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            CaseDetail p = obj as CaseDetail;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (CaseNumber == p.CaseNumber) && (CaseKey == p.CaseKey);
        }

        public decimal? GetInitialAllocation()
        {
            var firstTransaction = this.Transactions.Where(t => t.TransactionTypeId == 1)
                                                    .FirstOrDefault();
            if (firstTransaction == null)
            {
                return null;
            }

            return firstTransaction.TransactionAmount.GetValueOrDefault(decimal.Zero);
        }
    }
}