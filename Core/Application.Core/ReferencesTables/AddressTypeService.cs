namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;

    #endregion

    public class AddressTypeService : IAddressTypeService
    {
        #region Constantes

        private const string DataCacheKey = "Nagnoi.AddressType";

        #endregion

        #region Miembros Privados

        private readonly IAddressTypeRepository addressTypeRepository = null;

        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructores

        public AddressTypeService()
            : this(
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<IAddressTypeRepository>()
            )
        { }

        internal AddressTypeService(ICacheManager gestorCache, IAddressTypeRepository addressTypeRepository)
        {
            this.cacheManager = gestorCache;
            this.addressTypeRepository = addressTypeRepository;
        }

        #endregion

        public IEnumerable<AddressType> GetAddressTypes()
        {
            return this.addressTypeRepository.GetAddressTypes();
        }
        public IEnumerable<AddressType> GetAddressTypesAll()
        {
            IEnumerable<AddressType> result;

            if (this.cacheManager.IsAdded(DataCacheKey))
            {
                result = this.cacheManager.Get(DataCacheKey) as IEnumerable<AddressType>;

                return result.Clone();
            }

            result = this.GetAddressTypes().ToList();
            
            this.cacheManager.Add(DataCacheKey, result);

            return result.Clone();
        }
        public AddressType InsertAddressType(AddressType addressType)
        {
            return this.addressTypeRepository.InsertAddressType(addressType);
        }

        public AddressType UpdateAddressType(AddressType addressType)
        {
            return this.addressTypeRepository.UpdateAddressType(addressType);
        }

        public void DeleteAddressType(int addressTypeId)
        {
            this.addressTypeRepository.DeleteAddressType(addressTypeId);
        }
    }
}