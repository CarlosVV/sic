var conceptTable;
var beneficiaryTable;

var showConceptTable = function () {

    if (!conceptTable) {
        conceptTable = $("#conceptTable").DataTable({
            "processing": true,
            "autoWidth": false,
            "drawCallback": function (settings) {
                $(".dataTables_empty").html('No se encontraron pagos.');
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
                "url": "/Payments/ChequesPorConcepto",
                "type": "POST",
                "data": {"caseNumber" : caseFolderId}
            },
            "order": [
                [1, 'desc']
            ],
            "columns": [
                {
                    "data": "Concepto"
                },
                {
                    "data": "Pagado",
                    "class": "text-right",
                    "render": function (data, type, row, meta) {
                        return numeral(data).format('$0,0.00');
                    }
                },
                {
                    "data": "No_Cobrado",
                    "class": "text-right",
                    "render": function (data, type, row, meta) {
                        return numeral(data).format('$0,0.00');
                    }
                }
            ],
            "dom": "rt"
        });
    }
    else
        conceptTable.ajax.reload();
}

var showBeneficiaryTable = function () {

    //if ($("#resultsPanel").css('display') == 'none')
    //    $("#resultsPanel").show();

    if (!beneficiaryTable) {
        beneficiaryTable = $("#beneficiaryTable").DataTable({
            "processing": true,
            "autoWidth": false,
            "drawCallback": function (settings) {
                $(".dataTables_empty").html('No se encontraron pagos.');
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
                "url": "/Payments/ChequesPorBeneficiario",
                "type": "POST",
                "data": { "caseNumber": caseFolderId }
            },
            "order": [
                [1, 'desc']
            ],
            "columns": [
                {
                    "data": "Beneficiario"
                },
                {
                    "data": "Pagado",
                    "class": "text-right",
                    "render": function (data, type, row, meta) {
                                    return numeral(data).format('$0,0.00');
                                }
                },
                {
                    "data": "No_Cobrado",
                    "class": "text-right",
                    "render": function (data, type, row, meta){
                                    return numeral(data).format('$0,0.00');
                                }
                }
            ],
            "dom": "rt"
        });
        }
        else
            beneficiaryTable.ajax.reload();
}

$(function () {
    showConceptTable();
    showBeneficiaryTable();
});