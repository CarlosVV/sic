var error = '';

$("#manageFile_file").on('filebatchpreupload', function (data) {
    //fileloaded
    if (error) {
        //$('#manageFile_file').fileinput('clear');
        $("#manageFile_file").fileinput('cancel');
        error = '';
    }

    if ($("#manageFile_documentType").val() == "") {
        $('.fileinput-remove-button').click();
        $('#manageFile_file').fileinput('clear');
        $("#manageFile_documentType").closest(".form-group").addClass("has-error");
        $("#manageFile_documentType").focus();
        CDI.displayNotification("Debe seleccionar tipo de documento antes de subir archivo.", "error");
    } else {
        $("#manageFile_documentType").closest(".form-group").removeClass("has-error");
    }

});

$('#manageFile_file').on('filebatchuploadsuccess', function (event, data, previewId, index) {
    
    $('#fileDataTable').DataTable().ajax.reload();
    $("#manageFile_documentType").val('');
    //show notification of success
    $('#manageFile_file')[0].placeholder = "Solo se permiten archivos PDF de hasta 1.5MB";
    CDI.displayNotification("Su archivo fue cargado exitosamente.", "success");
});

$('#manageFile_documentType').on('change', function () {
    if ($("#manageFile_documentType").val() == "") {
        $("#manageFile_documentType").closest(".form-group").addClass("has-error");
     } else {
        $("#manageFile_documentType").closest(".form-group").removeClass("has-error");
    }
});

$('#manageFile_file').on('fileuploaderror', function (event, data) {
  //  $("#manageFile_file").fileinput('cancel');
    error = 'error';
    $("#manageFile_file").trigger('filereset');
    $('.fileinput-remove-button').click();

    CDI.displayNotification("Por favor verifique el archivo y el tamaño del mismo.", "error");
});

$(function () {
    var tbl = $("#fileDataTable").DataTable({
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
            "lengthMenu": "Mostrar _MENU_ registros",
        },
        "drawCallback": function(settings){
            $(".dataTables_empty").html('No se encontraron registros.');
        },
        "ajax": {
            "url": "/File/GetFileList/"+findingId,
            "type": "GET"
        },
        "columns": [
            {
                "name": "deleteDocCol",
                "data": "Id",
                "orderable": false,
                "class":"text-center",
                "render": function (data, type, row) {
                    return "<a class='text-danger fileDelete text-danger' id=" + data + " ><i class='fa fa-times'></i></a>";
                }
            },
            {
                "name": "documentNameCol",
                "data": "Name",
                "orderable": false,
                "render": function (data, type, row) {
                    return "<a class='text-danger' href='/File/View/" + row["FindingId"] + "/" + row["Id"] + "' >" + data + "</a>";
                }
            },
            {
                "name": "documentTypeCol",
                "data": "DocumentType.Description",
                "orderable":false
            }
        ],
        "dom": "lrtip"
    });

    var deleteDocument = function (row, id) {
       $.ajax({
           url: "/File/Delete/",
           type: "POST",
           dataType: "json",
           data:{Id: id},
           success:function(){
               var tbl = $('#fileDataTable').DataTable();
               tbl.row(row).remove().draw();

               CDI.displayNotification("Su archivo fue eliminado.", "success");
           },
           error: function (data) {
               alert(data);
           }
       });
   };

    $('#fileDataTable').on('click', '.fileDelete', function (e) {
        e.preventDefault();
        // TODO: Usar CDI.displayNotification
        generateNoty('Seguro que desea eliminar documento?', 'notification', true, 'center', 'center', 'y', deleteDocument, 1, $(this).closest('tr'), $(this)[0].id);
    });
});