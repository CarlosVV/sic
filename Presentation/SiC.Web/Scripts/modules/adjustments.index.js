CDI.AdjustmentsIndex = (function () {
    var _caseData;
    var _rules1 = {
        AdjustmentReasonId: {
            required: true
        },
        MonthlyInstallment: {
            required: true,
            lessThanZero: false
        }
    };
    var _rules2 = {
        AdjustmentReasonId: {
            required: true
        },
        CurrentBalance: {
            required: true
        }
    };
    var _rules3 = {
        AdjustmentReasonId: {
            required: true
        }
    };
    var _rules4 = {
        CancellationId: {
            required: true
        },
        FromDate: {
            required: true,
            dateUS: true
        }
    };
    var _rules5 = {
        AdjustmentReasonId: {
            required: true
        }
    };
    var _rules6 = {
        AdjustmentReasonId: {
            required: true
        },
        TransactionTypeId: {
            required: true
        },
        NewAdjudicacion: {
            required: true
        }
    };

    var _resetDetalleFields = function () {
        $("#MonthlyInstallment").autoNumeric('set', 0);
        $("#AdjustmentReasonId").val("");
        $("#CurrentBalance").autoNumeric('set', 0);
        $("#FromDate").val("");
        $("#CancellationId").val("");
        $("#TransactionTypeId").val("");
        $("#NewAdjudicacion").autoNumeric('set', 0);
        $("#Comments").val("");
        $("#btnClear").click();
    };

    var _bindUIActions = function () {
        $(CDI.CaseSearch).on('case.selected', function (e, data) {
            _caseData = data.selectedCase;

            _resetDetalleFields();
            $("#ajuste-form").fadeIn(1000).removeClass("hidden");
            $("#AdjustmentTypeId").val("");
        });

        $(CDI.CaseSearch).on('started', function (e, data) {
            $("#ajuste-form").fadeOut(1000).addClass("hidden");
            $("#AdjustmentTypeId").val("");
        });

        $(CDI.CaseSearch).on("cleaned", function () {
            $("#ajuste-form").fadeOut(1000).addClass("hidden");
            $("#AdjustmentTypeId").val("");
        });

        $("#AdjustmentTypeId").on("change", function () {
            var selectedValue = $(this).val();
            switch (selectedValue) {
                case "":
                    $("#detalle-panel").fadeOut(1000).addClass("hidden");
                    CDI.removeValidationRules(_rules1);
                    CDI.removeValidationRules(_rules2);
                    CDI.removeValidationRules(_rules3);
                    CDI.removeValidationRules(_rules4);
                    CDI.removeValidationRules(_rules5);
                    CDI.removeValidationRules(_rules6);
                    break;
                case "1":
                    $("#detalle-panel").fadeIn(1000).removeClass("hidden");

                    $("#monthly-installment-field").fadeIn(1000).removeClass("hidden");
                    $("#current-balance-field").fadeOut(1000).addClass("hidden");
                    $("#from-date-field").fadeOut(1000).addClass("hidden");
                    $("#transaction-type-field").fadeOut(1000).addClass("hidden");
                    $("#new-adjudicacion-field").fadeOut(1000).addClass("hidden");
                    $("#cancellation-field").fadeIn(1000).addClass("hidden");
                    $("#adjustment-reason-field").fadeIn(1000).removeClass("hidden");

                    CDI.addValidationRules(_rules1);
                    CDI.removeValidationRules(_rules2);
                    CDI.removeValidationRules(_rules3);
                    CDI.removeValidationRules(_rules4);
                    CDI.removeValidationRules(_rules5);
                    CDI.removeValidationRules(_rules6);
                    break;
                case "2":
                    $("#detalle-panel").fadeIn(1000).removeClass("hidden");

                    $("#monthly-installment-field").fadeOut(1000).addClass("hidden");
                    $("#current-balance-field").fadeIn(1000).removeClass("hidden");
                    $("#from-date-field").fadeOut(1000).addClass("hidden");
                    $("#transaction-type-field").fadeOut(1000).addClass("hidden");
                    $("#new-adjudicacion-field").fadeOut(1000).addClass("hidden");
                    $("#cancellation-field").fadeIn(1000).addClass("hidden");
                    $("#adjustment-reason-field").fadeIn(1000).removeClass("hidden");

                    CDI.removeValidationRules(_rules1);
                    CDI.addValidationRules(_rules2);
                    CDI.removeValidationRules(_rules3);
                    CDI.removeValidationRules(_rules4);
                    CDI.removeValidationRules(_rules5);
                    CDI.removeValidationRules(_rules6);
                    break;
                case "3":
                    $("#detalle-panel").fadeIn(1000).removeClass("hidden");

                    $("#monthly-installment-field").fadeOut(1000).addClass("hidden");
                    $("#current-balance-field").fadeOut(1000).addClass("hidden");
                    $("#from-date-field").fadeOut(1000).addClass("hidden");
                    $("#transaction-type-field").fadeOut(1000).addClass("hidden");
                    $("#new-adjudicacion-field").fadeOut(1000).addClass("hidden");
                    $("#cancellation-field").fadeIn(1000).addClass("hidden");
                    $("#adjustment-reason-field").fadeIn(1000).removeClass("hidden");

                    CDI.removeValidationRules(_rules1);
                    CDI.removeValidationRules(_rules2);
                    CDI.addValidationRules(_rules3);
                    CDI.removeValidationRules(_rules4);
                    CDI.removeValidationRules(_rules5);
                    CDI.removeValidationRules(_rules6);
                    break;
                case "4":
                    $("#detalle-panel").fadeIn(1000).removeClass("hidden");

                    $("#monthly-installment-field").fadeOut(1000).addClass("hidden");
                    $("#current-balance-field").fadeOut(1000).addClass("hidden");
                    $("#from-date-field").fadeIn(1000).removeClass("hidden");
                    $("#adjustment-reason-field").fadeIn(1000).addClass("hidden");
                    $("#cancellation-field").fadeIn(1000).removeClass("hidden");
                    $("#transaction-type-field").fadeOut(1000).addClass("hidden");
                    $("#new-adjudicacion-field").fadeOut(1000).addClass("hidden");

                    CDI.removeValidationRules(_rules1);
                    CDI.removeValidationRules(_rules2);
                    CDI.removeValidationRules(_rules3);
                    CDI.addValidationRules(_rules4);
                    CDI.removeValidationRules(_rules5);
                    CDI.removeValidationRules(_rules6);
                    break;
                case "5":
                    $("#detalle-panel").fadeIn(1000).removeClass("hidden");

                    $("#monthly-installment-field").fadeOut(1000).addClass("hidden");
                    $("#current-balance-field").fadeOut(1000).addClass("hidden");
                    $("#from-date-field").fadeOut(1000).addClass("hidden");
                    $("#transaction-type-field").fadeOut(1000).addClass("hidden");
                    $("#new-adjudicacion-field").fadeOut(1000).addClass("hidden");
                    $("#cancellation-field").fadeIn(1000).addClass("hidden");
                    $("#adjustment-reason-field").fadeIn(1000).removeClass("hidden");

                    CDI.removeValidationRules(_rules1);
                    CDI.removeValidationRules(_rules2);
                    CDI.removeValidationRules(_rules3);
                    CDI.removeValidationRules(_rules4);
                    CDI.addValidationRules(_rules5);
                    CDI.removeValidationRules(_rules6);
                    break;
                case "6":
                    $("#detalle-panel").fadeIn(1000).removeClass("hidden");

                    $("#monthly-installment-field").fadeOut(1000).addClass("hidden");
                    $("#current-balance-field").fadeOut(1000).addClass("hidden");
                    $("#from-date-field").fadeIn(1000).removeClass("hidden");
                    $("#adjustment-reason-field").fadeIn(1000).addClass("hidden");
                    $("#cancellation-field").fadeIn(1000).removeClass("hidden");
                    $("#transaction-type-field").fadeOut(1000).addClass("hidden");
                    $("#new-adjudicacion-field").fadeOut(1000).addClass("hidden");

                    CDI.removeValidationRules(_rules1);
                    CDI.removeValidationRules(_rules2);
                    CDI.removeValidationRules(_rules3);
                    CDI.addValidationRules(_rules4);
                    CDI.removeValidationRules(_rules5);
                    CDI.removeValidationRules(_rules6);
                    break;
                default:
                    break;
            }
        });

        $("#btnNew").on("click", function (evt) {
            evt.preventDefault();

            _resetDetalleFields();

        });

        $("#btnCancel").on("click", function (evt) {
            evt.preventDefault();

            var result = confirm("¿Seguro que desea cancelar la operación?");
            if (result) {
                _resetDetalleFields();

                $("#ajuste-panel").fadeOut(1000).addClass("hidden");
            }
        });

        $("#btnSave").on("click", function (evt) {
            evt.preventDefault();

            var isValid = $("#ajuste-form").valid();

            var adjustmentsModel = {
                AdjustmentTypeId : 0,
                AdjustmentReasonId : 0,
                MonthlyInstallment : 0,
                CurrentBalance : 0,
                NewAdjudicacion: 0,
                FromDate: '',
                Comments: '',
                CaseDetailId: 0,
                CancellationId: 0
            };

            if (isValid) {

                adjustmentsModel.AdjustmentTypeId = $("#AdjustmentTypeId").val();
                adjustmentsModel.AdjustmentReasonId = $("#AdjustmentReasonId").val();
                adjustmentsModel.AdjustmentReasonText = $("#AdjustmentReasonId").find("option:selected").text();
                adjustmentsModel.MonthlyInstallment = $("#MonthlyInstallment").autoNumeric('get');
                adjustmentsModel.CurrentBalance = $("#CurrentBalance").autoNumeric('get');
                adjustmentsModel.NewAdjudicacion = $("#NewAdjudicacion").autoNumeric('get');
                adjustmentsModel.FromDate = $("#FromDate").datepicker("getDate");
                adjustmentsModel.Comments = $("#Comments").val();
                adjustmentsModel.CaseDetailId = _caseData.CaseDetailId;
                adjustmentsModel.CancellationId = $("#CancellationId").val();

                $.ajax({
                    url: root + "adjustments/save",
                    data: JSON.stringify({ model: adjustmentsModel }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    type: "POST",
                    beforeSend: function () {
                        CDI.showWaitingMessage();
                    }
                }).done(function (response) {
                    CDI.hideWaitingMessage();

                    if (response.data.Status === 'OK') {
                        CDI.displayNotification('Datos guardados', 'info');

                        $("#detalle-panel").fadeOut(1000).addClass("hidden");
                        CDI.removeValidationRules(_rules1);
                        CDI.removeValidationRules(_rules2);
                        CDI.removeValidationRules(_rules3);
                        CDI.removeValidationRules(_rules4);
                        CDI.removeValidationRules(_rules5);
                        CDI.removeValidationRules(_rules6);
                        _resetDetalleFields();
                    } else {
                        _showDialogMessage('Error al grabar datos');
                    }
                }).fail(function () {
                    CDI.hideWaitingMessage();
                });
            }
        });
    };

    var _init = function () {
        $("#ajuste-form").validate();

        _bindUIActions();

        $("#FromDate").datepicker({
            startDate: "-100y",
            endDate: "-1d"
        });

        $('#MonthlyInstallment, #CurrentBalance, #NewAdjudicacion').autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " , vMin: '0'});
        _resetDetalleFields();
    };

    return {
        init: _init
    };
})();

$(function () {
    CDI.CaseSearch.init();
    CDI.AdjustmentsIndex.init();
});