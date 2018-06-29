CDI.DisplayPayments = (function () {
    var tablaCasos;
    var clinics = $("#ClinicList option").clone();
    var casedata;
    var resumencaseId;
    var invResumenTable;
    var dueResumenTable;
    var ippResumenTable;
    var balance = 0;
    var inicializarControles = function () {
                
        $("#direccion_zipcode").mask("99999-9999");
        $("#inv_totalinversiones").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#inv_totalinversion").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#ipp_adjudicacionadicional").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#diff_monto").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        
        $("#ipp_mensualidad").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#ipp_semanas").autoNumeric("init", { vMin: "0", vMax: "999999999", mDec: "0" });
        $("#due_cantidadsolicitada").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });

        $("#info_menor").prop("disabled", true);
        $("#info_muerte").prop("disabled", true);
        $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
        $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");
        $("#panel_entidad").hide();
        $("#panel_custodio").hide();
        $("#CourtList").prop("disabled", "disabled");

        $("#fnacimiento2, #fradicacion2, #ipp_fechaadjudicacion2, #inv_fechaemisiondecision2, #diff_fechadecision2, #dieta_fvisita2, #dieta_fnotificacion2, #dieta_fechadecision2").datepicker();
        
        $("#dieta_montototal").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });

        cargaclinicas();       
    }

    var cargaclinicas = function () {
        $("#ClinicList").empty();
        $("#ClinicList").append("<option value=\"0\">--Seleccione Dispensario --</option>");
        $("#RegionList").change(function () {
            var val = $(this).val();
            $("#ClinicList").empty();
            clinics.filter(function (idx, el) {
                return $(el).val() == "0" || $(el).text().indexOf("[" + val + "]") >= 0;
            }).appendTo("#ClinicList");
        });
    }

    var data = function () {
        return {
            "nombre": $("#name").val(),
            "ssn": $("#ssn").val(),
            "fnacimiento": $("#fnacimiento").val(),
            "numeroCaso": $("#caso").val(),
            "fradicacion": $("#fradicacion").val(),
            "region": $("#RegionList").val(),
            "dispensario": $("#ClinicList").val()
        };
    }

    var dataresumen = function () {
        return {
            "entityId" : idresumen
        }
    }

    var dataresumenInv = function () {
        return {
            "caseId" : resumencaseId
        }
    }

    var validateSearch = function () {
        var isValidate = false;

        if ($("#name").val() != "") {
            isValidate = true;
        }

        if ($("#ssn").val() != "") {
            isValidate = true;
        }

        if ($("#caso").val() != "") {
            isValidate = true;
        }

        if ($("#fnacimiento").val() != "") {
            isValidate = true;
        }

        if ($("#fradicacion").val() != "") {
            isValidate = true;
        }

        if ($("#RegionList").val() != "0") {
            isValidate = true;
        }
        if ($("#ClinicList").val() != "0") {
            isValidate = true;
        }

        return isValidate;
    }

    var msgBox = function (msg) {
        $("#msg span").text(msg);
        $("#modalMsg").modal("show");
    }

    var showProgress = function (msg) {
        $("#msg span").text(msg);
        winp = $("#winprogress").modal("show");
    }

    var closeProgress = function () {

        winp.modal("hide");
    }

    var showResults = function () {

        if (!validateSearch()) {
            msgBox("Debe ingresar o seleccionar un valor de busqueda");
            return;
        }

        if ($("#resultsPanel").css("display") == "none")
            $("#resultsPanel").show();

        $("#formadepagos").hide();

        if (!tablaCasos) {
            tablaCasos = $("#CasesTable").DataTable({
                "processing": true,
                "autoWidth": false,
                "drawCallback": function (settings) {
                    $(".dataTables_empty").html("No se encontraron casos.");
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
                    "url": "/PaymentMethod/Search",
                    "type": "POST",
                    "data": data
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "NumeroCaso"
                    }
                    ,
                    {
                        "data": "Nombre"
                    },
                    {
                        "data": "SSN"
                    },
                    {
                        "data": "FechaNacimiento"
                    },
                    {
                        "data": "FechaRadicacion"
                    },
                    {
                        "data": "Region"
                    },
                    {
                        "data": "Dispensario"
                    }
                ]
                ,
                "dom": "rtp"
            });
        }
        else
            tablaCasos.ajax.reload();
    }

    var showResumenInv = function (caseId) {

        resumencaseId = caseId;

        if (!invResumenTable) {
            invResumenTable = $("#inv_resumenTable").DataTable({
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
                    "url": "/PaymentMethod/GetTransactionSummaryInv",
                    "type": "POST",
                    "data": dataresumenInv
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "Fecha"
                    }
                    ,
                    {
                        "data": "Cantidad",
                        "mRender": function (data, type, full) {                           
                            return "$ " + data.toString();
                        }
                    }
                ],
                "fnInitComplete": function(oSettings, json) {
                    calcular_resumen_inversiones();
                },
                "dom": "rtp"
            });
        }
        else {
				invResumenTable.ajax.reload(function ( json ) {
				calcular_resumen_inversiones();});   				
        }
            
    }

	var alinear_resumen_due = function () {
		  $("#due_resumenTable tbody tr:has(td)").each(function () {
                        var row = $(this);                       
                        row.find("td:eq(3)").css("text-align", "right");
                    });
	}

	var showResumenDue = function (caseId) {

        resumencaseId = caseId;

        if (!dueResumenTable) {
            dueResumenTable = $("#due_resumenTable").DataTable({
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
                    "url": "GetTransactionSummaryDue",
                    "type": "POST",
                    "data": dataresumenInv
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "Beneficiario"
                    }
                    ,
                    {
                        "data": "Relacion"
                    }
                    ,
                    {
                        "data": "Fecha"
                    }
                    ,
                    {
                        "data": "MontoPagado",
                        "mRender": function (data, type, full) {                           
                            return "$ " + data.toString();
                        }
                    }
                ],
                "fnInitComplete": function(oSettings, json) {
                  alinear_resumen_due();
                },
                "dom": "rtp"
            });
        }
        else{
			dueResumenTable.ajax.reload(function ( json ) {
			alinear_resumen_due();});   			
		}
    }

	var alinear_resumen_ipp = function (){
		  $("#ipp_resumenTable tbody tr:has(td)").each(function () {
                        var row = $(this);
                        row.find("td:eq(0)").css("text-align", "right");
                        row.find("td:eq(1)").css("text-align", "right");
                        row.find("td:eq(3)").css("text-align", "right");
                    });
	}

	var showResumenIpp = function (caseId) {

        resumencaseId = caseId;

        if (!ippResumenTable) {
            ippResumenTable = $("#ipp_resumenTable").DataTable({
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
                    "url": "/PaymentMethod/GetTransactionSummaryIpp",
                    "type": "POST",
                    "data": dataresumenInv
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "CantidadAdjudicada",
                        "mRender": function (data, type, full) {                           
                            return "$ " + data.toString();
                        }
                    }
                    ,
                    {
                        "data": "PagoInicial",
                        "mRender": function (data, type, full) {                        
                            return "$ " + data.toString();
                        }
                    }
                    ,
                    {
                        "data": "FechaAdjudicacion"
                    }                    
                    ,                    
                    {
                        "data": "Semanas"
                    }
                ],
                "fnInitComplete": function(oSettings, json) {
                   alinear_resumen_ipp();
                }
                ,
                "dom": "rtp"
            });
        }
        else{
			ippResumenTable.ajax.reload(function ( json ) {
   		    alinear_resumen_ipp();});   
		}
    }
   
    var showCaseDetail = function (casedata) {
        balance = casedata.data.Balance;

        $("#caseid").val(casedata.data.caseId);
        $("#entityid").val(casedata.data.entityId);
        $("#info_numerocaso").val(casedata.data.NumeroCaso);
        $("#info_lesionado").val(casedata.data.Lesionado);
        $("#info_ssn").val(casedata.data.SSN);
        $("#info_faccidente").val(casedata.data.FechaAccidente);
        $("#info_fradicacion").val(casedata.data.FechaRadicacion);
        $("#info_fnacimiento").val(casedata.data.FechaNacimiento);
        $("#direccion_linea1").val(casedata.data.InfoDireccion.Linea1);
        $("#direccion_linea2").val(casedata.data.InfoDireccion.Linea2);
        $("#direccion_ciudad").val(casedata.data.InfoDireccion.Ciudad);
        $("#direccion_estado").val(casedata.data.InfoDireccion.Estado);
        $("#direccion_zipcode").val(casedata.data.InfoDireccion.ZipCode);
        $("#info_region").val(casedata.data.Region);
        $("#info_dispensario").val(casedata.data.Dispensario);

        $("#info_patrono").val(casedata.data.Patrono);
        $("#info_ein").val(casedata.data.EIN);
        $("#info_estatus_patronal").val(casedata.data.EstatusPatronal);
        $("#info_poliza").val(casedata.data.Poliza);

        if (casedata.data.CasoMenor == "True") {
            $("#info_menor").prop("checked", true);
        }

        if (casedata.data.CasoMuerte == "True") {
            $("#info_muerte").prop("checked", true);
        }

        if (casedata.data.CasoTutor == "True") {
            $("#info_tutor").prop("checked", true);
        }        
    }

    var showInvDetail = function(){}

    var calcular_resumen_inversiones = function (){
        var suma_inversiones = 0;
        var count = $("#inv_resumenTable tbody tr:has(td)").length;
       
        for (i = 0; i < count; i++) {
            var inversion = $($("#inv_resumenTable tbody tr:has(td)")[i]).find("td:eq(1)").text();            
            var inversion_valor = parseFloat(inversion.replace("$ ", ""));           
            suma_inversiones = suma_inversiones + inversion_valor;
            $($("#inv_resumenTable tbody tr:has(td)")[i]).find("td:eq(1)").css("text-align", "right");
        }
        $("#inv_totalinversion").autoNumeric("set", suma_inversiones);
        var foot = $("#inv_resumenTable").find("tfoot");
        if (!foot.length) foot = $("<tfoot>").appendTo("#inv_resumenTable");
        foot.html("<tr><th style=\"text-align:right\"><b>Total:</b></th><th style=\"text-align:right\">" + $("#inv_totalinversion").val() + "</th></tr>");
    }

    var calcular_total_inversiones = function () {
        var suma_inversiones = 0;
        $("#inv_pagostable tbody tr:has(td)").each(function () {
            var inversion_valor = 0;
            if ($(this).find("#inversion") === undefined)
            {
                inversion_valor = 0;
            }
            if ($(this).find("#inversion") == "") {
                inversion_valor = 0;
            }
            var inversion = $(this).find("#inversion").autoNumeric("get");
            inversion_valor = parseFloat(inversion);
            if (isNaN(inversion)) {
                inversion_valor = 0;
            }
            suma_inversiones = suma_inversiones + inversion_valor;
        });
        $("#inv_totalinversiones").autoNumeric("set", suma_inversiones);
        var foot = $("#inv_pagostable").find("tfoot");
        if (!foot.length) foot = $("<tfoot>").appendTo("#inv_pagostable");
        foot.html("<tr><td></td><td></td><td style=\"text-align:right\"><b>Total: " + $("#inv_totalinversiones").val() + "</b></td></tr>");
    }

    var calcular_total_dietas_pendientes = function () {
        var suma_montos = 0;
        $("#periodos-dieta-table tbody tr:has(td)").each(function () {
            var cantidad_valor = 0;
            if ($(this).find("#cantidad") === undefined) {
                cantidad_valor = 0;
            }
            if ($(this).find("#cantidad") == "") {
                cantidad_valor = 0;
            }
            var cantidad_control = $(this).find("#cantidad").autoNumeric("get");
            cantidad_valor = parseFloat(cantidad_control);
            if (isNaN(cantidad_valor)) {
                cantidad_valor = 0;
            }
            suma_montos = suma_montos + cantidad_valor;
        });
        $("#dieta_montototal").autoNumeric("set", suma_montos);
        var foot = $("#periodos-dieta-table").find("tfoot");
        if (!foot.length) foot = $("<tfoot>").appendTo("#periodos-dieta-table");
        foot.html("<tr><td></td><td></td><td></td><td style=\"text-align:right\"><b>Total: " + $("#dieta_montototal").val() + "</b></td></tr>");
    }

    var limpiar_campos = function () {
        $("#inv_pagostable tbody").find("tr").remove();
        $("#inv_pagostable tfoot").find("tr").remove();

        $("#inv_fechaemisiondecision").val("");
        $("#inv_fechaemisiondecision2").datepicker("setDate", null);
        $("#inv_fechaemisiondecision2 input").val("");

        $("#ipp_adjudicacionadicional").autoNumeric("set", 0);        
        $("#ipp_mensualidad").autoNumeric("set", 0);
        $("#ipp_semanas").autoNumeric("set", 0);
        $("#ipp_num_transaccion").val("");
        $("#ipp_observaciones").val("");
        $("#ipp_fechaadjudicacion").val("");
        $("#ipp_fechaadjudicacion2").datepicker("setDate", null);
        $("#ipp_fechaadjudicacion2 input").val("");

        $("#due_cantidadsolicitada").autoNumeric("set", 0);
        $("#due_num_transaccion").val("");
        $("#due_beneficiario").val(0);
        $("#due_relacion").val("");
        $("#due_observaciones").val("");

        $("#inv_num_transaccion").val("");
        $("#inv_fechaemisiondecision").val("");      
        $("#inv_totalinversiones").autoNumeric("set", 0);
        $("#inv_totalinversion").autoNumeric("set", 0);
        $("#Inv_observaciones").val("");

        $("#info_menor").prop("disabled", true);
        $("#info_muerte").prop("disabled", true);
        $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
        $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");

        $("#panel_entidad").hide();
        $("#panel_custodio").hide();
        $("#CourtList").prop("disabled", "disabled");

        $("#dieta_fechadecision").val("");
        $("#dieta_fechadecision2").datepicker("setDate", null);
        $("#dieta_fechadecision2 input").val("");
        $("#dieta_fvisita").val("");
        $("#dieta_fvisita2").datepicker("setDate", null);
        $("#dieta_fvisita2 input").val("");
        $("#dieta_fnotificacion").val("");
        $("#dieta_fnotificacion2").datepicker("setDate", null);
        $("#dieta_fnotificacion2 input").val("");
        $("#dieta_numerocaso").val("");
        $("#periodos-dieta-table tbody").find("tr").remove();
        $("#periodos-dieta-table tfoot").find("tr").remove();
        
        if (invResumenTable !== undefined) {
            invResumenTable.clear().draw();
        }        
        if (dueResumenTable !== undefined) {
            dueResumenTable.clear().draw();
        }        
        if (ippResumenTable !== undefined) {
            ippResumenTable.clear().draw();
        }
        
    }

    var guardar_inversiones = function (){
        var validationstatus = true;
        var validationmessage = "";

        if ($("#inv_fechaemisiondecision").val() == "") {
            validationmessage = validationmessage + "</BR>" + "Debe ingresar un valor para Fecha Decision";
            validationstatus = false;
        }

        var total = $("#inv_totalinversiones").autoNumeric("get");
        if (total == 0) {
            validationmessage = validationmessage + "</BR>" +  "Debe ingresar valores mayores a cero en los campos de cantidades";              
            validationstatus = false;
        }

        if (total > balance) {
            validationmessage = validationmessage + "</BR>" + "El caso no tiene suficiente balance para registrar el pago";
            validationstatus = false;
        }

        var payments_valid = true;
        var payments_array = [];
        $("#inv_pagostable tbody tr:has(td)").each(function () {
            var entidad = $(this).find("#entidad").val();
            var inversion = $(this).find("#inversion").autoNumeric("get");
            if (inversion == 0 || entidad == "") {
                payments_valid = false;                    
            }
            payments_array.push(  { entidad: entidad,  inversion: inversion });
        });     

        if (!payments_valid) {
            validationmessage = validationmessage + "</BR>" + "Todos los pagos y entidades deben ser llenados";
            validationstatus = false;
        }            

        if (!validationstatus) {
            msgBox(validationmessage);
            return false;
        }

        var payments = JSON.stringify(payments_array);   
        var insertPaymentInvestReq = {
            "caseId": $("#caseid").val(),
            "fechadecision": $("#inv_fechaemisiondecision").val(),
            "payments": payments,
            "comment": $("#Inv_observaciones").val(),
            "caseNumber": $("#info_numerocaso").val()
        };          

        showProgress("Grabando...");

        $.ajax({
            url: "InsertPaymentInv",
            data: insertPaymentInvestReq,
            dataType: "json",
            type: "POST",
            success: function (result) {
                closeProgress();
                if (result.data.Status === "OK") {
                    msgBox("Datos guardados");
                    limpiar_campos();
                    invResumenTable.ajax.reload(function ( json ) {
			         calcular_resumen_inversiones();
                     $("#formadepagos").hide();});					
                }                        
                else {
                    msgBox("Error al grabar datos");
                }
            },
            error: function () {
                closeProgress();
                msgBox("Ha ocurrido un error. Consulte con el Administrador.");
            }
        });
    }

    var guardar_perentorios = function() {
        var validationstatus = true;
        var validationmessage = "";

        var total = $("#due_cantidadsolicitada").autoNumeric("get");
        if (total == 0) {
            validationmessage = validationmessage + "</BR>" + "Debe ingresar valores mayores a cero en los campos de cantidades";
            validationstatus = false;
        }
        if (total > balance) {
            validationmessage = validationmessage + "</BR>" + "El caso no tiene suficiente balance para registrar el pago";
            validationstatus = false;
        }
        if ($("#due_cantidadsolicitada").val().trim().length == 0) {
            validationmessage = validationmessage + "</BR>" + "Cant. Solicitada es obligatoria";
            validationstatus = false;
        }

        if ($("#due_beneficiario").val() == 0) {
            validationmessage = validationmessage + "</BR>" + "Debe elegir benef.";
            validationstatus = false;
        }

        if ($("#due_cantidadsolicitada").autoNumeric("get") == 0) {
            validationmessage = validationmessage + "</BR>" + "Debe ingresar cantidad solic. mayor a cero";
            validationstatus = false;
        }       

        if (!validationstatus) {
            msgBox(validationmessage);
            validationstatus = false;
        }

        var data = {
            "caseId": $("#caseid").val(),
            "entityid": $("#entityid").val(),
            "cantidad": $("#due_cantidadsolicitada").autoNumeric("get"),            
            "beneficiario": $("#due_beneficiario").val().split("_")[0],
            "comment": $("#due_observaciones").val(),
            "caseNumber": $("#info_numerocaso").val()
        };

        showProgress("Grabando...");
        $.ajax({
            url: "InsertPaymentDue",
            data: data,
            dataType: "json",
            type: "POST",
            success: function (data) {
                closeProgress();
                if (data.data.Status == "OK") {
                    msgBox("Datos guardados");                       
                    limpiar_campos();
                    dueResumenTable.ajax.reload(function ( json ) {
			        alinear_resumen_due();
                    $("#formadepagos").hide();});
					
                }
                else {
                    msgBox("Error al grabar datos.");
                }
            },
            error: function () {
                closeProgress();
                msgBox("Ha ocurrido un error. Consulte con el Administrador.");
            }
        });
    }

    var guardar_dietas = function () {
        var validationstatus = true;
        var validationmessage = "";

        var total = $("#dieta_montototal").autoNumeric("get");
        if (total == 0) {
            validationmessage = validationmessage + "</BR>" + "Debe ingresar valores mayores a cero en los campos de cantidades";
            validationstatus = false;
        }
        if (total > balance) {
            validationmessage = validationmessage + "</BR>" + "El caso no tiene suficiente balance para registrar el pago";
            validationstatus = false;
        }

        if ($("#dieta_fechadecision").val() == "") {
            validationmessage = validationmessage + "</BR>" + "Debe ingresar un valor para Fecha Decisión";
            validationstatus = false;
        }

        if ($("#dieta_fvisita").val() == "") {
            validationmessage = validationmessage + "</BR>" + "Debe ingresar un valor para Fecha Visita";
            validationstatus = false;
        }

        if ($("#dieta_fnotificacion").val() == "") {
            validationmessage = validationmessage + "</BR>" + "Debe ingresar un valor para Fecha Notificación";
            validationstatus = false;
        }

        if ($("#dieta_numerocaso").val() == "") {
            validationmessage = validationmessage + "</BR>" + "Debe ingresar un valor para Número de Caso";
            validationstatus = false;
        }

        var periods_valid = true;
        var periods_array = [];
        $("#periodos-dieta-table tbody tr:has(td)").each(function () {
            var desde = $(this).find("#desde").data("date");
            var hasta = $(this).find("#hasta").data("date");
            var cantidad = $(this).find("#cantidad").autoNumeric("get");
            if (cantidad == 0 || cantidad == "" || desde == "" || hasta == "") {
                periods_valid = false;
            }
            periods_array.push({ desde: desde, hasta: hasta, cantidad:cantidad });
        });

        if (!periods_valid) {
            validationmessage = validationmessage + "</BR>" + "Todos los periodos y cantidades deben ser ingresados";
            validationstatus = false;
        }

        if (!validationstatus) {
            msgBox(validationmessage);
            validationstatus = false;
        }
        var periods = JSON.stringify(periods_array);   
        var data = {
            "caseId": $("#caseid").val(),          
            "fechadecision": $("#dieta_fechadecision").val(),
            "fechavisita": $("#dieta_fvisita").val(),
            "fechanotificacion": $("#dieta_fnotificacion").val(),
            "numerocaso": $("#dieta_numerocaso").val(),
            "montototal": $("#dieta_montototal").val(),
            "periods": periods,
            "comment": $("#due_observaciones").val(),
            "caseNumber": $("#info_numerocaso").val()
        };
        showProgress("Grabando...");
        $.ajax({
            url: "InsertPaymentDieta",
            data: data,
            dataType: "json",
            type: "POST",
            success: function (data) {
                closeProgress();
                if (data.data.Status == "OK") {
                    msgBox("Datos guardados");                       
                    limpiar_campos();					
                }
                else {
                    msgBox("Error al grabar datos.");
                }
            },
            error: function () {
                closeProgress();
                msgBox("Ha ocurrido un error. Consulte con el Administrador.");
            }
        });
    }
    
    var _start = function () {

        inicializarControles();

        $("#searchBtn").on("click", showResults);

        $("#searchPanel input").keyup(function (event) {
            if (event.keyCode == 13) {
                $("#searchBtn").click();
            }
        });

        $("#limpiarBtn").on("click", function (e) {
            $("#name").val("");
            $("#ssn").val("");
            $("#caso").val("");
            $("#RegionList").val("0");
            $("#ClinicList").val("0");
            $("#fnacimiento2").data("DatePicker").date(null);
            $("#fradicacion2").data("DatePicker").date(null);
            
        });

        $("#CasesTable tbody").on("click", "tr", function () {
            var selcaso = tablaCasos.row(this).data()
            var caso = selcaso.NumeroCaso;

            if ($("#formadepagos").css("display") == "none")
                $("#formadepagos").show();

            $.ajax({
                url: "GetCaseByNumber/" + caso,
                datatype: "text",
                type: "GET",
                success: function (data) {
                    casedata = data;
                    showCaseDetail(casedata);
                },
                error: function () {
                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                }
            });
        });
              
        $("#guardarbtninv").on("click", function (e) {

            $.ajax({
                type: "GET",
                url: "GetBalance/" + $("#caseid").val(),
                dataType: "json",
                success: function (data) {
                    var validationstatus = true;
                    var validationmessage = "";
                    var balance = parseFloat(data);
                    var total = $("#inv_totalinversiones").autoNumeric("get");
                    if (total > balance) {
                        validationmessage = validationmessage + "</BR>" + "El caso no tiene suficiente balance para registrar el pago";
                        validationstatus = false;
                    }
                    if (!validationstatus) {
                        msgBox(validationmessage);
                        return false;
                    }
                    guardar_inversiones();
                }
            });
           
            
        });

        $("#guardarbtnipp").on("click", function (e) {            
            var validationstatus = true;
            var validationmessage = "";

            if ($("#ipp_fechadjudicacion").val().trim().length == 0) {
                validationmessage = validationmessage + "</BR>" + "Fecha adj. obligatoria";
                validationstatus = false;
            }

            if ($("#inv_adjudicacionadicional").autoNumeric("get") == 0) {
                validationmessage = validationmessage + "</BR>" + "Adj. Adicional de ser mayor a cero";
                validationstatus = false;
            }

            if ($("#ipp_mensualidad").val().trim().length == 0) {
                validationmessage = validationmessage + "</BR>" + "falta mens.";
                validationstatus = false;
            }

            if ($("#ipp_semanas").val().trim().length == 0) {
                validationmessage = validationmessage + "</BR>" + "falta num. sem.";
                validationstatus = false;
            }            

            if ($("#ipp_mensualidad").autoNumeric("get") == 0) {
                validationmessage = validationmessage + "</BR>" + "Ingrese mensualidad mayor a cero";
                validationstatus = false;
            }

            if ($("#ipp_semanas").autoNumeric("get") == 0) {
                validationmessage = validationmessage + "</BR>" + "Ingrese num. de semanas mayor a cero";
                validationstatus = false;
            }

            if (!validationstatus) {
                msgBox(validationmessage);
                return false;
            }
           

            var data = {
                "caseId": $("#caseid").val(),
                "entityid": $("#entityid").val(),
                "ipp_adjudicacionadicional": $("#ipp_adjudicacionadicional").autoNumeric("get"),
                "mensualidad": $("#ipp_mensualidad").autoNumeric("get"),
                "semanas": $("#ipp_semanas").autoNumeric("get"),                
                "comment": $("#ipp_observaciones").val(),
                "caseNumber": $("#info_numerocaso").val(),
                "fechaadjudicacion": $("#ipp_fechaadjudicacion").val() 
            };
            showProgress("Grabando...");
            $.ajax({
                url: "InsertPaymentIpp",
                data: data,
                dataType: "json",
                type: "POST",
                success: function (data) {
                    closeProgress();
                    if (data.data.Status == "OK") {
                    
                        msgBox("Datos guardados");                   
                        limpiar_campos();
                        ippResumenTable.ajax.reload(function ( json ) {
				          alinear_resumen_ipp();
                          $("#formadepagos").hide();});                       
                    }
                    else {
                        msgBox("Error al grabar los datos");
                    }

                },
                error: function () {
                    closeProgress();
                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                }
            });
        });

        $("#guardarbtndue").on("click", function (e) {
            $.ajax({
                type: "GET",
                url: "GetBalance/" + $("#caseid").val(),
                dataType: "json",
                success: function (data) {
                    var validationstatus = true;
                    var validationmessage = "";
                    var balance = parseFloat(data);
                    var total = $("#due_cantidadsolicitada").autoNumeric("get");
                    if (total > balance) {
                        validationmessage = validationmessage + "</BR>" + "El caso no tiene suficiente balance para registrar el pago";
                        validationstatus = false;
                    }
                    if (!validationstatus) {
                        msgBox(validationmessage);
                        return false;
                    }
                    guardar_perentorios();
                }
            });
        });

        $("#guardarbtndieta").on("click", function (e) {
            $.ajax({
                type: "GET",
                url: "GetBalance/" + $("#caseid").val(),
                dataType: "json",
                success: function (data) {
                    var validationstatus = true;
                    var validationmessage = "";
                    var balance = parseFloat(data);
                    var total = $("#dieta_montototal").autoNumeric("get");
                    if (total > balance) {
                        validationmessage = validationmessage + "</BR>" + "El caso no tiene suficiente balance para registrar el pago";
                        validationstatus = false;
                    }
                    if (!validationstatus) {
                        msgBox(validationmessage);
                        return false;
                    }
                    guardar_dietas();
                }
            });
        });

        $("#addRow").on("click", function (e) {
            var count = $("#inv_pagostable").children("tr").length;
            var myrow = "<tr class=\"active\">" +
                "<td><button type=\"button\" class=\"btn btn-default deleterow\" aria-label=\"Left Align\">"
                + "<span class=\"glyphicon glyphicon-minus\"></span></button></td>" +
                "<td><input class=\"form-control\" id=\"entidad\" name=\"entidad\" type=\"text\"/></td>" +
                "<td style=\"text-align:right\"><input class=\"form-control\" id=\"inversion\" name=\"inversion\" type=\"text\" style=\"text-align:right\"/></td></tr>"
            $("#inv_pagostable tbody").append(myrow);
            $("#inv_pagostable tbody").find("tr").last().find("#inversion").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
            $("#inv_pagostable tbody").change(function () {
                $.ajax({
                    type: "GET",
                    url: "GetBalance/" + $("#caseid").val(),
                    dataType: "json",
                    success: function (data) {

                        calcular_total_inversiones();

                        var total = $("#inv_totalinversiones").autoNumeric("get");
                        if (total > balance) {
                            msgBox("El caso no tiene suficiente balance para registrar el pago");
                        }
                        
                    }
                });              
            });
            calcular_total_inversiones();
        });

        $("#inv_pagostable").on("click", ".deleterow", function () {
            $(this).closest("tr").remove();
            calcular_total_inversiones();
        });

        $("#nuevoperiododietabtn").on("click", function (e) {
            var count = $("#periodos-dieta-table").children("tr").length;
            var myrow = "<tr>" +
                "<td style=\"width:5%\"><button type=\"button\" class=\"btn btn-default delete-row-dieta\" aria-label=\"Left Align\">"
                + "<span class=\"glyphicon glyphicon-minus\"></span></button></td>" +
                "<td style=\"width:20%\">" + 
                "            <div class=\"input-group date\" id=\"desde\">" +
                "                <input type=\"text\" data-format=\"MM/DD/YYYY\" class=\"form-control\" />" + 
                "                <span class=\"input-group-addon\">" + 
                "                    <span class=\"glyphicon glyphicon-calendar\"></span>" + 
                "                </span>" + 
                "            </div></td>" +
                "<td style=\"width:20%\">" +
                "            <div class=\"input-group date\" id=\"hasta\">" +
                "                <input type=\"text\" data-format=\"MM/DD/YYYY\" class=\"form-control\" />" + 
                "                <span class=\"input-group-addon\">" + 
                "                    <span class=\"glyphicon glyphicon-calendar\"></span>" + 
                "                </span>" + 
                "            </div></td>" +
                "<td style=\"width:10%;text-align:right\"><input class=\"form-control\" id=\"cantidad\" name=\"cantidad\" type=\"text\" style=\"text-align:right\"/></td></tr>"
            $("#periodos-dieta-table tbody").append(myrow);

            $("#periodos-dieta-table tbody").find("tr").last().find("#desde").datepicker();
            $("#periodos-dieta-table tbody").find("tr").last().find("#hasta").datepicker();
            $("#periodos-dieta-table tbody").find("tr").last().find("#cantidad").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
                      
            $("#periodos-dieta-table tbody").change(function () {
                $.ajax({
                    type: "GET",
                    url: "GetBalance/" + $("#caseid").val(),
                    dataType: "json",
                    success: function (data) {
                        
                        calcular_total_dietas_pendientes();

                        var total = $("#dieta_montototal").autoNumeric("get");
                        if (total > balance) {
                            msgBox("el caso no tiene suficiente balance para registrar el pago");
                        }

                    }
                });
            });
            calcular_total_dietas_pendientes();
        });

        $("#periodos-dieta-table").on("click", ".delete-row-dieta", function () {
            $(this).closest("tr").remove();
            calcular_total_dietas_pendientes();
        });

        $("#nuevobtn").on("click", function (e) {
            limpiar_campos();
            $("#panel_entidad").hide();
            $("#panel_custodio").hide();
            $("#panelpaymentdue").hide();
            $("#panelpaymentinvest").hide();
            $("#panelpaymentipp").hide();
        });

        $("#cancelarbtn").on("click", function (e) {
            limpiar_campos();
            $("#panel_entidad").hide();
            $("#panel_custodio").hide();
            $("#ddlclasedocumento").val(0);
            $("#formadepagos").hide();
            $("#panelpaymentdue").hide();
            $("#panelpaymentinvest").hide();
            $("#panelpaymentipp").hide();
        });
                
        $("input:radio[name=pago_dirigido_entidad]").click(function () {
            $("#pago_dirigido_entidad").prop("checked", true);
            $("input:radio[name=pago_dirigido_cust]").removeAttr("checked");
            $("#CourtList").prop("disabled", false);
            $("#CourtList").val("0");
            $("#panel_custodio").hide();
        });

        $("input:radio[name=pago_dirigido_cust]").click(function () {
            $("#pago_dirigido_cust").prop("checked", true);
            $("input:radio[name=pago_dirigido_entidad]").removeAttr("checked");
            $("#CourtList").prop("disabled", "disabled");
            $("#panel_custodio").show();
            $("#panel_entidad").hide();
        });

        $("#CourtList").change(function () {
            var val = $(this).val();
            var text = $("#CourtList option:selected").text();
            $("#panel_entidad").hide();
            $("#panel_custodio").hide();

            if (val === "0") {
                return false;
            }

            if (text === "Otro") {
                $("#panel_entidad").show();
                $("#panel_custodio").hide();
            }
            else {
                var courtObject = JSON.parse(val);
                $("#nombre_entidad").val(courtObject.CourtName);
                $("#direccion_entidad").val(
                    "" + courtObject.AddressLine1 + " " +
                    "" + courtObject.AddressLine2 + " " +
                    "" + courtObject.City + " " + courtObject.Region + " " +
                    "" + courtObject.ZipCode + " " + courtObject.ZipCodeExt + " "
                    );
            }

        });

        $("#fnacimiento2").on("dp.change", function (e) {
            $("#fnacimiento").val($("#fnacimiento2").data("date"));
        });

        $("#fradicacion2").on("dp.change", function (e) {
            $("#fradicacion").val($("#fradicacion2").data("date"));
        });

        $("#ipp_fechaadjudicacion2").on("dp.change", function (e) {
            $("#ipp_fechaadjudicacion").val($("#ipp_fechaadjudicacion2").data("date"));
        });

        $("#inv_fechaemisiondecision2").on("dp.change", function (e) {
            $("#inv_fechaemisiondecision").val($("#inv_fechaemisiondecision2").data("date"));
        });

        $("#diff_fechadecision2").on("dp.change", function (e) {
            $("#diff_fechadecision").val($("#diff_fechadecision2").data("date"));
        });

        $("#dieta_fvisita2").on("dp.change", function (e) {
            $("#dieta_fvisita").val($("#dieta_fvisita2").data("date"));
        });

        $("#dieta_fnotificacion2").on("dp.change", function (e) {
            $("#dieta_fnotificacion").val($("#dieta_fnotificacion2").data("date"));
        });

        $("#dieta_fechadecision2").on("dp.change", function (e) {
            $("#dieta_fechadecision").val($("#dieta_fechadecision2").data("date"));
        });
        
        $("#due_beneficiario").change(function () {
            var val = $(this).val();
            var text = $("#due_beneficiario option:selected").text();
            $("#due_relacion").val("");

            if (val === "0") {
                $("#due_relacion").val("");
                return false;
            }

            $("#due_relacion").val($("#due_beneficiario").val().split("_")[1]);
        });

        $("#ddlclasedocumento").change(function () {
            var val = $(this).val();
            limpiar_campos();
            
            switch (val) {
                case "0":
                    $("#panelpaymentdue").hide();
                    $("#panelpaymentinvest").hide();
                    $("#panelpaymentipp").hide();
                    $("#panelpaymentdiff").hide();
                    break;
                case "1":
                    $("#panelpaymentinvest").show();
                    $("#panelpaymentipp").hide();
                    $("#panelpaymentdue").hide();
                    $("#panelpaymentdiff").hide();
                    showResumenInv(casedata.data.caseId);
                    break;
                case "2":
                    $("#panelpaymentinvest").hide();
                    $("#panelpaymentipp").show();
                    $("#panelpaymentdue").hide();
                    $("#panelpaymentdiff").hide();
                    showResumenIpp(casedata.data.caseId);
                    break;
                case "3":
                    $("#panelpaymentinvest").hide();
                    $("#panelpaymentipp").hide();
                    $("#panelpaymentdiff").hide();
                    $("#panelpaymentdue").show();
                    showResumenDue(casedata.data.caseId);
                    $.ajax({
                        type: "GET",
                        url: "GetAllRelatives/" + $("#entityid").val(),
                        dataType: "json",
                        success: function (data) {
                            $.each(data, function (i, obj) {
                                var div_data = "<option value=" + obj.Value + ">" + obj.Text + "</option>";
                                $(div_data).appendTo("#due_beneficiario");
                            });
                        }
                    });
                    break;
                case "4":
                    $("#panelpaymentinvest").hide();
                    $("#panelpaymentipp").hide();
                    $("#panelpaymentdiff").show();
                    $("#panelpaymentdue").hide();
                    break;
            }
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayPayments.start();
});