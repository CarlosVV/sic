namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.Gender")]
    public class Gender : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Gender()
        {
            Entities = new HashSet<Entity>();
            Masters = new HashSet<Master>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GenderId { get; set; }

        [StringLength(1)]
        public string GenderCode { get; set; }

        [Column("Gender")]
        [StringLength(10)]
        public string Gender1 { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entity> Entities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Master> Masters { get; set; }

        public object Clone()
        {
            Gender clonedGender = new Gender();

            clonedGender.GenderId = this.GenderId;
            clonedGender.GenderCode = this.GenderCode;
            clonedGender.Hidden = this.Hidden;
            clonedGender.CreatedBy = this.CreatedBy;
            clonedGender.CreatedDateTime = this.CreatedDateTime;
            clonedGender.ModifiedBy = this.ModifiedBy;
            clonedGender.ModifiedDateTime = this.ModifiedDateTime;

            return clonedGender;
        }
    }
}