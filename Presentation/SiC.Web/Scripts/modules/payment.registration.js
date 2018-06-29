CDI.PaymentRegistration = (function () {
    var balance = 0;
    var caseData;
    var tablePeremptory;
    var tableInvestment;
    var tableIpp;
    var documentTypes;

    var ippMaximunAmount = 0;
    var ippTotalTransactionAmount = 0;

    var startDateDecision = new Date();
    var startDateVisita = new Date();
    var startDateNotificacion = new Date();

    var resetDocumentTypes = function () {
        $("#ddlDocumentType").find("option").remove().end().append(documentTypes);
        $("#ddlDocumentType").val("");
    };

    var scrollToDocumentType = function () {
        $("html, body").animate({
            scrollTop: $("#doc-class-panel").offset().top
        }, 1000);
    };

    var hideAllPaymentMethodPanels = function () {
        $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
        $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
        $("#payment-per-panel").fadeOut(1000).addClass("hidden");
        $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
        $("#payment-lawyer-panel").fadeOut(1000).addClass("hidden");
        $("#header-panel").fadeOut(1000).addClass("hidden");
    };

    var hidePaymentMethodPanels = function () {
        $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
        $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
        $("#payment-per-panel").fadeOut(1000).addClass("hidden");
        $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
        $("#payment-lawyer-panel").fadeOut(1000).addClass("hidden");
    };

    var showInversionPanel = function () {
        $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
        $("#payment-per-panel").fadeOut(1000).addClass("hidden");
        $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
        $("#payment-lawyer-panel").fadeOut(1000).addClass("hidden");
        $("#payment-inv-panel").fadeIn(1000).removeClass("hidden");
    };

    var showIppPanel = function () {
        $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
        $("#payment-per-panel").fadeOut(1000).addClass("hidden");
        $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
        $("#payment-lawyer-panel").fadeOut(1000).addClass("hidden");
        $("#payment-ipp-panel").fadeIn(1000).removeClass("hidden");
    };

    var showPeremptoryPanel = function () {
        $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
        $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
        $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
        $("#payment-lawyer-panel").fadeOut(1000).addClass("hidden");
        $("#payment-per-panel").fadeIn(1000).removeClass("hidden");
    };

    var showPendingDietPanel = function () {
        $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
        $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
        $("#payment-per-panel").fadeOut(1000).addClass("hidden");
        $("#payment-lawyer-panel").fadeOut(1000).addClass("hidden");
        $("#payment-diet-panel").fadeIn(1000).removeClass("hidden");
    };

    var showLawyerPanel = function () {
        $("#payment-inv-panel").fadeOut(1000).addClass("hidden");
        $("#payment-ipp-panel").fadeOut(1000).addClass("hidden");
        $("#payment-per-panel").fadeOut(1000).addClass("hidden");
        $("#payment-diet-panel").fadeOut(1000).addClass("hidden");
        $("#payment-lawyer-panel").fadeIn(1000).removeClass("hidden");
    };

    var clearLawyerPanel = function () {
        $("#lawyer_fechadecision").val("");
        $("#lawyer_fvisita").val("");
        $("#lawyer_fnotificacion").val("");
        $("#lawyer_numerocaso").val("");
        $("#lawyer_montototal").autoNumeric("set", 0);
        $("#lawyer_observaciones").val("");
    };

    var clearPendingDietPanel = function () {
        $("#dieta_fechadecision").val("");
        $("#dieta_fvisita").val("");
        $("#dieta_fnotificacion").val("");
        $("#dieta_numerocaso").val("");
        $("#dieta_observaciones").val("");
        $("#tblPendingDietPeriod tbody").find("tr").remove();
        $("#tblPendingDietPeriod tfoot").find("tr").remove();
    };

    var clearInversionPanel = function () {
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
    };

    var clearIppPanel = function () {
        $("#ipp_adjudicacionadicional").autoNumeric("set", 0);
        $("#ipp_semanas").autoNumeric("set", 0);
        $("#ipp_mensualidad").autoNumeric("set", 0);
        $("#ipp_num_transaccion").val("");
        $("#ipp_observaciones").val("");
        $("#ipp_fechaadjudicacion").val("");
    };

    var clearPeremptoryPanel = function () {
        $("#due_cantidadsolicitada").autoNumeric("set", 0);
        $("#due_beneficiario").val("");
        $("#due_beneficiario").removeAttr("disabled");
        $("#due_relacion").val("");
        $("#due_observaciones").val("");
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

    var expandSearchSection = function () {
        expandPanel($("#searchPanel .panel-heading a.collapse-click"));
        expandPanel($("#resultsPanel .panel-heading a.collapse-click"));
    };

    var showDialogMessage = function (message) {
        toastr.warning(message, "SiC");
    };

    var calculateTotalInversion = function () {

        var sumaInversiones = 0;

       $("#inv_totalinversion").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ "
        });

        for (var i = 0; i < $("#tblInvestment tbody tr:has(td)").length; i++) {
            var $cellInversion = $($("#tblInvestment tbody tr:has(td)")[i]).find("td:eq(3)");
            var inversion = $cellInversion.text();
            var inversionValor = parseFloat(inversion.replace("$ ", "").replace(",", ""));

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
            var $inversion = $element.find("#inversion");
            var inversionValor = parseFloat($inversion.autoNumeric("get"));
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
            var cantidadValor = $element.find("#total-pagar").autoNumeric("get");

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

    var showTableInvestment = function (caseId, caseDetailId, esBeneficiario) {
        if ($.fn.dataTable.isDataTable("#tblInvestment")) {
            tableInvestment.destroy();
        }

        $("#tblInvestment tbody").find("tr").remove();
        $("#tblAddInvestment tbody").find("tr").remove();
        $("#tblAddInvestment tfoot").find("tr").remove();

        tableInvestment = $("#tblInvestment").DataTable({
            "ordering": false,
            "autoWidth": false,
            "pagingType": "simple_numbers",
            "processing": true,
            "paging": true,
            "deferRender": true,
            "drawCallback": function() {
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
                    data.caseDetailId = caseDetailId;
                    data.esBeneficiario = esBeneficiario;

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
            "fnInitComplete": function() {
                calculateTotalInversion();
            },
            "dom": "rtp"
        });
    };

    var showTablePeremptory = function (caseId, caseDetailId, esBeneficiario) {
        if ($.fn.dataTable.isDataTable("#tblPeremptory")) {
            tablePeremptory.destroy();
        }

        $("#tblPeremptory tbody").find("tr").remove();
        $("#btnSavePeremptory").prop("disabled", true);

        tablePeremptory = $("#tblPeremptory").DataTable({
            "ordering": false,
            "autoWidth": false,
            "pagingType": "simple_numbers",
            "processing": true,
            "paging": true,
            "deferRender": true,
            "drawCallback": function() {
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
                    data.caseDetailId = caseDetailId;
                    data.esBeneficiario = esBeneficiario;

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
            "fnRowCallback": function (nRow) {
                $("td:eq(3)", nRow).addClass("text-right");
            },
            "dom": "rtp",
            "initComplete": function () {
                $("#btnSavePeremptory").prop("disabled", false);
            }
        });
    };

    var showTableIpp = function (caseDetailId) {
        if ($.fn.dataTable.isDataTable("#tblIpp")) {
            tableIpp.destroy();
        }

        $("#tblIpp tbody").find("tr").remove();

        tableIpp = $("#tblIpp").DataTable({
            "ordering": false,
            "processing": true,
            "autoWidth": false,
            "drawCallback": function () {
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
            "fnRowCallback": function (nRow) {
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
            "caseId": caseData.CaseDetailId
        };     

        $.ajax({
            url: root + "paymentregistration/getmonthlyconceptandtransactioninformation",
            data: data,
            dataType: "json",
            type: "POST",            
            success: function (response) {
                if (response != null && response.Success) {
                    var mensualidad = parseFloat(response.Data[1]);
                    $("#ipp_mensualidad").autoNumeric("set", mensualidad);

                    //Get Max Date of the IPP list
                    if (response.Data[0] != null && response.Data[0] != undefined) {
                        var maxIppDate = new Date(response.Data[0]);                        

                        if (true)                            
                        {
                            $("#ipp_fechaadjudicacion").datepicker("setStartDate", maxIppDate);
                            $("#ipp_fechaadjudicacion").datepicker("setEndDate", "-1d");
                        }
                    }
                    else {
                        $("#ipp_fechaadjudicacion").datepicker("setStartDate", "-100y");
                        $("#ipp_fechaadjudicacion").datepicker("setEndDate", "-0d");
                    }
                    ippMaximunAmount = response.Data[2];
                    ippTotalTransactionAmount = response.Data[3];
                }
                else {
                    showDialogMessage("Mensualidad no definida para el año del caso.");
                }
            }
        });
    };

    var showCaseDetail = function () {
        balance = caseData.Balance;
        CDI.CaseSearch.header(caseData);        

        if (caseData.FromCase) {
            $("#ddlDocumentType option[value='1']").remove();
            $("#ddlDocumentType option[value='2']").remove();
            $("#ddlDocumentType option[value='3']").remove();
        }
        else {
            if (caseData.CasoMenor) {
                $("#ddlDocumentType option[value='3']").remove();

                if (!caseData.EsLesionado) {
                    $("#ddlDocumentType option[value='1']").remove();
                }
            }

            if (caseData.CasoMuerte) {
                if (caseData.EsBeneficiario) {
                    if (!caseData.Beneficiario.EsBeneficiarioValido)
                        $("#ddlDocumentType option[value='1']").remove();
                }
            }
            else {
                if (caseData.EsBeneficiario) {
                    $("#ddlDocumentType option[value='1']").remove();
                }
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

                if (caseData.CasoMuerte &&
                    caseData.TieneInversionMenor3 &&
                    (caseData.CivilStatusId === 3 || caseData.CivilStatusId === 6)) {
                    $("#ddlDocumentType option[value='1']").remove();
                }
            }
        }
    };

    var saveInvestments = function () {
        var isValid = true;
        var validationmessage = "";

        if ($("#inv_beneficiario").val() === "") {
            validationmessage = validationmessage + "<br />" + "Debe seleccionar un beneficiario";
            isValid = false;
        }

        var total = $("#inv_totalinversiones").autoNumeric("get");
        if (total === 0) {
            validationmessage = validationmessage + "<br />" + "Debe ingresar valores mayores a cero en los campos de cantidades";
            isValid = false;
        }

        if (total > balance) {
            validationmessage = validationmessage + "<br />" + "El caso no tiene suficiente balance para registrar el pago";
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

            if (response.data.Status === "OK") {
                CDI.displayNotification("Datos guardados", "info");

                resetDocumentTypes();
                hideAllPaymentMethodPanels();
                expandSearchSection();
            } else {
                showDialogMessage("Error al grabar datos");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
        
    };

    var savePeremptories = function () {
        var isValid = true;
        var validationmessage = "";

        var total = $("#due_cantidadsolicitada").autoNumeric("get");
       
        if (total > balance) {
            validationmessage = validationmessage + "<br />" + "El caso no tiene suficiente balance para registrar el pago";
            isValid = false;
            }

        if ($("#due_cantidadsolicitada").val().trim().length === 0) {
            validationmessage = validationmessage + "<br />" + "La cantidad solicitada es obligatoria";
            isValid = false;
        }

        if ($("#due_beneficiario").val() === 0) {
            validationmessage = validationmessage + "<br />" + "Debe elegir un beneficiario";
            isValid = false;
        }

        if ($("#due_cantidadsolicitada").autoNumeric("get") <= 0) {
            validationmessage = validationmessage + "<br />" + "Debe ingresar cantidad solicitada mayor a cero";
            isValid = false;
        }

        if(isValid) {

            var caseDetailId = $("#due_beneficiario").val() !== "" ? $("#due_beneficiario").val().split("_")[2]: caseData.CaseDetailId;

            var data = {
                caseId: caseData.CaseId,
                caseDetailId: caseDetailId,
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

                if (response.data.Status === "OK") {
                    CDI.displayNotification("Datos guardados", "info");

                    resetDocumentTypes();
                    hideAllPaymentMethodPanels();
                    expandSearchSection();
                } else {
                    showDialogMessage(data.data.Message);
                }
            }).fail(function () {
                CDI.hideWaitingMessage();
            });
        }
        else {
            showDialogMessage(validationmessage);
        }
    };

    var savePendingDiets = function () {
        var isValid = true;
        var validationmessage = "";

        var total = $("#dieta_montototal").autoNumeric("get");
        if (total === 0) {
            validationmessage = validationmessage + "<br />" + "Debe ingresar valores mayores a cero en los campos de cantidades";
            isValid = false;
        }

        if ($("#dieta_fechadecision").val() === "") {
            validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Decisión";
            isValid = false;
        }
        else {
            if(!CDI.isValidDateUS($("#dieta_fechadecision").val()))
            {
                validationmessage = validationmessage + "<br />" + "Por favor, escriba una fecha válida para Fecha Decisión.";
                isValid = false;
            }

        }

        if ($("#dieta_fvisita").val() === "") {
            validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Visita";
            isValid = false;
        }
        else {
            if (!CDI.isValidDateUS($("#dieta_fvisita").val()))
            {
                validationmessage = validationmessage + "<br />" + "Por favor, escriba una fecha válida para Fecha Visita.";
                isValid = false;
            }

        }

        if ($("#dieta_numerocaso").val() === "") {
            validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Número de Caso";
            isValid = false;
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
            isValid = false;
        }

        if (!isValid) {
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
            if (response.data.Status === "OK") {
                CDI.displayNotification("Datos guardados", "info");

                resetDocumentTypes();
                hideAllPaymentMethodPanels();
                expandSearchSection();
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
        $("#inv_totalinversion").autoNumeric("init", {
            vMin: "0"
        });
        $("#ipp_adjudicacionadicional").autoNumeric("init", {
            vMin: "0"
        });
        $("#ipp_mensualidad").autoNumeric("init");
        $("#ipp_semanas").autoNumeric("init", {
            aSign: ""
        });      

        $("#diff_monto").autoNumeric("init");
        $("#due_cantidadsolicitada").autoNumeric("init", {
            vMin: "0"
        });

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
        $("#dieta_numerocaso").inputmask({ mask: "***********[ **]" });

        $("#lawyer_fechadecision").datepicker({
            startDate: "-50y",
            endDate: "-1d"
        }).on("changeDate", function (selected) {
            startDateDecision = new Date(selected.date.valueOf());
            startDateDecision.setDate(startDateDecision.getDate(new Date(selected.date.valueOf())) - 1);
            $("#lawyer_fvisita").datepicker("setEndDate", startDateDecision);
            $("#lawyer_fvisita").datepicker("setStartDate", "-50y");
            $("#lawyer_fnotificacion").datepicker("setEndDate", startDateDecision);
        });
        $("#lawyer_fvisita").datepicker({
            startDate: "-50y",
            endDate: "-1d"
        }).on("changeDate", function (selected) {
            startDateVisita = new Date(selected.date.valueOf());
            startDateVisita.setDate(startDateVisita.getDate(new Date(selected.date.valueOf())));
            $("#lawyer_fechadecision").datepicker("setStartDate", startDateVisita);
            $("#lawyer_fnotificacion").datepicker("setStartDate", startDateVisita);
            $("#lawyer_fnotificacion").datepicker("setEndDate", startDateDecision);
        });
        $("#lawyer_fnotificacion").datepicker({
            startDate: "-50y",
            endDate: "-1d"
        }).on("changeDate", function (selected) {
            startDateNotificacion = new Date(selected.date.valueOf());
            startDateNotificacion.setDate(startDateNotificacion.getDate(new Date(selected.date.valueOf())));
            $("#lawyer_fechadecision").datepicker("setStartDate", startDateNotificacion);
            $("#lawyer_fvisita").datepicker("setEndDate", startDateNotificacion);
        });
        $("#lawyer_montototal").autoNumeric("init");
        $("#lawyer_montototal").autoNumeric("set", 0);
        $("#lawyer_numerocaso").inputmask({ mask: "***************" });
    };

    var bindUiActions = function() {
        $(CDI.CaseSearch).on("case.selected", function(e, data) {
            caseData = data.selectedCase;

            resetDocumentTypes();
            hideAllPaymentMethodPanels();
            showCaseDetail();
            scrollToDocumentType();
        });

        $(CDI.CaseSearch).on("started", function () {
            resetDocumentTypes();
            hideAllPaymentMethodPanels();
        });

        $(CDI.CaseSearch).on("cleaned", function () {
            resetDocumentTypes();
            hideAllPaymentMethodPanels();
        });

        $("#btnSaveInvestment").on("click", function(e) {
            e.preventDefault();

            $.ajax({
                type: "GET",
                url: root + "case/getbalance/" + caseData.CaseDetailId,
                dataType: "json",
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) {
                CDI.hideWaitingMessage();

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
            }).fail(function () {
                CDI.hideWaitingMessage();
            });

            
        });

        $("#btnSaveIpp").on("click", function(e) {
            e.preventDefault();

            var isValid = true;
            var validationmessage = "";

            if ($("#ipp_fechaadjudicacion").val().trim().length === 0) {
                validationmessage = validationmessage + "<br />" + "La fecha de adjudicación es obligatoria";
                isValid = false;
            }
            else
            {
                if (!CDI.isValidDateUS($("#ipp_fechaadjudicacion").val().trim()))
                {
                    validationmessage = validationmessage + "<br />" + "Por favor, escribe una fecha válida.";
                    isValid = false;
                }
            }

            if ($("#ipp_adjudicacionadicional").autoNumeric("get") <= 0) {
                validationmessage = validationmessage + "<br />" + "La cantidad adjudicada de ser mayor a cero";
                isValid = false;
            }
            else
            {
                var total = ippTotalTransactionAmount == null? 0 : ippTotalTransactionAmount;

                total = $("#ipp_adjudicacionadicional").autoNumeric("get") + total;

                if(ippMaximunAmount == null || ippMaximunAmount == undefined)
                {
                    validationmessage = validationmessage + "<br />" + "No se ha encontrado el máximo monto definido para el año del Caso";
                    isValid = false;
                }
                else {
                    if (total > ippMaximunAmount) {
                        validationmessage = validationmessage + "<br />" + "El monto total de las transacciones máa el total adjudicado supera al máximo monto definido para el año del caso";
                        isValid = false;
                    }                    
                }

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

            model.Desgloses = CDI.DesgloseIPP.getDesglose();
            
            $.ajax({
                url: root + "paymentregistration/insertipp",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ model: model }),
                dataType: "json",
                type: "POST",
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) { 
                CDI.hideWaitingMessage();

                if (response.data.Status === "OK") {
                    CDI.displayNotification("Datos guardados", "info");

                    resetDocumentTypes();
                    hideAllPaymentMethodPanels();
                    expandSearchSection();
                    $("#tblDesglosarIpp > tbody").find(".hidden").nextAll().remove();

                } else {
                    showDialogMessage("Error al grabar los datos");
                }
            }).fail(function () { 
                CDI.hideWaitingMessage();
            });
        });

        $("#btnSavePeremptory").on("click", function(e) {
            e.preventDefault();

                var caseDetailId = $("#due_beneficiario").val() !== "" ? $("#due_beneficiario").val().split("_")[2]: caseData.CaseDetailId;

            $.ajax({
                type: "GET",
                url: root + "case/getbalance/" + caseDetailId,
                dataType: "json",
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) {
                CDI.hideWaitingMessage();

                balance = parseFloat(response);

                var total = $("#due_cantidadsolicitada").autoNumeric("get");
                if (total > balance) {
                    showDialogMessage("El caso no tiene suficiente balance para registrar el pago");
                }
                else {
                    savePeremptories();
                }
            }).fail(function () {
                CDI.hideWaitingMessage();
            });
        });

        $("#btnSavePendingDiet").on("click", function(e) {
            e.preventDefault();

            $.ajax({
                type: "GET",
                url: root + "case/getbalance/" + caseData.CaseDetailId,
                dataType: "json",
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) {
                CDI.hideWaitingMessage();

                savePendingDiets();

            }).fail(function () {
                CDI.hideWaitingMessage();
            });
        });

        $("#btnSaveLawyer").on("click", function (evt) {
            evt.preventDefault();

            var isValid = true;
            var validationmessage = "";

            if (!caseData.TieneAbogado) {                
                validationmessage = validationmessage + "<br />" + "El caso no tiene abogado.";
                isValid = false;
            }

            var total = $("#lawyer_montototal").autoNumeric("get");
            if (total === 0) {
                validationmessage = validationmessage + "<br />" + "Debe ingresar valores mayores a cero en el monto a pagar";
                isValid = false;
            }
            
            if ($("#lawyer_fechadecision").val() === "") {
                validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Decisión";
                isValid = false;
            }
            else {
                if(!CDI.isValidDateUS($("#lawyer_fechadecision").val()))
                {
                    validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Decisión";
                    isValid = false;
                }
            }

            if ($("#lawyer_fvisita").val() === "") {
                validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Visita";
                isValid = false;
            }
            else {
                if (!CDI.isValidDateUS($("#lawyer_fvisita").val())) {
                    validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Visita";
                    isValid = false;
                }
            }

            if ($("#lawyer_fnotificacion").val() === "") {
                validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Notificación";
                isValid = false;
            }
            else {
                if (!CDI.isValidDateUS($("#lawyer_fnotificacion").val())) {
                    validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Fecha Notificación";
                    isValid = false;
                }
            }

            if ($("#lawyer_numerocaso").val() === "") {
                validationmessage = validationmessage + "<br />" + "Debe ingresar un valor para Número de Caso";
                isValid = false;
            }

            if (!isValid) {
                showDialogMessage(validationmessage);
                return false;
            }

            var data = {
                caseId: caseData.CaseId,
                caseDetailId: caseData.CaseDetailId,
                fechadecision: $("#lawyer_fechadecision").val(),
                fechavisita: $("#lawyer_fvisita").val(),
                fechanotificacion: $("#lawyer_fnotificacion").val(),
                numerocaso: $("#lawyer_numerocaso").val().replace(/_/g, ""),
                montototal: $("#lawyer_montototal").autoNumeric("get"),
                comment: $("#lawyer_observaciones").val(),
                caseNumber: caseData.NumeroCaso
            };

            $.ajax({
                url: root + "paymentregistration/inserthonorarylawyer",
                data: data,
                dataType: "json",
                type: "POST",
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) { 
                CDI.hideWaitingMessage();
                if (response.data.Status === "OK") {
                    CDI.displayNotification("Datos guardados", "info");

                    resetDocumentTypes();
                    hideAllPaymentMethodPanels();
                    expandSearchSection();
                } else {
                    showDialogMessage("Error al grabar datos.");
                }
            }).fail(function () {
                CDI.hideWaitingMessage();
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

        $(".nuevo").off("click").on("click", function (evt) {
            evt.preventDefault();

            resetDocumentTypes();
            hidePaymentMethodPanels();
            expandSearchSection();
        });

        $(".cancelar").off("click").on("click", function(evt) {
            evt.preventDefault();

            resetDocumentTypes();
            hidePaymentMethodPanels();
            expandSearchSection();
            scrollToDocumentType();
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
            $("#due_relacion").val("");

            if (val === "") {
                $("#due_relacion").val("");
                return false;
            }

            $("#due_relacion").val($("#due_beneficiario").val().split("_")[1]);
           
        });

        $("#inv_beneficiario").change(function() {
            var val = $(this).val();
            $("#inv_relacion").val("");

            if (val === "") {
                $("#inv_relacion").val("");
            }

            $("#inv_relacion").val($("#inv_beneficiario").val().split("_")[1]);
            
        });

        $("#ddlDocumentType").change(function() {
            var selectedValue = $(this).val();

            switch (selectedValue) {
                case "":
                    hidePaymentMethodPanels();
                    break;
                case "1":
                    clearInversionPanel();
                    showInversionPanel();
                    showTableInvestment(caseData.CaseId, caseData.CaseDetailId, caseData.EsBeneficiario);

                    $.ajax({
                        type: "GET",
                        url: root + "paymentregistration/getentityrelatives/" + caseData.CaseId,
                        cache: false
                    }).done(function (response) {
                        $("#inv_beneficiario").find("option:gt(0)").remove();

                        $.each(response, function(index, element) {
                            var $option;

                            if (element.Value.split("_")[3] === "True") {
                                var entityId = element.Value.split("_")[0];
                                if (entityId === caseData.EntityId) {
                                    $option = "<option value=" + element.Value + " selected>" + element.Text + "</option>";

                                    $("#inv_relacion").val(element.Value.split("_")[1]);
                                }
                                else {
                                    $option = "<option value=" + element.Value + ">" + element.Text + "</option>";
                                }

                                $($option).appendTo("#inv_beneficiario");
                            }
                        });

                        if (caseData.EsBeneficiario) {
                            $("#inv_beneficiario").prop("disabled", true);
                                }
                        else {
                              if(!caseData.CasoMuerte) {
                                var optionValue = caseData.EntityId + "_0_" +caseData.CaseDetailId;

                                var $option = "<option value=" +optionValue + " selected>" +caseData.Lesionado + "</option>";

                                $($option).appendTo("#inv_beneficiario");
                                $("#inv_beneficiario").prop("disabled", true);
                            }
                        }
                    });
                    break;
                case "2":
                    clearIppPanel();
                    showIppPanel();
                    showTableIpp(caseData.CaseDetailId);
                    showMonthlyPaymentAndNumberOfWeeks();
                    CDI.DesgloseIPP.setCompensation("#ipp_semanas", caseData.CompSemanalInca);
                    CDI.DesgloseIPP.restartValues();                   
                    break;
                case "3":
                    clearPeremptoryPanel();
                    showPeremptoryPanel();
                    showTablePeremptory(caseData.CaseId, caseData.CaseDetailId, caseData.EsBeneficiario);

                    $.ajax({
                        type: "GET",
                        url: root + "paymentregistration/getentityrelatives/" + caseData.CaseId,
                        cache: false,
                        dataType: "json"
                    }).done(function (response) {
                        $("#due_beneficiario").find("option:gt(0)").remove();

                        $.each(response, function (index, element) {
                            var $option;
                            var entityId = element.Value.split("_")[0];
                            if (entityId === caseData.EntityId) {
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
                        else
                        {
                            if(!caseData.CasoMuerte)
                            {
                                var optionValue = caseData.EntityId + "_0_" + caseData.CaseDetailId;

                                var $option = "<option value=" + optionValue + " selected>" + caseData.Lesionado + "</option>";

                                $($option).appendTo("#due_beneficiario");
                                $("#due_beneficiario").prop("disabled", true);
                            }
                        }
                    });
                    break;
                case "4":
                    clearPendingDietPanel();
                    showPendingDietPanel();
                    break;
                case "5":
                    if (!caseData.TieneAbogado) {
                        showDialogMessage("El caso no tiene abogado.");
                    }
                    clearLawyerPanel();
                    showLawyerPanel();
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

        $("#ipp_adjudicacionadicional").blur(function () {
            var currentValue = $(this).autoNumeric("get");

            if (currentValue > 0) {
                $("#btnShowDesglose").removeAttr("disabled");
            }
            else {
                $("#btnShowDesglose").prop("disabled", true);
            }
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

$(function () {
    CDI.DesgloseIPP.init();
    CDI.CaseSearch.init();
    CDI.PaymentRegistration.init();    
});