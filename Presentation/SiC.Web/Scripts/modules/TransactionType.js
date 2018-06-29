var editor;

CDI.DisplayTransactionType = (function () {
    var _start = function () {

        $.ajaxSetup({ cache: false });

        editor = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/transactiontypeupdate",
            table: "#grid-data-transactiontype",
            idSrc: "TransactionTypeId",
            fields: [
            {
                label: "Tipo de Transacción:",
                name: "TransactionType1"
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
                    title: "Crear nuevo Tipo de Transacción",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Tipo de Transacción",
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

        $('#grid-data-transactiontype').DataTable({
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
                "url": root + "referenceTables/listartablareferenciatipodetransaccion",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "TransactionType1",
                    "data": "TransactionType1",
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
                   { extend: "create", editor: editor },
                   { extend: "edit", editor: editor },
                   { extend: "remove", editor: editor }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayTransactionType.start();
});