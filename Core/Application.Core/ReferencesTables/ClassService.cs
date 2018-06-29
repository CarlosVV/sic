namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System.Collections.Generic;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Dependencies;

    #endregion

    public class ClassService : IClassService
    {
        #region Private Members

        private readonly IClassRepository classRepository = null;

        #endregion

        #region Constructors

        public ClassService()
            : this(IoC.Resolve<IClassRepository>())
        { }

        internal ClassService(IClassRepository classRepository)
        {
            this.classRepository = classRepository;
        }

        #endregion

        #region Public Methods

        public IEnumerable<Class> GetClasses()
        {
            return this.classRepository.GetAll();
        }

        public Class InsertClass(Class _class)
        {
            return this.classRepository.InsertClass(_class);
        }

        public Class UpdateClass(Class _class)
        {
            return this.classRepository.UpdateClass(_class);
        }

        public void DeleteClass(int classId)
        {
            this.classRepository.DeleteClass(classId);
        }

        #endregion
    }
}