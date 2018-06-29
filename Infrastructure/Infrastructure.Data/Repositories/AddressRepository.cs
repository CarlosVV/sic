namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class AddressRepository : EfRepository<Address>, IAddressRepository
    {
        public override void Insert(Address entity)
        {
            int nextAddressId = EntitiesNoTracking.OrderByDescending(a => a.AddressId)
                                                  .FirstOrDefault()
                                                  .AddressId + 1;
            entity.AddressId = nextAddressId;

            base.Insert(entity);
        }
    }
}