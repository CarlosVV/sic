namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System.Collections.Generic;
    using Domain.Core.Model;

    #endregion

    /// <summary>
    /// Access Control Level Service
    /// </summary>
    public interface IAccessControlLevelService 
    {
        #region Access Control Level

        /// <summary>
        /// Creates a new access control level
        /// </summary>
        /// <param name="accessControlLevel">Access control level instance</param>
        /// <returns>Returns the new identifier</returns>
        void CreateAccessControlLevel(AccessControlLevel accessControlLevel);

        /// <summary>
        /// Modifies an access control level
        /// </summary>
        /// <param name="accessControlLevel">Access control level instance</param>
        void ModifyAccessControlLevel(AccessControlLevel accessControlLevel);

        /// <summary>
        /// Deletes an access control level
        /// </summary>
        /// <param name="accessControlLevelId">Access control level identifier</param>
        void DeleteAccessControlLevel(int accessControlLevelId);

        /// <summary>
        /// Gets an access control level by identifier
        /// </summary>
        /// <param name="accessControlLevelId">Access control level identifier</param>
        /// <returns>Returns the instance of access control level</returns>
        AccessControlLevel GetAccessControlLevelById(int accessControlLevelId);

        /// <summary>
        /// Gets all access controls levels 
        /// </summary>
        /// <param name="functionalityId">Functionality identifier</param>
        /// <param name="profileId">Profile identifier</param>
        /// <param name="allow">Value indicating whether action is allowed; null to load all records</param>
        /// <returns>Returns the contents of access control levels</returns>
        IEnumerable<AccessControlLevel> FindAccessControlLevels(int functionalityId, int profileId, bool? allow);
        
        /// <summary>
        /// Indicates whether a functionality is allowed
        /// </summary>
        /// <param name="systemKeyword">System keyword</param>
        /// <returns>Returns true or false</returns>
        bool IsFunctionalityAllowed(string systemKeyword);
        
        #endregion

        #region Functionalities

        /// <summary>
        /// Gets a functionality by identifier
        /// </summary>
        /// <param name="functionalityId">Functionality identifier</param>
        /// <returns>Returns a functionality</returns>
        Functionality GetFunctionalityById(int functionalityId);

        /// <summary>
        /// Gets all functionalities
        /// </summary>
        /// <returns>Returns the functionality contents</returns>
        IEnumerable<Functionality> SelectAllFunctionalities();
        
        /// <summary>
        /// Creates a new functionality
        /// </summary>
        /// <param name="functionality">Functionality instance</param>
        /// <returns>Returns the new identifier</returns>
        void CreateFunctionality(Functionality functionality);

        #endregion
    }
}