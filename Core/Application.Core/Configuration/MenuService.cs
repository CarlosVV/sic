namespace Nagnoi.SiC.FrontEnd.Application.Common
{
    #region References

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
    
    public sealed class MenuService : IMenuService
    {
        #region Constants

        /// <summary>
        /// Cache key for Screens
        /// </summary>
        private readonly string MenusAllCacheDependencyKey = "Nagnoi.SiC.Menus.All";

        #endregion

        #region Private Members

        /// <summary>
        /// Screen repository reference
        /// </summary>
        private readonly IRepository<Menu> menuRepository = null;

        /// <summary>
        /// Cache manager reference
        /// </summary>
        private readonly ICacheManager cacheManager = null;

        /// <summary>
        /// Access Control Level reference
        /// </summary>
        private readonly IAccessControlLevelService accessControlLevelService = null;

        /// <summary>
        /// Setting facade reference
        /// </summary>
        private readonly ISettingService settingService = null;


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuService"/> class
        /// </summary>
        public MenuService() : this(
            IoC.Resolve<IRepository<Menu>>(),
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<IAccessControlLevelService>(),
            IoC.Resolve<ISettingService>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuService"/> class
        /// </summary>
        /// <param name="menuRepository">Menu repository</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="accessControlLevelService">Access Control Level facade</param>
        internal MenuService(
            IRepository<Menu> menuRepository,
            ICacheManager cacheManager,
            IAccessControlLevelService accessControlLevelService,
            ISettingService settingService)
        {
            this.menuRepository = menuRepository;

            this.cacheManager = cacheManager;

            this.accessControlLevelService = accessControlLevelService;

            this.settingService = settingService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a item indicating whether cache is enabled
        /// </summary>
        public bool CacheEnabled
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Public Methods
        
        public IEnumerable<Menu> GetAllMenus()
        {
            IEnumerable<Menu> result;

            string cacheKey = string.Format(this.MenusAllCacheDependencyKey);

            if (this.CacheEnabled && this.cacheManager.IsAdded(cacheKey))
            {
                Debug.WriteLine("Get Menus from Cache");

                result = this.cacheManager.Get(cacheKey) as IEnumerable<Menu>;

                return result.Clone();
            }
            
            result = this.menuRepository.GetAll(m => m.Functionality).ToList();

            if (this.CacheEnabled)
            {
                Debug.WriteLine("Insert Menus on Cache");

                this.cacheManager.Add(cacheKey, result);
            }

            return result.Clone();
        }
        
        public Menu GetMenuById(int menuId)
        {
            if (menuId > 0)
            {
                var menus = this.GetAllMenus();

                return menus.Where(m => m.MenuId == menuId)
                            .FirstOrDefault();
            }

            return null;
        }

        #endregion
    }
}