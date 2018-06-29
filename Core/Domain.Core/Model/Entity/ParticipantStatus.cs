namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.ParticipantStatus")]
    public partial class ParticipantStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParticipantStatus()
        {
            Entities = new HashSet<Entity>();
        }

        [Key]
        public int ParticipantStatusId { get; set; }

        [Required]
        [StringLength(1)]
        public string ParticipantStatusCode { get; set; }

        [Required]
        [Column("ParticipantStatus")]
        [StringLength(50)]
        public string ParticipantStatus1 { get; set; }

        [Required]
        [StringLength(50)]
        public string ParticipantStatusCategory { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entity> Entities { get; set; }
    }
}
