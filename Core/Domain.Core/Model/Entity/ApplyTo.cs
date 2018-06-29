namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    [Table("Entity.ApplyTo")]
    public class ApplyTo : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApplyTo()
        {
            RelationshipCategories = new HashSet<RelationshipCategory>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApplyToId { get; set; }

        [Column("ApplyTo")]
        [Required]
        [StringLength(50)]
        public string ApplyTo1 { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RelationshipCategory> RelationshipCategories { get; set; }

        public object Clone()
        {
            return new ApplyTo()
            {
                ApplyToId = this.ApplyToId,
                ApplyTo1 = this.ApplyTo1,
                Hidden = this.Hidden,
                ModifiedBy = this.ModifiedBy,
                CreatedBy = this.CreatedBy
            };
        }
    }
}