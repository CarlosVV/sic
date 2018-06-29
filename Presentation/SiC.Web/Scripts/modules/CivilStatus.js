CDI.DisplayCivilStatus = (function () {
    var editorCivilStatus;
    var _start = function () {

        editorCivilStatus = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/civilstatusupdate",
            table: "#grid-data-civilstatus",
            idSrc: "CivilStatusId",
            fields: [
            {
                label: "Estado Civil:",
                name: "CivilStatus1"
            }, {
                label: "Código:",
                name: "CivilStatusCode"
            }, {
                "label": "¿Oculto?",
                "name": "Hidden",
                "type": "radio",
                "options": [
					{ label: "Sí", value: true },
                    { label: "No", value: false }
                ]
            }
            ],
            i18n: {
                create: {
                    button: "Nuevo",
                    title: "Crear nuevo Estado Civil",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Estado Civil",
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
                    previous: 'Atrás',
                    next: 'Adelante',
                    months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    weekdays: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab']
                }
            }
        });

        editorCivilStatus.on('preSubmit', function (e, o, action) {
            if (action !== 'remove') {
                var CivilStatus1 = editorCivilStatus.field('CivilStatus1');

                // Only validate user input values - different values indicate that
                // the end user has not entered a value
                if (!CivilStatus1.isMultiValue()) {
                    if (!CivilStatus1.val()) {
                        CivilStatus1.error('Estado civil debe ser informado.');
                    }                  
                }

                var CivilStatusCode = editorCivilStatus.field('CivilStatusCode');

                if (!CivilStatusCode.isMultiValue()) {
                    if (!CivilStatusCode.val()) {
                        CivilStatusCode.error('Código estado civil debe ser informado.');
                    }
                }                

                // If any error was reported, cancel the submission so it can be corrected
                if (this.inError()) {
                    return false;
                }
            }
        });

        $('#grid-data-civilstatus').DataTable({
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
                $(".dataTables_empty").html('No se encontraron registros.');
            },
            "ajax": {
                "url": root + "referencetables/listartablareferenciacivilstatus",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "CivilStatus1",
                    "data": "CivilStatus1",
                    "orderable": false,
                    "class": "text-center"
                },
                {
                    "name": "CivilStatusCode",
                    "data": "CivilStatusCode",
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
                { extend: "create", editor: editorCivilStatus },
                { extend: "edit", editor: editorCivilStatus },
                { extend: "remove", editor: editorCivilStatus }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayCivilStatus.start();
});