CDI.ThirdParties = (function() {
    var _caseData;
    var _tableResumen;
    
    var _showDialogMessage = function (message) {
        toastr.warning(message, "SiC");
    };

    var _showCaseDetail = function () {
        _balance = _caseData.Balance;
           
        CDI.CaseSearch.header(_caseData);

        if ($.fn.dataTable.isDataTable("#tblResumen")) {
            _tableResumen.destroy();
        }

        _tableResumen = $("#tblResumen").DataTable({
            "ordering": false,
            "processing": true,
            "autoWidth": false,
            "drawCallback": function (settings) {
                $(".dataTables_empty").html("No se encontraron registros.");
            },
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
                "lengthMenu": "Mostrar _MENU_ gestiones"
            },
            "ajax": {
                "url": root + "thirdpartypayment/findbycaseid",
                "type": "POST",
                "data": function (data) {
                    data.caseId = _caseData.CaseId;

                    return data;
                }
            },
            "columns": [
                {
                    "data": "NombreEntidad"
                },
                {
                    "data": "NroOrden"
                },
                {
                    "data": "NroParticipante"
                },
                {
                    "data": "MontoOrden"
                }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $("td:eq(3)", nRow).addClass("text-right");
            },
            "dom": "rtp"
        });
    };

    var _initControls = function () {
        $("#unsolopago").autoNumeric("init", {
            aSep: ",",
            aDec: ".",
            aSign: "$ "
        });
        $("#pago1").autoNumeric("init", {
            aSep: ",",
            aDec: ".",
            aSign: "$ "
        });
        $("#pago2").autoNumeric("init", {
            aSep: ",",
            aDec: ".",
            aSign: "$ "
        });
        $("#agencia_monto_orden").autoNumeric("init", {
            aSep: ",",
            aDec: ".",
            aSign: "$ "
        });
        $("#direccion_zipcode").mask("99999-9999");
        $("#direccion_entidad_codigopostal").mask("99999-9999");
        $("#custodio_codigopostal").mask("99999-9999");
        $("#unsolopago").autoNumeric("set", 0);
        $("#pago1").autoNumeric("set", 0);
        $("#pago2").autoNumeric("set", 0);

        $("#info_menor").prop("disabled", true);
        $("#info_muerte").prop("disabled", true);
        $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
        $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");
        $("#entity-panel").fadeOut(1000).addClass("hidden");
        $("#custodio-panel").fadeOut(1000).addClass("hidden");

        $("#fecha_terminacion").parent().hide();
        $("#numero_orden_terminacion").hide();
        $("label[for=\"fecha_terminacion\"]").hide();
        $("label[for=\"numero_orden_terminacion\"]").hide();

        $("#fnacimiento, #fradicacion, #fecha_terminacion").datepicker({
            startDate: "-100y",
            endDate: "-1d"
        });

        $("#Courts").prop("disabled", "disabled");
    };

    var _saveThirdPartyPayment = function () {
        // TipoEntidad =  1: Custodio, 2: Nueva Entidad/Otro, 3: Court
        var tipo_entidad = 0;
        if ($("#pago_dirigido_cust").is(":checked")) {
            tipo_entidad = 1;
        }

        if ($("#pago_dirigido_entidad").is(":checked") &&
            $("#Courts").val() === "-1") {
            tipo_entidad = 2;
        }

        if ($("#pago_dirigido_entidad").is(":checked") &&
            $("#Courts").val() !== "0" &&
            $("#Courts").val() !== "-1") {
            tipo_entidad = 3
        }
        if (tipo_entidad == 0) {
            _showDialogMessage("Debe elegir una entidad o custodio.");
            return false;
        }
        // Validar que se seleccione una corte o seleccione otro
        if ($("#pago_dirigido_entidad").is(":checked") &&
            $("#Courts").val() === "") {
            _showDialogMessage("Debe elegir una corte");
            return false;
        }

        if (tipo_entidad == 1 &&
            $("#custodio_primer_nombre").val().trim().length === 0) {
            _showDialogMessage("Primer nombre de Custodio debe estar lleno!");
            return false;
        }

        if (tipo_entidad == 2 &&
            $("#nombre_entidad").val().trim().length === 0) {
            _showDialogMessage("Nombre de Entidad debe estar lleno");
            return false;
        }

        if ($("#direccion_entidad_linea2").val().trim().length > 0 &&
            $("#direccion_entidad_linea1").val().trim().length === 0) {
            _showDialogMessage("Linea 1 en Dirección de Entidad debe estar lleno si Linea2 esta lleno");
            return false;
        }

        if ($("#custodio_segundo_nombre").val().trim().length > 0 &&
            $("#custodio_primer_nombre").val().trim().length === 0) {
            _showDialogMessage("Primer nombre de Custodio debe estar lleno si Segundo Nombre esta lleno");
            return false;
        }

        if ($("#custodio_segundo_apellido").val().trim().length > 0 &&
            $("#custodio_primer_apellido").val().trim().length === 0) {
            _showDialogMessage("Primer apellido de Custodio debe estar lleno si Segundo apellido esta lleno");
            return false;
        }

        if ($("#custodio_linea2").val().trim().length > 0 &&
            $("#custodio_linea1").val().trim().length === 0) {
            _showDialogMessage("Linea 1 en Dirección de Custodio debe estar lleno si Linea2 esta lleno");
            return false;
        }

        if ($("input:radio[name=tipo_pago]:checked").val() === "1" &&
            $("#unsolopago").autoNumeric("get") === 0) {
            _showDialogMessage("Debe ingresar un valor para Un Solo pago");
            return false;
        }
        if ($("input:radio[name=tipo_pago]:checked").val() === "2" &&
            ($("#pago1").autoNumeric("get") + $("#pago2").autoNumeric("get") === 0)) {
            _showDialogMessage("Debe ingresar un Pago 1 o un Pago 2");
            return false;
        }

        var orden_terminacion = false;

        if ($("input:radio[name=orden_terminacion]:checked").val() == "0") {
            orden_terminacion = true;

            if ($("#agencia_numerocaso").val().trim().length +
                $("#agencia_idparticipante").val().trim().length +
                $("#agencia_monto_orden").val().trim().length == 0) {
                _showDialogMessage("Si no hay orden de terminación, debe llenarse participante y monto de orden");
                return false;
            }

            if (parseFloat($("#pago1").autoNumeric("get")) + parseFloat($("#pago2").autoNumeric("get")) > parseFloat($("#agencia_monto_orden").autoNumeric("get"))) {
                _showDialogMessage("Si no hay orden de terminación, pago1 + pago2 < monto según orden");
                return false;
            }

            if (parseFloat($("#unsolopago").autoNumeric("get")) > parseFloat($("#agencia_monto_orden").autoNumeric("get"))) {
                _showDialogMessage("Si no hay orden de terminación, un solo pago < monto según orden");
                return false;
            }
        } else {
            orden_terminacion = false;
            if ($("#fecha_terminacion").val().trim().length == 0 ||
                $("#numero_orden_terminacion").val().trim().length == 0) {
                _showDialogMessage("Si hay orden de terminación debe llenarse la orden y fecha de terminacion");
                return false;
            }
            else
            {
                if (!CDI.isValidDateUS($("#fecha_terminacion").val())) {
                    _showDialogMessage("Por favor, escriba una fecha válida para Fecha Terminación.");                    
                    return false;
                }
            }
        }

        var entidad = {};
        var court = 0;
        if ($("#pago_dirigido_entidad").is(":checked") &&
            $("#Courts").val() != "0" && $("#Courts").val() != "-1" && $("#Courts").val() != "") {
            var entidad = JSON.parse($("#Courts").val());
            court = entidad.CourtId;
        }

        var data = {
            CaseId: _caseData.CaseId,
            CaseDetailId: _caseData.CaseDetailId,
            ClaimNumber: orden_terminacion ? $("#agencia_idparticipante").val() : null,
            OrderIdentifier: orden_terminacion ? $("#agencia_numerocaso").val() : null,
            TerminationFlag: $("input:radio[name=orden_terminacion]:checked").val() === "1" ? true : false,
            EffectiveDate: !orden_terminacion ? $("#fecha_terminacion").val() : null,
            TerminationOrderNumber: $("#numero_orden_terminacion").val(),
            SinglePaymentAmount: $("#unsolopago").autoNumeric("get"),
            FirstInstallmentAmount: $("#pago1").autoNumeric("get"),
            SecondInstallmentAmount: $("#pago2").autoNumeric("get"),
            Comment: $("#observaciones").val(),
            EntityTypeId: tipo_entidad,
            CourtId: court,
            EntityName: $("#nombre_entidad").val(),
            EntityAddressLine1: $("#direccion_entidad_linea1").val(),
            EntityAddressLine2: $("#direccion_entidad_linea2").val(),
            EntityCityId: $("#direccion_entidad_ciudad").val(),
            EntityPostalCode: $("#direccion_entidad_codigopostal").val(),
            CustodyFirstName: $("#custodio_primer_nombre").val(),
            CustodySecondName: $("#custodio_segundo_nombre").val(),
            CustodyFirstLastName: $("#custodio_primer_apellido").val(),
            CustodySecondLastName: $("#custodio_segundo_apellido").val(),
            CustodyAddressLine1: $("#custodio_linea1").val(),
            CustodyAddressLine2: $("#custodio_linea2").val(),
            CustodyCityId: $("#custodio_ciudad").val(),
            CustodyPostalCode: $("#custodio_codigopostal").val(),
            OrderAmount: orden_terminacion ? $("#agencia_monto_orden").autoNumeric("get") : null,
            TerminationDate: !orden_terminacion ? $("#fecha_terminacion").val() : null,
        };

        $.ajax({
            url: root + "thirdpartypayment/insert",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ model: data }),
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();
            if (response.data.Status == "OK") {
                _showDialogMessage("Datos guardados");
                $("#frmPagoTerceros").trigger("reset");
                $("option").attr("selected", false);
                $("select").val("");
                $("#frmPagoTerceros").fadeOut(1000).addClass("hidden");
            } else {
                CDI.hideWaitingMessage();
                _showDialogMessage("Ha ocurrido un error. Consulte con el Administrador.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
            _showDialogMessage("Ha ocurrido un error. Consulte con el Administrador.");
        });
    };

    var _bindUIActions = function () {
        $(CDI.CaseSearch).on("case.selected", function(e, data) {
            _caseData = data.selectedCase;

            _showCaseDetail();

            $("#frmPagoTerceros").fadeIn(1000).removeClass("hidden");
        });

        $(CDI.CaseSearch).on("started", function(e, data) {
            $("#frmPagoTerceros").fadeOut(1000).addClass("hidden");
        });

        $("#btnSave").on("click", function (evt) {
            evt.preventDefault();

            $.ajax({
                type: "GET",
                url: root + "case/getbalance/" + _caseData.CaseDetailId,
                dataType: "json",
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) {
                CDI.hideWaitingMessage();

                var isValid = true;
                
                var balance = parseFloat(response);
                var total = parseFloat($("#pago1").autoNumeric("get")) + parseFloat($("#pago2").autoNumeric("get")) + parseFloat($("#unsolopago").autoNumeric("get"));
                if (total > balance) {
                    _showDialogMessage("El caso no tiene suficiente balance para registrar el pago");
                }
                else {
                    _saveThirdPartyPayment();
                }
            }).fail(function () {
                CDI.hideWaitingMessage();
            });

        });

        $("#btnNew").on("click", function (evt) {
            evt.preventDefault();

            $("#frmPagoTerceros").trigger("reset");
            $("option").attr("selected", false);
            $("select").val(0);
            $("#frmPagoTerceros").fadeOut(1000).addClass("hidden");

        });

        $("#btnCancel").on("click", function (evt) {
            evt.preventDefault();

            $("#frmPagoTerceros").trigger("reset");
            $("option").attr("selected", false);
            $("select").val(0);
            $("#frmPagoTerceros").fadeOut(1000).addClass("hidden");

        });

        $("input:radio[name=pago_dirigido_entidad]").click(function() {
            $("#pago_dirigido_entidad").prop("checked", true);
            $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
            $("#Courts").prop("disabled", false);
            $("#Courts").val("");
            $("#custodio-panel").fadeOut(1000).addClass("hidden");
        });

        $("input:radio[name=pago_dirigido_cust]").click(function() {
            $("#pago_dirigido_cust").prop("checked", true);
            $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");
            $("#Courts").prop("disabled", "disabled");

            $("#nombre_entidad_court").val("");
            $("#direccion_entidad").val("");

            $("#custodio-panel").fadeIn(1000).removeClass("hidden");
            $("#entity-panel").fadeOut(1000).addClass("hidden");
        });

        $("input:radio[name=orden_terminacion]").click(function() {
            if ($("input:radio[name=orden_terminacion]:checked").val() === "1") {
                $("#fecha_terminacion").parent().show();
                $("#numero_orden_terminacion").show();                
                $("label[for=\"fecha_terminacion\"]").show();
                $("label[for=\"numero_orden_terminacion\"]").show();                
            } else {
                $("#fecha_terminacion").parent().hide();
                $("#numero_orden_terminacion").hide();                
                $("label[for=\"fecha_terminacion\"]").hide();
                $("label[for=\"numero_orden_terminacion\"]").hide();                
            }
        });

        $("input:radio[name=tipo_pago]").click(function() {
            if ($("input:radio[name=tipo_pago]:checked").val() === "1") {
                $("#pago1").autoNumeric("set", 0);
                $("#pago2").autoNumeric("set", 0);
                $("#pago1").prop("readonly", true);
                $("#pago2").prop("readonly", true);
                $("#unsolopago").prop("readonly", false);
            }
            if ($("input:radio[name=tipo_pago]:checked").val() === "2") {
                $("#unsolopago").autoNumeric("set", 0);
                $("#unsolopago").prop("readonly", true);
                $("#pago1").prop("readonly", false);
                $("#pago2").prop("readonly", false);
            }

        });

        $("#Courts").change(function() {
            var selectedValue = $(this).val();
            var selectedText = $("#Courts option:selected").text();
            $("#entity-panel").fadeOut(1000).addClass("hidden");
            $("#custodio-panel").fadeOut(1000).addClass("hidden");

            if (selectedText === "Otro") {
                $("#entity-panel").fadeIn(1000).removeClass("hidden");
                $("#custodio-panel").fadeOut(1000).addClass("hidden");
            } else {
                var court = JSON.parse(selectedValue);
                $("#nombre_entidad_court").val(court.CourtName);
                $("#direccion_entidad").val(
                    "" + court.AddressLine1 + " " +
                    "" + court.AddressLine2 + " " +
                    "" + court.City + " " +
                    "" + court.ZipCode + " " +
                    court.ZipCodeExt
                );
            }
        });
    };

    var _init = function() {
        _initControls();
        _bindUIActions();
    };

    return {
        init: _init
    };
})();

$(function() {
    CDI.CaseSearch.init();
    CDI.ThirdParties.init();
});