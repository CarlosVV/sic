namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Core.Helpers;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class CivilStatusRepository : EfRepository<CivilStatus>, ICivilStatusRepository
    {
        public IEnumerable<CivilStatus> GetCivilStatus()
        {
            return this.Context.CivilStatus.OrderBy(m => m.CivilStatusId);
        }

        public CivilStatus InsertCivilStatus(CivilStatus civilStatus)
        {
            civilStatus.CivilStatusId = CreateNewId();           
            civilStatus.CreatedBy = WebHelper.GetUserName();
            civilStatus.CreatedDateTime = DateTime.Now;
            civilStatus.ModifiedBy = WebHelper.GetUserName();
            civilStatus.ModifiedDateTime = DateTime.Now;
            this.Context.CivilStatus.Add(civilStatus);
            this.Context.SaveChanges();
            return civilStatus;
        }

        public CivilStatus UpdateCivilStatus(CivilStatus civilStatus)
        {
            var civilStatusUpdate = this.Context.CivilStatus.Find(civilStatus.CivilStatusId);
            civilStatusUpdate.CivilStatus1 = civilStatus.CivilStatus1;
            civilStatusUpdate.CivilStatusCode = civilStatus.CivilStatusCode;
            civilStatusUpdate.Hidden = civilStatus.Hidden;
            civilStatusUpdate.ModifiedBy = WebHelper.GetUserName();
            civilStatusUpdate.ModifiedDateTime = DateTime.Now;
            this.Context.Entry(civilStatusUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return civilStatusUpdate;
        }

        public void DeleteCivilStatus(int civilStatusId)
        {
            var civilstatus = this.Context.CivilStatus.Find(civilStatusId);
            this.Context.CivilStatus.Remove(civilstatus);
            this.Context.SaveChanges();
        }

        private int CreateNewId() {
            var civilStatusList = from d in this.Context.CivilStatus orderby d.CivilStatusId descending select d;
            return civilStatusList.FirstOrDefault().CivilStatusId + 1;
        }
    }
}