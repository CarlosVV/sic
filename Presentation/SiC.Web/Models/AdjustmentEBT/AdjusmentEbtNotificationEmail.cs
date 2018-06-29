using Nagnoi.SiC.Infrastructure.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nagnoi.SiC.Web.Models.AdjustmentEBT
{
    public class AdjusmentEbtNotificationEmail : BaseViewModel
    {
        public string Lesionado { get; set; }
        public string NumeroCaso { get; set; }
        public string NumeroCuentaEbt { get; set; }
        public List<AdjusmentEbtViewModel> PaymentTable { get; set; }
    }
}