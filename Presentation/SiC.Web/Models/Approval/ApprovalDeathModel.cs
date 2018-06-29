namespace Nagnoi.SiC.Web.Models.Approval {

    #region References

    using System;
    using System.Collections.Generic;

    #endregion

    public class ApprovalDeathModel : AllowOperationsModel {

        #region Constructor
        public ApprovalDeathModel() {
            this.Beneficiarios = new List<ApprovalDeathBeneficiariesModel>();
            
        }
        #endregion

        #region Properties
        public int TransactionId { get; set; }
            public int CaseId { get; set; }
            public string CaseNumber { get; set; }
            public int CaseDetailId { get; set; }
            public string Lesionado { get; set; }
            public string Ssn { get; set; }
            public string FechaDefuncion { get; set; }
            public string FechaDecision { get; set; }
            public IList<ApprovalDeathBeneficiariesModel> Beneficiarios { get; set; }
        #endregion
    }

    public class ApprovalDeathBeneficiariesModel {

        #region Properties
        public int TransactionId { get; set; }
        public int CaseId { get; set; }
        public string CaseNumber { get; set; }
        public int CaseDetailId { get; set; }
        public string Beneficiario { get; set; }
        public string Ssn { get; set; }
        public string FechaNacimiento { get; set; }
        public string Relacion { get; set; }
        public string Estudiante { get; set; }
        public string Tutor { get; set; }
        public string PagoInicial { get; set; }
        public string Reserva { get; set; }
        public string Mensualidad { get; set; }
        public string MensualidadesVencidas { get; set; }
        public string TotalAPagar { get; set; }
        public string Estatus { get; set; }
        public int EstatusId { get; set; }
        #endregion
    }
}