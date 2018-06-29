namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System.Collections.Generic;
    using Model;

    #endregion

    public interface ILocationService
    {
        IEnumerable<Region> GetAllRegions();

        IEnumerable<Clinic> GetAllClinics();

        IEnumerable<Clinic> GetAllClinicsByRegion(int regionId);

        IEnumerable<City> GetAllCities();

        //IEnumerable<Court> GetAllCourts();

        IEnumerable<State> GetAllStates();

        IEnumerable<Country> GetAllCountries();
    }
}