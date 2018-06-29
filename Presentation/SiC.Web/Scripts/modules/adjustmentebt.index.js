CDI.AdjustmentEBTIndex = (function () {
    var _searchRules = {
        CaseNumber: {
            require_from_group: [1, ".search"]
        },
        EntityName: {
            require_from_group: [1, ".search"]
        },
        SocialSecurityNumber: {
            require_from_group: [1, ".search"]
        },
        BirthDate: {
            require_from_group: [1, ".search"],
            date: true
        },
        FilingDate: {
            require_from_group: [1, ".search"],
            date: true
        },
        RegionId: {
            require_from_group: [1, ".search"]
        },
        ClinicId: {
            require_from_group: [1, ".search"]
        },
        EBTNumber: {
            require_from_group: [1, ".search"]
        }
    };
    var _caseData;
    var _tableAjustes;
    var _clinics = $("#ClinicId option").clone();
    var _adjustmentStatuses = $("#AdjustmentStatuses option").clone();
    var _adjustmentTypes = $("#AdjustmentTypes option").clone();

    var _clearFields = function () {
        $("#EntityName").val("");
        $("#SocialSecurityNumber").val("");
        $("#CaseNumber").val("");
        $("#RegionId").val("");
        $("#ClinicId").val("");
        $("#EBTNumber").val("");
    };

    var _clearTable = function () {
        if ($.fn.dataTable.isDataTable("#tblAjustes")) {
            _tableAjustes.destroy();
        }
        $("#tblAjustes tbody").find("tr").remove();
    };

    var _bindUIActions = function () {
        $('#RegionId').change(function () {
            var val = $(this).val();

            $('#ClinicId').empty();

            _clinics.filter(function (index, element) {
                return $(element).val() === "" || $(element).text().indexOf('[' + val + ']') >= 0;
            }).appendTo('#ClinicId');
        });

        $("#btnSearch").on("click", function (evt) {
            evt.preventDefault();

            CDI.addValidationRules(_searchRules);

            var isValid = $("#frmSearch").valid();
            if (isValid) {
                if ($.fn.dataTable.isDataTable("#tblAjustes")) {
                    _tableAjustes.destroy();
                }

                _tableAjustes = $("#tblAjustes").DataTable({
                    "ordering": false,
                    "autoWidth": false,
                    "pagingType": "simple_numbers",
                    "processing": true,                    
                    "deferRender": true,
                    "drawCallback": function (settings) {
                        $(".dataTables_empty").html('No se encontraron registros.');
                    },
                    "language": {
                        "paginate": {
                            "first": "Primera",
                            "last": "Última",
                            "next": "Siguiente",
                            "previous": "Anterior"
                        },
                        "infoFiltered": " (de un total _MAX_ de registros filtrados)",
                        "info": "Mostrando del _START_ al _END_ de _TOTAL_ registros",
                        "infoEmpty": "Se encontraron 0 registros",
                        "processing": "Buscando <i class='fa fa-spinner fa-spin'></i>",
                        "lengthMenu": "Mostrar _MENU_ gestiones"
                    },
                    "ajax": {
                        url: root + "adjustmentebt/findrequests",
                        contentType: 'application/json; charset=utf-8',
                        type: "POST",
                        data: function (data) {
                            data.EntityName = $("#EntityName").val();
                            data.SocialSecurityNumber = $("#SocialSecurityNumber").val();
                            data.BirthDate = $("#BirthDate").val();
                            data.CaseNumber = $("#CaseNumber").val();
                            data.FilingDate = $("#FilingDate").val();
                            data.RegionId = $("#RegionId").val();
                            data.ClinicId = $("#ClinicId").val();
                            data.EBTNumber = $("#EBTNumber").val();

                            return data = JSON.stringify(data);
                        }
                    },
                    "columns": [
                        {
                            "data": "PaymentId",
                            "className": "hidden"
                        },
                        {
                            "data": "CaseNumber"
                        },
                        {
                            "data": "AdjustmentStatus",
                            "render": function (data, type, row, meta) {
                                var $ddlAdjustmentStatuses = $("#AdjustmentStatuses").clone();
                                if (data !== null || data !== "") {
                                    $ddlAdjustmentStatuses.find("option[value='" + row.AdjustmentStatusId + "']").attr("selected", true);
                                }

                                return $ddlAdjustmentStatuses.wrap("<div>").parent().html();
                            }
                        },
                        {
                            "data": "AdjustmentType",
                            "render": function (data, type, row, meta) {
                                var $ddlAdjustmentTypes = $("#AdjustmentTypes").clone();
                                if (data === null || data === "") {
                                    $ddlAdjustmentTypes.find("option:eq(2)").prop("selected", true);
                                }
                                else {
                                    $ddlAdjustmentTypes.find("option:contains('" + data  + "')").attr("selected", true);
                                }

                                return $ddlAdjustmentTypes.wrap("<div>").parent().html();
                            }
                        },
                        {
                            "data": "Concept"
                        },
                        {
                            "data": "TransactionNumber"
                        },
                        {
                            "data": "IssueDate"
                        },
                        {
                            "data": "FromDate"
                        },
                        {
                            "data": "ToDate"
                        },
                        {
                            "data": "PaymentDay"
                        },
                        {
                            "data": "Amount"
                        }
                    ],
                    "dom": "rtp"
                });

                $("#frmAjustes").fadeIn(1000).removeClass("hidden");
            }

            CDI.removeValidationRules(_searchRules);
             
        });

        $("#btnClear").on("click", function (evt) {
            _clearFields();
            _clearTable();
        });

        $("#btnSave").on("click", function(evt) {
            evt.preventDefault();

            var requests = [];
            var request = {
                PaymentId: 0,
                AdjustmentStatusId: 0,
                AdjustmentType: ""
            };

            $('#tblAjustes tbody tr:has(td)').each(function(index, element) {
                var $element = $(element);
                var paymentId = parseInt($.trim($element.find("td:eq(0)").html()));
                var adjustmentStatusId = $element.find('#AdjustmentStatuses').val();
                if (adjustmentStatusId !== "") {
                    var adjustmentType = $element.find('#AdjustmentTypes').val();

                    request.PaymentId = paymentId;
                    request.AdjustmentStatusId = adjustmentStatusId;
                    request.AdjustmentType = adjustmentType;

                    requests.push(request);

                    request = {
                        PaymentId: 0,
                        AdjustmentStatusId: 0,
                        AdjustmentType: ""
                    };
                }
            });

            var isValid = requests.length > 0;
            if (isValid) {
                $.ajax({
                    url: root + "adjustmentebt/saverequests",
                    data: JSON.stringify({ requests: requests }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    beforeSend: function() {
                        CDI.showWaitingMessage();
                    }
                }).done(function(response) {
                    CDI.hideWaitingMessage();

                    if (response.data.Status === 'OK') {
                        CDI.displayNotification('Datos guardados', 'info');

                        _tableAjustes.ajax.reload();
                    } else {
                        _showDialogMessage('Error al grabar datos');
                    }
                }).fail(function() {
                    CDI.hideWaitingMessage();
                });
            } else {
                alert('Debe seleccionar al menos una solicitud');
            }

             
        });
    };

    var _init = function () {
        $("#frmSearch").validate();

        $("#BirthDate, #FilingDate").datepicker({
            startDate: "-100y",
            endDate: "-1d",
            language: "es",
            format: "dd/mm/yyyy",
        });

        $('#ClinicId').find("option:gt(0)").remove();

        $("#SocialSecurityNumber").inputmask({ mask: "999-99-9999" });

        $("#EBTNumber").inputmask({ mask: "999999999" });

        $("#CaseNumber").inputmask({ mask: "99999999999[ 99]" });

        _bindUIActions();
    };

    return {
        init: _init
    };
})();

$(function() {
    CDI.AdjustmentEBTIndex.init();
});