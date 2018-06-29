$(function () {
    $("#searchWithoutPolicyForm input, #searchWithoutPolicyForm select").keyup(function (event) {
        if (event.keyCode == 13) {
            $("#searchBtn").click();
        }
    });

    var table = $("#EmployerWithoutPolicyDataTable").DataTable({
        "processing": true,
        "serverSide": true,      
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
        "order":[[6, 'desc']],
        "drawCallback": function (settings) {
            $(".dataTables_empty").html('No se encontraron registros.');
        },
        "ajax": {
            "url": "/Search/SearchEmployerWithoutPolicy",
            "type": "POST",
            "data": {
                "Region": UserRegion,
                "AssignedToUser": AssignedToUser
            }
        },
        "columns": [
            {
                "name": "employerNameCol",
                "data": "Name",
                "render": function (data, type, row) {
                    return "<a class = 'text-danger' href='/Employer/EmployerWithoutPolicy/" + row["FindingId"] + "'>" + data + "</a>";
                }
            },
            {
                "name": "einCol",
                "data": "EIN"
            },
            {
                "name": "employerBkCol",
                "data": "EmployerBk"
            },
            {
                "name": "findingDateCol",
                "data": "FindingDate",
                "orderable": false,
                "render": function (data, type, row) {
                    return formatDate(data, 1);
                }
            },
            {
                "name": "cityCol",
                "data": "City"
            },
            {
                "name": "naicsCodeCol",
                "data": "DescriptionSp"
            },
            {
                "name": "payrollTotalCol",
                "type": "num-fmt",
                "data": "TotalWages",
                "render": function (data, type, row) {
                    return numeral(data).format('$0,0.00');
                }
            },
            {
                "name": "yearCol",
                "data": "Year",
                "orderable": false
            },
            {
                "name": "payrollEmployeesCol",
                "data": "EmpAvg",
                "orderable": false,
                "render": function (data, type, row) {
                    return numeral(data).format('0,0');
                }
            },
            {
                "name": "legalNameCol",
                "data": "LegalName",
                "visible": false
            },
            {
                "name": "findingIdCol",
                "data": "FindingId",
                "visible": false
            },
            {
                "name": "phoneCol",
                "data": "PhoneNumber",
                "visible": false
            }
        ],
        "dom": "lrtip"
    });

    $("#searchBtn").on("click", function () {
        if (isFormValid()) {
            filterData();
        }
        else {
            CDI.displayNotification("La forma no es válida. Favor arreglar los campos con error y asegurarse que tiene por lo menos 1 campo lleno.", "error");
        }
    });

    $("#clearBtn").on("click", function () {
        $("#formPanel :input").each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase();
            if (type == 'text' ||
                type == 'password' ||
                tag == 'textarea' ||
                type == 'number' ||
                type == 'tel')
                this.value = "";
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = 0;
        });
        filterData();
 
        form.resetForm();

    });

    $("#backBtn").on("click", function () {
        if (history.length) {
            history.go(-1);
        }
    });

    function filterData() {
        var table = $("#EmployerWithoutPolicyDataTable").DataTable();
        table.column("employerNameCol:name").search($("#name").val());
        table.column("einCol:name").search($("#ein").val());
        table.column("legalNameCol:name").search($("#legalName").val());
        table.column("employerBkCol:name").search($("#employerBk").val());
        table.column("phoneCol:name").search($("#phone").cleanVal());
        table.column("cityCol:name").search($("#city").val() || 0);
        
        var payrollFrom = $("#payrollFrom").val().replace(',', '');
        var payrollTo = $("#payrollTo").val().replace(',', '');

        var payrollTotalSearch = '';
        if (payrollFrom != '' && payrollTo != '')
            payrollTotalSearch = payrollFrom + ";" + payrollTo;
        else if (payrollFrom != '')
            payrollTotalSearch = payrollFrom + ';-1';
        else if (payrollTo != '')
            payrollTotalSearch = '-1;' + payrollTo;
        else
            payrollTotalSearch = '';


        table.column("payrollTotalCol:name").search(payrollTotalSearch);

        table.draw();
    }

    function isFormValid() {
        return $('#searchWithoutPolicyForm').valid() &&
            ($("#name").val() !== '' ||
            $("#ein").val() !== '' ||
            $("#legalName").val() !== '' ||
            $("#employerBk").val() !== '' ||
            $("#phone").cleanVal() !== '' ||
            $("#city").val() !== '' ||
            $("#payrollTo").val() !== '' ||
            $("#payrollFrom").val() !== '');
    }

    $("#phone").mask('(000) 000-0000');
    $("#payrollFrom").mask("0,000,000,000.00", { reverse: true });
    $("#payrollTo").mask("0,000,000,000.00", { reverse: true });

    //Validations
    var form = $('#searchWithoutPolicyForm').validate({
        rules: {
            ein: {
                minlength: 4,
                maxlength: 13
            },
            name: {
                minlength: 3,
                maxlength: 75
            },
            legalName: {
                minlength: 3,
                maxlength: 60
            },
            employerBk: {
                maxlength: 10,
                number: true
            },
            payrollFrom: {
                currency: ["$", false]
            },
            payrollTo: {
                currency: ["$", false]
            }
        },
        messages: {
            ein: {
                minlength: "Requiere 4 o más caracteres",
                maxlength: "Requiere 10 o menos caracteres"
            },
            name: {
                minlength: "Requiere 3 o más caracteres",
                maxlength: "Requiere 75 o menos caracteres"
            },
            legalName: {
                minlength: "Requiere 3 o más caracteres",
                maxlength: "Requiere 60 o menos caracteres"
            },
            employerBk: {
                number: "Requiere ser numero",
                maxlength: "Requere 10 o menos caracteres"
            },
            payrollFrom: {
                currency: "Debe ser un número, mayor a 0 y menor que 9,999,999,999.99"
            },
            payrollTo: {
                currency: "Debe ser un número, mayor a 0 y menor que 9,999,999,999.99"
            }
        }
    });
});