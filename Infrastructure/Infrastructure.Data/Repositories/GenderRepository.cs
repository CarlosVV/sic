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

    public sealed class GenderRepository : EfRepository<Gender>, IGenderRepository
    {
        public IEnumerable<Gender> GetGender()
        {
            return this.Context.Genders.OrderBy(m => m.GenderId);
        }

        public Gender InsertGender(Gender gender)
        {
           gender.GenderId = CreateNewId(); 
           gender.CreatedBy = WebHelper.GetUserName(); 
           gender.CreatedDateTime = DateTime.Now; 
           this.Context.Genders.Add(gender);
           this.Context.SaveChanges();
           return gender;
        }

        public Gender UpdateGender(Gender gender)
        {
            var gendersToUpdate = this.Context.Genders.Find(gender.GenderId);
            gendersToUpdate.Gender1 = gender.Gender1;
            gendersToUpdate.GenderCode = gender.GenderCode;
            gendersToUpdate.Hidden = gender.Hidden; 
            gendersToUpdate.ModifiedBy = WebHelper.GetUserName(); 
            gendersToUpdate.ModifiedDateTime = DateTime.Now; 
            this.Context.Entry(gendersToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges(); 
            return gendersToUpdate;
        }

        public void DeleteGender(int genderId)
        {
            var gender = this.Context.Genders.Find(genderId);
            this.Context.Genders.Remove(gender);
            this.Context.SaveChanges();
        }

        private int CreateNewId(){ 
            var genderList = from d in this.Context.Genders orderby  d.GenderId descending select d; 
            return genderList.FirstOrDefault().GenderId + 1;
        }
    }
}