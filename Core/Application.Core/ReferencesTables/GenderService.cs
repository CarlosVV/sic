namespace Nagnoi.SiC.Application.Core
{
    #region Referencias

    using System.Collections.Generic;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;
    using Nagnoi.SiC.Domain.Core.Services;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;

    #endregion

    public class GenderService : IGenderService
    {
         #region Miembros Privados

        private readonly IGenderRepository genderRepository = null;

        #endregion

        #region Constructores

        public GenderService()
            : this(IoC.Resolve<IGenderRepository>())
        { }

        internal GenderService(IGenderRepository genderRepository)
        {
            this.genderRepository = genderRepository;
        }

        #endregion
        public IEnumerable<Gender> GetGender()
        {
            return this.genderRepository.GetGender();
        }

        public Gender InsertGender(Gender gender)
        {
            return this.genderRepository.InsertGender(gender);
        }

        public Gender UpdateGender(Gender gender)
        {
            return this.genderRepository.UpdateGender(gender);
        }

        public void DeleteGender(int genderId)
        {
            this.genderRepository.DeleteGender(genderId);
        }
    }
}


