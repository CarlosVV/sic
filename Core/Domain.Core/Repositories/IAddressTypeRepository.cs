namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IAddressTypeRepository : IRepository<AddressType>
    {
        IEnumerable<AddressType> GetAddressTypes();
        AddressType InsertAddressType(AddressType addressType);
        AddressType UpdateAddressType(AddressType addressType);
        void DeleteAddressType(int addressTypeId);
    }
}