namespace Nagnoi.SiC.Web.Models.PreExisting
{
    #region References

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Infrastructure.Web.ViewModels;
    using Domain.Core.Model;

    #endregion

    public class CaseViewModel : BaseViewModel
    {
        public CaseViewModel()
        {
            RegionList = new List<SelectListItem>();
            ClinicList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
        }

        public Case Case { get; set; }

        public IEnumerable<SelectListItem> RegionList { get; set; }

        public IEnumerable<SelectListItem> ClinicList { get; set; }

        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}