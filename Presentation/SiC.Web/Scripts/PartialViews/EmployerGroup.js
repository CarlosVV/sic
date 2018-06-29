var employerSelectedRow;
var employerTable;

function unlinkEmployer(employerData) {

    //selectedRow = table.row(rowIndex).data();
    var employerId = employerData.EmployerID;
    var findingId = $("#FindingId").val();

    $.ajax({
        url: '/Employer/UnlinkEmployer',
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            employerId: employerId,
            findingId: findingId
        }),
        async: false,
        success: function (data) {

            if (data = 1)
                $("#employerTable").DataTable().ajax.reload();
            else {
                alert("No se pudo desenlazar el patorno");
            }

            selectedRow = null;

            //$("#panelAlertDetail").hide();
            //$('#btnSave').hide();
            //$("#inputComments").val("");
            // alert("Información de alertas ha sido actualizada.");
        },
        error: function (xhr) {
            //alert("Información de eventos no pudo ser actualizada.");
        }
    });
}

var selectEmployer = function (elem) {

    var row = $(elem).closest('tr');

    var data = employerTable.row(row).data();

    selectedRow = data;

    if (data.EmployerMasterFlag == 1 || data.EmployerMasterFlag == 2) {
        $("#EmployerMasterFlag").prop('checked', true);
        $("#EmployerMasterFlag").prop('disabled', true);
    }
    else {
        $("#EmployerMasterFlag").prop('checked', false);
        $("#EmployerMasterFlag").prop('disabled', false);
    }

    $('#employerInformation_EmployerId').val(data.EmployerID);
    $('#employerInformation_EmployerName1').val(data.EmployerName1);
    $("#employerInformation_EmployerName2").val(data.EmployerName2);
    $("#employerInformation_EmployerName3").val(data.EmployerName3);
    $("#employerInformation_EmployerName4").val(data.EmployerName4);
    $('#employerInformation_EmployerLegalName').val(data.EmployerLegalName);
    $("#employerInformation_EmployerEIN").val(data.EmployerEIN);
    $("#employerInformation_Policy").val(data.EmployerPolicyNo);
    $("#employerInformation_NAICSDescriptionAndCode").val(data.NAICSDescriptionAndCode);
    $('#employerInformation_BusinessTypeId').val(data.BusinessTypeId);
    $("#employerInformation_EmployerTypeId").val(data.EmployerTypeId);
    //$("#employerInformation_PayrollCFSETotalWages").val(data.PayrollCFSETotalWages);
    //$("#employerInformation_PayrollOtherTotalWages").val(data.PayrollOtherTotalWages);
    //$("#employerInformation_PayrollDifference").val(data.PayrollDifference);
    //$("#employerInformation_EmployerNo").val(data.EmployerNo);

    $("#employerAddress_phisicalAddress_Id").val(data.PhisicalAddressId);
    $("#employerAddress_phisicalAddress_Line1").val(data.PhisicalAddressLine1);
    $("#employerAddress_phisicalAddress_Line2").val(data.PhisicalAddressLine2);
    if (data.PhisicalAddressCountryId == 0) {
        var ddl = $("#employerAddress_phisicalAddress_CountryId")[0];
        ddl.selectedIndex = 0;
        setCity("#employerAddress_phisicalAddress_CityId", 0, data.PhisicalAddressCityId);
    } else {
        $("#employerAddress_phisicalAddress_CountryId").val(data.PhisicalAddressCountryId);
        setCity("#employerAddress_phisicalAddress_CityId", data.PhisicalAddressCountryId, data.PhisicalAddressCityId);
    }
    $("#employerAddress_phisicalAddress_ZipCode").val(data.PhisicalZipCode);

    $("#employerAddress_postalAddress_Id").val(data.PostalAddressId);
    $("#employerAddress_postalAddress_Line1").val(data.PostalAddressLine1);
    $("#employerAddress_postalAddress_Line2").val(data.PostalAddressLine2);
    if (data.PostalAddressCountryId == 0) {
        var ddl = $("#employerAddress_postalAddress_CountryId")[0];
        ddl.selectedIndex = 0;
        setCity("#employerAddress_postalAddress_CityId", 0, data.PostalAddressCityId);
    } else {
        $("#employerAddress_postalAddress_CountryId").val(data.PostalAddressCountryId);
        setCity("#employerAddress_postalAddress_CityId", data.PostalAddressCountryId, data.PostalAddressCityId);
    }
    $("#employerAddress_phisicalAddress_ZipCode").val(data.PostalZipCode);

    $("#employerContact_ContactName").val(data.ContactName);
    $("#employerContact_Email").val(data.ContactEmail);

    $("#employerContact_Phone_id").val(data.ContactPhoneId);
    $("#employerContact_Phone_phoneNumber").val(data.ContactPhone).keyup();
    $("#employerContact_Phone_phoneExt").val(data.ContactPhoneExt).keyup();
    $("#employerContact_Fax_id").val(data.ContactFaxId);
    $("#employerContact_Fax_phoneNumber").val(data.ContactFax).keyup();
    //$("#employerContact_Fax_phoneNumber").mask("(000) 000-0000");

    //formatField("#employerInformation_PayrollCFSETotalWages", '0,0.00');
    //formatField("#employerInformation_PayrollOtherTotalWages", '0,0.00');
    //formatField("#employerInformation_PayrollDifference", "(0.00 %)");

    CDI.displayNotification('Se ha cargado el patrono con EIN: ' + data.EmployerEIN, "success");

    $("html, body").animate({ scrollTop: 0 }, "slow");
}

