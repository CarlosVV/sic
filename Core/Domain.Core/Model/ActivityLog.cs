namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WebApp.ActivityLog")]
    public partial class ActivityLog
    {
        public int ActivityLogId { get; set; }

        public int ObjectTypeId { get; set; }

        [StringLength(20)]
        public string ObjectId { get; set; }

        public int ActivityLogTypeId { get; set; }

        [StringLength(4000)]
        public string Comment { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public virtual ActivityLogType ActivityLogType { get; set; }

        public virtual ObjectType ObjectType { get; set; }
    }
}
