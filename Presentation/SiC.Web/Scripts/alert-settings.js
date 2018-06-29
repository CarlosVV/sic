var selectedRow;
var alertObj = {};
var validator;
var alertRecords = 0;
var alertSettings = function () {    
    return $.ajax({
        type:'GET',
        url: '/AlertDetail/GetAlertSettings',
        success: function (data) {
            alertObj = data;
        }
    });

}

var fillDetail = function (index) {
    selectedRow = $('#tblAlertSetting').DataTable().row(index).data();
    $("#alertTypeId").val(selectedRow.Id);
    (selectedRow.IsActive ? $('#isActive').prop("checked", true) : $('#isActive').prop("checked", false));
    $('[id=selAlertType] option').filter(function () {
        return ($(this).text() == selectedRow.Description); //To select Blue
    }).prop('selected', true);
    $('#selYear').val(selectedRow.Year);
    $('#selPolicyType').val(selectedRow.PolicyTypeId);
    $('#selRegion').val(selectedRow.RegionId);
    $('#selEmployerType').val(selectedRow.EmployerTypeId);
    $('#selDataType').val(selectedRow.DataType);
    $('#txtValue').val(selectedRow.Value);
    $('#btnReset').removeClass('hidden');
    
    $("#selAlertType").prop("disabled", true);
    validateAlertType();
};

var validateAlertType = function () {
    var selIndex = $("#selAlertType")[0].selectedIndex - 1;
    if (selIndex !== (-1)) {
        var alertPolType = !alertObj[selIndex].ConfigurePolicyType;
        var alertRegion = !alertObj[selIndex].ConfigureRegion;
        var alertEmpType = !alertObj[selIndex].ConfigureEmployer;
        var alertVal = !alertObj[selIndex].ConfigureValue;
        var alertValType = !alertObj[selIndex].ConfigureValueType;

        $("#selRegion").prop("disabled", alertRegion);
        $("#selEmployerType").prop("disabled", alertEmpType);
        $("#selPolicyType").prop("disabled", alertPolType);
        $("#selDataType").prop("disabled", alertValType);
        $("#txtValue").prop("disabled", alertVal);

        $("#alertSettingsForm").validate();

        if (!alertRegion) {
            $("#selRegion").rules("add", "required");
        } else {
            $("#selRegion").val('');
            $("#selRegion").rules("remove", "required");
        }

        if (!alertPolType) {
            $("#selPolicyType").rules("add", "required");
        } else {
            $("#selPolicyType").val('');
            $("#selPolicyType").rules("remove", "required");
        }

        if (!alertEmpType) {
            $("#selEmployerType").rules("add", "required");
        } else {
            $("#selEmployerType").val('');
            $("#selEmployerType").rules("remove", "required");
        }

        if (!alertValType) {
            $("#selDataType").rules("add", "required");
        } else {
            $("#selDataType").val('');
            $("#selDataType").rules("remove", "required");
        }

        if (!alertVal) {
            $("#txtValue").rules("add", "required");
        } else {
            $("#txtValue").val('');
            $("#txtValue").rules("remove", "required");
        }
    }

};

var checkIfUnique = function () {
    var obj = {
        "Id": $("#alertTypeId").val(),
        "IsActive": $("#isActive").val(),
        "AlertSettingsId": $('#selAlertType').val(),
        "Year": $('#selYear option:selected').text(),
        "RegionId": $('#selRegion').val(),
        "EmployerTypeId": $('#selEmployerType').val(),
        "PolicyTypeId": $("#selPolicyType").val(),
        "Value": $('#txtValue').val()
    };

    posting = $.post("/AlertDetail/Check/", obj);

    posting.done(function (data) {
        if (data.quantity > 0) {
            CDI.displayNotification("Para la combinación seleccionada ya existe una alerta registrada.", "error");
            $("#alertSettingsForm")[0].reset();
            $("#alertSettingsForm").find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");
            $('#btnReset').click();
        }

        alertRecords = data.quantity;
    });

    return posting;
};


