CDI.DisplayAddressType = (function () {
    var editorAddressType;
    var start = function () {

        editorAddressType = new $.fn.dataTable.Editor({
            ajax: root + "referencetables/addresstypeupdate",
            table: "#grid-data-addresstype",
            idSrc: "AddressTypeId",
            fields: [{
                label: "Tipo de Dirección:",
                name: "AddressType1"
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
                    title: "Crear nuevo Tipo de Dirección",
                    submit: "Crear"
                },
                edit: {
                    button: "Modificar",
                    title: "Modificar Tipo de Dirección",
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

        editorAddressType.on("preSubmit", function (e, o, action) {
            if (action !== "remove") {
                var addressType1 = editorAddressType.field("AddressType1");

                // Only validate user input values - different values indicate that
                // the end user has not entered a value
                if (!addressType1.isMultiValue()) {
                    if (!addressType1.val()) {
                        addressType1.error("Tipo dirección debe ser informado.");
                    }
                }              

                // If any error was reported, cancel the submission so it can be corrected
                if (this.inError()) {
                    return false;
                }
            }
            
        });

        $("#grid-data-addresstype").DataTable({
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
                "lengthMenu": "Mostrar _MENU_ registros"
            },
            "drawCallback": function () {
                $(".dataTables_empty").html("No se encontraron registros.");
            },
            "ajax": {
                "url": root + "referencetables/listartablareferenciatipodireccion",
                "type": "GET"
            },
            "columns": [
                {
                    "name": "AddressType1",
                    "data": "AddressType1",
                    "orderable": false,
                    "class": "text-center"
                },
                {
                    "name": "Hidden",
                    "data": "Hidden",
                    "orderable": false,
                    "class": "text-center",
                    "render": function (data) {
                        return data ? "Si" : "No";
                    }
                }
            ],
            "select": true,
            "buttons": [
                { extend: "create", editor: editorAddressType },
                { extend: "edit", editor: editorAddressType },
                { extend: "remove", editor: editorAddressType }
            ],
            dom: "Brtip"
        });
    };

    return {
        start: start
    }
})();

$(function () {
    CDI.DisplayAddressType.start();
});