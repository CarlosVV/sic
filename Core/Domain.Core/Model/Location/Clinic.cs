namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Location.Clinic")]
    public class Clinic : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clinic()
        {
            Payments = new HashSet<Payment>();
            CaseClinics = new HashSet<Case>();
            CaseClinicServices = new HashSet<Case>();
        }

        public int ClinicId { get; set; }

        [Column("Clinic")]
        [Required]
        [StringLength(80)]
        public string Clinic1 { get; set; }

        [StringLength(2)]
        public string ClinicCode { get; set; }

        public int? RegionId { get; set; }

        [NotMapped]
        [StringLength(10)]
        public string CostCenter { get; set; }
        [NotMapped]
        [StringLength(8)]
        public string FundCenter { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public virtual Region Region { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> CaseClinics { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> CaseClinicServices { get; set; }

        public object Clone()
        {
            return new Clinic()
            {
                ClinicId = this.ClinicId,
                Clinic1 = this.Clinic1,
                ClinicCode = this.ClinicCode,
                RegionId = this.RegionId,
                Region = this.Region,
                CreatedBy = this.CreatedBy,
                CreatedDateTime = this.CreatedDateTime,
                ModifiedBy = this.ModifiedBy,
                ModifiedDateTime = this.ModifiedDateTime
            };
        }
    }
}