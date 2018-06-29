namespace Nagnoi.SiC.Web.Models.Approval
{
    #region References

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Infrastructure.Web.ViewModels;

    #endregion

    public class AprobacionViewModel : BaseViewModel
    {
        public AprobacionViewModel()
        {
            RegionList = new List<SelectListItem>();
            ClinicList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
        }

        public IEnumerable<SelectListItem> RegionList { get; set; }

        public IEnumerable<SelectListItem> ClinicList { get; set; }

        public IEnumerable<SelectListItem> CityList { get; set; }

        public IEnumerable<SelectListItem> CourtList { get; set; }

        public IEnumerable<SelectListItem> Beneficiarios { get; set; }

        public IEnumerable<SelectListItem> PaymentStatus { get; set; }
    }
}