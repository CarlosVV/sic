var editorInternetType;

CDI.DisplayInternetType = (function () {
    var _start = function () {

        $.ajaxSetup({ cache: false });

        editorInternetType = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/internettypeupdate",
            table: "#grid-data-internettype",
            idSrc: "InternetTypeId",
            fields: [
            {
                label: "Tipo de Internet:",
                name: "InternetType1"
            }, {
                "label": "¿Oculto?",
                "name": "Hidden",
                "type": "radio",
                "options": [
	     			{ label: "Si", value: true },
                    { label: "No", value: false }
                ]
            }
            ],
            i18n: {
                create: {
                    button: "Nuevo",
                    title: "Crear nuevo Tipo de Internet",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Tipo de Internet",
                    submit: "Actualizar"
                },
                remove: {
                    button: "Eliminar",
                    title: "Eliminar",
                    submit: "Eliminar",
                    confirm: {
                        _: "¿Está seguro de eliminar %d registros?",
                        1: "¿Está seguro de eliminar 1 registro?"
                    }
                },
                error: {
                    system: "Ocurrió un error en el sistema, por favor contacte con el administrador del Sistema."
                },
                datetime: {
                    previous: "Atrás",
                    next: "Adelante",
                    months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"],
                    weekdays: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"]
                }
            }
        });

        editorInternetType.on("preSubmit", function (e, o, action) {
            if (action !== "remove") {
                var InternetType1 = editorInternetType.field("InternetType1");

                // Only validate user input values - different values indicate that
                // the end user has not entered a value
                if (!InternetType1.isMultiValue()) {
                    if (!InternetType1.val()) {
                        InternetType1.error("Tipo internet debe ser informado.");
                    }
                }               

                // If any error was reported, cancel the submission so it can be corrected
                if (this.inError()) {
                    return false;
                }
            }
        });

        $("#grid-data-internettype").DataTable({
            "autoWidth": false,
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
            "drawCallback": function (settings) {
                $(".dataTables_empty").html("No se encontraron registros.");
            },
            "ajax": {
                "url": root + "referencetables/listartablareferenciatipointernet",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "InternetType1",
                    "data": "InternetType1",
                    "orderable": false,
                    "class": "text-center"
                },
                {
                    "name": "Hidden",
                    "data": "Hidden",
                    "orderable": false,
                    "class": "text-center",
                    "render": function (data, type, row) {
                        return data ? "Si" : "No";
                    }
                }
            ],
            "select": true,
            "buttons": [
                   { extend: "create", editor: editorInternetType },
                   { extend: "edit", editor: editorInternetType },
                   { extend: "remove", editor: editorInternetType }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayInternetType.start();
});