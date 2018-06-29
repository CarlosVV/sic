namespace Nagnoi.SiC.Web.Models.Case
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Address;
    using Beneficiary;
    using Domain.Core.Model;
    using Employer;
    using Infrastructure.Core.Helpers;

    #endregion

    public class CaseViewModel
    {
        #region Constructor

        public CaseViewModel()
        {
            Patrono = new EmployerViewModel();
            Direccion = new AddressViewModel();
            Beneficiario = new BeneficiaryViewModel();
        }

        #endregion

        #region Properties

        public int CaseId { get; set; }

        public int CaseDetailId { get; set; }

        public string CaseKey { get; set; }

        public int EntityId { get; set; }

        public string NumeroCaso { get; set; }

        public string NumeroCasoMostrado { get; set; }

        public string Lesionado { get; set; }

        public string SSN { get; set; }

        public string EBT { get; set; }

        public string FechaNacimiento { get; set; }

        public string FechaRadicacion { get; set; }

        public string FechaAccidente { get; set; }

        public string Region { get; set; }

        public string Dispensario { get; set; }

        public bool EsBeneficiario { get; set; }

        public bool EsLesionado { get; set; }

        public EmployerViewModel Patrono { get; set; }

        public AddressViewModel Direccion { get; set; }

        public bool CasoMenor { get; set; }

        public bool CasoMuerte { get; set; }

        public string TipoIncapacidad { get; set; }

        public bool TienePerentorio { get; set; }

        public bool TieneInversionMenor3 { get; set; }

        public bool TienePagoTercerosVigente { get; set; }

        public decimal? Balance { get; set; }

        public decimal? AdjudicacionInicial { get; set; }

        public BeneficiaryViewModel Beneficiario { get; set; }

        public int CivilStatusId { get; set; }

        public int CivilStatus { get; set; }

        public bool FechaAccidenteMenor1984 { get; set; }

        public int DiasSemana { get; set; }

        public decimal CompSemanal { get; set; }

        public decimal CompSemanalInca { get; set; }

        public string EBTStatus { get; set; }
        public decimal? EBTBalance { get; set; }

        public string PaymentStatus { get; set; }

        public string FechaSuspension { get; set; }

        public string RazonSuspension {get; set;}

        public string FechaReanudacion { get; set; }

        public string BalanceFormateado { get; set; }

        public bool FromCase { get; set; }

        public int? CaseFolderId { get; set; }

        #endregion

        #region Public Methods

        public static IEnumerable<CaseViewModel> CreateFrom(IEnumerable<CaseDetail> caseDetails)
        {
            foreach (var caseDetail in caseDetails)
            {
                yield return CreateFrom(caseDetail, null);
            }
        }

        public static CaseViewModel CreateFrom(CaseDetail caseDetail, decimal? balance, CaseDetail caseMain = null, decimal? balanceLesionado = null)
        {
            var model = new CaseViewModel();

            DateTime now = DateTime.Now;
     
            model.CaseId = caseDetail.CaseId.Value;
            model.CaseDetailId = caseDetail.CaseDetailId;
            model.NumeroCaso = caseDetail.CaseNumber;
            model.NumeroCasoMostrado = string.Format("{0} {1}", caseDetail.CaseNumber, caseDetail.CaseKey);
            model.CaseKey = caseDetail.CaseKey;
            model.FechaRadicacion = caseDetail.Case.CaseDate.HasValue ? caseDetail.Case.CaseDate.Value.ToShortDateString() : string.Empty;
            model.FechaAccidente = caseDetail.Case.AccidentDate.HasValue ? caseDetail.Case.AccidentDate.Value.ToShortDateString() : string.Empty;
            model.Region = caseDetail.Case.Region == null ? string.Empty : caseDetail.Case.Region.Region1;
            model.Dispensario = caseDetail.Case.Clinic == null ? string.Empty : caseDetail.Case.Clinic.Clinic1;
            model.CasoMenor = caseDetail.Case.IsMinor.GetValueOrDefault(false);
            model.CasoMuerte = caseDetail.Case.DeathFlag.GetValueOrDefault(false);
            model.TipoIncapacidad = caseDetail.Case.Concept == null ? string.Empty : caseDetail.Case.Concept.Concept1;
            model.TienePerentorio = caseDetail.Transactions.Any(t => t.TransactionTypeId == 4);
            model.TieneInversionMenor3 = caseDetail.Transactions.Any(t => t.TransactionTypeId == 3 && (t.TransactionDate.HasValue && t.TransactionDate.Value.AddYears(3) > now));
            model.FechaAccidenteMenor1984 = caseDetail.Case.AccidentDate.HasValue ? (caseDetail.Case.AccidentDate.Value < new DateTime(1984, 5, 30)) : false;
            model.TienePagoTercerosVigente = caseDetail.ThirdPartySchedules.Any(t => !t.TerminationDate.HasValue || (t .TerminationDate.Value < now));
            model.DiasSemana = caseDetail.Case.DaysWeek.GetValueOrDefault(0);
            model.CompSemanal = caseDetail.Case.WeeklyComp.GetValueOrDefault(decimal.Zero);
            model.CompSemanalInca = caseDetail.Case.WeeklyCompDisability.GetValueOrDefault(decimal.Zero);            
            model.FechaSuspension = caseDetail.CancellationDate.HasValue ? caseDetail.CancellationDate.Value.ToShortDateString() : string.Empty;
            model.RazonSuspension = caseDetail.Cancellation.IsNull() ? string.Empty : caseDetail.Cancellation.Cancellation1;
            model.FechaReanudacion = caseDetail.RestartDate.HasValue ? caseDetail.RestartDate.Value.ToShortDateString() : string.Empty;
            model.FromCase = caseDetail.CaseFolderId.HasValue ? true : false;
            model.CaseFolderId = caseDetail.CaseFolderId.IsNull() ? 0 : caseDetail.CaseFolderId ;
           

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

            if (caseDetail.Entity == null)
            {
                model.Lesionado = string.Empty;
                model.SSN = string.Empty;
                model.FechaNacimiento = string.Empty;
                model.EsBeneficiario = false;
                model.EsLesionado = false;
            }
            else
            {
                model.EntityId = caseDetail.EntityId.Value;
                model.EsLesionado = caseDetail.Entity.ParticipantTypeId == 8;
                model.EsBeneficiario = caseDetail.Entity.ParticipantTypeId == 4;


                if (model.EsBeneficiario && !caseMain.IsNull()) {
                    model.Lesionado = caseMain.Entity.FullName;
                    model.SSN = caseMain.Entity.SSN.ToSSN();
                    model.FechaNacimiento = caseMain.Entity.BirthDate.HasValue ? caseMain.Entity.BirthDate.Value.ToShortDateString() : string.Empty;

                    if (caseMain.Entity.Addresses.Any()) {
                        var addressPostal = caseMain.Entity.Addresses.Where(a => a.AddressTypeId == 2).FirstOrDefault();
                        if (addressPostal != null) {
                            model.Direccion = AddressViewModel.CreateFrom(addressPostal);
                        }
                    } else {
                        model.Direccion = new AddressViewModel();
                    }

                    model.EBT = caseMain.EBTAccount.IsNull() ? string.Empty : caseMain.EBTAccount;
                    model.EBTStatus = caseMain.EBTStatus.IsNull() ? string.Empty : caseMain.EBTStatus;
                    model.EBTBalance = caseMain.EBTBalance.IsNull() ? 0 : caseMain.EBTBalance;
                    model.BalanceFormateado = balanceLesionado.HasValue ? balanceLesionado.Value.ToCurrency() : string.Empty;

                } else {

                model.Lesionado = caseDetail.Entity.FullName;
                model.SSN = caseDetail.Entity.SSN.ToSSN();
                model.FechaNacimiento = caseDetail.Entity.BirthDate.HasValue ? caseDetail.Entity.BirthDate.Value.ToShortDateString() : string.Empty;

                    if (caseDetail.Entity.Addresses.Any()) {
                    var addressPostal = caseDetail.Entity.Addresses.Where(a => a.AddressTypeId == 2).FirstOrDefault();
                        if (addressPostal != null) {
                        model.Direccion = AddressViewModel.CreateFrom(addressPostal);
                    }
                    } else {
                    model.Direccion = new AddressViewModel();
                }
                    model.EBT = caseDetail.EBTAccount.IsNull() ? string.Empty : caseDetail.EBTAccount;
                    model.EBTStatus = caseDetail.EBTStatus.IsNull() ? string.Empty : caseDetail.EBTStatus;
                    model.EBTBalance = caseDetail.EBTBalance.IsNull() ? 0 : caseDetail.EBTBalance;
                    model.BalanceFormateado = balanceLesionado.HasValue ? balanceLesionado.Value.ToCurrency() : string.Empty;
                }
            }
            
            model.Patrono = EmployerViewModel.CreateFrom(caseDetail.Case);
            model.Beneficiario = BeneficiaryViewModel.CreateFrom(caseDetail, balance);
            model.Balance = balance;            
            model.AdjudicacionInicial = caseDetail.GetInitialAllocation();
            model.TieneAbogado = !caseDetail.EntityId_Lawyer.IsNull();

            return model;
        }

        #endregion

        public bool TieneAbogado { get; set; }
    }
}