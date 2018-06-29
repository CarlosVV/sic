namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System;
    using System.Data.Entity;
    using Core.Helpers;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class SettingRepository : EfRepository<Setting>, ISettingRepository
    {
        public void Update(Setting setting)
        {
            var settingToUpdate = this.Context.Settings.Find(setting.SettingId);
            settingToUpdate.SettingId = setting.SettingId;
            settingToUpdate.Name = setting.Name;
            settingToUpdate.Description = setting.Description;
            settingToUpdate.Value = setting.Value;
            settingToUpdate.ModifiedBy = WebHelper.GetUserName();
            settingToUpdate.ModifiedDateTime = DateTime.Now;
            this.Context.Entry(settingToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();          
        }

        public int Insert(Setting setting)
        {           
            setting.CreatedBy = WebHelper.GetUserName();
            setting.CreatedDateTime = DateTime.Now;
            this.Context.Settings.Add(setting);
            this.Context.SaveChanges();
            return setting.SettingId;
        }
    }
}