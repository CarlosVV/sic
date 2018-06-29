var selectedRow;
var table;
var eventFormValidator;

//function removeEvent(rowIndex) {

//    selectedRow = table.row(rowIndex).data();

//    $.ajax({
//        url: '/Event/RemoveEvent',
//        dataType: "json",
//        type: "POST",
//        contentType: 'application/json; charset=utf-8',
//        data: JSON.stringify({
//            removeEvent: {
//                Id: selectedRow.Id,
//                FindingId: selectedRow.FindingId,
//                ActionTypeId: selectedRow.ActionTypeId,
//                AssignedToId: selectedRow.AssignedToId,
//                EventDate: moment(selectedRow.EventDate).utc(),
//                Description: selectedRow.Description
//            }
//        }),
//        async: false,
//        success: function (data) {
//            $("#eventTable").DataTable().ajax.reload();

//            selectedRow = null;

//            //$("#panelAlertDetail").hide();
//            //$('#btnSave').hide();
//            //$("#inputComments").val("");
//            // alert("Información de alertas ha sido actualizada.");
//        },
//        error: function (xhr) {
//            //alert("Información de eventos no pudo ser actualizada.");
//        }
//    });

//}

var refreshUserList = function (actionTypeId) {
    var ddl = $("#gestionTrabajadaPor");
    ddl.find('option[value]').remove();
    $("#gestionTrabajadaPor").attr('readonly', 'readonly');
    //$("#gestionTrabajadaPor_Loading").show("fast");

    var post = $.Deferred();

    if (actionTypeId) {
        var post = $.ajax({
            cache: false,
            url: "/Event/GetFilteredUserList/" + actionTypeId,
            dataType: "json",
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            async: true,
            success: function (data) {
                $.each(data, function (i, data) {
                    ddl.append('<option value="' + data.Value + '">' + data.Text + '</option>');
                });

                $("#gestionTrabajadaPor option:first-child").text('');

                //$("#gestionTrabajadaPor_Loading").hide("fast");
                ddl.removeAttr('readonly');
            },
            error: function (jqxhr, textStatus, errorThrown) {
                //$("#gestionTrabajadaPor_Loading").hide("fast");
                $("#gestionTrabajadaPor").attr('readonly', 'readonly');
            }
        })

    }
    else {
        //$("#gestionTrabajadaPor_Loading").hide("fast");
        $("#gestionTrabajadaPor").attr('readonly', 'readonly');
        post.resolve;
    }
    return post.promise();
}


