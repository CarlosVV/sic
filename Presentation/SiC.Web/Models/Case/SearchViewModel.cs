namespace Nagnoi.SiC.Web.Models.Case
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Domain.Core.Services;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Web.Utilities;
    using Infrastructure.Web.ViewModels;

    #endregion

    public class SearchViewModel : BaseViewModel
    {
        #region Private Members
        
        private string caseNumber = null;

        #endregion

        #region Constructor

        public SearchViewModel()
        {
            Regions = new List<SelectListItem>();
            Clinics = new List<SelectListItem>();
            Cities = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [Display(Name = "Número de Caso")]
        public string CaseNumber
        {
            get { return caseNumber; }
            set
            {
                caseNumber = value;

                EnsureCaseNumberAndCaseKey();
            }
        }

        public string CaseKey { get; set; }
        
        [Display(Name = "Lesionado / Beneficiario")]
        public string EntityName { get; set; }

        [Display(Name = "Número Seguro Social")]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Fecha de Radicación")]
        public DateTime? FilingDate { get; set; }

        [Display(Name = "Número EBT")]
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

        public void EnsureCaseNumberAndCaseKey()
        {
            if (string.IsNullOrEmpty(caseNumber))
            {
                return;
            }
            else if (caseNumber.Match(@"^[0-9]{1,11} [0-9]{2}$"))
            {
                string[] caseNumberIncludingCaseKey = caseNumber.Split(new char[] { ' ' });

                caseNumber = caseNumberIncludingCaseKey[0];
                CaseKey = caseNumberIncludingCaseKey[1];
            }
            else if (CaseNumber.Match(@"^[0-9]{1,11}$"))
            {
                CaseKey = null;
            }
            else
            {
                CaseKey = null;
            }
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

            return clinics.ToSelectList(c => c.Clinic1, c => string.Format("{0}-[{1}]", c.ClinicId.ToString(), c.RegionId.ToString()), null, "Seleccionar");
        }

        #endregion
    }
}