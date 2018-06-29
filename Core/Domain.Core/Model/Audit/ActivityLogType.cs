namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WebApp.ActivityLogType")]
    public partial class ActivityLogType : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ActivityLogType()
        {
            ActivityLogs = new HashSet<ActivityLog>();
        }

        public int ActivityLogTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string SystemKeyword { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}
