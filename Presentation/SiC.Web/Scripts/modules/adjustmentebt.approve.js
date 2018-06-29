CDI.AdjustmentEBTApprove = (function () {
    var tablePagos;
    var bindUiActions = function () {
        $("#btnSave").on("click", function (evt) {
            evt.preventDefault();

            var requests = [];
            var request = {
                PaymentId: 0,
                AdjustmentStatusId: 0,
                Comments: ""
            };

            $("#tblPagos tbody tr:has(td)").each(function (index, element) {
                var $element = $(element);
                var paymentId = parseInt($.trim($element.find("td:eq(0)").html()));
                var adjustmentStatusId = $element.find("#AdjustmentStatuses").val();
                var comments = $element.find("input[type='text']").val();
                
                request.PaymentId = paymentId;
                request.AdjustmentStatusId = adjustmentStatusId;
                request.Comments = comments;

                requests.push(request);

                request = {
                    PaymentId: 0,
                    AdjustmentStatusId: 0,
                    Comments: ""
                };
            });

            var isValid = requests.length > 0;
            if (isValid) {
                $.ajax({
                    url: root + "adjustmentebt/saveapprovals",
                    data: JSON.stringify({ requests: requests }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    beforeSend: function () {
                        CDI.showWaitingMessage();
                    }
                }).done(function (response) {
                    CDI.hideWaitingMessage();

                    if (response.data.Status === "OK") {
                        CDI.displayNotification("Datos guardados", "info");

                        tablePagos.ajax.reload();
                    } else {
                        CDI.toastr("Error al grabar datos");
                    }
                }).fail(function () {
                    CDI.hideWaitingMessage();
                });
            }
            else {
                alert("No existen solicitudes de crédito a EBT");
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
            "drawCallback": function () {
                $(".dataTables_empty").html("No se encontraron registros.");
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
                url: root + "adjustmentebt/findrequeststoapprove",
                contentType: "application/json; charset=utf-8",
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
                    "render": function (data, type, row) {
                        var $ddlAdjustmentStatuses = $("#AdjustmentStatuses").clone();
                        if (data !== null && data !== "") {
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
                    "data": "AdjustmentCompletedDate"
                },
                {
                    "data": "Comments",
                    "render": function () {
                        return "<input type='text' class='form-control input-sm' />";
                    }
                }
            ],
            "dom": "rtp"
        });
    };

    return {
        init: init
    };
})();

$(function () {
    CDI.AdjustmentEBTApprove.init();
});