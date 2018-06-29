namespace Nagnoi.SiC.Infrastructure.Web.Security
{
    #region References

    using System.Linq;
    using System.Web.Security;
    using Core.Caching;
    using Core.Dependencies;

    #endregion

    public class WindowsTokenCacheRoleProvider : WindowsTokenRoleProvider
    {
        #region Public Methods

        public override string[] GetRolesForUser(string username)
        {
            var cacheManager = IoC.Resolve<ICacheManager>("sic_cache_por_request");
            var cacheKey = string.Format("{0}:{1}", base.ApplicationName, username);
            if (cacheManager.IsAdded(cacheKey))
            {
                return cacheManager.Get(cacheKey) as string[];
            }

            var roles = base.GetRolesForUser(username);

            cacheManager.Add(cacheKey, roles);

            return roles;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var roles = GetRolesForUser(username);
            return roles.Any(role => role.Trim().ToUpperInvariant().Contains(roleName.Trim().ToUpperInvariant()));
        }

        #endregion
    }
}