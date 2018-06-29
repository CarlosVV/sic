using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Domain.Core.Model
{
    [Serializable]
    public class CompensationRegion2
    {

        public CompensationRegion2()
        {
            
        }
        public int CompensationRegionId { get; set; }

        [StringLength(3)]
        public string Code { get; set; }

        [StringLength(75)]
        public string CodeDescription { get; set; }

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

    }
}



