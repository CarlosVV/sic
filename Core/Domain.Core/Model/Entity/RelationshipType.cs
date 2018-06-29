namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.RelationshipType")]
    public partial class RelationshipType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RelationshipType()
        {
            CaseDetailEntities = new HashSet<CaseDetail>();
        }

        public int RelationshipTypeId { get; set; }

        [Column("RelationshipType")]
        [Required]
        [StringLength(50)]
        public string RelationshipType1 { get; set; }

        [StringLength(1)]
        public string RelationshipTypeCode { get; set; }

        public bool? SchoolCertification { get; set; }

        public bool? WidowCertification { get; set; }

        public bool? VitalData { get; set; }

        public bool? Handicapped { get; set; }

        public bool? WithChildren { get; set; }

        public int? RelationshipCategoryId { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }
        
        public virtual RelationshipCategory RelationshipCategory { get; set; }

        public virtual ICollection<CaseDetail> CaseDetailEntities { get; set; }
    }
}
