namespace Nagnoi.SiC.Application.Core
{
    #region References

    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;

    #endregion

    public sealed class LocationService : ILocationService
    {
        #region Constantes

        private const string DataCacheKeyRegions = "Nagnoi.Regions";

        private const string DataCacheKeyClinics = "Nagnoi.Clinics";

        private const string DataCacheKeyCities = "Nagnoi.Cities";

        private const string DataCacheKeyStates = "Nagnoi.States";

        private const string DataCacheKeyCountries = "Nagnoi.Countries";

        private const string DataCacheKeyCourts = "Nagnoi.Courts";

        #endregion

        #region Private Members

        private readonly IRepository<Region> regionRepository = null;

        private readonly IRepository<Clinic> clinicRepository = null;

        //private readonly IRepository<Court> courtRepository = null;

        private readonly IRepository<City> cityRepository = null;

        private readonly IRepository<State> stateRepository = null;

        private readonly IRepository<Country> countryRepository = null;

        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructors

        public LocationService() : this(
            IoC.Resolve<IRepository<Region>>(),
            IoC.Resolve<IRepository<Clinic>>(),
            //IoC.Resolve<IRepository<Court>>(),
            IoC.Resolve<IRepository<City>>(),
            IoC.Resolve<IRepository<State>>(),
            IoC.Resolve<IRepository<Country>>(),
            IoC.Resolve<ICacheManager>())
        { }

        internal LocationService(
            IRepository<Region> regionRepository,
            IRepository<Clinic> clinicRepository,
            //IRepository<Court> courtRepository,
            IRepository<City> cityRepository,
            IRepository<State> stateRepository,
            IRepository<Country> countryRepository,
            ICacheManager cacheManager)
        {
            this.regionRepository = regionRepository;
            this.clinicRepository = clinicRepository;
            //this.courtRepository = courtRepository;
            this.cityRepository = cityRepository;
            this.stateRepository = stateRepository;
            this.countryRepository = countryRepository;
            this.cacheManager = cacheManager;
        }

        #endregion

        #region Public Methods

        public IEnumerable<Region> GetAllRegions()
        {
            IEnumerable<Region> result;

            if (this.cacheManager.IsAdded(DataCacheKeyRegions))
            {
                Debug.WriteLine("Get Regions from Cache");

                result = this.cacheManager.Get(DataCacheKeyRegions) as IEnumerable<Region>;

                return result.Clone();
            }

            result = this.regionRepository.GetAll().Where(p => p.Hidden.Value == false).ToList();

            this.cacheManager.Add(DataCacheKeyRegions, result);

            return result.Clone();
        }

        public IEnumerable<Clinic> GetAllClinics()
        {
            IEnumerable<Clinic> result;

            if (this.cacheManager.IsAdded(DataCacheKeyClinics))
            {
                Debug.WriteLine("Get Clinics from Cache");

                result = this.cacheManager.Get(DataCacheKeyClinics) as IEnumerable<Clinic>;

                return result.Clone();
            }

            result = this.clinicRepository.GetAll(c => c.Region).Where(p => p.Hidden.Value == false).ToList();

            this.cacheManager.Add(DataCacheKeyClinics, result);

            return result.Clone();
        }

        public IEnumerable<Clinic> GetAllClinicsByRegion(int regionId)
        {
            return GetAllClinics().Where(c => c.RegionId == regionId);
        }

        //public IEnumerable<Court> GetAllCourts()
        //{
        //    //IEnumerable<Court> result;

        //    if (this.cacheManager.IsAdded(DataCacheKeyCourts))
        //    {
        //        Debug.WriteLine("Get Cities from Cache");

        //        //result = this.cacheManager.Get(DataCacheKeyCourts) as IEnumerable<Court>;

        //        return result.Clone();
        //    }

        //    result = this.courtRepository.GetAll().ToList();

        //    this.cacheManager.Add(DataCacheKeyCourts, result);

        //    return result.Clone();
        //}

        public IEnumerable<City> GetAllCities()
        {
            IEnumerable<City> result;

            if (this.cacheManager.IsAdded(DataCacheKeyCities))
            {
                Debug.WriteLine("Get Cities from Cache");

                result = this.cacheManager.Get(DataCacheKeyCities) as IEnumerable<City>;

                return result.Clone();
            }

            result = this.cityRepository.GetAll().Where(p => p.Hidden.Value == false).ToList();

            this.cacheManager.Add(DataCacheKeyCities, result);

            return result.Clone();
        }

        public IEnumerable<State> GetAllStates()
        {
            IEnumerable<State> result;

            if (this.cacheManager.IsAdded(DataCacheKeyStates))
            {
                Debug.WriteLine("Get Cities from Cache");

                result = this.cacheManager.Get(DataCacheKeyStates) as IEnumerable<State>;

                return result.Clone();
            }

            result = this.stateRepository.GetAll().Where(p => p.Hidden.Value == false).ToList();

            this.cacheManager.Add(DataCacheKeyStates, result);

            return result.Clone();
        }

        public IEnumerable<Country> GetAllCountries()
        {
            IEnumerable<Country> result;

            if (this.cacheManager.IsAdded(DataCacheKeyCountries))
            {
                Debug.WriteLine("Get Cities from Cache");

                result = this.cacheManager.Get(DataCacheKeyCountries) as IEnumerable<Country>;

                return result.Clone();
            }

            result = this.countryRepository.GetAll().Where(p => p.Hidden.Value == false).ToList();

            this.cacheManager.Add(DataCacheKeyCountries, result);

            return result.Clone();
        }
        #endregion
    }
}