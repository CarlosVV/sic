namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("SiC.Compensation")]
    public partial class Compensation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Compensation()
        {
            Cases = new HashSet<Case>();
        }

        public int CompensationId { get; set; }

        [StringLength(6)]
        public string CompensationKey { get; set; }

        [StringLength(2)]
        public string CompensationKey1 { get; set; }

        [StringLength(2)]
        public string CompensationKey2 { get; set; }

        [StringLength(2)]
        public string CompensationKey3 { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Concept { get; set; }

        [StringLength(100)]
        public string Determination { get; set; }

        [StringLength(100)]
        public string Movement { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
