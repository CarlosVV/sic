namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.Address")]
    public class Address : ICloneable
    {
        public int AddressId { get; set; }

        public int EntityId { get; set; }

        public long EntityBk { get; set; }

        public int SourceId { get; set; }

        public int? AddressBk { get; set; }

        [StringLength(50)]
        public string UniqueHashId { get; set; }

        [StringLength(230)]
        public string FullAddress { get; set; }

        [StringLength(60)]
        public string Line1 { get; set; }

        [StringLength(60)]
        public string Line2 { get; set; }

        public int? CityId { get; set; }

        [StringLength(50)]
        public string OtherCity { get; set; }

        public int? StateId { get; set; }

        public int? CountryId { get; set; }

        [StringLength(5)]
        public string ZipCode { get; set; }

        [StringLength(4)]
        public string ZipCodeExt { get; set; }

        public int? AddressTypeId { get; set; }

        [StringLength(20)]
        public string XCord { get; set; }

        [StringLength(20)]
        public string YCord { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(2)]
        public string DeliverPl { get; set; }

        [StringLength(1)]
        public string CkDig { get; set; }

        [StringLength(8)]
        public string Letterman { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        [StringLength(50)]
        public string ETLFingerprint { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public virtual AddressType AddressType { get; set; }

        public virtual Entity Entity { get; set; }

        public virtual City City { get; set; }

        public virtual Country Country { get; set; }

        public virtual State State { get; set; }

        public object Clone()
        {
            Address clonedAddress = new Address();

            clonedAddress.AddressId = this.AddressId;
            clonedAddress.EntityId = this.EntityId;
            clonedAddress.EntityBk = this.EntityBk;
            clonedAddress.SourceId = this.SourceId;
            clonedAddress.AddressBk = this.AddressBk;
            clonedAddress.UniqueHashId = this.UniqueHashId;
            clonedAddress.FullAddress = this.FullAddress;
            clonedAddress.Line1 = this.Line1;
            clonedAddress.Line2 = this.Line2;
            clonedAddress.CityId = this.CityId;
            clonedAddress.StateId = this.StateId;
            clonedAddress.CountryId = this.CountryId;
            clonedAddress.ZipCode = this.ZipCode;
            clonedAddress.ZipCodeExt = this.ZipCodeExt;
            clonedAddress.AddressTypeId = this.AddressTypeId;
            clonedAddress.XCord = this.XCord;
            clonedAddress.YCord = this.YCord;
            clonedAddress.Status = this.Status;
            clonedAddress.DeliverPl = this.DeliverPl;
            clonedAddress.CkDig = this.CkDig;
            clonedAddress.Letterman = this.Letterman;
            clonedAddress.Deleted = this.Deleted;
            clonedAddress.DeletedDate = this.DeletedDate;
            clonedAddress.ETLFingerprint = this.ETLFingerprint;
            clonedAddress.CreatedBy = this.CreatedBy;
            clonedAddress.CreatedDateTime = this.CreatedDateTime;
            clonedAddress.ModifiedBy = this.ModifiedBy;
            clonedAddress.ModifiedDateTime = this.ModifiedDateTime;

            if (this.City != null)
            {
                clonedAddress.City = (City)this.City.Clone();
            }

            if (this.Country != null)
            {
                clonedAddress.Country = (Country)this.Country.Clone();
            }

            if (this.State != null)
            {
                clonedAddress.State = (State)this.State.Clone();
            }

            if (this.AddressType != null)
            {
                clonedAddress.AddressType = (AddressType)this.AddressType.Clone();
            }

            return clonedAddress;
        }
    }
}