namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Serializable]
    [Table("SiC.Case")]
    public partial class Case
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Case()
        {
            Payments = new HashSet<Payment>();
            CaseDetails = new HashSet<CaseDetail>();
            Transactions = new HashSet<Transaction>();
        }

        public int CaseId { get; set; }

        [StringLength(15)]
        public string CaseNumber { get; set; }

        [StringLength(11)]
        public string PolicyNo { get; set; }

        public int? CaseFolderId { get; set; }

        [StringLength(10)]
        public string CaseFolder { get; set; }

        public int? DietBK { get; set; }

        public int? IncaBk { get; set; }

        public int? CheckCaseBK { get; set; }

        public DateTime? CaseDate { get; set; }

        public DateTime? AccidentDate { get; set; }

        public DateTime? TreatmentRestDate { get; set; }

        public DateTime? TreatmentWorkDate { get; set; }

        [StringLength(75)]
        public string EmployerName { get; set; }

        [StringLength(9)]
        public string EmployerEIN { get; set; }

        public int? EmployerStatusId { get; set; }

        [StringLength(50)]
        public string TreatmentStatus { get; set; }

        [StringLength(50)]
        public string DischargeStatus { get; set; }

        public int? CompensationId { get; set; }

        public int? KeyRiskIndicatorId { get; set; }

        public int? ConceptId { get; set; }

        public int? ClinicId { get; set; }

        public int? RegionId { get; set; }

        public int? ClinicId_Service { get; set; }

        public int? RegionId_Service { get; set; }

        [Column(TypeName = "money")]
        public decimal? DailyWage { get; set; }

        public int? DaysWeek { get; set; }

        [Column(TypeName = "money")]
        public decimal? WeeklyComp { get; set; }

        [Column(TypeName = "money")]
        public decimal? WeeklyCompDisability { get; set; }

        [Column(TypeName = "money")]
        public int? DaysPaid { get; set; }

        [StringLength(200)]
        public string CompOfficer { get; set; }

        public bool? DeathFlag { get; set; }

        public DateTime? DeceaseDate { get; set; }

        public bool? IsMinor { get; set; }

        public int? CityId_Accident { get; set; }

        public int? Age { get; set; }

        public DateTime? LastPaymentDate { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }
        
        //public virtual ActiveIdent ActiveIdent { get; set; }

       // public virtual ActiveOnOff ActiveOnOff { get; set; }
        
        public virtual Clinic Clinic { get; set; }

        public virtual Clinic ClinicService { get; set; }

        public virtual Region Region { get; set; }

        public virtual Region RegionService { get; set; }

        public virtual Concept Concept { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual Compensation Compensation { get; set; }

        public virtual EmployerStatus EmployerStatus { get; set; }

        public virtual KeyRiskIndicator KeyRiskIndicator { get; set; }

        //public int? WeeksPaid { get; set; }
    }
}