namespace Nagnoi.SiC.Web.Models.Approval
{
    #region References

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Core.Model;
    using Infrastructure.Web.Utilities;
    using Infrastructure.Web.ViewModels;

    #endregion

    public class IndexViewModel : BaseViewModel
    {
        #region Constructor

        public IndexViewModel()
        {
            Regions = new List<SelectListItem>();
            Clinics = new List<SelectListItem>();
            Cities = new List<SelectListItem>();
            Courts = new List<SelectListItem>();
            Beneficiaries = new List<SelectListItem>();
            PaymentStatus = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public IList<SelectListItem> Regions { get; set; }

        public IList<SelectListItem> Clinics { get; set; }

        public IList<SelectListItem> Cities { get; set; }

        public IList<SelectListItem> Courts { get; set; }

        public IList<SelectListItem> Beneficiaries { get; set; }

        public IList<SelectListItem> PaymentStatus { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string EBTNumber { get; set; }

        #endregion

        #region Public Methods

        public IndexViewModel Rebuild(IndexViewModel model, IEnumerable<Region> regions, IEnumerable<City> cities, IEnumerable<Clinic> clinics, IEnumerable<Status> paymentStatuses)
        {
            if (model == null)
            {
                model = new IndexViewModel();
            }

            model.Regions = regions.ToSelectList(r => r.Region1, r => r.RegionId.ToString(), null, "Seleccionar");
            model.Cities = cities.ToSelectList(c => c.City1, c => c.CityId.ToString(), null, "Seleccionar");
            model.Clinics = clinics.ToSelectList(c => c.Clinic1, c => string.Format("{0}-[{1}]", c.Clinic1, c.RegionId.ToString()), null, "Seleccionar");
            model.PaymentStatus = paymentStatuses.ToSelectList(s => s.Status1, s => s.StatusId.ToString(), null, "Seleccionar");

            return model;
        }

        #endregion
    }
}