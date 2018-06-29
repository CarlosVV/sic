namespace Nagnoi.SiC.Domain.Core.Model {

    #region Imports

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    #endregion

    [Serializable]
    [Table("Sap.Transaction")]
    public partial class SapTransaction : AuditableEntity {
        [Key, Column(Order = 0)]
        public int TransactionId { get; set; }
        
        [Column(TypeName="xml")]
        public string TransactionDetail { get; set; }        
    }
}
