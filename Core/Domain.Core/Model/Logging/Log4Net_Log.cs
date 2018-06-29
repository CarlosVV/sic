namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WebApp.Log4Net_Log")]
    public partial class Log4Net_Log
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(255)]
        public string Thread { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Level { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(255)]
        public string Logger { get; set; }

        [StringLength(50)]
        public string User { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(4000)]
        public string Message { get; set; }

        [StringLength(2000)]
        public string Exception { get; set; }

        [StringLength(255)]
        public string MachineName { get; set; }

        public int? ApplicationId { get; set; }

        [StringLength(100)]
        public string IpAddress { get; set; }

        [StringLength(100)]
        public string PageUrl { get; set; }

        [StringLength(100)]
        public string ReferrerUrl { get; set; }
    }
}
