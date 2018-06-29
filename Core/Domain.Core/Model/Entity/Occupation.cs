using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Domain.Core.Model
{
    [Table("Entity.Occupation")]
    public class Occupation
    {
        public int OccupationId { get; set; }

        [Column("Occupation")]
        public string Occupation1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entity> Entity { get; set; }
    }
}
