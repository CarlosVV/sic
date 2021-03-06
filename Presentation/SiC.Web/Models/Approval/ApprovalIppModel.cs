﻿namespace Nagnoi.SiC.Web.Models.Approval
{
    public class ApprovalIppModel : AllowOperationsModel
    {
        #region Properties

        public int TransactionId { get; set; }

        public int CaseId { get; set; }

        public string CaseNumber { get; set; }

        public int CaseDetailId { get; set; }

        public string Lesionado { get; set; }

        public string Ssn { get; set; }

        public string FechaAdjudicacion { get; set; }

        public string TipoAdjudicacion { get; set; }

        public string CantidadAdjudicada { get; set; }

        public string AdjudicacionAdicional { get; set; }

        public string PagoInicial { get; set; }

        public string Mensualidad { get; set; }

        public decimal? Semanas { get; set; }

        public decimal? MensualidadesVencidas { get; set; }

        public string TotalPagar { get; set; }

        public string Estado { get; set; }

        public int? PaymentId { get; set; }

        public int StatusId { get; set; }

        #endregion

        public string RazonRechazo { get; set; }
    }
}