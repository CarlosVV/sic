namespace Nagnoi.SiC.Application
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;

    #endregion

    public sealed class AccessControlLevelService : IAccessControlLevelService
    {
        #region Constants

        private const string AccessControlLevelsByProfileCacheKey = "Nagnoi.SiC.Acls.Profile-{0}";

        private const string FunctionalitiesAllCacheKey = "Nagnoi.SiC.Functionalities.All";

        #endregion

        #region Private Members
        
        private readonly IFunctionalityRepository functionalityRepository = null;
        private readonly IAccessControlLevelRepository accessControlLevelRepository = null;
        private readonly ICacheManager cacheManager = null;
        private readonly ISettingService settingService = null;

        #endregion

        #region Constructors

        public AccessControlLevelService() : this(
            IoC.Resolve<IFunctionalityRepository>(),
            IoC.Resolve<IAccessControlLevelRepository>(),
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<ISettingService>())
        {
        }

        internal AccessControlLevelService(
            IFunctionalityRepository functionalityRepository,
            IAccessControlLevelRepository accessControlLevelRepository,
            ICacheManager cacheManager,
            ISettingService settingService)
        {
            this.functionalityRepository = functionalityRepository;
            this.accessControlLevelRepository = accessControlLevelRepository;
            this.cacheManager = cacheManager;
            this.settingService = settingService;
        }

        #endregion

        #region Properties

        public bool CacheEnabled
        {
            get
            {
                return true;
                //return this.settingService.GetSettingValueBoolean("Cache.AccessControlLevelFacade.CacheEnabled");
            }
        }

        public bool Enabled
        {
            get
            {
                return this.settingService.GetSettingValueBoolean("AccessControlLevel.Enabled");
            }
        }

        public TimeSpan FunctionalitiesCacheDuration
        {
            get
            {
                return new TimeSpan(0, 0, 5, 0, 0);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<AccessControlLevel> FindAccessControlLevelsByProfile(string[] profileNames)
        {
            IEnumerable<AccessControlLevel> result;

            result = this.accessControlLevelRepository.FindByProfileName(profileNames).ToList();
         
            return result.Clone();
        }

        #endregion

        #region Public Methods

        public Functionality GetFunctionalityById(int functionalityId)
        {
            var allFunctionalities = this.SelectAllFunctionalities();

            return allFunctionalities.Where(f => f.FunctionalityId == functionalityId)
                                     .FirstOrDefault();
        }

        public IEnumerable<Functionality> SelectAllFunctionalities()
        {
            IEnumerable<Functionality> result;

            string cacheKey = FunctionalitiesAllCacheKey;

            if (this.CacheEnabled && this.cacheManager.IsAdded(cacheKey))
            {
                Debug.WriteLine("Get Functionalities from Cache");

                result = this.cacheManager.Get(cacheKey) as IEnumerable<Functionality>;

                return result.Clone();
            }

            result = this.functionalityRepository.GetAll().ToList();

            if (this.CacheEnabled)
            {
                Debug.WriteLine("Insert Functionalities in Cache");

                this.cacheManager.Add(cacheKey, result, this.FunctionalitiesCacheDuration, CachingTime.LongTermSliding);
            }

            return result.Clone();
        }

        public void CreateFunctionality(Functionality functionality)
        {
            if (functionality.IsNull())
            {
                throw new ArgumentNullException("functionality");
            }

            this.functionalityRepository.Insert(functionality);
        }

        public AccessControlLevel GetAccessControlLevelById(int accessControlLevelId)
        {
            if (accessControlLevelId == 0)
            {
                return null;
            }

            return this.accessControlLevelRepository.GetById(accessControlLevelId);
        }

        public IEnumerable<AccessControlLevel> FindAccessControlLevels(int functionalityId, int profileId, bool? allow)
        {
            return this.accessControlLevelRepository.GetAll(functionalityId, profileId, allow.HasValue ? allow.Value : false);
        }

        public bool IsFunctionalityAllowed(string systemKeyword)
        {
            string loggedInUserName = WebHelper.GetUserName();
            if (string.IsNullOrEmpty(loggedInUserName))
            {
                return false;
            }

            var roleNames = WebHelper.GetRolesForUser();
            //string[] roleNames = { @"CFSE0\SIC_Compensation_Officer", @"CFSE0\Domain Users", "Everyone", @"BUILTIN\Users",@"NT AUTHORITY\NETWORK", @"NT AUTHORITY\Authenticated Users", @"NT AUTHORITY\This Organization", @"NT AUTHORITY\NTLM Authentication" };

            var acls = FindAccessControlLevelsByProfile(roleNames);

            if (acls.IsNullOrEmpty())
            {                
                return false;
            }

            return acls.Any(a => a.Functionality.FunctionalityName.Equals(systemKeyword, StringComparison.OrdinalIgnoreCase) &&
                                 a.Functionality.IsActive &&
                                 a.Allow);
        }
        
        public void CreateAccessControlLevel(AccessControlLevel accessControlLevel)
        {
            if (accessControlLevel.IsNull())
            {
                throw new ArgumentNullException("accessControlLevel");
            }

            this.accessControlLevelRepository.Insert(accessControlLevel);
        }

        public void ModifyAccessControlLevel(AccessControlLevel accessControlLevel)
        {
            if (accessControlLevel.IsNull())
            {
                return;
            }

            this.accessControlLevelRepository.Update(accessControlLevel);
        }

        public void DeleteAccessControlLevel(int accessControlLevelId)
        {
            if (accessControlLevelId == 0)
            {
                return;
            }

            AccessControlLevel accessControlLevel = this.GetAccessControlLevelById(accessControlLevelId);

            if (accessControlLevel.IsNull())
            {
                return;
            }

            this.accessControlLevelRepository.Delete(accessControlLevel);
        }
        
        #endregion
    }
}