namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Core.Helpers;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class ClassRepository : EfRepository<Class>, IClassRepository
    {
        public override IEnumerable<Class> GetAll()
        {
            return base.GetAll().OrderBy(c => c.ClassId);
        }
        
        public Class InsertClass(Class _class)
        {
            _class.ClassId = CreateNewId();
            _class.CreatedBy = WebHelper.GetUserName();
            _class.CreatedDateTime = DateTime.Now;
            this.Context.Classes.Add(_class);
            this.Context.SaveChanges();
            return _class;
        }

        public Class UpdateClass(Class _class)
        {
            var classToUpdate = this.Context.Classes.Find(_class.ClassId);
            classToUpdate.Class1 = _class.Class1;
            classToUpdate.Concept = _class.Concept;
            classToUpdate.Hidden = _class.Hidden;
            classToUpdate.ModifiedBy = WebHelper.GetUserName();
            classToUpdate.ModifiedDateTime = DateTime.Now;
            this.Context.Entry(classToUpdate).State = EntityState.Modified;
            this.Context.SaveChanges();
            return classToUpdate;
        }

        public void DeleteClass(int classId)
        {
            var _class = this.Context.Classes.Find(classId);
            this.Context.Classes.Remove(_class);
            this.Context.SaveChanges();
        }

        private int CreateNewId()
        {
            var classesList = from d in this.Context.Classes orderby d.ClassId descending select d;
            return classesList.FirstOrDefault().ClassId + 1;
        }
    }
}