namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WebApp.AccessControlLevel")]
    public partial class AccessControlLevel : ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccessControlLevelId { get; set; }

        public int FunctionalityId { get; set; }

        public int ProfileId { get; set; }

        public bool Allow { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public virtual Functionality Functionality { get; set; }

        public virtual Profile Profile { get; set; }

        public object Clone()
        {
            var clonedAcl = new AccessControlLevel();

            clonedAcl.AccessControlLevelId = this.AccessControlLevelId;
            clonedAcl.FunctionalityId = this.FunctionalityId;
            clonedAcl.ProfileId = this.ProfileId;
            clonedAcl.Allow = this.Allow;
            clonedAcl.Functionality = this.Functionality.Clone() as Functionality;
            clonedAcl.Profile = this.Profile.Clone() as Profile;

            return clonedAcl;
        }
    }
}