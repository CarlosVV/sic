namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System.Collections.Generic;
    using Domain.Core.Model;

    #endregion

    /// <summary>
    /// Menu Facade interface
    /// </summary>
    public interface IMenuService 
    {
        /// <summary>
        /// Retrieves all screens
        /// </summary>
        /// <returns>Return the contents of menus</returns>
        IEnumerable<Menu> GetAllMenus();
        
        /// <summary>
        /// Gets a menu by its identifier
        /// </summary>
        /// <param name="menuId">Menu identifier</param>
        /// <returns>Returns the menu instance</returns>
        Menu GetMenuById(int menuId);
    }
}