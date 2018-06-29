namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("SiC.CaseRelationship")]
    public partial class CaseRelationship
    {
        public int CaseRelationshipId { get; set; }

        public int CaseId1 { get; set; }

        [Required]
        [StringLength(15)]
        public string CaseNumber1 { get; set; }

        public int CaseId2 { get; set; }

        [Required]
        [StringLength(15)]
        public string CaseNumber2 { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }
    }
}
