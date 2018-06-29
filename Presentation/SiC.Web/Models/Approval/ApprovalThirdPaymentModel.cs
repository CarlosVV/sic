namespace Nagnoi.SiC.Web.Models.Approval
{
    public class ApprovalThirdPaymentModel : AllowOperationsModel
    {
        #region Properties

        public int ThirdPartyScheduleId { get; set; }

        public int TransactionId { get; set; }

        public int CaseId { get; set; }

        public int CaseDetailId { get; set; }

        public string CaseNumber { get; set; }

        public string Lesionado { get; set; }

        public string Ssn { get; set; }

        public string Custodio { get; set; }

        public string FechaPago { get; set; }

        public string TotalPagar { get; set; }

        public string Estado { get; set; }

        public string FechaDecision { get; set; }

        #endregion       
    
        public int PaymentId { get; set; }

        public int StatusId { get; set; }

        public string RazonRechazo { get; set; }
    }
}