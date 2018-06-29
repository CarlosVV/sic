namespace Nagnoi.SiC.Web.Models.Approval
{
    public class ApprovalPeremptoryModel : AllowOperationsModel
    {
        #region Properties

        public int TransactionId { get; set; }

        public int CaseId { get; set; }
        
        public string CaseNumber { get; set; }

        public int CaseDetailId { get; set; }

        public string Lesionado { get; set; }

        public string Ssn { get; set; }

        public string FechaDecision { get; set; }

        public string Fecha { get; set; }

        public string Beneficiario { get; set; }

        public string Relacion { get; set; }

        public string Estado { get; set; }

        public string Cantidad { get; set; }

        public int? PaymentId { get; set; }

        public int StatusId { get; set; }

        #endregion

        public string RazonRechazo { get; set; }
    }
}