namespace Nagnoi.SiC.Web.Models.Adjustments
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Domain.Core.Model;
    using Infrastructure.Web.Utilities;
    using Nagnoi.SiC.Infrastructure.Web.ViewModels;

    #endregion

    public class IndexViewModel
    {
        #region Constructor

        public IndexViewModel()
        {
            AdjustmentTypes = new List<SelectListItem>();
            AdjustmentReasons = new List<SelectListItem>();
            TransactionTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int CaseDetailId { get; set; }

        [Display(Name = "Tipo de Ajuste")]
        public int AdjustmentTypeId { get; set; }

        public string AdjustmentReasonText { get; set; }

        public IList<SelectListItem> AdjustmentTypes { get; set; }

        [Display(Name = "Razón")]
        public int AdjustmentReasonId { get; set; }

        public IList<SelectListItem> AdjustmentReasons { get; set; }

        [Display(Name = "Tipo de Adjudicación")]
        public int? TransactionTypeId { get; set; }

        public IList<SelectListItem> TransactionTypes { get; set; }

        [Display(Name = "Mensualidad Actual")]
        public decimal? MonthlyInstallment { get; set; }

        [Display(Name = "Reserva Actual")]
        public decimal? CurrentBalance { get; set; }

        [Display(Name = "Nueva Adjudicación")]
        public decimal? NewAdjudicacion { get; set; }

        [Display(Name = "Efectivo en")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "Comentarios")]
        public string Comments { get; set; }

        public IList<SelectListItem> Cancellation { get; set; }

        [Display(Name = "Razón")]
        public int? CancellationId { get; set; }

        public CaseDetailViewModel  CaseModel { get; set; }
        #endregion

        #region Public Methods

        public IndexViewModel Rebuild(IndexViewModel model, IEnumerable<AdjustmentReason> adjustmentReasons, IEnumerable<TransactionType> transactionTypes, IEnumerable<Cancellation> cancellation)
        {
            if (model == null)
            {
                model = new IndexViewModel();
            }

            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "Seleccionar",
                Value = string.Empty
            });
            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "Ajuste de Mensualidad",
                Value = "1"
            });
            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "Ajuste de Reserva",
                Value = "2"
            });
            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "Re-Evaluación",
                Value = "3"
            });
            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "Suspender Pago",
                Value = "4"
            });
            model.AdjustmentTypes.Add(new SelectListItem {

                Text = "Reanudar Pago de Incapacidad",
                Value = "6"
            });
            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "Cierre de Balance",
                Value = "5"
            });
            model.AdjustmentReasons = adjustmentReasons.ToSelectList(a => a.AdjustmentReason1, a => a.AdjustmentReasonId.ToString(), null, null);
            model.TransactionTypes = transactionTypes.ToSelectList(t => t.TransactionType1, t => t.TransactionTypeId.ToString(), null, null);
            model.Cancellation = cancellation.ToSelectList(c => c.Cancellation1, c => c.CancellationId.ToString(), null, null);

            return model;
        }

        #endregion
    }
}