namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System.Linq;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class ProfileRepository : EfRepository<Profile>, IProfileRepository
    {
        public Profile GetById(int profileId) {
            var query = from item in this.Context.Profiles 
                        where item.ProfileId == profileId
                        select item;
            return query.FirstOrDefault();
        }        
    }
}