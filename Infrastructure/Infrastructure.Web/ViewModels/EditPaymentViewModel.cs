using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nagnoi.SiC.Infrastructure.Web.ViewModels {
    public class EditPaymentViewModel : BaseViewModel {
        public EditPaymentViewModel() {
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
