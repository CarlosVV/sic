namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Location.State")]
    public class State : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public State()
        {
            Cities = new HashSet<City>();
            Addresses = new HashSet<Address>();
        }

        public int StateId { get; set; }

        [Column("State")]
        [Required]
        [StringLength(100)]
        public string State1 { get; set; }

        [Required]
        [StringLength(2)]
        public string Abbreviation { get; set; }

        public int? CountryId { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<City> Cities { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        public object Clone()
        {
            State clonedState = new State();

            clonedState.StateId = this.StateId;
            clonedState.State1 = this.State1;
            clonedState.Abbreviation = this.Abbreviation;
            clonedState.CountryId = this.CountryId;
            clonedState.Hidden = this.Hidden;
            clonedState.CreatedBy = this.CreatedBy;
            clonedState.CreatedDateTime = this.CreatedDateTime;
            clonedState.ModifiedBy = this.ModifiedBy;
            clonedState.ModifiedDateTime = this.ModifiedDateTime;

            return clonedState;
        }
    }
}