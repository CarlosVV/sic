namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface IAddressTypeService
    {
        IEnumerable<AddressType> GetAddressTypes();

        IEnumerable<AddressType> GetAddressTypesAll();

        AddressType InsertAddressType(AddressType addressType);

        AddressType UpdateAddressType(AddressType addressType);

        void DeleteAddressType(int addressTypeId);
    }
}