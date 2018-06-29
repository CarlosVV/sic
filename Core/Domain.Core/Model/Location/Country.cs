namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Location.Country")]
    public class Country : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            Addresses = new HashSet<Address>();
            States = new HashSet<State>();
        }

        public int CountryId { get; set; }

        [Column("Country")]
        [Required]
        [StringLength(100)]
        public string Country1 { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        [StringLength(2)]
        public string CountryKey { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<State> States { get; set; }

        public object Clone()
        {
            Country clonedCountry = new Country();

            clonedCountry.CountryId = this.CountryId;
            clonedCountry.Country1 = this.Country1;
            clonedCountry.Hidden = this.Hidden;
            clonedCountry.CreatedBy = this.CreatedBy;
            clonedCountry.CreatedDateTime = this.CreatedDateTime;
            clonedCountry.ModifiedBy = this.ModifiedBy;
            clonedCountry.ModifiedDateTime = this.ModifiedDateTime;

            return clonedCountry;
        }
    }
}