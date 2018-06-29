var editorTransferType;

CDI.DisplayTransferType = (function () {
    var _start = function () {

        $.ajaxSetup({ cache: false });

        editorTransferType = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/transfertypeupdate",
            table: "#grid-data-transfertype",
            idSrc: "TransferTypeId",
            fields: [
            {
                label: "Tipo de Transferencia:",
                name: "TransferType1"
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
                    title: "Crear nuevo Tipo de Transferencia",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Tipo de Transferencia",
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
                    months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    weekdays: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab']
                }
            }
        });

        editorTransferType.on('preSubmit', function (e, o, action) {
            if (action !== 'remove') {
                var TransferType1 = editorTransferType.field('TransferType1');

                // Only validate user input values - different values indicate that
                // the end user has not entered a value
                if (!TransferType1.isMultiValue()) {
                    if (!TransferType1.val()) {
                        TransferType1.error('Tipo Transferencia debe ser informado.');
                    }
                }               

                // If any error was reported, cancel the submission so it can be corrected
                if (this.inError()) {
                    return false;
                }
            }
        });

        $("#grid-data-transfertype").DataTable({
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
                "url": root + "referencetables/listartablareferenciatipodetransferencia",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "TransferType1",
                    "data": "TransferType1",
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
                   { extend: "create", editor: editorTransferType },
                   { extend: "edit", editor: editorTransferType },
                   { extend: "remove", editor: editorTransferType }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayTransferType.start();
});