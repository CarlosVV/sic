using System.Collections.Generic;
using System.Web.Mvc;

namespace Nagnoi.SiC.Infrastructure.Web.ViewModels
{
    public class TransactionDetailViewModel : BaseViewModel
    {
        public decimal? TransactionAmount { get; set; }
        public decimal? TransactionPercent { get; set; }
        public IEnumerable<SelectListItem> CompensationRegionList { get; set; }
        public IEnumerable<SelectListItem> CompensationCodeList { get; set; }

        public TransactionDetailViewModel()
        {
            CompensationRegionList = new List<SelectListItem>();
            CompensationCodeList = new List<SelectListItem>();
            TransactionAmount = new decimal?();
            TransactionPercent = new decimal?();
        }
    }
}