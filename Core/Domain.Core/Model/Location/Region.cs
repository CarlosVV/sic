namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Location.Region")]
    public class Region : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Region()
        {
            Clinics = new HashSet<Clinic>();
            Payments = new HashSet<Payment>();
            CaseRegions = new HashSet<Case>();
            CaseRegionServices = new HashSet<Case>();
        }

        public int RegionId { get; set; }

        [Column("Region")]
        [Required]
        [StringLength(80)]
        public string Region1 { get; set; }

        [StringLength(2)]
        public string RegionCode { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clinic> Clinics { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> CaseRegions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> CaseRegionServices { get; set; }

        public object Clone()
        {
            Region clonedRegion = new Region();

            clonedRegion.RegionId = this.RegionId;
            clonedRegion.Region1 = this.Region1;
            clonedRegion.RegionCode = this.RegionCode;
            clonedRegion.Hidden = this.Hidden;
            clonedRegion.CreatedBy = this.CreatedBy;
            clonedRegion.CreatedDateTime = this.CreatedDateTime;
            clonedRegion.ModifiedBy = this.ModifiedBy;
            clonedRegion.ModifiedDateTime = this.ModifiedDateTime;

            return clonedRegion;
        }
    }
}