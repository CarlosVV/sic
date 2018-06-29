CDI.DisplayPayments = (function () {
    var balance = 0;
    var caseData;
    var tablePeremptory;
    var tableInvestment;
    var tableIpp;
    var documentTypes;
    var startDateDecision = new Date();
    var startDateVisita = new Date();
    var startDateNotificacion = new Date();

    var showDialogMessage = function (message) {
        toastr.warning(message, "SiC");
    };

    var expandPanel = function (button) {
        if (button.hasClass("panel-collapsed")) {
            button.parents(".panel").find(".panel-body").slideDown();
            button.removeClass("panel-collapsed");
            button.find("i").removeClass("fa-chevron-down").addClass("fa-chevron-up");
        }
    };

    var collapsePanel = function (button) {
        if (!button.hasClass("panel-collapsed")) {
            button.parents(".panel").find(".panel-body").slideUp();
            button.addClass("panel-collapsed");
            button.find("i").removeClass("fa-chevron-up").addClass("fa-chevron-down");
        }
    };

    var calculateTotalInversion = function () {
        var sumaInversiones = 0;
        for (i = 0; i < $("#tblInvestment tbody tr:has(td)").length; i++) {
            var $cellInversion = $($("#tblInvestment tbody tr:has(td)")[i]).find("td:eq(3)");
            var inversion = $cellInversion.text();
            var inversionValor = parseFloat(inversion.replace("$ ", ""));

            if (isNaN(inversion)) {
                inversionValor = 0;
            }

            sumaInversiones = sumaInversiones + inversionValor;

            $cellInversion.addClass("text-right");
        }
        if (isNaN(sumaInversiones)) {
            sumaInversiones = 0;
        }
        $("#inv_totalinversion").autoNumeric("set", sumaInversiones);

        var foot = $("#tblInvestment").find("tfoot");
        if (!foot.length) {
            foot = $("<tfoot>").appendTo("#tblInvestment");
        }
        foot.html("<tr><th></th><th></th><th class=\"text-right\"><strong>Total</strong></th><th class=\"text-right\">" + $("#inv_totalinversion").val() + "</th></tr>");
    };

    var calculateNewInversions = function () {
        var sumaInversiones = 0;
        $("#tblAddInvestment tbody tr:has(td)").each(function (index, element) {
            var $element = $(element);
            var inversionValor = 0;
            var $inversion = $element.find("#inversion");
            if (($inversion.length === 0) ||
                ($inversion.val() === "")) {
                inversionValor = 0;
            }
            
            var inversion = $inversion.autoNumeric("get");
            inversionValor = parseFloat(inversion);
            if (isNaN(inversionValor)) {
                inversionValor = 0;
            }
            sumaInversiones = sumaInversiones + inversionValor;
        });
        $("#inv_totalinversiones").autoNumeric("set", sumaInversiones);

        var foot = $("#tblAddInvestment").find("tfoot");
        if (!foot.length) {
            foot = $("<tfoot>").appendTo("#tblAddInvestment");
        }
        foot.html("<tr><td></td><td></td><td class=\"text-right\"><strong>Total " + $("#inv_totalinversiones").val() + "</strong></td></tr>");
    };

    var calculateTotalPendingDiets = function () {
        var sumaMontos = 0;
        $("#tblPendingDietPeriod > tbody > tr").each(function (index, element) {
            var $element = $(element);
            var cantidadValor = 0;
            var $totalPagar = $element.find("#total-pagar");
            if (($totalPagar.length === 0) ||
                ($totalPagar.val() === "")) {
                cantidadValor = 0;
            }
            else {
                cantidadValor = $totalPagar.autoNumeric("get");
            }

            cantidadValor = parseFloat(cantidadValor);

            if (isNaN(cantidadValor)) {
                cantidadValor = 0;
            }
            
            sumaMontos = sumaMontos + cantidadValor;
        });
        $("#dieta_montototal").autoNumeric("set", sumaMontos);

        var foot = $("#tblPendingDietPeriod").find("tfoot");
        if (!foot.length) {
            foot = $("<tfoot>").appendTo("#tblPendingDietPeriod");
        }
        foot.html("<tr><td colspan=\"6\" class=\"text-right\"><strong>Total: " + $("#dieta_montototal").val() + "</strong></td></tr>");
    };

    var showTableInvestment = function(caseId) {
        tableInvestment = $("#tblInvestment").DataTable({
            "ordering": false,
            "autoWidth": false,
            "pagingType": "simple_numbers",
            "processing": true,            
            "deferRender": true,
            "drawCallback": function(settings) {
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
                "url": root + "paymentregistration/getsummaryinvestments",
                "type": "POST",
                "data": function(data) {
                    data.caseId = caseId;

                    return data;
                }
            },
            "columns": [{
                "data": "Beneficiario.Nombre"
            }, {
                "data": "Beneficiario.Relacion"
            }, {
                "data": "Fecha"
            }, {
                "data": "Cantidad"
            }],
            "fnInitComplete": function(oSettings, json) {
                calculateTotalInversion();
            },
            "dom": "rtp"
        });
    };

    var showTablePeremptory = function(caseId) {
        tablePeremptory = $("#tblPeremptory").DataTable({
            "ordering": false,
            "autoWidth": false,
            "pagingType": "simple_numbers",
            "processing": true,            
            "deferRender": true,
            "drawCallback": function(settings) {
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
                url: root + "paymentregistration/getsummaryperemptories",
                type: "POST",
                data: function(data) {
                    data.caseId = caseId;

                    return data;
                }
            },
            "columns": [{
                "data": "Beneficiario"
            }, {
                "data": "Relacion"
            }, {
                "data": "Fecha"
            }, {
                "data": "MontoPagado"
            }],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $("td:eq(3)", nRow).addClass("text-right");
            },
            "dom": "rtp"
        });
    };

    var showTableIpp = function (caseDetailId) {
        tableIpp = $("#tblIpp").DataTable({
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
                "url": root + "paymentregistration/getsummaryipps",
                "type": "POST",
                "data": function (data) {
                    data.caseDetailId = caseDetailId;

                    return data;
                }
            },
            "columns": [{
                "data": "FechaAdjudicacion"
            },
            {
                "data": "Semanas"
            },
            {
                "data": "Mensualidad"
            },
            {
                "data": "CantidadAdjudicada"
            }],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $("td:eq(0)", nRow).addClass("text-right");
                $("td:eq(1)", nRow).addClass("text-right");
                $("td:eq(2)", nRow).addClass("text-right");
                $("td:eq(3)", nRow).addClass("text-right");
            },
            "dom": "rtp"
        });
    };

    var showMonthlyPaymentAndNumberOfWeeks = function () {

        var data = {
            "caseDate": caseData.FechaRadicacion,
            "caseId": caseData.CaseId
        };

        $.ajax({
            url: root + "paymentregistration/getmonthlypaymentandnumberofweeks",
            data: data,
            dataType: "json",
            type: "POST",            
            success: function (response) {
                if (response != null && response.Success) {
                    var mensualidad = parseFloat(response.Data);
                    $("#ipp_mensualidad").autoNumeric("set", mensualidad);
                }
                else {
                    showDialogMessage("Mensualidad no definida para el año del caso.");
                }
            }
        });
    };

    var showCaseDetail = function () {
        balance = caseData.Balance;

        $("#info_numerocaso").text(caseData.NumeroCaso);
        $("#info_lesionado").text(caseData.Lesionado);
        $("#info_ssn").text(caseData.SSN);
        $("#info_faccidente").text(caseData.FechaAccidente);
        $("#info_fradicacion").text(caseData.FechaRadicacion);
        $("#info_fnacimiento").text(caseData.FechaNacimiento);
        $("#direccion_linea1").text(caseData.Direccion.Linea1);
        $("#direccion_linea2").text(caseData.Direccion.Linea2);
        $("#direccion_ciudad").text(caseData.Direccion.Ciudad);
        $("#direccion_estado").text(caseData.Direccion.Estado);
        $("#direccion_zipcode").text(caseData.Direccion.ZipCode);
        $("#info_region").text(caseData.Region);
        $("#info_dispensario").text(caseData.Dispensario);
        $("#info_ebt").text(caseData.EBT);

        $("#info_patrono").text(caseData.Patrono.Nombre);
        $("#info_ein").text(caseData.Patrono.EIN);
        $("#info_estatus_patronal").text(caseData.Patrono.Estatus);
        $("#info_poliza").text(caseData.Patrono.NumeroPoliza);

        if (caseData.CasoMenor) {
            $("#info_menor").prop("checked", true);
            $("#ddlDocumentType option[value='3']").remove();

            if (!caseData.EsLesionado) {
                $("#ddlDocumentType option[value='1']").remove();
            }
        }

        if (caseData.CasoMuerte) {
            $("#info_muerte").prop("checked", true);
        }

        if (caseData.Balance === 0) {
            $("#ddlDocumentType option[value='1']").remove();
            $("#ddlDocumentType option[value='2']").remove();
            $("#ddlDocumentType option[value='3']").remove();
        }
        else {
            if (caseData.TienePerentorio) {
                $("#ddlDocumentType option[value='3']").remove();
            }

            if (caseData.FechaAccidenteMenor1984) {
                $("#ddlDocumentType option[value='1']").remove();
            }

            if (caseData.CasoMuerte &&
                caseData.TieneInversionMenor3 &&
                (caseData.CivilStatusId == 3 || caseData.CivilStatusId == 6)) {
                $("#ddlDocumentType option[value='1']").remove();
            }
        }

        if (caseData.EsBeneficiario) {
            $("#info_benef_relacion").text(caseData.Beneficiario.Relacion);
            $("#info_benef_nombre").text(caseData.Beneficiario.Nombre);
            $("#info_benef_ssn").text(caseData.Beneficiario.SSN);
            $("#info_benef_fnac").text(caseData.Beneficiario.FechaNacimiento);
            $("#info_benef_ebt").text(caseData.Beneficiario.EBT);

            $("#case-beneficiary-info").fadeIn(1000).removeClass("hidden");
        }

        $("#payment-method-panel").fadeIn(1000).removeClass("hidden");
    };

    var resetFields = function () {
        $("#ipp_adjudicacionadicional").autoNumeric("set", 0);
        $("#ipp_semanas").autoNumeric("set", 0);
        $("#ipp_mensualidad").autoNumeric("set", 0);
        $("#ipp_num_transaccion").val("");
        $("#ipp_observaciones").val("");
        $("#ipp_fechaadjudicacion").val("");
        $("#tblIpp tbody").find("tr").remove();
        if ($.fn.dataTable.isDataTable("#tblIpp")) {
            tableIpp.destroy();
        }
        $("#tblDesglosarIpp > tbody > tr").not(".hidden").remove();

        $("#due_cantidadsolicitada").autoNumeric("set", 0);
        $("#due_num_transaccion").val("");
        $("#due_beneficiario").val(0);
        $("#due_relacion").val("");
        $("#due_observaciones").val("");
        $("#tblPeremptory tbody").find("tr").remove();
        if ($.fn.dataTable.isDataTable("#tblPeremptory")) {
            tablePeremptory.destroy();
        }

        $("#inv_num_transaccion").val("");
        $("#inv_fechaemisiondecision").val("");
        $("#inv_totalinversiones").autoNumeric("set", 0);
        $("#inv_totalinversion").autoNumeric("set", 0);
        $("#Inv_observaciones").val("");
        $("#inv_beneficiario").val("");
        $("#inv_beneficiario").removeAttr("disabled");
        $("#inv_relacion").val("");
        $("#tblAddInvestment tbody").find("tr").remove();
        $("#tblAddInvestment tfoot").find("tr").remove();
        if ($.fn.dataTable.isDataTable("#tblInvestment")) {
            tableInvestment.destroy();
        }
        $("#inv_fechaemisiondecision").val("");

        $("#info_menor").prop("disabled", true);
        $("#info_muerte").prop("disabled", true);
        $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
        $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");

        $("#panel_entidad").hide();
        $("#panel_custodio").hide();

        $("#dieta_fechadecision").val("");
        $("#dieta_fvisita").val("");
        $("#dieta_fnotificacion").val("");
        $("#dieta_numerocaso").val("");
        $("#dieta_observaciones").val("");
        $("#tblPendingDietPeriod tbody").find("tr").remove();
        $("#tblPendingDietPeriod tfoot").find("tr").remove();

        $("#info_benef_relacion").text("");
        $("#info_benef_nombre").text("");
        $("#info_benef_ssn").text("");
        $("#info_benef_fnac").text("");
        $("#info_benef_ebt").text("");

        $("#case-beneficiary-info").fadeOut(1000).addClass("hidden");
    };

    var clearFields = function () {
        $("#info_numerocaso").text("");
        $("#info_lesionado").text("");
        $("#info_ssn").text("");
        $("#info_ebt").text("");
        $("#info_faccidente").text("");
        $("#info_fradicacion").text("");
        $("#info_fnacimiento").text("");
        $("#info_benef_relacion").text("");
        $("#info_benef_nombre").text("");
        $("#info_benef_ssn").text("");
        $("#info_benef_fnac").text("");
        $("#info_benef_ebt").text("");
        $("#direccion_linea1").text("");
        $("#direccion_linea2").text("");
        $("#direccion_ciudad").text("");
        $("#direccion_estado").text("");
        $("#direccion_zipcode").text("");
        $("#info_region").text("");
        $("#info_dispensario").text("");
        $("#info_patrono").text("");
        $("#info_ein").text("");
        $("#info_estatus_patronal").text("");
        $("#info_poliza").text("");
        $("#info_menor").removeProp("checked");
        $("#info_muerte").removeProp("checked");
        $("#info_benef_relacion").text("");
        $("#info_benef_nombre").text("");
        $("#info_benef_ssn").text("");
        $("#info_benef_fnac").text("");
        $("#info_benef_ebt").text("");

        $("#ipp_adjudicacionadicional").autoNumeric("set", 0);
        $("#ipp_num_transaccion").val("");
        $("#ipp_observaciones").val("");
        $("#ipp_fechaadjudicacion").val("");
        $("#tblIpp tbody").find("tr").remove();

        if ($.fn.dataTable.isDataTable("#tblIpp")) {
            tableIpp.destroy();
        }

        $("#due_cantidadsolicitada").autoNumeric("set", 0);
        $("#due_num_transaccion").val("");
        $("#due_beneficiario").val(0);
        $("#due_relacion").val("");
        $("#due_observaciones").val("");
        $("#tblPeremptory tbody").find("tr").remove();

        if ($.fn.dataTable.isDataTable("#tblPeremptory")) {
            tablePeremptory.destroy();
        }

        $("#inv_num_transaccion").val("");
        $("#inv_fechaemisiondecision").val("");
        $("#inv_totalinversiones").autoNumeric("set", 0);
        $("#inv_totalinversion").autoNumeric("set", 0);
        $("#Inv_observaciones").val("");
        $("#inv_beneficiario").val("");
        $("#tblAddInvestment tbody").find("tr").remove();
        $("#tblAddInvestment tfoot").find("tr").remove();

        if ($.fn.dataTable.isDataTable("#tblInvestment")) {
            tableInvestment.destroy();
        }

        $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
        $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");

        $("#panel_entidad").hide();
        $("#panel_custodio").hide();

        $("#dieta_fechadecision").val("");
        $("#dieta_fvisita").val("");
        $("#dieta_fnotificacion").val("");
        $("#dieta_numerocaso").val("");
        $("#dieta_observaciones").val("");
        $("#tblPendingDietPeriod tbody").find("tr").remove();
        $("#tblPendingDietPeriod tfoot").find("tr").remove();

        $("#ddlDocumentType").find("option").remove().end().append(documentTypes);
        $("#ddlDocumentType").val("");

        $("#payment-method-panel, #payment-per-panel, #payment-inv-panel, #payment-ipp-panel, #payment-diet-panel, #case-beneficiary-info").fadeOut(1000).addClass("hidden");
    };

    var saveInvestments = function () {
        var isValid = true;
        var validationmessage = "";

        if ($("#inv_beneficiario").val() === "") {
            validationmessage += "<br />" + "Debe seleccionar un beneficiario";
            isValid = false;
        }

        var total = $("#inv_totalinversiones").autoNumeric("get");
        if (total === 0) {
            validationmessage += "<br />" + "Debe ingresar valores mayores a cero en los campos de cantidades";
            isValid = false;
        }

        if (total > balance) {
            validationmessage += "<br />" + "El caso no tiene suficiente balance para registrar el pago";
            isValid = false;
        }

        var paymentsIsValid = true;
        var payments = [];
        $("#tblAddInvestment tbody tr:has(td)").each(function (index, element) {
            var $element = $(element);
            var entidad = $element.find("#entidad").val();
            var inversion = $element.find("#inversion").autoNumeric("get");
            if (inversion === 0 || entidad === "") {
                paymentsIsValid = false;
            }

            payments.push({
                entidad: entidad,
                inversion: inversion
            });
        });

        if (!paymentsIsValid) {
            validationmessage = validationmessage + "<br />" + "Todos los pagos y entidades deben ser llenados";
            isValid = false;
        }

        if (!isValid) {
            showDialogMessage(validationmessage);
            return false;
        }

        var data = {
            caseId: caseData.CaseId,
            caseDetailId: caseData.CaseDetailId,
            fechadecision: $("#inv_fechaemisiondecision").val(),
            payments: JSON.stringify(payments),
            comment: $("#Inv_observaciones").val(),
            caseNumber: caseData.NumeroCaso,
            beneficiario: $("#inv_beneficiario").val().split("_")[0]
        };

        $.ajax({
            url: root + "paymentregistration/insertinvestment",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            if (response.data.Status == "OK") {
                CDI.displayNotification("Datos guardados", "info");

                resetFields();

                $("#ddlDocumentType").val("");

                $("#payment-per-panel, #payment-inv-panel, #payment-ipp-panel, #payment-diet-panel").fadeOut(1000).addClass("hidden");

                expandPanel($("#searchPanel .panel-heading a.collapse-click"));
                expandPanel($("#resultsPanel .panel-heading a.collapse-click"));

                tableInvestment.ajax.reload(function (json) {
                    calculateTotalInversion();
                    $("#payment-method-panel").fadeOut(1000).addClass("hidden");
                });
            } else {
                showDialogMessage("Error al grabar datos");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var savePeremptories = function () {
        var validationstatus = true;
        var validationmessage = "";

        var total = $("#due_cantidadsolicitada").autoNumeric("get");
        if (total == 0) {
            validationmessage = validationmessage + "<br />" + "Debe ingresar valores mayores a cero en los campos de cantidades";
            validationstatus = false;
        }
        if (total > balance) {
            validationmessage = validationmessage + "<br />" + "El caso no tiene suficiente balance para registrar el pago";
            validationstatus = false;
        }
        if ($("#due_cantidadsolicitada").val().trim().length === 0) {
            validationmessage = validationmessage + "<br />" + "Cant. Solicitada es obligatoria";
            validationstatus = false;
        }

        if ($("#due_beneficiario").val() == 0) {
            validationmessage = validationmessage + "<br />" + "Debe elegir benef.";
            validationstatus = false;
        }

        if ($("#due_cantidadsolicitada").autoNumeric("get") === 0) {
            validationmessage = validationmessage + "<br />" + "Debe ingresar cantidad solic. mayor a cero";
            validationstatus = false;
        }

        if (!validationstatus) {
            showDialogMessage(validationmessage);
            validationstatus = false;
        }

        var data = {
            caseId: caseData.CaseId,
            caseDetailId: caseData.CaseDetailId,
            entityid: caseData.EntityId,
            cantidad: $("#due_cantidadsolicitada").autoNumeric("get"),
            beneficiario: $("#due_beneficiario").val().split("_")[0],
            comment: $("#due_observaciones").val(),
            caseNumber: caseData.NumeroCaso
        };

        $.ajax({
            url: root + "paymentregistration/insertperemptory",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) { 
            CDI.hideWaitingMessage();

            if (response.data.Status == "OK") {
                CDI.displayNotification("Datos guardados", "info");

                resetFields();

                $("#ddlDocumentType").val("");

                $("#payment-per-panel, #payment-inv-panel, #payment-ipp-panel, #payment-diet-panel").fadeOut(1000).addClass("hidden");

                expandPanel($("#searchPanel .panel-heading a.collapse-click"));
                expandPanel($("#resultsPanel .panel-heading a.collapse-click"));

                tablePeremptory.ajax.reload(function (json) {
                    $("#payment-method-panel").fadeOut(1000).addClass("hidden");
                });
            } else {
                showDialogMessage(data.data.Message);
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var savePendingDiets = function () {
        var validationstatus = true;
        var validationmessage = "";

        var total = $("#dieta_montototal").autoNumeric("get");
        if (total === 0) {
            validationmessage = validationmessage + "<br />" + "Debe ingresar valores mayores a cero en los campos de cantidades";
            validationstatus = false;
        }
        if (total > balance) {
            validationmessage = validationmessage + "<br />" + "El caso no tiene suficiente balance para registrar el pago";
            validationstatus = false;
        }

        if ($("#dieta_fechadecision").val() == "") {
            validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Decisión";
            validationstatus = false;
        }

        if ($("#dieta_fvisita").val() == "") {
            validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Visita";
            validationstatus = false;
        }

        if ($("#dieta_fnotificacion").val() == "") {
            validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Notificación";
            validationstatus = false;
        }

        if ($("#dieta_numerocaso").val() == "") {
            validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Número de Caso";
            validationstatus = false;
        }

        var periodsIsValid = true;
        var periods = [];
        $("#tblPendingDietPeriod tbody tr:has(td)").each(function (index, element) {
            var $element = $(element);
            var desde = $element.find("#desde").val();
            var hasta = $element.find("#hasta").val();
            var cantidad = $element.find("#cantidad").autoNumeric("get");
            var descuento = $element.find("#descuento").autoNumeric("get");
            if (descuento === 0 || descuento === "" || desde === "" || hasta === "") {
                periodsIsValid = false;
            }

            periods.push({
                desde: desde,
                hasta: hasta,
                cantidad: cantidad,
                descuento: descuento
            });
        });

        if (!periodsIsValid) {
            validationmessage = validationmessage + "<br />" + "Todos los periodos y cantidades deben ser ingresados";
            validationstatus = false;
        }

        if (!validationstatus) {
            showDialogMessage(validationmessage);
            return;
        }

        var data = {
            caseId: caseData.CaseId,
            caseDetailId: caseData.CaseDetailId,
            fechadecision: $("#dieta_fechadecision").val(),
            fechavisita: $("#dieta_fvisita").val(),
            fechanotificacion: $("#dieta_fnotificacion").val(),
            numerocaso: $("#dieta_numerocaso").val(),
            montototal: $("#dieta_montototal").autoNumeric("get"),
            periods: JSON.stringify(periods),
            comment: $("#dieta_observaciones").val(),
            caseNumber: caseData.NumeroCaso
        };

        $.ajax({
            url: root + "paymentregistration/insertpendingdiet",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) { 
            CDI.hideWaitingMessage();
            if (response.data.Status == "OK") {
                CDI.displayNotification("Datos guardados", "info");

                resetFields();

                $("#ddlDocumentType").val("");

                $("#payment-per-panel, #payment-inv-panel, #payment-ipp-panel, #payment-diet-panel").fadeOut(1000).addClass("hidden");

                expandPanel($("#searchPanel .panel-heading a.collapse-click"));
                expandPanel($("#resultsPanel .panel-heading a.collapse-click"));
            } else {
                showDialogMessage("Error al grabar datos.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var initControls = function () {
        documentTypes = $("#ddlDocumentType option").clone();

        $("#direccion_zipcode").mask("99999-9999");
        $("#inv_totalinversiones").autoNumeric("init");
        $("#inv_totalinversion").autoNumeric("init");
        $("#ipp_adjudicacionadicional").autoNumeric("init");
        $("#ipp_mensualidad").autoNumeric("init");
        $("#ipp_semanas").autoNumeric("init", {
            aSign: ""
        });
        $("#ipp_fechaadjudicacion").datepicker({
            startDate: "-100y",
            endDate: "-1d"
        });

        $("#diff_monto").autoNumeric("init");
        $("#due_cantidadsolicitada").autoNumeric("init");

        $("#info_menor").prop("disabled", true);
        $("#info_muerte").prop("disabled", true);
        $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
        $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");
        $("#panel_entidad").hide();
        $("#panel_custodio").hide();

        $("#inv_fechaemisiondecision, #diff_fechadecision").datepicker();
        $("#dieta_fechadecision").datepicker({
            startDate: "-50y",
            endDate: "-1d"
        }).on("changeDate", function (selected) {
            startDateDecision = new Date(selected.date.valueOf());
            startDateDecision.setDate(startDateDecision.getDate(new Date(selected.date.valueOf())) - 1);
            $("#dieta_fvisita").datepicker("setEndDate", startDateDecision);
            $("#dieta_fvisita").datepicker("setStartDate", "-50y");
            $("#dieta_fnotificacion").datepicker("setEndDate", startDateDecision);

            $("#tblPendingDietPeriod tbody > tr").find("#desde").each(function (index, element) {
                $(element).datepicker("setEndDate", startDateDecision);
            });
            $("#tblPendingDietPeriod tbody > tr").find("#hasta").each(function (index, element) {
                $(element).datepicker("setEndDate", startDateDecision);
            });
        });
        $("#dieta_fvisita").datepicker({
            startDate: "-50y",
            endDate: "-1d"
        }).on("changeDate", function (selected) {
            startDateVisita = new Date(selected.date.valueOf());
            startDateVisita.setDate(startDateVisita.getDate(new Date(selected.date.valueOf())));
            $("#dieta_fechadecision").datepicker("setStartDate", startDateVisita);
            $("#dieta_fnotificacion").datepicker("setStartDate", startDateVisita);
            $("#dieta_fnotificacion").datepicker("setEndDate", startDateDecision);
        });
        $("#dieta_fnotificacion").datepicker({
            startDate: "-50y",
            endDate: "-1d"
        }).on("changeDate", function (selected) {
            startDateNotificacion = new Date(selected.date.valueOf());
            startDateNotificacion.setDate(startDateNotificacion.getDate(new Date(selected.date.valueOf())));
            $("#dieta_fechadecision").datepicker("setStartDate", startDateNotificacion);
            $("#dieta_fvisita").datepicker("setEndDate", startDateNotificacion);
        });
        $("#dieta_montototal").autoNumeric("init");
        $("#dieta_montototal").autoNumeric("set", 0);
        $("#dieta_numerocaso").inputmask({ mask: "99999999999[ 99]" });
    };

    var bindUiActions = function() {
        $(CDI.CaseSearch).on("case.selected", function(e, data) {
            caseData = data.selectedCase;

            $("#ddlDocumentType").find("option").remove().end().append(documentTypes);
            $("#ddlDocumentType").val("");
            $("#payment-method-panel, #payment-per-panel, #payment-inv-panel, #payment-ipp-panel, #payment-diet-panel, #case-beneficiary-info").fadeOut(1000).addClass("hidden");

            resetFields();
            showCaseDetail();

            $("html, body").animate({
                scrollTop: $("#doc-class-panel").offset().top
            }, 2000);
        });

        $(CDI.CaseSearch).on("started", function (e, data) {
            clearFields();
        });

        $(CDI.CaseSearch).on("cleaned", function () {
            clearFields();
        });

        $("#btnSaveInvestment").on("click", function(e) {
            e.preventDefault();

            $.ajax({
                type: "GET",
                url: root + "case/getbalance/" + caseData.CaseDetailId,
                dataType: "json"
            }).done(function (response) { 
                var isValid = true;
                var validationmessage = "";

                balance = parseFloat(response);

                var total = $("#inv_totalinversiones").autoNumeric("get");
                if (total > balance) {
                    validationmessage = validationmessage + "<br />" + "El caso no tiene suficiente balance para registrar el pago";
                    isValid = false;
                }
                if (isValid) {
                    saveInvestments();
                } else {
                    showDialogMessage(validationmessage);
                }
            });
        });

        $("#btnSaveIpp").on("click", function(e) {
            e.preventDefault();

            var isValid = true;
            var validationmessage = "";

            if ($("#ipp_fechaadjudicacion").val().trim().length === 0) {
                validationmessage = validationmessage + "<br />" + "Fecha Adjudicación es obligatoria.";
                isValid = false;
            }

            if ($("#ipp_adjudicacionadicional").autoNumeric("get") === 0) {
                validationmessage = validationmessage + "<br />" + "Cantidad adjudicada de ser mayor a cero";
                isValid = false;
            }

            if ($("#tblDesglosarIpp > tbody > tr").not(".hidden").length === 0) {
                validationmessage = validationmessage + "<br />" + "Debe ingresar al menos un desglose";
                isValid = false;
            }

            if (!isValid) {
                showDialogMessage(validationmessage);
                return false;
            }

            var model = {
                CaseId: 0,
                CaseDetailId: 0,
                EntityId: 0,
                CaseNumber: "",
                CaseKey: "",
                CantidadAdjudicada: null,
                Mensualidad: null,
                Semanas: null,
                FechaAdjudicacion: null,
                Comments: null,
                CompSemanalInca: caseData.CompSemanalInca,
                Desgloses: []
            };

            var desglose = {
                CompensationRegionId: 0,
                Percent: 0,
                Weeks: 0
            };

            model.CaseId = caseData.CaseId;
            model.CaseDetailId = caseData.CaseDetailId;
            model.EntityId = caseData.EntityId;
            model.CaseKey = caseData.CaseKey;
            model.CaseNumber = caseData.NumeroCaso;
            model.CantidadAdjudicada = parseFloat($("#ipp_adjudicacionadicional").autoNumeric("get"));
            model.Mensualidad = parseFloat($("#ipp_mensualidad").autoNumeric("get"));
            model.Semanas = parseFloat($("#ipp_semanas").autoNumeric("get"));
            model.Comments = $("#ipp_observaciones").val();
            model.FechaAdjudicacion = $("#ipp_fechaadjudicacion").val();
            model.CompSemanalInca = parseFloat(caseData.CompSemanalInca);

            $("#tblDesglosarIpp > tbody > tr").not(".hidden").each(function (index, element) {
                var $element = $(element);
                var $ddlCodificacionValue = $element.find(".desglose-codificacion").val();

                desglose.CompensationRegionId = $ddlCodificacionValue.split("|")[4];
                desglose.Percent = $element.find(".desglose-percentage").autoNumeric("get") / 100;
                desglose.Weeks = $ddlCodificacionValue.split("|")[3];

                model.Desgloses.push(desglose);

                desglose = {
                    CompensationRegionId: 0,
                    Percent: 0,
                    Weeks: 0
                };
            });
            
            $.ajax({
                url: root + "paymentregistration/insertipp",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ model: model }),
                dataType: "json",
                type: "POST",
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response, textStatus, xhr) { 
                CDI.hideWaitingMessage();

                if (response.data.Status === "OK") {
                    CDI.displayNotification("Datos guardados", "info");

                    resetFields();

                    $("#ddlDocumentType").val("");

                    $("#payment-per-panel, #payment-inv-panel, #payment-ipp-panel, #payment-diet-panel").fadeOut(1000).addClass("hidden");

                    expandPanel($("#searchPanel .panel-heading a.collapse-click"));
                    expandPanel($("#resultsPanel .panel-heading a.collapse-click"));

                    tableIpp.ajax.reload(function(json) {
                        $("#payment-method-panel").fadeOut(1000).addClass("hidden");
                    });
                } else {
                    showDialogMessage("Error al grabar los datos");
                }
            }).fail(function () { 
                CDI.hideWaitingMessage();
            });
        });

        $("#btnSavePeremptory").on("click", function(e) {
            e.preventDefault();

            $.ajax({
                type: "GET",
                url: root + "case/getbalance/" + caseData.CaseDetailId,
                dataType: "json"
            }).done(function (response) {
                var isValid = true;
                var validationmessage = "";

                balance = parseFloat(response);

                var total = $("#due_cantidadsolicitada").autoNumeric("get");
                if (total > balance) {
                    validationmessage = validationmessage + "<br />" + "El caso no tiene suficiente balance para registrar el pago";
                    isValid = false;
                }
                if (isValid) {
                    savePeremptories();
                } else {
                    showDialogMessage(validationmessage);
                }
            });
        });

        $("#btnSavePendingDiet").on("click", function(e) {
            e.preventDefault();

            $.ajax({
                type: "GET",
                url: root + "case/getbalance/" + caseData.CaseDetailId,
                dataType: "json"
            }).done(function (response) {
                var isValid = true;
                var validationmessage = "";

                balance = parseFloat(response);

                var total = $("#dieta_montototal").autoNumeric("get");
                if (total > balance) {
                    validationmessage = validationmessage + "<br />" + "El caso no tiene suficiente balance para registrar el pago";
                    isValid = false;
                }
                if (isValid) {
                    savePendingDiets();
                } else {
                    showDialogMessage(validationmessage);
                }
            });
      
        });

        $("#addRow").on("click", function(e) {
            e.preventDefault();

            var newRow = "<tr>" +
                            "<td><button type=\"button\" class=\"btn btn-default btn-sm deleterow\" aria-label=\"Left Align\">" +
                            "<i class=\"fa fa-minus\"></i></button></td>" +
                            "<td><input class=\"form-control\" id=\"entidad\" name=\"entidad\" type=\"text\"/></td>" +
                            "<td class=\"text-right\"><input class=\"form-control text-right\" id=\"inversion\" name=\"inversion\" type=\"text\"/>" +
                            "</td>" +
                        "</tr>";
            $("#tblAddInvestment tbody").append(newRow);
            $("#tblAddInvestment tbody").find("tr").last().find("#inversion").autoNumeric("init");

            $("#tblAddInvestment tbody").change(function() {
                $.ajax({
                    type: "GET",
                    url: root + "case/getbalance/" + caseData.CaseDetailId,
                    dataType: "json"
                }).done(function (response) {
                    calculateNewInversions();

                    balance = parseFloat(response);

                    var total = $("#inv_totalinversiones").autoNumeric("get");
                    if (total > balance) {
                        showDialogMessage("El caso no tiene suficiente balance para registrar el pago");
                    }
                });
            });

            calculateNewInversions();
        });

        $("#tblAddInvestment").on("click", ".deleterow", function () {
            $(this).closest("tr").remove();

            calculateTotalInversion();
        });

        $("#btnNewPendigDietPeriod").on("click", function (evt) {
            evt.preventDefault();

            var newRow = "<tr>" +
                "<td>" +
                    "<button type=\"button\" class=\"btn btn-default delete-row-dieta\" aria-label=\"Left Align\">" +
                        "<span class=\"glyphicon glyphicon-minus\"></span>" +
                    "</button>" +
                "</td>" +
                "<td>" +
                    "<div class=\"input-group date\">" +
                        "<input id=\"desde\" type=\"text\" class=\"form-control\" />" +
                        "<span class=\"input-group-addon\">" +
                            "<span class=\"glyphicon glyphicon-calendar\"></span>" +
                        "</span>" +
                    "</div>" +
                "</td>" +
                "<td>" +
                    "<div class=\"input-group date\">" +
                        "<input id=\"hasta\" type=\"text\" class=\"form-control\" />" +
                        "<span class=\"input-group-addon\">" +
                            "<span class=\"glyphicon glyphicon-calendar\"></span>" +
                        "</span>" +
                    "</div>" +
                "</td>" +
                "<td class=\"hidden\">" +
                    "<input id=\"nrodias\" name=\"nrodias\" class=\"form-control text-right\" type=\"text\" />" +
                "</td>" +
                "<td class=\"text-right\">" +
                    "<input id=\"cantidad\" name=\"cantidad\" class=\"form-control text-right\" type=\"text\" readonly />" +
                "</td>" +
                "<td class=\"text-right\">" +
                    "<input id=\"descuento\" name=\"descuento\" class=\"form-control text-right\" type=\"text\" />" +
                "</td>" +
                "<td class=\"text-right\">" +
                    "<input id=\"total-pagar\" name=\"total-pagar\" class=\"form-control text-right\" type=\"text\" readonly />" +
                "</td>" +
                "</tr>";
            $("#tblPendingDietPeriod tbody").append(newRow);

            var $lastRow = $("#tblPendingDietPeriod tbody").find("tr").last();

            var $desde = $lastRow.find("#desde");
            $desde.datepicker({
                startDate: "-50y",
                endDate: "-1d"
            }).on("changeDate", function (selected) {
                var startDateDesde = new Date(selected.date.valueOf());
                startDateDesde.setDate(startDateDesde.getDate(new Date(selected.date.valueOf())));

                var $currentRow = $(this).closest("tr");

                var $hastaDate = $currentRow.find("#hasta");
                $hastaDate.datepicker("setStartDate", startDateDesde);

                if ($hastaDate.val() !== "") {
                    var timeDiff = Math.abs(startDateDesde - selected.date);
                    var diffDays = Math.ceil(timeDiff) / (1000 * 3600 * 24);

                    $currentRow.find("#nrodias").val(diffDays);

                    var cantidad = 0;
                    if (caseData.DiasSemana !== 0) {
                        cantidad = diffDays * (caseData.CompSemanal / caseData.DiasSemana);
                    }
                    $currentRow.find("#cantidad").autoNumeric("set", cantidad);
                }
            });
            $desde.datepicker("setEndDate", $("#dieta_fechadecision").datepicker("getDate"));

            var $hasta = $lastRow.find("#hasta");
            $hasta.datepicker({
                startDate: "-50y",
                endDate: "-1d"
            }).on("changeDate", function (selected) {
                var startDateHasta = new Date(selected.date.valueOf());
                startDateHasta.setDate(startDateHasta.getDate(new Date(selected.date.valueOf())));

                var $currentRow = $(this).closest("tr");

                var $desdeDate = $currentRow.find("#desde");
                $desdeDate.datepicker("setEndDate", startDateHasta);

                var timeDiff = Math.abs(startDateHasta - $desdeDate.datepicker("getDate"));
                var diffDays = Math.ceil(timeDiff) / (1000 * 3600 * 24);

                $currentRow.find("#nrodias").val(diffDays);

                var cantidad = 0;
                if (caseData.DiasSemana !== 0) {
                    cantidad = diffDays * (caseData.CompSemanal / caseData.DiasSemana);
                }
                $currentRow.find("#cantidad").autoNumeric("set", cantidad);
            });
            $hasta.datepicker("setEndDate", $("#dieta_fechadecision").datepicker("getDate"));

            $lastRow.find("#cantidad").autoNumeric("init");

            $lastRow.find("#total-pagar").autoNumeric("init");

            $lastRow.find("#descuento").autoNumeric("init").blur(function () {
                var $this = $(this);
                var currentDescuento = $this.autoNumeric("get");
                currentDescuento = parseFloat(currentDescuento);
                if (!isNaN(currentDescuento)) {
                    var $row = $this.closest("tr");
                    var cantidad = $row.find("#cantidad").autoNumeric("get");
                    cantidad = parseFloat(cantidad);
                    if (isNaN(cantidad)) {
                        cantidad = 0;
                    }

                    var totalPagar = cantidad - currentDescuento;

                    $row.find("#total-pagar").autoNumeric("set", totalPagar);
                }

                calculateTotalPendingDiets();
            });

            $("#tblPendingDietPeriod tbody").change(function() {
                $.ajax({
                    type: "GET",
                    url: root + "case/getbalance/" + caseData.CaseDetailId,
                    dataType: "json"
                }).done(function (response) {
                    calculateTotalPendingDiets();

                    balance = parseFloat(response);

                    var total = $("#dieta_montototal").autoNumeric("get");
                    if (total > balance) {
                        showDialogMessage("El caso no tiene suficiente balance para registrar el pago.");
                    }

                   
                });
            });

            calculateTotalPendingDiets();
            
        });

        $("#tblPendingDietPeriod").on("click", ".delete-row-dieta", function() {
            $(this).closest("tr").remove();
            calculateTotalPendingDiets();
        });

        $(".nuevo").on("click", function (evt) {
            evt.preventDefault();

            resetFields();

            $("#ddlDocumentType").val("");
            
            $("#payment-per-panel, #payment-inv-panel, #payment-ipp-panel, #payment-diet-panel").fadeOut(1000).addClass("hidden");
            
            expandPanel($("#searchPanel .panel-heading a.collapse-click"));
            expandPanel($("#resultsPanel .panel-heading a.collapse-click"));
            
        });

        $(".cancelar").on("click", function(evt) {
            evt.preventDefault();

            resetFields();

            $("#ddlDocumentType").val("");

            $("#payment-diet-panel, #payment-per-panel, #payment-inv-panel, #payment-ipp-panel").fadeOut(1000).addClass("hidden");
            
            expandPanel($("#searchPanel .panel-heading a.collapse-click"));
            expandPanel($("#resultsPanel .panel-heading a.collapse-click"));

            // scroll to clase documento
            $("html, body").animate({
                scrollTop: $("#doc-class-panel").offset().top
            }, 2000);
        });

        $("input:radio[name=pago_dirigido_entidad]").click(function() {
            $("#pago_dirigido_entidad").prop("checked", true);
            $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
            $("#panel_custodio").hide();
        });

        $("input:radio[name=pago_dirigido_cust]").click(function() {
            $("#pago_dirigido_cust").prop("checked", true);
            $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");
            $("#panel_custodio").show();
            $("#panel_entidad").hide();
        });

        $("#due_beneficiario").change(function() {
            var val = $(this).val();
            var text = $("#due_beneficiario option:selected").text();
            $("#due_relacion").val("");

            if (val === "") {
                $("#due_relacion").val("");
                return false;
            }

            $("#due_relacion").val($("#due_beneficiario").val().split("_")[1]);
        });

        $("#inv_beneficiario").change(function() {
            var val = $(this).val();
            var text = $("#inv_beneficiario option:selected").text();
            $("#inv_relacion").val("");

            if (val === "") {
                $("#inv_relacion").val("");
                return false;
            }

            $("#inv_relacion").val($("#inv_beneficiario").val().split("_")[1]);
        });

        $("#ddlDocumentType").change(function() {
            var selectedValue = $(this).val();

            resetFields();

            switch (selectedValue) {
                case "":
                    $("#payment-per-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
                    break;
                case "1":
                    $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-per-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-inv-panel").fadeIn(1000).removeClass("hidden");

                    showTableInvestment(caseData.CaseId);
                    $.ajax({
                        type: "GET",
                        url: root + "paymentregistration/getentityrelatives/" + caseData.CaseId,
                        cache: false
                    }).done(function (response) {
                        $("#inv_beneficiario").find("option:gt(0)").remove();

                        $.each(response, function(index, element) {
                            var $option;
                            var entityId = element.Value.split("_")[0];
                            if (entityId == caseData.EntityId) {
                                $option = "<option value=" + element.Value + " selected>" + element.Text + "</option>";

                                $("#inv_relacion").val(element.Value.split("_")[1]);
                            }
                            else {
                                $option = "<option value=" + element.Value + ">" + element.Text + "</option>";
                            }

                            $($option).appendTo("#inv_beneficiario");
                        });

                        if (caseData.EsBeneficiario) {
                            $("#inv_beneficiario").prop("disabled", true);
                        }
                    });
                    break;
                case "2":
                    $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-per-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-ipp-panel").fadeIn(1000).removeClass("hidden");

                    showTableIpp(caseData.CaseDetailId);
                    showMonthlyPaymentAndNumberOfWeeks();
                    break;
                case "3":
                    $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-per-panel").fadeIn(1000).removeClass("hidden");

                    showTablePeremptory(caseData.CaseId);

                    $.ajax({
                        type: "GET",
                        url: root + "paymentregistration/getentityrelatives/" + caseData.CaseId,
                        dataType: "json"
                    }).done(function (response) {
                        $("#due_beneficiario").find("option:gt(0)").remove();

                        $.each(response, function (index, element) {
                            var $option;
                            var entityId = element.Value.split("_")[0];
                            if (entityId == caseData.EntityId) {
                                $option = "<option value=" + element.Value + " selected>" + element.Text + "</option>";

                                $("#due_relacion").val(element.Value.split("_")[1]);
                            }
                            else {
                                $option = "<option value=" + element.Value + ">" + element.Text + "</option>";
                            }

                            $($option).appendTo("#due_beneficiario");
                        });

                        if (caseData.EsBeneficiario) {
                            $("#due_beneficiario").prop("disabled", true);
                        }
                    });
                    break;
                case "4":
                    $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-per-panel").fadeOut(1000).addClass("hidden");
                    $("#payment-diet-panel").fadeIn(1000).removeClass("hidden");
                    break;
            }
        });

        $(".panel-heading").on("click", "a.collapse-click", function(evt) {
            evt.preventDefault();

            var $this = $(this);

            if ($this.hasClass("panel-collapsed")) {
                // expand the panel
                expandPanel($this);
            } else {
                // collapse the panel
                collapsePanel($this);
            }
        });

        $("#btnShowDesglose").on("click", function (evt) {
            evt.preventDefault();

            $("#panel-desglose").fadeIn(1000).removeClass("hidden");
            
        });

        $("#btnCancelDesglose").on("click", function (evt) {
            evt.preventDefault();

            $("#panel-desglose").fadeOut(1000).addClass("hidden");
            
        });

        $("#btnAddDesglose").on("click", function (evt) {
            evt.preventDefault();

            var $tr = $("#tblDesglosarIpp > tbody").find(".hidden");
            var $clonedCodificacion = $tr.find(".desglose-codificacion").clone();
            $clonedCodificacion.find("option:gt(0)").remove();
            
            var newRow = "<tr>" +
                            "<td>" +
                                "<button type='button' class='btn btn-default btn-sm desglose-remove'><i class='fa fa-minus'></i></button>" +
                            "</td>" +
                            "<td>" +
                                $tr.find(".desglose-area").clone().wrap("<div>").parent().html() +
                            "</td>" +
                            "<td>" +
                                $clonedCodificacion.wrap("<div>").parent().html() +
                            "</td>" +
                            "<td>" +
                                "<input type='text' class='form-control desglose-percentage' />" +
                            "</td>" +
                         "</tr>";

            var $lastRow = $("#tblDesglosarIpp > tbody:last");
            $lastRow.append(newRow);
            $lastRow.find(".desglose-percentage").autoNumeric("init", {
                aSign: ""
            });

            $("#tblDesglosarIpp tbody").not(".hidden").off("change").on("change", ".desglose-area", function (evt) {
                var $this = $(this);
                var selectedVal = $this.val();
                var $rowHidden = $("#tblDesglosarIpp > tbody > tr.hidden");
                var filtered = $rowHidden.find(".desglose-codificacion option").clone().filter(function (index, element) {
                    return $(element).val().split("|")[1] === selectedVal;
                });

                var $ddlCodificacion = $this.closest("tr").find(".desglose-codificacion");
                $ddlCodificacion.find("option[value!='']").remove();
                $ddlCodificacion.append(filtered);
            });

        });

        $("#tblDesglosarIpp tbody").on("click", ".desglose-remove", function (evt) {
            evt.preventDefault();

            $(this).closest("tr").remove();
           
        });

        $("#ipp_adjudicacionadicional").blur(function () {
            var currentValue = $(this).autoNumeric("get");

            if (currentValue > 0) {
                $("#btnShowDesglose").removeAttr("disabled");
            }
            else {
                $("#btnShowDesglose").prop("disabled", true);
            }
        });

        $("#btnSaveDesglose").on("click", function () {
            if ($("#tblDesglosarIpp > tbody > tr").not(".hidden").length == 0) {
                alert("Debe ingresar al menos un desglose");
            }

            var semanas = 0;
            var totalAdjudicado = 0;
            $("#tblDesglosarIpp > tbody > tr").not(".hidden").each(function (index, element) {
                var $element = $(element);

                var percentage = 0;
                if ($element.find(".desglose-percentage") === undefined) {
                    percentage = 0;
                }
                if ($element.find(".desglose-percentage") === "") {
                    percentage = 0;
                }
                percentage = $element.find(".desglose-percentage").autoNumeric("get");
                percentage = parseFloat(percentage);
                if (isNaN(percentage)) {
                    percentage = 0;
                }
                percentage = percentage / 100;

                var numberOfWeeks = $element.find(".desglose-codificacion").val().split("|")[3];
                numberOfWeeks = parseFloat(numberOfWeeks);
                if (isNaN(numberOfWeeks)) {
                    numberOfWeeks = 0;
                }

                semanas = semanas + (numberOfWeeks * percentage);
                totalAdjudicado = totalAdjudicado + (numberOfWeeks * percentage * caseData.CompSemanalInca)
            });

            if (totalAdjudicado < $("#ipp_adjudicacionadicional").autoNumeric("get")) {
                CDI.displayNotification("El total adjudicado debe ser mayor o igual a la cantidad adjudicada", "warning");

                return false;
            }

            $("#ipp_semanas").autoNumeric("set", semanas);

            $("#panel-desglose").fadeOut(1000).addClass("hidden");
        });
    };

    var init = function() {
        initControls();
        bindUiActions();
    };

    return {
        init: init
    };
})();

$(function() {
    CDI.CaseSearch.init();
    CDI.DisplayPayments.init();
});