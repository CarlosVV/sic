namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WebApp.Profile")]
    public partial class Profile : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profile()
        {
            AccessControlLevels = new HashSet<AccessControlLevel>();
        }

        public int ProfileId { get; set; }

        [Required]
        [StringLength(30)]
        public string ProfileName { get; set; }

        [StringLength(100)]
        public string ProfileDescription { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessControlLevel> AccessControlLevels { get; set; }

        public object Clone()
        {
            var clonedProfile = new Profile();

            clonedProfile.ProfileId = this.ProfileId;
            clonedProfile.ProfileName = this.ProfileName;
            clonedProfile.ProfileDescription = this.ProfileDescription;

            return clonedProfile;
        }
    }
}