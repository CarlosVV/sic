namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.CivilStatus")]
    public class CivilStatus : ICloneable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CivilStatusId { get; set; }

        [Column("CivilStatus")]
        [StringLength(25)]        
        public string CivilStatus1 { get; set; }

        [StringLength(1)]
        public string CivilStatusCode { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public object Clone()
        {
            CivilStatus clonedCivilStatus = new CivilStatus();

            clonedCivilStatus.CivilStatusId = this.CivilStatusId;
            clonedCivilStatus.CivilStatus1 = this.CivilStatus1;
            clonedCivilStatus.CivilStatusCode = this.CivilStatusCode;
            clonedCivilStatus.Hidden = this.Hidden;
            clonedCivilStatus.CreatedBy = this.CreatedBy;
            clonedCivilStatus.CreatedDateTime = this.CreatedDateTime;
            clonedCivilStatus.ModifiedBy = this.ModifiedBy;
            clonedCivilStatus.ModifiedDateTime = this.ModifiedDateTime;

            return clonedCivilStatus;
        }
    }
}