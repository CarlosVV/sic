namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    [Table("Entity.RelationshipCategory")]
    public partial class RelationshipCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RelationshipCategory()
        {
            RelationshipTypes = new HashSet<RelationshipType>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RelationshipCategoryId { get; set; }

        [Column("RelationshipCategory")]
        [Required]
        [StringLength(50)]
        public string RelationshipCategory1 { get; set; }

        public int? ApplyToId { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public virtual ApplyTo ApplyTo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RelationshipType> RelationshipTypes { get; set; }
    }
}
