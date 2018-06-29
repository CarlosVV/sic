using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Domain.Core.Model
{
    [Table("Entity.ModifiedReason")]
    public class ModifiedReason
    {
        [Key]
        [Column("ModifiedReasonId")]
        public int ModifiedReasonId { get; set; }

        [Column("ModifiedReason")]
        [StringLength(250)]
        public string ModifiedReason1 { get; set; }

        [Column("Hidden")]
        public bool? Hidden { get; set; }

        [Column("CreatedBy")]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        [Column("CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [Column("ModifiedBy")]
        [StringLength(150)]
        public string ModifiedBy {get;set;}

        [Column("ModifiedDateTime")]
        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entity> Entity { get; set; }
    }
}