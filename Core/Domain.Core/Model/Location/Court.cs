namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Location.Court")]
    public class Court : ICloneable
    {
        public int CourtId { get; set; }

        [StringLength(180)]
        public string CourtName { get; set; }

        public decimal? S_ROWID { get; set; }

        [StringLength(30)]
        public string AddressLine1 { get; set; }

        [StringLength(30)]
        public string AddressLine2 { get; set; }

        [StringLength(30)]
        public string City { get; set; }

        [StringLength(35)]
        public string Region { get; set; }

        [StringLength(5)]
        public string ZipCode { get; set; }

        [StringLength(4)]
        public string ZipCodeExt { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }
        public object Clone()
        {
            return new Court()
            {
                CourtId = this.CourtId,
                CourtName = this.CourtName,
                AddressLine1 = this.AddressLine1,
                AddressLine2 = this.AddressLine2,
                City = this.City,
                Region = this.Region,
                ZipCode = this.ZipCode,
                ZipCodeExt = this.ZipCodeExt
            };
        }
    }
}