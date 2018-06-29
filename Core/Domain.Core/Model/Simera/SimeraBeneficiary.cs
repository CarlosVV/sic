namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    [Table("Simera.Beneficiary")]
    public partial class SimeraBeneficiary
    {
        public int? EntityId { get; set; }

        [Key, Column(Order = 0), StringLength(11)]
        public string CaseNumber { get; set; }

        [Key, Column(Order = 1), StringLength(2)]
        public string CaseKey { get; set; }

        [StringLength(200)]
        public string FullName { get; set; }

        [StringLength(50)]
        //[Required]
        public string RelationshipType { get; set; }

        [StringLength(1)]
        public string RelationshipTypeCode { get; set; }

        public bool? HasDisability { get; set; }

        public bool? HasWidowCertification { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? Age { get; set; }

        [StringLength(25)]
        public string CivilStatus { get; set; }

        [StringLength(25)]
        public string Ocupation { get; set; }

        public Decimal? MonthlyIncome { get; set; }
    }
}