var setMaster = function () {
    
        $.ajax({
            url: '/Employer/SetMaster',
            dataType: "json",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                employerId: selectedRow.EmployerID
            }),
            async: false,
            success: function (data) {

                if (data = 1) {
                    $("#employerTable").DataTable().ajax.reload();
                    $("#EmployerMasterFlag").prop('disabled', true);
                    CDI.displayNotification("Asignado como master.", "success");
                }
                else {
                    CDI.displayNotification("No se pudo asignar como master al patorno.", "error");
                }

                $("#EmployerMasterFlag").prop('disabled', true);
            },
            error: function (xhr) {
                CDI.displayNotification("No se pudo asignar como master al patorno.", "error");
            }
        });
    }

//Plugin to add order by radiobutton state (checked or not checked)
$.fn.dataTable.ext.order['dom-radio'] = function (settings, col) {
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
        return $('input', td).prop('checked') ? '1' : '0';
    });
};

$(function () {

    $('#employerTable tbody').on('click', 'td:nth-child(1) a', function () {
        var row = $(this).closest('tr');

        var data = employerTable.row(row).data();

        employerSelectedRow = data;

        if (data.EmployerMasterFlag)
            CDI.displayNotification("No se puede desenlazar el patrono principal.", "error");
        else
            // TODO: Usar CDI.displayNotification
            generateNoty('¿Seguro que desea desenlazar este patrono?', 'notification', true, 'center', 'center', 'y', unlinkEmployer, 1, data, null);
    });

    employerTable = $("#employerTable").DataTable({
        "processing": true,
        "autoWidth": false,
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
            "url": "/Employer/GetEmployerGroup/" + findingId,
            "type": "GET"
        },
        "order": [
            [1, 'desc']
        ],
        "columns": [
        {
            "orderable": false,
            "data": "EmployerID",
            "render": function (data, type, row, meta) {
                return "<a class='btn btn-link'><i class='fa fa-chain-broken text-warning'></i></a>"
            },
            "class": "text-center"
        },
        {
            "orderable": true,
            "data": "EmployerMasterFlag",
            "render": function (data, type, row, meta) {
                return "<input type='radio' value='" + data + "' name='masterRadios" + meta.row + "' id='masterRadios" + meta.row + "' " + (data ? "checked " : "") + "disabled></a>"
            },
            "class": "text-center",
            "orderDataType": "dom-radio"
        },
        {
            "data": "EmployerEIN",
            "render": function (data, type, row, meta) {
                return '<button type="button" class="btn btn-link" onclick="selectEmployer(this)"><span class="text-danger">'+data+'</span></button>';
            }
        },
        {
            "data": "EmployerBk"
        },
        {
            "data": "EmployerPolicyNo"
        },
        {
            "data": "EmployerName"
        },
        {
            "data": "EmployerLegalName"
        },
        {
            "data": "ContactPhone",
            "render": function (data, type, row, meta) {
                var phone = data;

                if(data && data.length >= 10 )
                    phone = '(' + data.substring(0, 3) + ') ' + data.substring(3, 6) + '-' + data.substring(6, 10) + '';

                return phone;
            }
        },
        {
            "data": "PhisicalAddressCity"
        },
        {
            "data": "ContactName"
        }],
        "dom": "rftp"
    });

    if ($("#EmployerMasterFlag")) {
        $("#EmployerMasterFlag").on("change", function () {
            if ($("#EmployerMasterFlag").is(':checked') == true) {
                // TODO: Usar CDI.displayNotification
                generateNoty('¿Seguro que desea asignar este patrono como Master?', 'notification', true, 'center', 'center', 'y', setMaster, 1, null, null);
            }
        });
    }
});