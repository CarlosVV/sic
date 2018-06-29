namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class FunctionalityRepository : EfRepository<Functionality>, IFunctionalityRepository
    {
        public bool IsAllowed(string systemKeyword)
        {
            var query = from item in this.Context.Functionalities
                        where item.FunctionalityName == systemKeyword
                        select item;

            if (query != null)
            {
                var fnc = query.FirstOrDefault();
                if (fnc != null)
                {
                    return fnc.IsActive;
                }
            }
            return false;
        }
    }
}