var table = "";

var showPaymentInsert = function () {
    table = $('#PaymentNonCompliant').DataTable({
        columns: [
            {
                data: null,
                render: function (data, type, row) {
                    return data.caseNumber + ' ' + data.caseKey;
                }
            },
            {
                data: "fullName"
            },
            {
                data: "ssn"
            },
            {
                data: "birthdate",
                className: "text-center",
                render: function (data, type, row) {
                    return moment(data).format("DD-MM-YYYY");
                }
            },
            {
                data: "fromDate",
                className: "text-center",
                render: function (data, type, row) {
                    return moment(data).format("DD-MM-YYYY");
                }
            },
            {
                data: "toDate",
                className: "text-center",
                render: function (data, type, row) {
                    return moment(data).format("DD-MM-YYYY");
                }
            },
            {
                data: "paymentDay",
                className: "text-center",
            },
            {
                data: "dailywage",
                class: "text-right",
                render: function (data, type, row) {
                    return numeral(data).format("$0,0.00");
                }
            },
            {
                data: "weeklycomp",
                class: "text-right",
                render: function (data, type, row) {
                    return numeral(data).format("$0,0.00");
                }
            },
            {
                data: "paymentAmount",
                class: "text-right",
                render: function (data, type, row) {
                    return numeral(data).format("$0,0.00");
                }
            },
            {
                data: "status",
            }
        ],
        select: true,
        dom: "Bfrtip",
        paging: true,
        scrollY: "300px",
        scrollCollapse: true,
        ordering: false,
        searching: false,
        language: {
            paginate: {
                "first": "Primera",
                "last": "Última",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            infoFiltered: " (de un total _MAX_ de registros filtrados)",
            info: "Mostrando del _START_ al _END_ de _TOTAL_ registros",
            infoEmpty: "Se encontraron 0 registros",
            processing: "Buscando <i class='fa fa-spinner fa-spin'></i>",
            lengthMenu: "Mostrar _MENU_ gestiones"
        },
        ajax: {
            url: root + "Queries/FindNonComplaintPayments",
            type: "GET",
            cache: false,
            error: function (msg) {
                alert(msg);
            }
        }
    });
}

$(document).ready(function () {
    showPaymentInsert();
});

