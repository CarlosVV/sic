using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nagnoi.SiC.Web.Models.Payment
{
    public class UpdateInvestmentRequest
    {
        public UpdateInvestmentRequest()
        {
            Payments = new List<PaymentTransaction>();
        }
        public int CaseId { get; set; }
        public int CaseDetailId { get; set; }
        public int TransactionId { get; set; }
        public string Comment { get; set; }
        public string CaseNumber { get; set; }
        public IEnumerable<PaymentTransaction> Payments { get; set; }
    }
}