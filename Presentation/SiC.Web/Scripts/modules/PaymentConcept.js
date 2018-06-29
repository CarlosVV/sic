var id;
var editorPaymentConcept;

CDI.DisplayPaymentConcept = (function () {
    var _start = function () {

        editorPaymentConcept = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/conceptupdate",
            table: "#grid-data-concept",
            idSrc: "ConceptId",
            fields: [{
                label: "Tipo:",
                name: "ConceptType"
            }, {
                label: "Concepto:",
                name: "Concept1"
            }, {
                label: "Código:",
                name: "ConceptCode"
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
                    title: "Crear nuevo Concepto de Pago",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Concepto de Pago",
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

        editorPaymentConcept.on('preSubmit', function (e, o, action) {
            if (action !== 'remove') {
                var ConceptType = editorPaymentConcept.field('ConceptType');

                // Only validate user input values - different values indicate that
                // the end user has not entered a value
                if (!ConceptType.isMultiValue()) {
                    if (!ConceptType.val()) {
                        ConceptType.error('Tipo Concepto debe ser informado.');
                    }
                }

                var Concept1 = editorPaymentConcept.field('Concept1');

                if (!Concept1.isMultiValue()) {
                    if (!Concept1.val()) {
                        Concept1.error('Concepto debe ser informado.');
                    }
                }

                var ConceptCode = editorPaymentConcept.field('ConceptCode');

                if (!ConceptCode.isMultiValue()) {
                    if (!ConceptCode.val()) {
                        ConceptCode.error('Código Concepto debe ser informado.');
                    }
                }

                // If any error was reported, cancel the submission so it can be corrected
                if (this.inError()) {
                    return false;
                }
            }
        });

        $('#grid-data-concept').DataTable({
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
                "url": root + "referencetables/listartablareferenciaconcept",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "ConceptType",
                    "data": "ConceptType",
                    "orderable": false,
                    "class": "text-center"
                },
                {
                    "name": "Concept1",
                    "data": "Concept1",
                    "orderable": false,
                    "class": "text-center"
                },
                {
                    "name": "ConceptCode",
                    "data": "ConceptCode",
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
                { extend: "create", editor: editorPaymentConcept },
                { extend: "edit", editor: editorPaymentConcept },
                { extend: "remove", editor: editorPaymentConcept }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayPaymentConcept.start();
});