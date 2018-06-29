namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.AddressType")]
    public class AddressType : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AddressType()
        {
            Addresses = new HashSet<Address>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AddressTypeId { get; set; }

        [Column("AddressType")]
        [Required]
        [StringLength(30)]
        public string AddressType1 { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        public object Clone()
        {
            AddressType clonedAddressType = new AddressType();

            clonedAddressType.AddressTypeId = this.AddressTypeId;
            clonedAddressType.AddressType1 = this.AddressType1;
            clonedAddressType.Hidden = this.Hidden;
            clonedAddressType.CreatedBy = this.CreatedBy;
            clonedAddressType.CreatedDateTime = this.CreatedDateTime;
            clonedAddressType.ModifiedBy = this.ModifiedBy;
            clonedAddressType.ModifiedDateTime = this.ModifiedDateTime;

            return clonedAddressType;
        }
    }
}