$(function () {
    
    $('#datePickerGestionFecha').datepicker();

    $('#gestionTipoGestion').on('change', function () {
       refreshUserList(this.value);
    });

    table = $("#eventTable").DataTable({
        "processing": true,
        "language": {
            "paginate": {
                "first": "Primera",
                "last": "Última",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "infoFiltered": " (de un total _MAX_ de gestiones filtradas)",
            "info": "Mostrando del _START_ al _END_ de _TOTAL_ gestiones",
            "infoEmpty": "No se encontraron gestiones.",
            "processing": "Buscando <i class='fa fa-spinner fa-spin'></i>",
            "lengthMenu": "Mostrar _MENU_ gestiones"
        },
        "drawCallback": function (settings) {
            $(".dataTables_empty").html('No se encontraron registros.');
        }, 
        "ajax": {
            "url": "/Event/SearchEvents/" + findingId,
            "type": "GET"
        },
        "columns": [
        //{
        //    "orderable": false,
        //    "targets": 'no-sort',
        //    "render": function (data, type, row, meta) {
        //        return "<a class='text-danger' onclick='removeEvent(" + meta.row + ")'><i class='fa fa-times'></i></a>"
        //    },
        //    "class": "text-center"
        //},
        {
            "name": "employerNameCol",
            "data": "ActionTypeDescription",
            "render": function (data, type, row, meta) {
                return data;//"<a class='text-danger' onclick=''>" + data + "</a>"
            }

        },
        {
            "name": "einCol",
            "data": "EventDate",
            "render": function (data, type, row) {
                return formatDate(data, 1);
            }
        },
        {
            "name": "assignedToUserName",
            "data": "AssignedToUserName"

        },
        {
            "name": "eventDescription",
            "data": "Description",
            "render": function (data, type, row, meta) {
                var elem = data;

                if (data && data.length > 35) {
                    elem = data.substring(0, 35) + '...';
                    return '<div data-toggle="popover" data-toggle="popover" data-trigger="hover" data-content="' + data + '">' + elem + '</div>';
                }
                
                return elem;
            }
        },
        {
            "name": "createdBy",
            "data": "CreatedBy"
        }
        ],
        "dom": "tp"
    });

    table.on('draw.dt', function () {
        $('[data-toggle="popover"]').popover({
            placement: 'bottom',
            template: '<div class="popover" role="tooltip" style="min-width:500px;"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content""></div></div>'
        })
    });

    $('#btnSaveEvent').click(function updateData() {
        if (!$('#eventForm').valid()) {
            CDI.displayNotification("La gestión no es válida. Favor corregir los campos con error.", "error");
        }
        else {
            $.ajax({
                url: '/Event/SaveEvent',
                dataType: "json",
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    updatedEvent: {
                        Id: selectedRow == null ? 0 : selectedRow.Id || 0,
                        FindingId: selectedRow == null ? findingId : selectedRow.FindingId || findingId,
                        ActionTypeId: $('#gestionTipoGestion').val(),
                        AssignedToId: $("#gestionTrabajadaPor").val(),
                        AssignedToUserName: $("#gestionTrabajadaPor option:selected").text(),
                        EventDate: moment($("#gestionFecha").val(), 'DD/MM/YYYY'),
                        Description: $("#gestionDescripcion").val()
                    }
                }),
                async: false,
                success: function (data) {
                    $("#eventTable").DataTable().ajax.reload();
                    selectedRow = null;

                    $('#gestionTipoGestion').val(0);
                    $("#gestionTrabajadaPor").val(0);
                    $("#gestionFecha").val('');
                    $("#gestionDescripcion").val('');



                    eventFormValidator.resetForm();
                    $("#eventForm").find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");

                    CDI.displayNotification("La gestión ha sido grabada.", "success");
                    //$("#panelAlertDetail").hide();
                    //$('#btnSave').hide();
                    //$("#inputComments").val("");
                    //alert("Información de alertas ha sido actualizada.");
                },
                error: function (xhr) {
                    CDI.displayNotification("La gestión no pudo ser grabada.", "error");
                }
            })
        }
    });

    //$('#eventTable tbody').on('click', 'td:nth-child(1) a', function () {

    //    var row = $(this).closest('tr');

    //    var data = table.row(row).data();

    //    selectedRow = data;

    //    $("#gestionFecha").val(formatDate(data.EventDate, 1));
    //    $("#gestionDescripcion").val(data.Description);

    //    $('html, body').animate({
    //        scrollTop: $("#eventPanel").offset().top -70
    //    }, 700);

    //    var nombre = $('#gestionTipoGestion');
    //    nombre.val(data.ActionTypeId);

    //    refreshUserList(selectedRow.ActionTypeId).then(function(){
    //        $("#gestionTrabajadaPor").val(selectedRow.AssignedToId)
    //    });

    //    eventFormValidator.resetForm();
    //    $("#eventForm").find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");
    //});

    $('#clearEventBtn').on('click', function () {
        selectedRow = null;

        var nombre = $('#gestionTipoGestion');

        $("#gestionTrabajadaPor").val(0);
        nombre.val(0);
        nombre.change();
        $("#gestionTrabajadaPor").val(0);
        $("#gestionFecha").val('');
        $("#gestionDescripcion").val('');

        eventFormValidator.resetForm();
        $("#eventForm").find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");
    });

    //Masking
    //$("#gestionFecha").mask('00/00/0000');

    //Validations
    eventFormValidator = $('#eventForm').validate({
        rules: {
            gestionTipoGestion: {
                required: true
            },
            gestionTrabajadaPor: {
                required: true
            },
            gestionFecha: {
                required: true
            },
            gestionDescripcion: {
                required: true,
                minlength: 3,
                maxlength: 250
            }
        },
        messages: {
            gestionTipoGestion: {
                required: "Requerido"
            },
            gestionTrabajadaPor: {
                required: "Requerido"
            },
            gestionFecha: {
                required: "Requerido"
            },
            gestionDescripcion: {
                required: "Requerido"
            }
        }
    });
});