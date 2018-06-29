namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Core.Helpers;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class AddressTypeRepository : EfRepository<AddressType>, IAddressTypeRepository
    {
        public IEnumerable<AddressType> GetAddressTypes()
        {
            return this.Context.AddressTypes.OrderBy(m => m.AddressTypeId);
        }

        public AddressType InsertAddressType(AddressType addressType)
        {
            addressType.AddressTypeId = CreateNewId();
            addressType.CreatedBy = WebHelper.GetUserName();
            addressType.CreatedDateTime = DateTime.Now;
            this.Context.AddressTypes.Add(addressType);
            this.Context.SaveChanges();
            return addressType;
        }

        public AddressType UpdateAddressType(AddressType addressType)
        {
            var addressTypeToUpdate = this.Context.AddressTypes.Find(addressType.AddressTypeId);
            addressTypeToUpdate.AddressType1 = addressType.AddressType1;
            addressTypeToUpdate.Hidden = addressType.Hidden;
            addressTypeToUpdate.ModifiedBy = WebHelper.GetUserName();
            addressTypeToUpdate.ModifiedDateTime = DateTime.Now;
            this.Context.Entry(addressTypeToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return addressTypeToUpdate;
        }

        public void DeleteAddressType(int addressTypeId)
        {
            var addressType = this.Context.AddressTypes.Find(addressTypeId);
            this.Context.AddressTypes.Remove(addressType);
            this.Context.SaveChanges();
        }

        private int CreateNewId()
        {
            var addressTypeList = from d in this.Context.AddressTypes orderby d.AddressTypeId descending select d;
            return addressTypeList.FirstOrDefault().AddressTypeId + 1;
        }
    }
}