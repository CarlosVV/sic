CDI.AdjustmentEBTDocument = (function () {
    var tablePagos;

    var bindUiActions = function () {
        $("#btnSave").on("click", function(evt) {
            evt.preventDefault();

            var requests = [];
            var request = {
                PaymentId: 0,
                AdjustmentStatusId: 0,
                AdjustmentCompletedDate: "",
                AdjustmentAmount: 0,
                Comments: ""
            };

            var isValid = true;
            var validationMessage = "";
            $('#tblPagos tbody tr:has(td)').each(function(index, element) {
                var $element = $(element);
                var paymentId = parseInt($.trim($element.find("td:eq(0)").html()));
                var adjustmentStatusId = $element.find('#AdjustmentStatuses').val();
                var amount = parseFloat($element.find("input.amount").autoNumeric('get'));
                var completedDate = $element.find("input.completed").val();
                var comments = $element.find("input.comments").val();

                if (completedDate === "") {
                    isValid = false;
                    validationMessage = "Debe ingresar la fecha completado";
                    return false;
                }

                if (isNaN(amount)) {
                    isValid = false;
                    validationMessage = "Debe ingresar el monto debitado";
                    return false;
                }

                if (comments === "") {
                    isValid = false;
                    validationMessage = "Debe ingresar alguna observación";
                    return false;
                }

                if (amount < 0) {
                    isValid = false;
                    validationMessage = "Debe ingresar un monto debitado positivo";
                    return false;
                }

                var montoPago = parseFloat($.trim($element.find("td:eq(10)").html()).replace("$", "").replace(",", ""));
                if (amount > montoPago) {
                    isValid = false;
                    validationMessage = "El monto debitado debe ser menor o igual al monto del pago";
                    return false;
                }

                request.PaymentId = paymentId;
                request.AdjustmentStatusId = adjustmentStatusId;
                request.AdjustmentCompletedDate = completedDate;
                request.AdjustmentAmount = amount;
                request.Comments = comments;

                requests.push(request);

                request = {
                    PaymentId: 0,
                    AdjustmentStatusId: 0,
                    AdjustmentCompletedDate: "",
                    AdjustmentAmount: 0,
                    Comments: ""
                };

            });

            if (isValid) {
                if (requests.length > 0) {
                    $.ajax({
                        url: root + "adjustmentebt/savedocumentrequests",
                        data: JSON.stringify({ requests: requests }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        type: "POST",
                        beforeSend: function() {
                            CDI.showWaitingMessage();
                        }
                    }).done(function(response) {
                        CDI.hideWaitingMessage();

                        if (response.data.Status === "OK") {
                            CDI.displayNotification('Datos guardados', 'info');

                            tablePagos.ajax.reload();
                        } else {
                            //_showDialogMessage('Error al grabar datos');
                        }
                    }).fail(function() {
                        CDI.hideWaitingMessage();
                    });
                } else {
                    alert("No existen solicitudes de crédito a EBT");
                }
            } else {
                alert(validationMessage);
            }
        });
    };

    var init = function () {
        bindUiActions();

        if ($.fn.dataTable.isDataTable("#tblPagos")) {
            tablePagos.destroy();
        }

        tablePagos = $("#tblPagos").DataTable({
            "ordering": false,
            "autoWidth": false,
            "pagingType": "simple_numbers",
            "processing": true,           
            "deferRender": true,
            "columnDefs": [
                {
                    targets: 13,
                    width: 140
                },
                {
                    targets: 14,
                    width: 50
                },
                {
                    targets: 15,
                    width: 160
                }
            ],
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
                url: root + "adjustmentebt/findrequeststodocument",
                contentType: 'application/json; charset=utf-8',
                type: "POST"
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
                    "data": "AdjustmentType"
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
                },
                {
                    "data": "AdjustmentRequestedBy"
                },
                {
                    "data": "AdjustmentRequestedDate"
                },
                {
                    "data": "AdjustmentCompletedDate",
                    "render": function (data, type, row, meta) {
                        return "<div class='input-group date'><input type='text' class='form-control completed' /><span class='input-group-addon'><i class='glyphicon glyphicon-calendar'></i></span></div>";
                    }
                },
                {
                    "data": "Amount",
                    "render": function (data, type, row, meta) {
                        return "<input type='text' class='form-control input-sm amount' />";
                    }
                },
                {
                    "data": "Comments",
                    "render": function (data, type, row, meta) {
                        return "<input type='text' class='form-control input-sm comments' />";
                    }
                }
            ],
            "dom": "rtp",
            "initComplete": function (settings, json) {
                $('#tblPagos > tbody > tr').find("input.completed").datepicker({
                    startDate: "-100y",
                    endDate: "-1d"
                });

                $('#tblPagos > tbody > tr').find("input.amount").autoNumeric('init', {
                    vMin: '0'
                });
            }
        });
    };

    return {
        init: init
    };
})();

$(function () {
    CDI.AdjustmentEBTDocument.init();
});