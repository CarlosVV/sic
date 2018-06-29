namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CDI.WebApplication.DataTables.Result;
    using Domain.Core.Model;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Web.Controllers;
    using Models.Approval;

    #endregion

    public class ApprovalController : BaseController
    {
        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel { Title = "Aprobaciones y Rechazos de Pagos", Subtitle = "Aprobaciones" };


            var roles = WebHelper.GetRolesForUser();

            if (!roles.IsNullOrEmpty())
            {
                if (roles.Length > 0)
                {
                    var indexCompensation = Array.FindIndex(roles, x => x.Contains("SIC_Compensation_Officer"));
                    model.Title = "Transacciones de Pagos";
                    model.Subtitle = "Transacciones";
                    ViewBag.Title = "Transacciones";

                    var indexComensationSupervisor = Array.FindIndex(roles, x => x.Contains("SIC_Compensation_Supervisor"));
                    model.Title = indexComensationSupervisor > 0 ? "Aprobaciones y Rechazos de Pagos" : "Transacciones de Pagos";
                    model.Subtitle = indexComensationSupervisor > 0 ? "Aprobaciones" : "Transacciones";
                    ViewBag.Title = "Aprobaciones y Rechazos de Pagos";
                }
            }

            var regions = LocationService.GetAllRegions();
            var clinics = LocationService.GetAllClinics();
            var cities = LocationService.GetAllCities();
            var paymentStatuses = PaymentService.GetAllPaymentStatuses();

            return View(model.Rebuild(model, regions, cities, clinics, paymentStatuses));
        }

        [HttpPost]
        public JsonResult FindTransactionsIpp(SearchViewModel model)
        {
            var result = new List<ApprovalIppModel>();

            int? conceptId = ConceptService.GetConceptByCode(((int)PaymentConceptEnum.IPP).ToString()).ConceptId;

            var transactions = TransactionService.FindTransactionsToApprove(model.EBTNumber, model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.DispensaryId, model.From, model.To, model.DocumentType, model.StatusId, conceptId).ToList();

            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = true; PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");

            foreach (var transaction in transactions)
            {
                if (transaction.CaseDetail.CaseId == null) continue;
                if (transaction.CaseDetailId == null) continue;
                if (transaction.TransactionDate == null) continue;
                var approvalIppModel = new ApprovalIppModel
                {
                    TransactionId = transaction.TransactionId,
                    CaseId = transaction.CaseDetail.CaseId.Value,
                    CaseNumber = transaction.CaseDetail.CaseNumber,
                    CaseDetailId = transaction.CaseDetailId.Value,
                    Lesionado = transaction.CaseDetail.Entity.FullName,
                    Ssn = transaction.CaseDetail.Entity.SSN,
                    FechaAdjudicacion = transaction.DecisionDate.HasValue ? transaction.TransactionDate.Value.ToShortDateString() : string.Empty,
                    TipoAdjudicacion = transaction.TransactionTypeId.HasValue ? transaction.TransactionType.TransactionType1 : string.Empty,
                    CantidadAdjudicada = transaction.TransactionAmount.ToCurrency(),
                    PagoInicial = 1500m.ToCurrency(),
                    Mensualidad = transaction.MonthlyInstallment.GetValueOrDefault(decimal.Zero).ToCurrency(),
                    Semanas = transaction.NumberOfWeeks,
                    MensualidadesVencidas = decimal.Zero,
                    AllowAprobar = allowAprobar,
                    AllowCancelar = allowCancelar,
                    AllowEditar = allowEditar && !transaction.CaseDetail.CaseFolderId.HasValue,
                    AllowRechazar = allowRechazar,
                    AllowReversar = allowReversar,
                    RazonRechazo = ""
                };

                if (transaction.Payments.Any())
                {
                    approvalIppModel.TotalPagar = transaction.Payments.Sum(p => p.Amount).GetValueOrDefault(decimal.Zero).ToCurrency();

                    var firstPayment = transaction.Payments.FirstOrDefault();
                    if (firstPayment != null)
                    {
                        approvalIppModel.Estado = firstPayment.Status.Status1;
                        approvalIppModel.PaymentId = firstPayment.PaymentId;
                        if (firstPayment.StatusId != null) approvalIppModel.StatusId = firstPayment.StatusId.Value;
                    }
                }
                else
                {
                    approvalIppModel.TotalPagar = decimal.Zero.ToCurrency();
                    approvalIppModel.Estado = string.Empty;
                }

                result.Add(approvalIppModel);
            }

            return JsonDataTable(result);
        }

        public JsonResult FindTransactionsItp(SearchViewModel model)
        {

            var result = new List<ApprovalItpModel>();

            int? conceptId = ConceptService.GetConceptByCode(((int)PaymentConceptEnum.ITP).ToString()).ConceptId;

            var transactions = TransactionService.FindTransactionsToApprove(model.EBTNumber, model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.DispensaryId, model.From, model.To, model.DocumentType, model.StatusId, conceptId).ToList();

            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");

            foreach (var transaction in transactions)
            {
                if (transaction.CaseDetail.CaseId == null) continue;
                if (transaction.CaseDetailId == null) continue;
                if (transaction.TransactionDate == null) continue;
                var approvalItpModel = new ApprovalItpModel
                {
                    TransactionId = transaction.TransactionId,
                    CaseId = transaction.CaseDetail.CaseId.Value,
                    CaseNumber = transaction.CaseDetail.CaseNumber,
                    CaseDetailId = transaction.CaseDetailId.Value,
                    Lesionado = transaction.CaseDetail.Entity.FullName,
                    Ssn = transaction.CaseDetail.Entity.SSN,
                    FechaAdjudicacion = transaction.DecisionDate.HasValue ? transaction.TransactionDate.Value.ToShortDateString() : string.Empty,
                    Mensualidad = transaction.CaseDetail.MonthlyInstallment.GetValueOrDefault(decimal.Zero).ToCurrency(),
                    Reserva = transaction.CaseDetail.Reserve.GetValueOrDefault(decimal.Zero).ToCurrency(),
                    NoCase = transaction.CaseDetail.CaseFolderId.HasValue,
                    AllowAprobar = allowAprobar,
                    AllowCancelar = allowCancelar,
                    AllowEditar = allowEditar && !transaction.CaseDetail.CaseFolderId.HasValue,
                    AllowRechazar = allowRechazar,
                    AllowReversar = allowReversar,
                };

                if (transaction.Payments.Any())
                {
                    approvalItpModel.TotalPagar = transaction.Payments.Sum(p => p.Amount).GetValueOrDefault(decimal.Zero).ToCurrency();

                    var firstPayment = transaction.Payments.FirstOrDefault();
                    if (firstPayment != null)
                    {
                        approvalItpModel.Estado = firstPayment.Status.Status1;
                        approvalItpModel.PaymentId = firstPayment.PaymentId;
                        if (firstPayment.StatusId != null) approvalItpModel.StatusId = firstPayment.StatusId.Value;
                    }
                }
                else
                {
                    approvalItpModel.TotalPagar = decimal.Zero.ToCurrency();
                    approvalItpModel.Estado = string.Empty;
                }

                result.Add(approvalItpModel);
            }

            return JsonDataTable(result);
        }

        public JsonResult FindTransactionsPostMortemItp(SearchViewModel model)
        {

            var result = new List<ApprovalPostMortemItpModel>();

            int? conceptId = ConceptService.GetConceptByCode(((int)PaymentConceptEnum.PostMortemITP).ToString()).ConceptId;

            var transactions = TransactionService.FindTransactionsToApprove(model.EBTNumber, model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.DispensaryId, model.From, model.To, model.DocumentType, model.StatusId, conceptId).ToList();

            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");

            foreach (var transaction in transactions)
            {
                if (transaction.CaseDetail.CaseId == null) continue;
                if (transaction.CaseDetailId == null) continue;
                if (transaction.TransactionDate == null) continue;
                var approvalItpModel = new ApprovalPostMortemItpModel
                {
                    TransactionId = transaction.TransactionId,
                    CaseId = transaction.CaseDetail.CaseId.Value,
                    CaseNumber = transaction.CaseDetail.CaseNumber,
                    CaseDetailId = transaction.CaseDetailId.Value,
                    Lesionado = transaction.CaseDetail.Entity.FullName,
                    Ssn = transaction.CaseDetail.Entity.SSN,
                    FechaAdjudicacion = transaction.DecisionDate.HasValue ? transaction.TransactionDate.Value.ToShortDateString() : string.Empty,
                    Mensualidad = transaction.CaseDetail.MonthlyInstallment.GetValueOrDefault(decimal.Zero).ToCurrency(),
                    Reserva = transaction.CaseDetail.Reserve.GetValueOrDefault(decimal.Zero).ToCurrency(),
                    NoCase = transaction.CaseDetail.CaseFolderId.HasValue,
                    AllowAprobar = allowAprobar,
                    AllowCancelar = allowCancelar,
                    AllowEditar = allowEditar && !transaction.CaseDetail.CaseFolderId.HasValue,
                    AllowRechazar = allowRechazar,
                    AllowReversar = allowReversar,
                };

                if (transaction.Payments.Any())
                {
                    approvalItpModel.TotalPagar = transaction.Payments.Sum(p => p.Amount).GetValueOrDefault(decimal.Zero).ToCurrency();

                    var firstPayment = transaction.Payments.FirstOrDefault();
                    if (firstPayment != null)
                    {
                        approvalItpModel.Estado = firstPayment.Status.Status1;
                        approvalItpModel.PaymentId = firstPayment.PaymentId;
                        if (firstPayment.StatusId != null) approvalItpModel.StatusId = firstPayment.StatusId.Value;
                    }
                }
                else
                {
                    approvalItpModel.TotalPagar = decimal.Zero.ToCurrency();
                    approvalItpModel.Estado = string.Empty;
                }

                result.Add(approvalItpModel);
            }

            return JsonDataTable(result);
        }

        [HttpPost]
        public JsonResult FindTransactionsInvestment(SearchViewModel model)
        {
            var result = new List<ApprovalInvestmentModel>();
            var transactions = TransactionService.FindTransactionsToApprove(model.EBTNumber, model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.DispensaryId, model.From, model.To, model.DocumentType, model.StatusId).ToList();

            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");


            foreach (var transaction in transactions)
            {
                if (transaction.CaseDetail.CaseId == null) continue;
                if (transaction.CaseDetailId == null) continue;
                var approvalInvestmentModel = new ApprovalInvestmentModel
                {
                    TransactionId = transaction.TransactionId,
                    CaseId = transaction.CaseDetail.CaseId.Value,
                    CaseNumber = transaction.CaseDetail.CaseNumber,
                    CaseDetailId = transaction.CaseDetailId.Value,
                    Lesionado = transaction.CaseDetail.Entity.FullName,
                    Ssn = transaction.CaseDetail.Entity.SSN,
                    FechaEmisionDecision = transaction.TransactionDate.HasValue ? transaction.TransactionDate.Value.ToShortDateString() : string.Empty,
                    RazonInversion = transaction.TransactionTypeId.HasValue ? transaction.TransactionType.TransactionType1 : string.Empty,
                    AllowAprobar = allowAprobar,
                    AllowCancelar = allowCancelar,
                    AllowEditar = allowEditar && !transaction.CaseDetail.CaseFolderId.HasValue,
                    AllowRechazar = allowRechazar,
                    AllowReversar = allowReversar,
                    Comments = transaction.Comment,
                    RazonRechazo = transaction.RejectedReason ?? string.Empty,
                    Estado = string.Empty
                };

                if (transaction.Payments.Any())
                {
                    approvalInvestmentModel.TotalInversion = transaction.Payments.Sum(p => p.Amount).GetValueOrDefault(decimal.Zero).ToCurrency();

                    var firstPayment = transaction.Payments.FirstOrDefault();

                    if (firstPayment != null)
                    {
                        approvalInvestmentModel.Estado = firstPayment.Status.Status1;
                        if (firstPayment.StatusId != null)
                            approvalInvestmentModel.StatusId = firstPayment.StatusId.Value;
                    }
                    approvalInvestmentModel.Pagos = transaction.Payments.Select(p => p.EntityId_RemitTo != null ? new ApprovalInvestmentPaymentModel
                    {
                        PaymentId = p.PaymentId,
                        EntityId = p.EntityId_RemitTo.Value,
                        PagoDirigidoA = p.Remitter.FullName,
                        Importe = p.Amount.GetValueOrDefault(decimal.Zero).ToCurrency()
                    } : null).ToList();
                }
                else
                {
                    approvalInvestmentModel.TotalInversion = decimal.Zero.ToCurrency();
                    approvalInvestmentModel.Estado = string.Empty;
                }

                result.Add(approvalInvestmentModel);
            }

            return JsonDataTable(result);
        }

        [HttpPost]
        public JsonResult FindTransactionsPeremptory(SearchViewModel model)
        {
            var result = new List<ApprovalPeremptoryModel>();
            var transactions = TransactionService.FindTransactionsToApprove(model.EBTNumber, model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.DispensaryId, model.From, model.To, model.DocumentType, model.StatusId).ToList();

            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");

            foreach (var transaction in transactions)
            {
                if (transaction.CaseDetail.CaseId == null) continue;
                if (transaction.CaseDetailId == null) continue;
                var approvalPeremptoryModel = new ApprovalPeremptoryModel
                {
                    TransactionId = transaction.TransactionId,
                    CaseId = transaction.CaseDetail.CaseId.Value,
                    CaseNumber = transaction.CaseDetail.CaseNumber,
                    CaseDetailId = transaction.CaseDetailId.Value,
                    Lesionado = this.CaseService.FindCaseDetailByIdAndKey(transaction.CaseDetail.CaseId.Value, "00").Entity.FullName,
                    Ssn = transaction.CaseDetail.Entity.SSN,
                    FechaDecision = transaction.DecisionDate.HasValue ? transaction.TransactionDate.Value.ToShortDateString() : string.Empty,
                    Fecha = transaction.TransactionDate.HasValue ? transaction.TransactionDate.Value.ToShortDateString() : string.Empty,
                    Beneficiario = transaction.CaseDetail.Entity.ParticipantTypeId == 4 ? transaction.CaseDetail.Entity.FullName : string.Empty,
                    Relacion = transaction.CaseDetail.RelationshipTypeId.HasValue ? transaction.CaseDetail.RelationshipType.RelationshipType1 : string.Empty,
                    Cantidad = transaction.TransactionAmount.ToCurrency(),
                    AllowAprobar = allowAprobar,
                    AllowCancelar = allowCancelar,
                    AllowEditar = allowEditar && !transaction.CaseDetail.CaseFolderId.HasValue,
                    AllowRechazar = allowRechazar,
                    AllowReversar = allowReversar,
                    RazonRechazo = transaction.RejectedReason
                };

                if (transaction.Payments.Any())
                {
                    var firstPayment = transaction.Payments.FirstOrDefault();
                    if (firstPayment != null)
                    {
                        approvalPeremptoryModel.Estado = firstPayment.Status.Status1;
                        approvalPeremptoryModel.PaymentId = firstPayment.PaymentId;
                        if (firstPayment.StatusId != null)
                            approvalPeremptoryModel.StatusId = firstPayment.StatusId.Value;
                    }
                }
                else
                {
                    approvalPeremptoryModel.Estado = string.Empty;
                }

                result.Add(approvalPeremptoryModel);
            }

            return JsonDataTable(result);
        }

        [HttpPost]
        public JsonResult FindTransactionsPendingDiet(SearchViewModel model)
        {
            var result = new List<ApprovalPendingDietModel>();

            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");

            int? conceptId = ConceptService.GetConceptByCode(((int)PaymentConceptEnum.Dieta).ToString()).ConceptId;

            var payments = PaymentService.FindPaymentsToApprove(model.EBTNumber, model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.DispensaryId, model.From, model.To, model.StatusId, conceptId).ToList();
            foreach (var payment in payments)
            {
                if (payment.CaseId == null) continue;
                if (payment.CaseDetailId != null)
                    result.Add(new ApprovalPendingDietModel
                    {
                        CaseId = payment.CaseId.Value,
                        CaseDetailId = payment.CaseDetailId.Value,
                        CaseNumber = payment.CaseNumber,
                        Lesionado = payment.CaseDetail.Entity.FullName,
                        Ssn = payment.CaseDetail.Entity.SSN,
                        FechaNacimiento = payment.CaseDetail.Entity.BirthDate.ToShortDateString(),
                        FechaDecision = payment.IssueDate.ToShortDateString(),
                        Desde = payment.FromDate.ToShortDateString(),
                        Hasta = payment.ToDate.ToShortDateString(),
                        NroDias = payment.PaymentDay.HasValue ? payment.PaymentDay.ToString() : string.Empty,
                        Jornal = payment.Case.DailyWage.ToCurrency(),
                        CompSemanal = payment.Case.WeeklyComp.ToCurrency(),
                        Dieta = payment.Amount.ToCurrency(),
                        Estado = payment.Status.Status1,
                        RazonRechazo = payment.Transaction.IsNull() ? string.Empty : payment.Transaction.RejectedReason,
                        TransactionId = payment.TransactionId,
                        PaymentId = payment.PaymentId,
                        DiasSemana = payment.Case.DaysWeek ?? 0,
                        StatusId = payment.StatusId.Value,
                        AllowAprobar = allowAprobar,
                        AllowCancelar = allowCancelar,
                        AllowEditar = allowEditar && !payment.CaseDetail.CaseFolderId.HasValue,
                        AllowRechazar = allowRechazar,
                        AllowReversar = allowReversar,
                    });
            }

            return JsonDataTable(result);
        }

        [HttpPost]
        public JsonResult FindTransactionsThirdPayment(SearchViewModel model)
        {
            int? conceptId = null; //ConceptService.GetConceptByCode(((int)PaymentConceptEnum.PagoTerceros).ToString()).ConceptId;
            var result = new List<ApprovalThirdPaymentModel>();
            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");

            var thirdpartyPaymentsToApprove = PaymentService.FindThirdPartyPaymentsToApprove(
                model.EBTNumber, model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate,
                model.FilingDate, model.RegionId, model.DispensaryId, model.From, model.To, model.DocumentType, model.StatusId, conceptId).ToList();


            foreach (var thirdPartyPayment in thirdpartyPaymentsToApprove)
            {
                var payments = thirdPartyPayment.Payments;
                if (thirdPartyPayment.CaseDetail.CaseId == null) continue;
                if (thirdPartyPayment.CaseDetailId == null) continue;

                var query = PaymentService.FindThirdPartyPaymentById(thirdPartyPayment.ThirdPartyScheduleId);
                var newrow =
                    new ApprovalThirdPaymentModel
                    {
                        ThirdPartyScheduleId = thirdPartyPayment.ThirdPartyScheduleId,
                        TransactionId = 0,
                        PaymentId = 0,
                        CaseId = thirdPartyPayment.CaseDetail.CaseId.Value,
                        CaseNumber = thirdPartyPayment.CaseDetail.CaseNumber,
                        CaseDetailId = thirdPartyPayment.CaseDetailId.Value,
                        Lesionado = thirdPartyPayment.CaseDetail.Entity.FullName,
                        Ssn = thirdPartyPayment.CaseDetail.Entity.SSN,
                        Custodio = query.Remitter == null ? string.Empty : query.Remitter.FullName,
                        TotalPagar = thirdPartyPayment.OrderAmount.HasValue ? thirdPartyPayment.OrderAmount.GetValueOrDefault(decimal.Zero).ToCurrency() : string.Empty,
                        FechaDecision = thirdPartyPayment.CreatedDateTime.HasValue ? thirdPartyPayment.CreatedDateTime.Value.ToShortDateString() : string.Empty,
                        Estado = "Programado",
                        AllowAprobar = allowAprobar,
                        AllowCancelar = allowCancelar,
                        AllowEditar = allowEditar && !thirdPartyPayment.CaseDetail.CaseFolderId.HasValue,
                        AllowRechazar = allowRechazar,
                        AllowReversar = allowReversar,
                    };

                if (payments != null && payments.Count == 0)
                {
                    newrow.AllowReversar =
                        newrow.AllowAprobar =
                            newrow.AllowRechazar = true;
                    result.Add(newrow);
                }
                else
                {
                    foreach (var payment in thirdPartyPayment.Payments)
                    {
                        newrow.PaymentId = payment.PaymentId;
                        newrow.Estado = payment.Status!=null ? payment.Status.Status1 : "Programado";
                        newrow.RazonRechazo = "";
                        if (payment.StatusId != null) newrow.StatusId = payment.StatusId.Value;
                        result.Add(newrow);
                    }
                }
            }

            return JsonDataTable(result);
        }

        [HttpPost]
        public JsonResult FindTransactionsLawyer(SearchViewModel model)
        {
            var result = new List<ApprovalLawyerModel>();
            var transactions = TransactionService.FindTransactionsToApprove(model.EBTNumber, model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.DispensaryId, model.From, model.To, model.DocumentType, model.StatusId).ToList();

            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");

            foreach (var transaction in transactions)
            {
                if (transaction.CaseDetail.CaseId == null) continue;
                if (transaction.CaseDetailId == null) continue;
                if (transaction.TransactionDate == null) continue;
                var approvalLawyerModel = new ApprovalLawyerModel
                {
                    TransactionId = transaction.TransactionId,
                    CaseId = transaction.CaseDetail.CaseId.Value,
                    CaseNumber = transaction.CaseDetail.CaseNumber,
                    CaseDetailId = transaction.CaseDetailId.Value,
                    Lesionado = transaction.CaseDetail.Entity.FullName,
                    Ssn = transaction.CaseDetail.Entity.SSN,
                    FechaDecision = transaction.DecisionDate.HasValue ? transaction.TransactionDate.Value.ToShortDateString() : string.Empty,
                    Abogado = transaction.CaseDetail.EntityId_Lawyer.HasValue ? transaction.CaseDetail.EntityLawyer.FullName : string.Empty,
                    FechaNotificacion = transaction.NotificationDateIC.HasValue ? transaction.NotificationDateIC.ToShortDateString() : string.Empty,
                    NumeroCasoCI = transaction.ICCaseNumber,
                    TotalPagar = transaction.TransactionAmount.ToCurrency(),
                    AllowAprobar = allowAprobar,
                    AllowCancelar = allowCancelar,
                    AllowEditar = allowEditar && !transaction.SourceId.HasValue ? (transaction.SourceId.Value == (int)ProcessSourceEnum.SicTransaction) ? true : false : false,
                    AllowRechazar = allowRechazar,
                    AllowReversar = allowReversar,
                    RazonRechazo = ""
                };

                if (transaction.Payments.Any())
                {
                    var firstPayment = transaction.Payments.FirstOrDefault();
                    if (firstPayment != null)
                    {
                        approvalLawyerModel.Estado = firstPayment.Status.Status1;
                        approvalLawyerModel.RazonRechazo = "";
                        approvalLawyerModel.PaymentId = firstPayment.PaymentId;
                        if (firstPayment.StatusId != null) approvalLawyerModel.StatusId = firstPayment.StatusId.Value;
                    }
                }
                else
                {
                    approvalLawyerModel.Estado = string.Empty;
                }

                result.Add(approvalLawyerModel);
            }

            return JsonDataTable(result);
        }

        [HttpPost]
        public JsonResult FindTransactionsDeath(SearchViewModel model)
        {

            var result = new List<ApprovalDeathModel>();
            int? conceptId = ConceptService.GetConceptByCode(((int)PaymentConceptEnum.Muerte).ToString()).ConceptId;

            var allowReversar = this.PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var allowAprobar = this.PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var allowRechazar = this.PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var allowCancelar = this.PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var allowEditar = this.PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Editar");

            var payments = PaymentService.FindPaymentsToApprove(model.EBTNumber, model.CaseNumber, model.EntityName, 
                model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.DispensaryId, 
                model.From, model.To, model.StatusId, conceptId).Where(p => p.Transaction.IsNull());
            var groupsCaseIds = payments.DistinctBy(p => p.CaseId);

            foreach (var parent in groupsCaseIds)
            {
                if (parent.CaseId == null) continue;
                var casedetail = CaseService.FindCaseDetailByIdAndKey(parent.CaseId.Value, parent.CaseKey);
                var lesionado = EntityService.GetById(casedetail.EntityId.Value);
                var groupPayments = payments.Where(x => x.CaseId != null && x.CaseId.Value == casedetail.CaseId);
                var transaction = TransactionService.FindTransactionById(parent.TransactionId.Value);

                if (casedetail.CaseId == null) continue;
                var approvalDeath = new ApprovalDeathModel
                {
                    CaseId = casedetail.CaseId.Value,
                    CaseDetailId = casedetail.CaseDetailId,
                    CaseNumber = casedetail.CaseNumber,
                    Lesionado = lesionado != null ? lesionado.FullName : string.Empty,
                    Ssn = lesionado != null ? lesionado.SSN : string.Empty,
                    FechaDecision = transaction != null && transaction.DecisionDate != null ? 
                        transaction.DecisionDate.ToShortDateString() : string.Empty,
                    FechaDefuncion = lesionado!= null && lesionado.DeceaseDate != null ? 
                        lesionado.DeceaseDate.ToShortDateString() : string.Empty,
                    AllowAprobar = true,
                    AllowCancelar = true,
                    AllowEditar = true,
                    AllowRechazar = true,
                    AllowReversar = true
                };

                foreach (var child in groupPayments)
                {
                    if (child.CaseId == null) continue;
                    if (child.CaseDetailId == null) continue;
                    if (child.StatusId == null) continue;                    
                    if (child.Amount != null)
                    {
                        var payment = PaymentService.FindPaymentById(child.PaymentId);
                        var entidad = EntityService.GetById(payment.CaseDetail.EntityId.Value);
                        var rel = RelationshipTypeService.GetRelationshipTypes().
                            Where(m => m.RelationshipCategoryId == child.CaseDetail.RelationshipTypeId).FirstOrDefault();
                        approvalDeath.Beneficiarios.Add(new ApprovalDeathBeneficiariesModel
                        {
                            CaseId = child.CaseId.Value,
                            CaseDetailId = child.CaseDetailId.Value,
                            CaseNumber = string.Format("{0} {1}", child.CaseNumber, child.CaseDetail != null ? 
                                child.CaseDetail.CaseKey : string.Empty),
                            TransactionId = child.TransactionId != null ? child.TransactionId.Value : 0,
                            Beneficiario = entidad.FullName,
                            Ssn = entidad.SSN,
                            FechaNacimiento = entidad.BirthDate.ToShortDateString(),
                            Relacion = rel.RelationshipType1,
                            Estudiante = entidad.IsStudying != null && entidad.IsStudying.Value ? "Si" : "No",
                            Tutor = string.Empty,
                            PagoInicial = child.Amount.Value.ToCurrency(),
                            Reserva = child.CaseDetail.Reserve.ToCurrency(),
                            Mensualidad = child.CaseDetail.MonthlyInstallment.ToCurrency(),
                            MensualidadesVencidas = (child.Amount / child.CaseDetail.MonthlyInstallment).ToString(),
                            TotalAPagar = child.Amount.ToCurrency(),
                            Estatus = child.Status.Status1,
                            EstatusId = child.StatusId.Value
                        });
                    }
                }
                result.Add(approvalDeath);
            }
            return JsonDataTable(result);
        }

        [HttpPost]
        public JsonResult Approve(int caseDetailId, int transactionId)
        {
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var status = string.Empty;

            if (allowAprobar)
            {
                TransactionService.ApproveTransactions(caseDetailId, transactionId, null, null);
                status = "OK";
            }
            else
                status = ResourceService.GetResourceString("Generico.Mensajes.ErrorPermiso");

            return Json(new BasicDataTablesResult(new { Status = status }));
        }

        [HttpPost]
        public JsonResult Reject(int caseDetailId, int transactionId, string reason)
        {
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var status = string.Empty;

            if (allowRechazar)
            {
                TransactionService.RejectTransactions(caseDetailId, transactionId, reason, null, null);
                status = "OK";
            }
            else
                status = ResourceService.GetResourceString("Generico.Mensajes.ErrorPermiso");

            return Json(new BasicDataTablesResult(new { Status = status }));
        }

        [HttpPost]
        public JsonResult Cancel(int caseDetailId, int transactionId)
        {
            var allowCancelar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Cancelar");
            var status = string.Empty;

            if (allowCancelar)
            {
                TransactionService.CancelTransactions(caseDetailId, transactionId);
                status = "OK";
            }
            else
                status = ResourceService.GetResourceString("Generico.Mensajes.ErrorPermiso");

            return Json(new BasicDataTablesResult(new { Status = status }));
        }

        [HttpPost]
        public JsonResult Reverse(int caseDetailId, int transactionId, string reason)
        {
            var allowReversar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Reversar");
            var status = string.Empty;

            if (allowReversar)
            {
                TransactionService.ReverseTransactions(caseDetailId, transactionId, reason);
                status = "OK";
            }
            else
                status = ResourceService.GetResourceString("Generico.Mensajes.ErrorPermiso");

            return Json(new BasicDataTablesResult(new { Status = status }));
        }

        [HttpPost]
        public JsonResult ApprovePendingDiets(int caseDetailId)
        {
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var status = string.Empty;

            if (allowAprobar)
            {
                TransactionService.ApprovePendingDiets(caseDetailId);
                status = "OK";
            }
            else
                status = ResourceService.GetResourceString("Generico.Mensajes.ErrorPermiso");

            return Json(new BasicDataTablesResult(new { Status = status }));
        }

        [HttpPost]
        public JsonResult RejectPendingDiets(int caseDetailId, string reason)
        {
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Rechazar");
            var status = string.Empty;

            if (allowRechazar)
            {
                TransactionService.RejectPendingDiets(caseDetailId, reason);
                status = "OK";
            }
            else
                status = ResourceService.GetResourceString("Generico.Mensajes.ErrorPermiso");


            return Json(new BasicDataTablesResult(new { Status = status }));
        }

        [HttpPost]
        public JsonResult ApproveThirdPartyPayment(int paymentId, int thirdPartyScheduleId)
        {
            var allowAprobar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var status = string.Empty;

            if (allowAprobar)
            {
                var payment = PaymentService.FindPaymentById(paymentId);
                DeletePaymentsRelationsForUpdate(payment);
                payment.StatusId = (int)PaymentStatusEnum.Aprobado;
                PaymentService.ModifyPayment(payment);
                status = "OK";
            }
            else
                status = ResourceService.GetResourceString("Generico.Mensajes.ErrorPermiso");

            return Json(new BasicDataTablesResult(new { Status = status }));
        }

        [HttpPost]
        public JsonResult RejectThirdPartyPayment(int paymentId, int thirdPartyScheduleId)
        {
            var allowRechazar = PermissionService.IsFunctionalityAllowed("GestionarAprobaciones.Aprobar");
            var status = string.Empty;

            if (allowRechazar)
            {
                var payment = PaymentService.FindPaymentById(paymentId);
                DeletePaymentsRelationsForUpdate(payment);
                payment.StatusId = (int)PaymentStatusEnum.Cancelado;
                PaymentService.ModifyPayment(payment);
                status = "OK";
            }
            else
                status = ResourceService.GetResourceString("Generico.Mensajes.ErrorPermiso");

            return Json(new BasicDataTablesResult(new { Status = status }));
        }
        #endregion

        #region private
        private static void DeletePaymentsRelationsForUpdate(Payment payment)
        {
            payment.AdjustmentStatus = null;
            payment.AdjustmentType = null;
            payment.Case = null;
            payment.CaseDetail = null;
            payment.Class = null;
            payment.Clinic = null;
            payment.Concept = null;
            payment.KeyRiskIndicator = null;
            payment.Region = null;
            payment.Remitter = null;
            payment.Status = null;
            payment.ThirdPartySchedule = null;
            payment.Transaction = null;
            payment.TransferType = null;
        }
        #endregion
    }
}