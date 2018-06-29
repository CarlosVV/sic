namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using Infrastructure.Web.Controllers;
    using Nagnoi.SiC.Domain.Core.Model;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Web.Mvc;
    using Nagnoi.SiC.Web.App_Start;

    #endregion

    [BaseAuthenticationFilter]        
    public class InterfaceController : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("Interface/CalculateMonthlyPayment/{caseNumber}")]
        public JsonResult CalculateMonthlyPayment(string caseNumber, List<SimeraBeneficiary> beneficiaries)
        {
            ServiceResult result = new ServiceResult();
            string errorMsg = "Error: {0} were not provided.";

            try {

                if (beneficiaries == null || beneficiaries.Count == 0)
                    return Json(new ServiceResult { Error = string.Format(errorMsg, "beneficiaries") }, JsonRequestBehavior.AllowGet);

                if (caseNumber == null)
                    return Json(new ServiceResult { Error = string.Format(errorMsg, "case number") }, JsonRequestBehavior.AllowGet);

                SimeraBeneficiaryService.DeleteCaseBeneficiaries(caseNumber);

                foreach (var beneficiary in beneficiaries)
                {
                    SimeraBeneficiaryService.InsertSimeraBeneficiary(beneficiary);
                }

                var urlRequest = CreateRequest(caseNumber);
                var data = RequestMonthlyPayment(urlRequest);

                result.Data = data;
                result.Error += string.IsNullOrEmpty(data.Message) ? null : data.Message;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                result.Error += ex.Message;
            }
            catch (Exception ex)
            {
                result.Error += ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string CreateRequest(string caseNumber)
        {
            var link = SettingService.GetSettingByName("Interface.MonthlyPaymentCalculatorLink", true);
            string urlRequest = string.Format(link.Value, caseNumber);
            return urlRequest;
        }

        private CalculateMonthlyPaymentResult RequestMonthlyPayment(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CalculateMonthlyPaymentResult));
                    CalculateMonthlyPaymentResult objResponse = (CalculateMonthlyPaymentResult)jsonSerializer.ReadObject(response.GetResponseStream());

                    return objResponse;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class ServiceResult
    {
        public string Error { get; set; }

        public object Data { get; set; }
    }

    [DataContract]
    public class Request
    {
        [DataMember]
        public List<SimeraBeneficiary> Beneficiaries { get; set; }
    }

    [DataContract]
    public class CalculateMonthlyPaymentResult
    {
        [DataMember(Name = "Accion")]
        public string Action { get; set; }

        [DataMember(Name = "Caso")]
        public string CaseNumber { get; set; }

        [DataMember(Name = "Combinacion")]
        public string Combination { get; set; }

        [DataMember(Name = "Contable")]
        public string Account { get; set; }

        [DataMember(Name = "Jornal")]
        public string Wage { get; set; }

        [DataMember(Name = "Mensaje")]
        public string Message { get; set; }

        [DataMember(Name = "Mensualidad")]
        public decimal? MonthlyPayment { get; set; }

        [DataMember(Name = "PagoInicial")]
        public decimal? InitialPayment { get; set; }

        [DataMember(Name = "Procesado")]
        public bool? Processed { get; set; }

        [DataMember(Name = "Relacion")]
        public string Relation { get; set; }

        [DataMember(Name = "Reserva")]
        public decimal? Reserve { get; set; }

        [DataMember(Name = "Retroactivo")]
        public decimal? Retroactive { get; set; }

        [DataMember(Name = "Tipo")]
        public string Type { get; set; }

        [DataMember(Name = "TipoProceso")]
        public string ProcessType { get; set; }

        [DataMember(Name = "TipoTransaccion")]
        public string TransactionType { get; set; }
    }
}
