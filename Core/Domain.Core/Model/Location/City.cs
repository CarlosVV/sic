namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Location.City")]
    public class City : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public City()
        {
            Addresses = new HashSet<Address>();
        }

        public int CityId { get; set; }

        [Column("City")]
        [Required]
        [StringLength(100)]
        public string City1 { get; set; }

        public int? StateId { get; set; }

        public int? CountryId { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        public virtual State State { get; set; }

        public object Clone()
        {
            return new City()
            {
                CityId = this.CityId,
                City1 = this.City1,
                CountryId = this.CountryId,
                Addresses = this.Addresses,
                CreatedBy = this.CreatedBy,
                CreatedDateTime = this.CreatedDateTime,
                ModifiedBy = this.ModifiedBy,
                ModifiedDateTime = this.ModifiedDateTime,
                State = this.State,
                StateId = this.StateId,
                Hidden = this.Hidden
            };
        }
    }
}