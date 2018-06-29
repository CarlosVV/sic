namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.ParticipantType")]
    public class ParticipantType : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParticipantType()
        {
            Entities = new HashSet<Entity>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ParticipantTypeId { get; set; }

        [Column("ParticipantType")]
        [Required]
        [StringLength(50)]
        public string ParticipantType1 { get; set; }

        public bool Status { get; set; }

        public bool AllowsPayment { get; set; }

        public bool RequiresSSN { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entity> Entities { get; set; }

        public object Clone()
        {
            ParticipantType clonedParticipantType = new ParticipantType();

            clonedParticipantType.ParticipantTypeId = this.ParticipantTypeId;
            clonedParticipantType.ParticipantType1 = this.ParticipantType1;
            clonedParticipantType.Status = this.Status;
            clonedParticipantType.AllowsPayment = this.AllowsPayment;
            clonedParticipantType.RequiresSSN = this.RequiresSSN;
            clonedParticipantType.Hidden = this.Hidden;
            clonedParticipantType.CreatedBy = this.CreatedBy;
            clonedParticipantType.CreatedDateTime = this.CreatedDateTime;
            clonedParticipantType.ModifiedBy = this.ModifiedBy;
            clonedParticipantType.ModifiedDateTime = this.ModifiedDateTime;

            return clonedParticipantType;
        }
    }
}