namespace Nagnoi.SiC.Web.Models.Beneficiary
{
    #region References

    using Domain.Core.Model;
    using Infrastructure.Core.Helpers;

    #endregion

    public class BeneficiaryViewModel
    {
        #region Constructor

        public BeneficiaryViewModel()
        {
            Relacion = string.Empty;
            Nombre = string.Empty;
            SSN = string.Empty;
            FechaNacimiento = string.Empty;
            EBT = string.Empty;
            Sufijo = string.Empty;
            EBTStatus = string.Empty;
            EBTBalance = 0;
        }

        #endregion

        #region Properties

        public string Relacion { get; set; }

        public string Nombre { get; set; }

        public string SSN { get; set; }

        public string FechaNacimiento { get; set; }

        public string EBT { get; set; }

        public string Sufijo { get; set; }

        public string EBTStatus { get; set; }
        public decimal? EBTBalance { get; set; }

        public bool EsBeneficiarioValido { get; set; }

        public string PaymentStatus { get; set; }

        public string FechaSuspension { get; set; }

        public string RazonSuspension { get; set; }

        public string FechaReanudacion { get; set; }

        public string BalanceFormateado { get; set; }

        #endregion

        #region Public Methods

        public static BeneficiaryViewModel CreateFrom(CaseDetail caseDetail, decimal? balance = null)
        {
            var model = new BeneficiaryViewModel();

            model.EBT = caseDetail.EBTAccount.IsNull()? string.Empty: caseDetail.EBTAccount;
            model.EBTStatus = caseDetail.EBTStatus.IsNull() ? string.Empty : caseDetail.EBTStatus;
            model.EBTBalance = caseDetail.EBTBalance.IsNull() ? 0 : caseDetail.EBTBalance;            
            model.FechaSuspension = caseDetail.CancellationDate.HasValue ? caseDetail.CancellationDate.Value.ToShortDateString() : string.Empty;
            model.RazonSuspension = caseDetail.Cancellation.IsNull() ? string.Empty : caseDetail.Cancellation.Cancellation1;
            model.FechaReanudacion = caseDetail.RestartDate.HasValue ? caseDetail.RestartDate.Value.ToShortDateString() : string.Empty;
            model.BalanceFormateado = balance.HasValue ? balance.Value.ToCurrency() : string.Empty;

            if (caseDetail.CancellationDate.HasValue) {
                model.PaymentStatus = "Suspendido";
            } else {
                if (caseDetail.ActiveIdent.IsNullOrEmpty()) {
                    model.PaymentStatus = "Activo";
                } else {
                    if (caseDetail.ActiveIdent.Contains("A")) {
                        if (!caseDetail.Cancellation.IsNull()) {
                            if (caseDetail.Cancellation.CancellationCode.Contains("A") ||
                                caseDetail.Cancellation.CancellationCode.Contains("C") ||
                                caseDetail.Cancellation.CancellationCode.Contains("T")) {
                                model.PaymentStatus = "Suspendido";
                            } else {
                                model.PaymentStatus = "Activo";
                            }
                        } else {
                            model.PaymentStatus = "Activo";
                        }
                    } else if (caseDetail.ActiveIdent.Contains("I")) {
                        model.PaymentStatus = "Inactivo";
                    } else if (caseDetail.ActiveIdent.Contains("S")) {
                        model.PaymentStatus = "Suspendido";
                    }
                }
            }
            model.Sufijo = caseDetail.CaseKey;

            if (caseDetail.Entity != null)
            {
                model.Nombre = caseDetail.Entity.FullName;
                model.SSN = caseDetail.Entity.SSN.ToSSN();
                model.FechaNacimiento = caseDetail.Entity.BirthDate.HasValue ? caseDetail.Entity.BirthDate.Value.ToShortDateString() : string.Empty;
            }

            if (caseDetail.RelationshipType != null)
            {
                model.Relacion = caseDetail.RelationshipType.RelationshipType1;
            }

            if (model.Relacion.IndexOf("Viuda") >= 0 || model.Relacion.IndexOf("Concubina") >= 0 || model.Relacion.IndexOf("Padre") >= 0 || model.Relacion.IndexOf("Madre") >= 0) {
                model.EsBeneficiarioValido = true;
            } else {
                model.EsBeneficiarioValido = false;
            }

            return model;
        }

        #endregion
    }
}