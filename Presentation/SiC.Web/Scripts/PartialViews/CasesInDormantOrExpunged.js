var table = "";

var showPaymentInsert = function () {
    table = $('#FindCasesInDormantExpunged').DataTable({
        columns: [
            {
                data: null,
                render: function (data, type, row) {
                    return data.caseNumber + ' ' + data.caseKey;
                }
            },
            {
                data: "fullName",
                className: "text-center",
            },
            {
                data: "ssn",
                className: "text-center",
            },
            {
                data: "birthDate",
                className: "text-center",
                render: function (data, type, row) {
                    if (data != "N/A")
                        return moment(data).format("DD-MM-YYYY");
                    else
                        return data;
                }
            },
            {
                data: "EBTAccount",
                className: "text-center"
            },
            {
                data: "Region",
                className: "text-center"
            },
            {
                data: "Clinic",
                className: "text-center"
            },
            {
                data: "Injury",
                className: "text-center"
            },
            {
                data: "EBTStatus",
                className: "text-center"
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
            url: root + "Queries/FindCasesDormantOrExpunged",
            type: "GET",
            cache: false
        }
    });
}

$(document).ready(function () {
    showPaymentInsert();
});

