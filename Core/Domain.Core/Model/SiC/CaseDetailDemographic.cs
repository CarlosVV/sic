using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Domain.Core.Model
{
    [Serializable]
    public class CaseDetailDemographic
    {
        public int CaseDetailId { get; set; }

        public int? EntityId { get; set; }

        public int? RelationshipTypeId { get; set; }

        [StringLength(25)]
        public string OtherRelationshipType { get; set; }

        public int? EntityId_LegalGuardian { get; set; }

        public int? EntityId_Lawyer { get; set; }

        public string Source { get; set; } 

        public int Priority { get; set; }
    }
}
