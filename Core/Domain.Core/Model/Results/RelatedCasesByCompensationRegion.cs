using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Nagnoi.SiC.Domain.Core.Model
{
    public class RelatedCasesByCompensationRegion
    {

        public int? CaseId { get; set; }
        public int? CaseDetailId { get; set; }
        public int? TransactionId { get; set; }
        public int? TransactionDetailId { get; set; }
        public int? CompensationRegionId { get; set; }

        [StringLength(11)]
        public string CaseNumber { get; set; }

        [StringLength(2)]
        public string CaseKey { get; set; }

        [StringLength(30)]
        public string Region { get; set; }

        [StringLength(30)]
        public string SubRegion { get; set; }

        [StringLength(3)]
        public string Code { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? Percent { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        public bool? IsRelated { get; set; }
    }
}
