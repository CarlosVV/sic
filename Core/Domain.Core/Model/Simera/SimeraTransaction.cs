namespace Nagnoi.SiC.Domain.Core.Model {

    #region Imports

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    #endregion
    [Serializable]
    [Table("Simera.Transaction")]
    public class SimeraTransaction {
        public decimal? TransactionId { get; set; }
         [Key, Column(Order = 0)]
        public int CaseDetailId { get; set; }
         [Key, Column(Order = 1), StringLength(15)]
        public string CaseNumber { get; set; }
         [StringLength(11)]
        public string SSN { get; set; }
         [StringLength(200)]
        public string FullName { get; set; }
         [StringLength(11)]
        public string SSN_Beneficiary { get; set; }
        public DateTime? BirthDate { get; set; }
         [StringLength(60)]
        public string Relationship { get; set; }
         [StringLength(200)]
        public string FullName_Beneficiary { get; set; }
        public DateTime? Date { get; set; }
        public decimal? AdvancePayment { get; set; }
         [StringLength(80)]
        public string AdjustmentReason { get; set; }
         [StringLength(80)]
        public string ClosingReason { get; set; }
        public DateTime? DeceaseDate { get; set; }
        public decimal? ExcessCompPaid { get; set; }
        public decimal? OtherDiscounts { get; set; }
        public decimal? MonthlyAdjustment { get; set; }
        public decimal? ReserveAdjustment { get; set; }
        public decimal? Remainder { get; set; }
        public decimal? InitialPayment { get; set; }
        public string TransactionType { get; set; }
        public string RequestCreatedByUser { get; set; }
         [StringLength(15)]
        public string StatusSIC { get; set; }
         [StringLength(15)]
        public string StatusSIMERA { get; set; }
         [StringLength(15)]
        public string Action { get; set; }
        public decimal? Retroactive { get; set; }
         [StringLength(1)]
        public string ProcessType { get; set; }
        public string ErrorMessage { get; set; }
         [StringLength(150)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
         [StringLength(150)]
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public decimal? S_ROWID { get; set; }
         [StringLength(20)]
        public string DocumentId { get; set; }
         [Key, Column(Order = 2), StringLength(2)]
        public string CaseKey { get; set; }
        public decimal? Amount { get; set; }
    }
}
