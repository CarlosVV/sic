namespace Nagnoi.SiC.Domain.Core.Services
{
    #region Imports

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion
    
    public interface ISettingService 
    {
        #region Properties
        bool CacheEnabled { get; }  

        #endregion

        #region Methods

        Setting GetSettingByName(string name, bool includeApplicationName);
        string GetSettingValueByName(string name);
        string GetSettingValueByName(string name, bool includeApplicationName);
        bool GetSettingValueBoolean(string name);
        bool GetSettingValueBoolean(string name, bool defaultValue);
        bool GetSettingValueBoolean(string name, bool defaultValue, bool includeApplicationName);
        int GetSettingValueInteger(string name);
        int GetSettingValueInteger(string name, int defaultValue);
        int GetSettingValueInteger(string name, int defaultValue, bool includeApplicationName);
        IEnumerable<Setting> GetAllSettings();
        Setting SetParam(string name, string value);
        Setting SetParam(string name, string value, string description);
        Setting InsertSetting(Setting setting);

        #endregion
    }
}