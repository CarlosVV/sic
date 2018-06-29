$(function () {
    //initialize payrollCFSE data table
    var tbl = $("#payrollCFSEDataTable").DataTable({
        "processing": true,
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
            "lengthMenu": "Mostrar _MENU_ registros"
        },
        "drawCallback": function (settings) {
            $(".dataTables_empty").html('No se encontraron registros.');
        },
        "ajax": {
            "url": "/Payroll/GetPayrollCFSEWithDetail/?payrollCFSEId=" + payrollCFSEId,
            "type": "GET"
        },
        "columns": [            
            {
                "name": "policyNumberCol",
                "data": "PolicyNo",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "yearCol",
                "data": "BillingYear",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "keyCol",
                "data": "RiskKey",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "groupCol",
                "data": "RiskGroup",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "prelPayrollCol",
                "data": "PayrollPrel",
                "orderable": false,
                "class": "text-right",
                "render": function (data, type, row) {
                    return numeral(data).format('$0,0.00');
                }
            },
            {
                "name": "finalPayrollCol",
                "data": "PayrollFinal",
                "orderable": false,
                "class": "text-right",
                "render": function (data, type, row) {
                    return numeral(data).format('$0,0.00');
                }
            },
            {
                "name": "investigatedPayrollCol",
                "data": "PayrollInv",
                "orderable": false,
                "class": "text-right",
                "render": function (data, type, row) {
                    return numeral(data).format('$0,0.00');
                }
            },
            {
                "name": "startDtCol",
                "data": "PolicyStartDate",
                "orderable": false,
                "class": "text-center",
                "render": function (data, type, row) {
                    return formatDate(data, 1);
                }               
            },
            {
                "name": "endDtCol",
                "data": "PolicyEndDate",
                "orderable": false,
                "class": "text-center",
                "render": function (data, type, row) {
                    return formatDate(data, 1);
                }
            },
            {
                "name": "rateCol",
                "data": "Rate",
                "orderable": false,
                "class": "text-right",
                "render": function (data, type, row) {
                    return numeral(data).format('$0,0.00');
                }
            },

        ],
        "dom": "rt"
    });

    if (payrollCFSEId > 0)
        $("#btnResearchedPayroll").show();
    else
        $("#btnResearchedPayroll").hide();

});