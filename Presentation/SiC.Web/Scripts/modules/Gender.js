var id;
var editorGender;

CDI.DisplayGender = (function () {
    var _start = function () {

        $.ajaxSetup({ cache: false });

        editorGender = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/genderupdate",
            table: "#grid-data-gender",
            idSrc: "GenderId",
            fields: [
             {

                 label: "Género:",
                 name: "Gender1"
             }, {
                 label: "Código:",
                 name: "GenderCode"
             }, {
                 "label": "¿Oculto?",
                 "name": "Hidden",
                 "type": "radio",
                 "options": [
                     { label: "Si", value: true },
                       { label: "No", value: false }
                 ]
             }],
            i18n: {
                create: {
                    button: "Nuevo",
                    title: "Crear nuevo Genero",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Genero",
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

        editorGender.on("preSubmit", function (e, o, action) {
            if (action !== "remove") {
                var Gender1 = editorGender.field("Gender1");

                // Only validate user input values - different values indicate that
                // the end user has not entered a value
                if (!Gender1.isMultiValue()) {
                    if (!Gender1.val()) {
                        Gender1.error("Género debe ser informado.");
                    }
                }

                var GenderCode = editorGender.field("GenderCode");

                if (!GenderCode.isMultiValue()) {
                    if (!GenderCode.val()) {
                        GenderCode.error("Código género debe ser informado.");
                    }
                }

                // If any error was reported, cancel the submission so it can be corrected
                if (this.inError()) {
                    return false;
                }
            }
        });

        $("#grid-data-gender").DataTable({
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
                "url": root + "referencetables/listartablareferenciagenero",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "Gender1",
                    "data": "Gender1",
                    "orderable": false,
                    "class": "text-center"
                },
                {
                    "name": "GenderCode",
                    "data": "GenderCode",
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
                   { extend: "create", editor: editorGender },
                   { extend: "edit", editor: editorGender },
                   { extend: "remove", editor: editorGender }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayGender.start();
});