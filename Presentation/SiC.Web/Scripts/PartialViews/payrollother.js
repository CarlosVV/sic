$(function () {
   //initialize payroll data table
    var tbl = $("#payrollOTherDataTable").DataTable({
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
            "url": "/Payroll/GetPayrollOtherWithDetail/?payrollOtherId=" + payrollOtherId,
            "type": "GET"
        },
        "columns": [
            {
                "name": "einCol",
                "data": "EIN",
                "orderable": false,
                "class": "text-left"
            },
            {
                "name": "yearCol",
                "data": "Year",
                "orderable": true,
                "class": "text-center"
            },
            {
                "name": "trimCol",
                "data": "Quarter",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "emp1Col",
                "data": "NumberOfEmployees1",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "emp2Col",
                "data": "NumberOfEmployees2",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "emp3Col",
                "data": "NumberOfEmployees3",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "employesAvgCol",
                "data": "EmpAvgDetail",
                "orderable": false,
                "class": "text-center"
            },
            {
                "name": "totalSalCol",
                "data": "TotalWagesDetail",
                "orderable": false,
                "class": "text-right",
                "render": function (data, type, row) {
                    return numeral(data).format('$0,0.00');
                }
            },           
            {
                "name": "salaryAvgCol",
                "data": "WagesAvg",
                "orderable": false,
                "class": "text-right",
                "render": function (data, type, row) {
                    return numeral(data).format('$0,0.00');
                }
            },

        ],
        "dom": "rt"
    });

});