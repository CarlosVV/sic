namespace Nagnoi.SiC.Web.Models.Approval
{
    public class ApprovalPendingDietModel : AllowOperationsModel
    {
        #region Properties

        public int CaseId { get; set; }

        public int CaseDetailId { get; set; }

        public string CaseNumber { get; set; }

        public string Lesionado { get; set; }

        public string Ssn { get; set; }

        public string FechaNacimiento { get; set; }
        
        public string FechaDecision { get; set; }

        public string Desde { get; set; }

        public string Hasta { get; set; }

        public string NroDias { get; set; }

        public string Jornal { get; set; }

        public string CompSemanal { get; set; }

        public string Dieta { get; set; }

        public string Estado { get; set; }

        public int? TransactionId { get; set; }

        public int PaymentId { get; set; }

        public int DiasSemana { get; set; }

        public int StatusId { get; set; }
        public string RazonRechazo { get; set; }

        #endregion
    }
}