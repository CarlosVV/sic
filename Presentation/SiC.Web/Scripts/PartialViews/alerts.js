var selectedRow;

$(function () {
    var inDiff = $("#inputDiference");
    inDiff.val( inDiff ? inDiff.val(numeral(inDiff.val()).format('0,0[.]00')) : 0);
    
    var tbl = $('#tblAlertDetail').dataTable({       
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
            "url": "/AlertDetail/GetAlertList/" + findingId,
            "type": "GET"
        },
        "columns": [
            {
                "title": "Descripción",
                "data": "AlertType.Description",
                "class":"col-md-3",
                "render": function (data, type, row, meta) {
                    //Return row index 
                    var index = meta.row;
                    if (linkAction === "AlertDetail") {
                        if (selectedAlert === row.Id) {
                            fillDetail(index);
                        }

                        return "<a class=\"text-danger\" onclick='fillDetail(" + index + ");'>" + data + "</a>";
                    }
                    else {
                        return "<a class='text-danger' href='/AlertDetail/Index/" + row["FindingId"] + "/" + row["Id"] + "' >" + data + "</a>";
                    }
                }
            },
            {
                "title": "Configurado",
                "data": "AlertType.Value",
                "class": "col-md-1",
                "render": function (data, type, row, meta) {
                    var value = data;

                    if (value != null) {
                        if (row.AlertType.DataType === 'Decimal')
                            value = numeral(value).format('0,0[.]00');
                        else if (row.AlertType.DataType === 'Porcentaje')
                            value = value + '%';
                    }else{
                        value = "";
                    }
                    return value;
                }

            },
            { 
                "title": "Valor",
                "data": "Value",
                "class": "col-md-1",
                "render": function (data, type, row, meta) {
                    var value = data;

                    if (value != null) {
                        if (row.AlertType.DataType === 'Decimal')
                            value = numeral(value).format('0,0[.]00');
                        else if (row.AlertType.DataType === 'Porcentaje')
                            value = value + '%';
                    } else {
                        value = "";
                    }
                    return value;
                }
            },
            {
                "title": "Estatus",
                "data": "AlertStatus.Description",
                "class": "col-md-1"
            },
            {
                "title": "Razón",
                "data": "StatusReason.Description",
                "class":"col-md-3",
                "orderable": false
            },
            {
                "title": "Comentarios",
                "data": "Comments",
                "class":"col-md-3",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var elem = data;
                    if (data && data.length > 45) {
                        elem = data.substring(0, 45) + '...'
                        return '<div data-toggle="popover" data-toggle="popover" data-html="true" data-trigger="hover" data-content="' + data.replace(/(?:\r\n|\r|\n)/g, '<br />') + '">' + elem + '</div>'
                    }

                    return elem;
                }
            }
        ],
        "dom": "lrtip"
    });

    tbl.on('draw.dt', function () {
        $('[data-toggle="popover"]').popover({
            placement: 'bottom',
            template: '<div class="popover" role="tooltip" style="min-width:500px;"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content""></div></div>'
        })
    });
});


//set the maxlength for the comment textarea
var setMaxLength = function (comment) {
    var today = new Date();
    var stampLength = ('\n --Comentado Por: ' + userName + ' - ' +
                    today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear() + ' ' + today.getHours() + ':' + today.getMinutes() + '--').length;
    var remainingChars = 990;
    if (comment) {
        remainingChars = (990 - stampLength - comment.length) > 0 ? (990 - stampLength - comment.length) : 0;
     } else {
        remainingChars = (990 - stampLength ) > 0 ? (990 - stampLength) : 0;
    }

    $("#inputComments")[0].maxLength = remainingChars;
    if (remainingChars > 0) {
        $("#inputComments").rules("add", {
            required: true,
            maxlength: remainingChars,
            messages: {
                required: "requerido",
                maxlength: "excedió tamaño máximo"
            }
        });
    } else {
        $("#inputComments").rules("remove");
    }
};

//Fills the data related to the selected alert by user.
var fillDetail = function (index) {
    var tbl = $('#tblAlertDetail').DataTable();
    selectedRow = tbl.row(index).data();

    setMaxLength(selectedRow.Comments);

    $("#inputDescription").val(selectedRow.AlertType.Description);
    $("#inputDiference").val(selectedRow.Value);
    $("#selAlertStatus").val(selectedRow.AlertStatusId);
    $("#selReason").val(selectedRow.StatusReasonId);
    $("#selWorkBy").val(selectedRow.AssigedToId);
    //$("#inputComment").val()
    $("#panelAlertDetail").show();
    $('#btnSave').show();
};

$('#btnGoBack').click(function goBack() {
    history.go(-1);
});


$('#btnSave').click(function updateData() {
    var errorFlag = $("#selAlertStatus").val() === "" /*||
                    $("#selWorkBy").val() === ""*/ ?
                        true : false;

    if (errorFlag && $("#alertSettingsForm").valid()) {
        CDI.displayNotification("Los campos de \"status\" y \"trabajado por\" son requeridos.", "error");
    }
    else {
        var today = new Date();

        $.ajax({
            url: '/AlertDetail/UpdateAlertDetail',
            dataType: "json",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                updatedRecord: {
                    Id: selectedRow.Id,
                    FindingId: selectedRow.FindingId,
                    AssignedToId: $("#selWorkBy").val(),
                    AlertStatusId: $("#selAlertStatus").val(),
                    StatusReasonId: $("#selReason").val(),
                    Comments: $.trim((selectedRow.Comments != null ? selectedRow.Comments + '\n \n' : '') + $("#inputComments").val() + '\n --Comentado Por: ' + userName + ' - ' +
                        today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear() + ' ' + today.getHours() + ':' + today.getMinutes() + '--')
                }
            }),
            async: true,
            success: function (data) {
                $("#tblAlertDetail").DataTable().ajax.reload();
                selectedRow = null;
                $("#panelAlertDetail").hide();
                $('#btnSave').hide();
                $("#inputComments").val("");
                $('#tblAlertDetail').DataTable().ajax.reload();
                $('[data-toggle="popover"]').data('bs.popover').setContent();
                CDI.displayNotification("Información de alertas ha sido actualizada.", "success");
            },
            error: function (xhr) {
                CDI.displayNotification("Información de alertas no pudo ser actualizada." + error, "error");
            }
        })
    }

   
});

$('#alertsForm').validate({
    rules: {
        selAlertStatus: "required"
    },
    messages: {
        selAlertStatus: "requerido"
    }
});

$("#selAlertStatus").change(function () {
    var stat = $("#selAlertStatus option:selected").text();
    if (stat === "Cerrado") {
        $("#selReason").removeClass("hidden");
        $("#selReasonLbl").removeClass("hidden");
        $("#selReason").rules("add", { required: true, messages: { required: "requerido" } });
    } else {
        $("#selReason").val('');
        $("#selReason").rules("remove", "required");
        
        $("#selReason").addClass("hidden");
        $("#selReasonLbl").addClass("hidden");
    }
});