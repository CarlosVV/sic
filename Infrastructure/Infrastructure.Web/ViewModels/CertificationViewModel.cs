using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nagnoi.SiC.Domain.Core.Model;

namespace Nagnoi.SiC.Infrastructure.Web.ViewModels
{
    public class CertificationViewModel : BaseViewModel
    {
        public CertificationViewModel() {
            caseNumber = "";
            certification = new List<CertificationModel>();
            payment = new List<Payment>();
            transaction = new List<Transaction>();
            statusCreate = new JsonResult();
            statusEdit = new JsonResult();
            concept = new JsonResult();
        }

        public string caseNumber;

        public List<CertificationModel> certification { get; set; }

        public List<Payment> payment {get; set; }
        
        public List<Transaction> transaction { get; set; }
        
        public JsonResult statusCreate { get; set; }

        public JsonResult statusEdit { get; set; }
        
        public JsonResult concept { get; set; }
    }

    public class CertificationModel
    {
        public int tableId { get; set; }
        public string caseNumber { get; set; }
        public string invoiceNumber { get; set; }
        public string amount { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string concept { get; set; }
        public string status { get; set; }
    }
}
