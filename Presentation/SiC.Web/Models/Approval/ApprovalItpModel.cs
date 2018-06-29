namespace Nagnoi.SiC.Web.Models.Approval {
    public class ApprovalItpModel : AllowOperationsModel {
        public int TransactionId { get; set; }
        public int CaseId { get; set; }
        public string CaseNumber { get; set; }
        public int CaseDetailId { get; set; }
        public string Lesionado { get; set; }
        public string Ssn { get; set; }
        public string FechaAdjudicacion { get; set; }
        public string Mensualidad { get; set; }
        public string Reserva { get; set; }
        public string TotalPagar { get; set; }
        public string Estado { get; set; }
        public int? PaymentId { get; set; }
        public int StatusId { get; set; }
        public bool NoCase { get; set; }
    }
}