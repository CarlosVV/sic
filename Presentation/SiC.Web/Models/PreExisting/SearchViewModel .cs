namespace Nagnoi.SiC.Web.Models.PreExisting
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Domain.Core.Services;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Web.Utilities;
    using Infrastructure.Web.ViewModels;

    #endregion

    public class SearchViewModel : BaseViewModel
    {
        public string Url = "";

        #region Constructor

        public SearchViewModel()
        {
            Regions = new List<SelectListItem>();
            Clinics = new List<SelectListItem>();
            Cities = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [Display(Name = "Nro. de Caso")]
        public string CaseNumber { get; set; }

        [Display(Name = "Lesionado / Beneficiario")]
        public string EntityName { get; set; }

        [Display(Name = "Nro. Seguro Social")]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Fecha de Radicación")]
        public DateTime? FilingDate { get; set; }

        [Display(Name = "Nro. EBT")]
        public string EBTNumber { get; set; }

        [Display(Name = "Región")]
        public int? RegionId { get; set; }

        [Display(Name = "Dispensario")]
        public int? ClinicId { get; set; }
        
        public IList<SelectListItem> Regions { get; set; }

        public IList<SelectListItem> Clinics { get; set; }

        public IList<SelectListItem> Cities { get; set; }

        #endregion

        #region Public Methods

        public SearchViewModel Rebuild(SearchViewModel model)
        {
            if (model == null)
            {
                model = new SearchViewModel();
            }

            model.Regions = GetRegions();
            model.Clinics = GetClinics();

            return model;
        }

        #endregion

        #region Private Methods

        private IList<SelectListItem> GetRegions()
        {
            var regions = IoC.Resolve<ILocationService>().GetAllRegions();

            return regions.ToSelectList(r => r.Region1, r => r.RegionId.ToString(), null, "Seleccionar");
        }

        private IList<SelectListItem> GetClinics()
        {
            var clinics = IoC.Resolve<ILocationService>().GetAllClinics();

            return clinics.ToSelectList(c => c.Clinic1, c => c.ClinicId.ToString(), null, "Seleccionar");
        }

        #endregion
    }
}