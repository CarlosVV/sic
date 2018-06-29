namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    [Table("Payment.Status")]
    public partial class Status : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Status()
        {
            Payments = new HashSet<Payment>();
        }

        public int StatusId { get; set; }

        [StringLength(1)]
        public string StatusCode { get; set; }

        [Column("Status")]
        [StringLength(50)]
        public string Status1 { get; set; }

        [StringLength(1)]
        public string Effect { get; set; }

        public bool? Hidden { get; set; }

        [StringLength(1)]
        public string SAPStatusCode { get; set; }

        [StringLength(15)]
        public string SAPStatus { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        public object Clone()
        {
            return new Status
            {
                StatusId = this.StatusId,
                StatusCode = this.StatusCode,
                Status1 = this.Status1,
                Effect = this.Effect,
                Hidden = this.Hidden,
                SAPStatus = this.SAPStatus,
                SAPStatusCode = this.SAPStatusCode,
                CreatedBy = this.CreatedBy,
                CreatedDateTime = this.CreatedDateTime,
                ModifiedBy = this.ModifiedBy,
                ModifiedDateTime = this.ModifiedDateTime
            };
        }
    }
}