$.when(alertSettings()).done(function () {

    $(function () {
        //alertSettings()

        $('#selAlertType ').on("change", function () {
            validateAlertType();
        })

        $('#selAlertType, #selRegion, #selPolicyType, #selEmployerType').on("change", function () {
            checkIfUnique();
        })

        $('#txtValue,  #selYear').on("blur", function () {
            checkIfUnique();
        })
        
        $('#selYear').append($("<option/>").val((new Date().getFullYear()) - 1).text((new Date().getFullYear()) - 1));
        $('#selYear').append($("<option/>").val((new Date().getFullYear())).text((new Date().getFullYear())));
        $('#selYear').append($("<option/>").val((new Date().getFullYear()) + 1).text((new Date().getFullYear()) + 1));

        $('#tblAlertSetting').DataTable({
            "processing": true,
            "language": {
                "paginate": {
                    "first": "Primera",
                    "last": "Última",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },
                "info": "Mostrando de _START_ a _END_ de _TOTAL_ registros",
                "infoEmpty": "Mostrando de _START_ a _END_ de _TOTAL_ registros",
                "processing": "Buscando...",
                "lengthMenu": "Mostrar _MENU_ registros",
                "search": "Búsqueda:"
            },
            "drawCallback": function (settings) {
                $(".dataTables_empty").html('No se encontraron registros.');
            },
            "ajax": {
                "url": "/AlertDetail/GetSystemAlerts/",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "activeCol",
                    "data": "IsActive",
                    "orderable": true,
                    "class": "text-center",
                    "render": function (data, type, row, meta) {
                        return "<input type='checkbox' " + (data ? "checked" : "") + " disabled/>"
                    }
                },
                {
                    "name": "alertCol",
                    "data": "Description",
                    "orderable": true,
                    "class": "text-left",
                    "render": function (data, type, row, meta) {
                    
                        //find selected index 
                        //var item, count = 0;
                        //for (item in alertObj) {
                        //    if (alertObj[count].Id === row["AlertSettingsId"]) {
                        //        break;
                        //    } else {
                        //        count++;
                        //    }
                        //}

                        //if (alertObj[count].ConfigureRegion || alertObj[count].ConfigureValue || alertObj[count].ConfigureEmployer ) {
                        //    return "<a class='text-danger te' href='#fillForm' onclick='fillDetail(" + meta.row + ");'><u>" + data + "</u></a>";
                        //} else {
                        //    return "<span class='text-danger'>"+data+"</span>";
                        //}


                        return "<a class='text-danger' href='#fillForm' onclick='fillDetail(" + meta.row + ");'><u>" + data + "</u></a>";

                   }
                },
                {
                    "name": "policyTypeCol",
                    "data": "PolicyTypeDescription",
                    "orderable": true,
                    "class": "text-center"
                },
                {
                    "name": "yearCol",
                    "data": "Year",
                    "orderable": true,
                    "class": "text-center"
                },
                {
                    "name": "employerCol",
                    "data": "EmployerTypeDescription",
                    "orderable": true,
                    "class": "text-center"
                
                },
                 {
                     "name": "regionCol",
                     "data": "RegionName",
                     "orderable": true,
                     "class": "text-center"
                 },
                {
                    "name": "dataTypeCol",
                    "data": "DataType",
                    "orderable": true,
                    "class": "text-center"
                },
                {
                    "name": "valCol",
                    "data": "Value",
                    "orderable": true,
                    "class": "text-right",
                    "render": function (data, type, row) {
                        var tp = row["DataType"]
                        if(tp=='Decimal'){
                            return data+'';
                        } else if (tp == 'Porcentaje') {
                            return data + '%';
                        } else {
                            return '';
                        }

                    }
                },

            ],
            "dom": "lfrtip"
        });

        $("#btnSave").on("click", function () {
       
            $.when(checkIfUnique()).done(function () {
                if ($("#alertSettingsForm").valid() && alertRecords == '0') {
                    selectedRow = {
                        "Id": $("#alertTypeId").val(),
                        "AlertSettingsId": $('#selAlertType').val(),
                        "Description": $('#selAlertType option:selected').text(),
                        "LongDescription": '',
                        "RegionId": $('#selRegion').val(),
                        "PolicyTypeId": $('#selPolicyType').val(),
                        "isActive": $('#isActive').prop("checked"),
                        "RegionName": $('#selRegion option:selected').text(),
                        "Year": $('#selYear option:selected').text(),
                        "EmployerTypeId": $('#selEmployerType').val(),
                        "EmployerTypeDescription": $('#selEmployerType option:selected').text(),
                        "DataType": $('#selDataType').val(),
                        "Value": $('#txtValue').val()
                    };

                    var posting = $.post("/AlertDetail/AlertType", selectedRow);

                    posting.success(function (data) {
                        $('#btnReset').click();
                        $('#btnReset').addClass('hidden');
                        CDI.displayNotification("Su alerta fue grabada.", "success");
                        $('#tblAlertSetting').DataTable().ajax.reload();
                        $("#alertSettingsForm").find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");

                    });
                } else {
                    CDI.displayNotification("Verifique la información", "error");
                }
            });
            
        });
    

         $('#btnReset').on("click", function () {
             $("#alertTypeId").val(0);            
             $('#isActive').removeAttr('checked');
             $('#btnReset').addClass('hidden');
             $("#selAlertType").prop("disabled", false);
             $("#selPolicyType").prop("disabled", false);
             $("#selRegion").prop("disabled", false);
             $("#selEmployerType").prop("disabled", false);
             $("#selDataType").prop("disabled", false);
             $("#txtValue").prop("disabled", false);
             $("#alertSettingsForm").find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");


         });

    
    });

    validator = $('#alertSettingsForm').validate({
        focusCleanup: true,
        rules: {
            selAlertType: "required",
            selYear: "required",
            selRegion: "required",
            selEmployerType: "required",
            selPolicyType: "required",
            selDataType: "required",
            txtValue: "required"
        },
        messages: {
            selAlertType: "requerido",
            selYear: "requerido",
            selRegion: "requerido",
            selEmployerType: "requerido",
            selPolicyType:"requerido",
            selDataType: "requerido",
            txtValue: "requerido"
        },
        submitHandler: function () {
            return false;
        }
        
    });

});
