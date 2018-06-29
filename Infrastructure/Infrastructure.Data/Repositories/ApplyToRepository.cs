namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Data.Entity;
    using System.Linq;
    using Core.Helpers;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class ApplyToRepository : EfRepository<ApplyTo>, IApplyToRepository
    {
        public ApplyTo InsertApplyTo(ApplyTo applyTo)
        {
            applyTo.ApplyToId = CreateNewId();
            applyTo.CreatedBy = WebHelper.GetUserName();
            applyTo.CreatedDateTime = DateTime.Now;
            this.Context.ApplyToes.Add(applyTo);
            this.Context.SaveChanges();
            return applyTo;
        }

        public ApplyTo UpdateApplyTo(ApplyTo applyTo)
        {
            var applyToUpdate = this.Context.ApplyToes.Find(applyTo.ApplyToId);
            applyToUpdate.ApplyTo1 = applyTo.ApplyTo1;
            applyToUpdate.Hidden = applyTo.Hidden;
            applyToUpdate.ModifiedBy = WebHelper.GetUserName();
            applyToUpdate.ModifiedDateTime = DateTime.Now;
            this.Context.Entry(applyToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return applyToUpdate;
        }

        public void DeleteApplyTo(int applyToId)
        {
            var applyTo = this.Context.ApplyToes.Find(applyToId);
            this.Context.ApplyToes.Remove(applyTo);
            this.Context.SaveChanges();
        }

        private int CreateNewId()
        {
            var applyToList = from d in this.Context.ApplyToes orderby d.ApplyToId descending select d;
            return applyToList.FirstOrDefault().ApplyToId + 1;
        }
    }
}