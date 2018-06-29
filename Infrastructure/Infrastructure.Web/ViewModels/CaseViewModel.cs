using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nagnoi.SiC.Domain.Core.Model;

namespace Nagnoi.SiC.Infrastructure.Web.ViewModels {
    public class CaseViewModel : BaseViewModel {

        public CaseDetail Case = new CaseDetail();

        public CaseViewModel()
        {
            RegionList = new List<SelectListItem>();
            ClinicList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
            BeneficiaryList = new List<CaseDetail>();
        }

        public IEnumerable<SelectListItem> RegionList { get; set; }

        public IEnumerable<SelectListItem> ClinicList { get; set; }

        public IEnumerable<SelectListItem> CityList { get; set; }

        public IEnumerable<CaseDetail> BeneficiaryList { get; set; }

    }
}
