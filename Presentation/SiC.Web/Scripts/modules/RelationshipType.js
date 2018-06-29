var id;
var editorRelationshipType;

CDI.DisplayRelationshipType = (function () {
    var _start = function () {

        $.ajaxSetup({ cache: false });

        editorRelationshipType = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/relationshiptypeupdate",
            table: "#grid-data-relationshiptype",
            idSrc: "RelationshipTypeId",
            fields: [{
                label: "Tipo de Relación:",
                name: "RelationshipType1"
            }, {
                label: "Código de Relación:",
                name: "RelationshipTypeCode"
            }, {
                "label": "¿Handicapped?",
                "name": "Handicapped",
                "type": "radio",
                "options": [
	     			{ label: "Si", value: true },
                    { label: "No", value: false }
                ]
            }, {
                "label": "¿WithChildren?",
                "name": "WithChildren",
                "type": "radio",
                "options": [
	     			{ label: "Si", value: true },
                    { label: "No", value: false }
                ]
            }, {
                "label": "¿WidowCertification?",
                "name": "WidowCertification",
                "type": "radio",
                "options": [
	     			{ label: "Si", value: true },
                    { label: "No", value: false }
                ]
            }, {
                "label": "¿SchoolCertification?",
                "name": "SchoolCertification",
                "type": "radio",
                "options": [
	     			{ label: "Si", value: true },
                    { label: "No", value: false }
                ]
            }, {
                "label": "¿VitalData?",
                "name": "VitalData",
                "type": "radio",
                "options": [
	     			{ label: "Si", value: true },
                    { label: "No", value: false }
                ]
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
                    title: "Crear nuevo Tipo de Relación",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Tipo de Relación",
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

        editorRelationshipType.on('preSubmit', function (e, o, action) {
            if (action !== 'remove') {
                var RelationshipType1 = editorRelationshipType.field('RelationshipType1');

                // Only validate user input values - different values indicate that
                // the end user has not entered a value
                if (!RelationshipType1.isMultiValue()) {
                    if (!RelationshipType1.val()) {
                        RelationshipType1.error('Tipo Relación debe ser informado.');
                    }
                }

                var RelationshipTypeCode = editorRelationshipType.field('RelationshipTypeCode');

                if (!RelationshipTypeCode.isMultiValue()) {
                    if (!RelationshipTypeCode.val()) {
                        RelationshipTypeCode.error('Código Relación debe ser informado.');
                    }
                }               

                // If any error was reported, cancel the submission so it can be corrected
                if (this.inError()) {
                    return false;
                }
            }
        });

        $('#grid-data-relationshiptype').DataTable({
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
                "url": root + "referencetables/listartablareferenciatipoderelacion",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "RelationshipType1",
                    "data": "RelationshipType1",
                    "orderable": true,
                    "class": "text-center"
                },
                {
                "name": "RelationshipTypeCode",
                "data": "RelationshipTypeCode",
                "orderable": true,
                "class": "text-center"
                },
                {
                    "name": "Handicapped",
                    "data": "Handicapped",
                    "orderable": false,
                    "class": "text-center",
                    "render": function (data, type, row) {
                        return data ? "Si" : "No";
                    }
                },
                {
                    "name": "WithChildren",
                    "data": "WithChildren",
                    "orderable": false,
                    "class": "text-center",
                    "render": function (data, type, row) {
                        return data ? "Si" : "No";
                    }
                },
                {
                    "name": "WidowCertification",
                    "data": "WidowCertification",
                    "orderable": false,
                    "class": "text-center",
                    "render": function (data, type, row) {
                        return data ? "Si" : "No";
                    }
                },
                {
                    "name": "SchoolCertification",
                    "data": "SchoolCertification",
                    "orderable": false,
                    "class": "text-center",
                    "render": function (data, type, row) {
                        return data ? "Si" : "No";
                    }
                },
                {
                    "name": "VitalData",
                    "data": "VitalData",
                    "orderable": false,
                    "class": "text-center",
                    "render": function (data, type, row) {
                        return data ? "Si" : "No";
                    }
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
                   { extend: "create", editor: editorRelationshipType },
                   { extend: "edit", editor: editorRelationshipType },
                   { extend: "remove", editor: editorRelationshipType }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayRelationshipType.start();
});