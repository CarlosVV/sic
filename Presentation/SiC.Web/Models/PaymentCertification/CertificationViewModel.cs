namespace Nagnoi.SiC.Web.Models.PaymentCertification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Infrastructure.Web.ViewModels;
    using Domain.Core.Model;


    public class CertificationViewModel : SearchViewModel
    {
        public CertificationViewModel() {
            CaseNumber = "";
            CaseKey = "";
            Comment = "";
            Certification = new List<CertificationModel>();
            Payment = new List<Payment>();
            Transaction = new List<Transaction>();
            Status = new JsonResult();
            Concept = new JsonResult();
            //Search = new SearchViewModel();
        }

        //public string caseNumber { get; set; }

        public string CaseKey { get; set; }

        public string Comment { get; set; }

        public List<CertificationModel> Certification { get; set; }

        public List<Payment> Payment {get; set; }
        
        public List<Transaction> Transaction { get; set; }
        
        public JsonResult Status { get; set; }

        public JsonResult Concept { get; set; }

        //public SearchViewModel Search { get; set; }
    }

    public class CertificationModel
    {
        public int PaymentId { get; set; }
        public string CaseNumber { get; set; }
        public string CaseKey { get; set; }
        public string InvoiceNumber { get; set; }
        public string Amount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Concept { get; set; }
        public string Status { get; set; }
    }
}
