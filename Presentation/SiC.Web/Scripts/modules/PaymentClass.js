var editorPaymentClass;

CDI.DisplayPaymentClass = (function () {
    var _start = function () {

        $.ajaxSetup({ cache: false });

        editorPaymentClass = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/classupdate",
            table: "#grid-data-paymentclass",
            idSrc: "ClassId",
            fields: [
            {
                label: "Clase de pago:",
                name: "Class1"
            }, {
                label: "Concepto:",
                name: "Concept"
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
                    title: "Crear nueva Clase de Pago",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Clase de Pago",
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

        editorPaymentClass.on('preSubmit', function (e, o, action) {
            if (action !== 'remove') {
                var Class1 = editorPaymentClass.field('Class1');

                // Only validate user input values - different values indicate that
                // the end user has not entered a value
                if (!Class1.isMultiValue()) {
                    if (!Class1.val()) {
                        Class1.error('Clase de Pago debe ser informado.');
                    }
                }

                var Concept = editorPaymentClass.field('Concept');

                if (!Concept.isMultiValue()) {
                    if (!Concept.val()) {
                        Concept.error('Concepto de Pago debe ser informado.');
                    }
                }

                // If any error was reported, cancel the submission so it can be corrected
                if (this.inError()) {
                    return false;
                }
            }
        });

        $('#grid-data-paymentclass').DataTable({
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
                "url": root + "referencetables/listartablareferenciaclasepago",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "Class1",
                    "data": "Class1",
                    "orderable": false,
                    "class": "text-center"
                },
                {
                    "name": "Concept",
                    "data": "Concept",
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
                   { extend: "create", editor: editorPaymentClass },
                   { extend: "edit", editor: editorPaymentClass },
                   { extend: "remove", editor: editorPaymentClass }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayPaymentClass.start();
});