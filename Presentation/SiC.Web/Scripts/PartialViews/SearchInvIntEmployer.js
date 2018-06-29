$(function () {
    $("#searchInvIntForm input, #searchInvIntForm select").keyup(function (event) {
        if (event.keyCode == 13) {
            $("#searchBtn").click();
        }
    });

    $('#datePickerBillingYear').datepicker({
        startView: 'year',
        format: 'YYYY'
    });

    var table = $("#ResearchedEmployerDataTable").DataTable({
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
        "drawCallback": function (settings) {
            $(".dataTables_empty").html('No se encontraron registros.');
        },
        "ajax": {
            "url": "/Search/SearchIntervenedInvestigatedEmployers",
            "type": "POST",
            "data": {
                "Region": UserRegion,
                "AssignedToUser": AssignedToUser
            }
        },
        "columns": [
            {
                "className": 'details-control text-center',
                "orderable": false,
                "data": '',
                "defaultContent": '<span class="fa fa-plus-circle fa-2x"></span>'
            },
            {
                "name": "employerNameCol",
                "data": "Name",
                "render": function (data, type, row) {
                    return "<a class = 'text-danger' href='/Employer/ResearchPayroll/" + row["FindingId"] + "'>" + data + "</a>";
                }
            },
            {
                "name": "einCol",
                "data": "EIN"
            },
            {
                "name": "policyNoCol",
                "data": "PolicyNo"
            },
            {
                "name": "billingYearCol",
                "data": "BillingYear"
            },
            {
                "name": "cityCol",
                "data": "City",
                "orderable": false
            },
            {
                "name": "findingIdCol",
                "data": "FindingId",
                "visible": false
            },
            {
                "name": "employerBkCol",
                "data": "EmployerBk",
                "visible": false
            },
            {
                "name": "phoneCol",
                "data": "PhoneNumber",
                "visible": false
            },
            {
                "name": "descriptionCol",
                "data": "Description",
                "visible": false
            },
            {
                "name": "valueCol",
                "data": "Value",
                "visible": false
            },
            {
                "name": "legalNameCol",
                "data": "LegalName",
                "visible": false
            }
        ],
        "dom": "lrtip"
    });

    function format(d) {
        var rows = '';
        $.each(d.data, function (i, alert) {
            var value = alert.Value;

            if (value != null) {
                if (alert.AlertType.DataType === 'Decimal')
                    value = numeral(value).format('0,0[.]00');
                else if (alert.AlertType.DataType === 'Porcentaje')
                    value = value + '%';
            } else {
                value = "";
            }

            rows += '<tr><td>' + value + '</td><td>' + alert.AlertType.Description + '</td></tr>'
        });

        var tbl = '<table class="table table-striped table-bordered">' +
                    '<thead>' +
                        '<tr>' +
                            '<th>Diferencia</th>' +
                            '<th>Razón</th>' +
                        '</tr>' +
                    '</thead>' +
                        rows
        '<tbody>' +
        '</tbody>' +
    '</table>';

        return tbl;
    }

    $("#searchBtn").on("click", function () {
        if (isFormValid()) {
            filterData();
        }
        else {
            CDI.displayNotification("La forma no es válida. Favor arreglar los campos con error y asegurarse que tiene por lo menos 1 campo lleno.", "error");
        }
    });

    $("#clearBtn").on("click", function () {
        $('#formPanel :input').each(function () {
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
                this.selectedIndex = -1;
        });
        filterData();

       form.resetForm();
       $('#searchInvIntForm').find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");
    });

    // Add event listener for opening and closing details
    $('#ResearchedEmployerDataTable tbody').on('click', 'td.details-control', function (e) {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var rowIcon = $('.fa', row.node());

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
            rowIcon.removeClass('fa-minus-circle');
            rowIcon.addClass('fa-plus-circle');
        }
        else {
            // Open this row

            var request = $.getJSON("/AlertDetail/GetAlertList/" + row.data().FindingId, function (data) {
                row.child(format(data)).show();
                tr.addClass('shown');
            });
            
            rowIcon.removeClass('fa-plus-circle');
            rowIcon.addClass('fa-minus-circle');
        }
    });

    function filterData() {
        var table = $("#ResearchedEmployerDataTable").DataTable();

        table.column("employerNameCol:name").search($("#name").val());
        table.column("einCol:name").search($("#ein").val());
        table.column("legalNameCol:name").search($("#legalName").val());
        table.column("employerBkCol:name").search($("#employerBk").val());
        table.column("phoneCol:name").search($("#phone").cleanVal());
        table.column("cityCol:name").search($("#city").val() || 0);
        table.column("policyNoCol:name").search($("#policyNo").val());
        table.column("descriptionCol:name").search($("#reason").val() || "");
        table.column("billingYearCol:name").search($("#billingYear").val());

        table.draw();
    }

    $("#backBtn").on("click", function () {
        if (history.length) {
            history.go(-1);
        }
    });

    $("#phone").mask('(000) 000-0000');


    //Validations
    var form = $('#searchInvIntForm').validate({
        debug:true,
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
                number: true, 
                maxlength: 10
            },
            billingYear: {
                min: 1900,
                number: true,
                max: getNextYear()
            },
            policyNo: {
                maxlength: 10,
                number: true
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
            billingYear: {
                min: "Requiere ser igual o mayor que 1900",
                number: "Requiere ser numero",
                max: "Requiere ser menor que " + getNextYear()
            },
            policyNo: {
                number: "Requiere ser numero",
                maxlength: "Requiere 10 caracteres"
            }
        }
    });

    function isFormValid() {
        return $('#searchInvIntForm').valid() &&
            ($("#name").val() !== '' ||
            $("#ein").val() !== '' ||
            $("#legalName").val() !== '' ||
            $("#employerBk").val() !== '' ||
            $("#phone").cleanVal() !== '' ||
            $("#city").val() !== '' ||
            $("#policyNo").val() !== '' ||
            $("#reason").val() !== '' ||
            $("#billingYear").val() !== '');
    }
});