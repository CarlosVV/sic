namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using CDI.WebApplication.DataTables.Result;
    using Infrastructure.Web.Controllers;

    #endregion    
    public class SearchController : BaseController
    {
        #region Public Methods
        
        public IEnumerable<SelectListItem> GetAllRegions()
        {
            IList<SelectListItem> lstRegions = new List<SelectListItem>();
            var regions = LocationService.GetAllRegions();
            lstRegions.Add(new SelectListItem { Value = "0", Text = "--Seleccione una Region--" });
            foreach (var reg in regions)
            {
                lstRegions.Add(new SelectListItem { Value = reg.RegionId.ToString(), Text = reg.Region1 });
            }
            return lstRegions;
        }

        public IEnumerable<SelectListItem> GetAllClinics()
        {
            IList<SelectListItem> lstClinics = new List<SelectListItem>();
            var clinics = LocationService.GetAllClinics();
            lstClinics.Add(new SelectListItem { Value = "0", Text = "--Seleccione Dispensario--" });
            foreach (var c in clinics)
            {
                lstClinics.Add(new SelectListItem
                {
                    Value = c.ClinicId.ToString(),
                    Text = String.Format("{0} [{1}]", c.Clinic1, c.RegionId)
                });
            }
            return lstClinics;
        }

        public IEnumerable<SelectListItem> GetAllCities()
        {
            IList<SelectListItem> lstCities = new List<SelectListItem>();
            var cities = LocationService.GetAllCities();
            lstCities.Add(new SelectListItem { Value = "0", Text = "--Seleccione una Ciudad--" });
            foreach (var c in cities)
            {
                lstCities.Add(new SelectListItem
                {
                    Value = c.CityId.ToString(),
                    Text = String.Format("{0} [{1}]", c.City1, c.StateId)
                });
            }
            return lstCities;
        }


        #endregion
    }
}