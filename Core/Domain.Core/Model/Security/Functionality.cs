namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WebApp.Functionality")]
    public partial class Functionality : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Functionality()
        {
            AccessControlLevels = new HashSet<AccessControlLevel>();
            Menus = new HashSet<Menu>();
        }

        public int FunctionalityId { get; set; }

        [Required]
        [StringLength(100)]
        public string FunctionalityName { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessControlLevel> AccessControlLevels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Menu> Menus { get; set; }

        public object Clone()
        {
            var clonedFunctionality = new Functionality();

            clonedFunctionality.FunctionalityId = this.FunctionalityId;
            clonedFunctionality.FunctionalityName = this.FunctionalityName;
            clonedFunctionality.IsActive = this.IsActive;

            return clonedFunctionality;
        }
    }
}