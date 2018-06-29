using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nagnoi.SiC.Domain.Core.Model
{
    [Serializable]
    [Table("SiC.CompensationRegion")]
    public partial class CompensationRegion : ICloneable
    {
        public CompensationRegion()
        {
            TransactionDetails = new HashSet<TransactionDetail>();
        }

        public int CompensationRegionId { get; set; }

        [StringLength(3)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int Weeks { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string SubRegion { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public ICollection<TransactionDetail> TransactionDetails { get; set; }

        public object Clone()
        {
            var clonedCompensationRegion = new CompensationRegion();

            clonedCompensationRegion.CompensationRegionId = this.CompensationRegionId;
            clonedCompensationRegion.Code = this.Code;
            clonedCompensationRegion.Description = this.Description;
            clonedCompensationRegion.Weeks = this.Weeks;
            clonedCompensationRegion.Region = this.Region;
            clonedCompensationRegion.SubRegion = this.SubRegion;
            clonedCompensationRegion.Hidden = this.Hidden;
            clonedCompensationRegion.CreatedBy = this.CreatedBy;
            clonedCompensationRegion.CreatedDateTime = this.CreatedDateTime;
            clonedCompensationRegion.ModifiedBy = this.ModifiedBy;
            clonedCompensationRegion.ModifiedDateTime = this.ModifiedDateTime;

            return clonedCompensationRegion;
        }
    }
}