CDI.DisplayAprobacion = (function () {
    var tablaCasosInv;
    var tablaCasosPerent;
    var tablaCasosIpp;
    var tablaCasosThird;
    var tablaCasosDieta;
    var clinics = $("#ClinicList option").clone();
    var clasedocumento;
    var winmodal;
    var winp;
    var caseData;

    var cargaclinicas = function () {
        $("#ClinicList").empty();
        $("#ClinicList").append("<option value=\"0\">--Seleccione Dispensario --</option>");
        $("#RegionList").change(function () {
            var val = $(this).val();
            $("#ClinicList").empty();
            clinics.filter(function (idx, el) {
                return $(el).val() === "0" || $(el).text().indexOf("[" + val + "]") >= 0;
            }).appendTo("#ClinicList");
        });
    }

    var inicializarCalendarios = function () {
        $("#fnacimiento2").on("dp.change", function (e) {
            $("#fnacimiento").val($("#fnacimiento2").data("date"));
        });

        $("#fradicacion2").on("dp.change", function (e) {
            $("#fradicacion").val($("#fradicacion2").data("date"));
        });

        $("#fdesde2").on("dp.change", function (e) {
            $("#fdesde").val($("#fdesde2").data("date"));
        });

        $("#fhasta2").on("dp.change", function (e) {
            $("#fhasta").val($("#fhasta2").data("date"));
        });
    }

    var inicializarControles = function () {
        $("#fnacimiento2").datepicker();
        $("#fradicacion2").datepicker();
        $("#fdesde2").datepicker();
        $("#fhasta2").datepicker();
        $("#unsolopago").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#pago1").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#pago2").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#unsolopago").autoNumeric("set", 0);
        $("#pago1").autoNumeric("set", 0);
        $("#pago2").autoNumeric("set", 0);
        $("#due_cantidadsolicitada").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#ipp_adjudicacionadicional").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#ipp_mensualidad").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
        $("#ipp_semanas").autoNumeric("init", { vMin: "0", vMax: "999999999", mDec: "0" });

        cargaclinicas();
        inicializarCalendarios();       

        $("input:radio[name=tipo_pago]").click(function () {
            if ($("input:radio[name=tipo_pago]:checked").val() == "1") {
                $("#pago1").autoNumeric("set", 0);
                $("#pago2").autoNumeric("set", 0);
                $("#pago1").prop("readonly", true);
                $("#pago2").prop("readonly", true);
                $("#unsolopago").prop("readonly", false);
            }
            if ($("input:radio[name=tipo_pago]:checked").val() == "2") {
                $("#unsolopago").autoNumeric("set", 0);
                $("#unsolopago").prop("readonly", true);
                $("#pago1").prop("readonly", false);
                $("#pago2").prop("readonly", false);
            }

        });

    }
    
    var showDetallePagosInversiones = function (d) {
        var detalle = "<table id=\"CasesTable\" class=\"table table-striped table-bordered table-hover\">" +
            "<thead><tr class=\"active\">" +
            "<th>Pago dirigido a</th><>" +
            "<th>Importe</th></tr></thead><tbody>";
        var count = d.Pagos.length;
        if (count > 0) {

            for (var i = 0; i < count; i++) {
                detalle = detalle + "<tr>" +
                  "<td>" + d.Pagos[i].PagoDirigidoA + "</td>" +
                  "<td>" + d.Pagos[i].Importe + "</td>" +
                  "</tr>";
            }
        }

        detalle = detalle + "</tbody></table>";
        return detalle;
    }

    var data = function () {
        return {
            "numerocaso": $("#caso").val(),
            "nombrelesionado": $("#name").val(),
            "numerosegurosocial": $("#ssn").val(),
            "fnacimiento": $("#fnacimiento").val(),
            "fradicacion": $("#fradicacion").val(),
            "region": $("#RegionList").val(),
            "dispensario": $("#ClinicList").val(),
            "fdesde": $("#fdesde").val(),
            "fhasta": $("#fhasta").val(),
            "clase": $("#ddlclasedocumento").val(),
            "estado": $("#ddlestado").val()
        };
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

        if ($("#fdesde").val() != "") {
            isValidate = true;
        }
        if ($("#fhasta").val() != "") {
            isValidate = true;
        }

        if ($("#ddlestado").val() != "0") {
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

    var loadGrillaEditarInversiones = function () {
        if ($("#resultsPanelInvest").css("display") == "none")
            $("#resultsPanelInvest").show();

        if (!tablaCasosInv) {
            tablaCasosInv = $("#CasesTableInvest").DataTable({
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
                    "url": "BuscarCasosEditar",
                    "type": "POST",
                    "data": data
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [                   
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                     {
                         "className": "details-control",
                         "orderable": false,
                         "data": null,
                         "defaultContent": ""
                     },
                    {
                        "data": "NumeroCaso"
                    },                    
                    {
                        "data": "Lesionado"
                    },
                    {
                        "data": "NumeroSeguroSocial"
                    },
                    {
                        "data": "FechaEmisionDecision"
                    },
                    {
                        "data": "TotalInversion"
                    },
                    {
                        "data": "RazonInversion"
                    },
                    {
                        "data": "Estado"
                    }
                    ,
                    {
                        "orderable": false,
                        "data": "TransaccionId",
                        "render": function (data, type, row, meta) {
                            return "<button class='editar' data-tx='" + data + "' data-caseid='" + row.CaseId + "'>Editar</button>";
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tablaCasosInv.ajax.reload();
    }
    
    var loadGrillaEditarPerent = function () {
        if ($("#resultsPanelPerent").css("display") == "none")
            $("#resultsPanelPerent").show();

        if (!tablaCasosPerent) {
            tablaCasosPerent = $("#CasesTablePerent").DataTable({
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
                    "url": "BuscarCasosEditar",
                    "type": "POST",
                    "data": data
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "PaymentId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "NumeroCaso"
                    },
                    {
                        "data": "Lesionado"
                    },
                    {
                        "data": "NumeroSeguroSocial"
                    },
                    {
                        "data": "FechaDecision"
                    },
                    {
                        "data": "Fecha"
                    },
                    {
                        "data": "Beneficiario"
                    },
                    {
                        "data": "Relacion"
                    },
                    {
                         "data": "Cantidad"
                    },
                    {
                        "data": "Estado"
                    },
                    {
                        "data": "EstadoOtros"
                    },
                    {
                        "orderable": false,
                        "data": "TransaccionId",
                        "render": function (data, type, row, meta) {
                            return "<button class='editar' data-tx='" + data + "' data-caseid='" +
                                row.CaseId + "' data-paymentid='" + row.PaymentId + "'>Editar</button>";
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tablaCasosPerent.ajax.reload();
    }
    var showDialogMessage = function (message) {
        toastr.warning(message, "SiC");
    };
    var loadGrillaEditarIpp = function () {
        if ($("#resultsPanelIpp").css("display") == "none")
            $("#resultsPanelIpp").show();

        if (!tablaCasosIpp) {
            tablaCasosIpp = $("#CasesTableIpp").DataTable({
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
                    "url": "BuscarCasosEditar",
                    "type": "POST",
                    "data": data
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                     {
                         "data": "PaymentId",
                         "orderable": false,
                         "visible": false
                     },
                     {
                         "data": "CaseId",
                         "orderable": false,
                         "visible": false
                     },
                    {
                        "data": "NumeroCaso"
                    },
                   
                    {
                        "data": "Lesionado"
                    },
                    {
                        "data": "NumeroSeguroSocial"
                    },
                    {
                        "data": "FechaAdjudicacion"
                    },
                    {
                        "data": "AdjudicacionAdicional"
                    },
                    {
                        "data": "PagoInicial"
                    },
                    {
                        "data": "Mensualidad"
                    },
                    {
                        "data": "Semanas"
                    },
                    {
                        "data": "Estado"
                    }
                    ,
                    {
                        "orderable": false,
                        "data": "TransaccionId",
                        "render": function (data, type, row, meta) {
                            return "<button class='editar' data-tx='" + data + "' data-caseid='" +
                                row.CaseId + "' data-paymentid='" + row.PaymentId + "'>Editar</button>";
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tablaCasosIpp.ajax.reload();
    }

    var loadGrillaEditarThird = function () {
        if ($("#resultsPanelThird").css("display") == "none")
            $("#resultsPanelThird").show();

        if (!tablaCasosThird) {
            tablaCasosThird = $("#CasesTableThird").DataTable({
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
                    "url": "BuscarPagoTercerosEditar",
                    "type": "POST",
                    "data": data
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                     {
                         "data": "CaseId",
                         "orderable": false,
                         "visible": false
                     },
                     {
                          "data": "ThirdPartyScheduleId",
                          "orderable": false,
                          "visible": false
                     },
                    {
                        "data": "NumeroCaso"
                    },                   
                    {
                        "data": "Lesionado"
                    },
                    {
                        "data": "NumeroSeguroSocial"
                    },
                    {
                        "data": "FechaDecision"
                    }
                    ,
                    {
                        "data": "UnSoloPago",
                        "render": function (data, type, row, meta) {
                            return "<span class='text-right'>$ " + data + "</span>";
                        }
                    }, 
                    {
                        "data": "Pago1",
                        "render": function (data, type, row, meta) {
                            return "<span class='text-right'>$ " + data + "</span>";
                        }
                    },                    
                    {
                        "data": "Pago2",
                        "render": function (data, type, row, meta) {
                            return "<span class='text-right'>$ " + data + "</span>";
                        }
                    }
                    ,
                    {
                        "data": "Estado"
                    }
                    ,
                    {
                        "orderable": false,
                        "data": null,
                        "render": function (data, type, row, meta) {
                            return "<button class='editar' data-thirdpartyscheduleid='" + row.ThirdPartyScheduleId + "'>Editar</button>";
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tablaCasosThird.ajax.reload();
    }

    var loadGrillaEditarDieta = function () {
        if ($("#resultsPanelDieta").css("display") == "none")
            $("#resultsPanelDieta").show();

        if (!tablaCasosIpp) {
            tablaCasosDieta = $("#CasesTableDieta").DataTable({
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
                    "url": "BuscarPagoDietasEditar",
                    "type": "POST",
                    "data": data
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [                   
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "NumeroCaso"
                    },                    
                    {
                        "data": "Lesionado"
                    },
                    {
                        "data": "NumeroSeguroSocial"
                    },
                    {
                        "data": "FechaDecision"
                    },
                    {
                        "data": "Estado"
                    }
                    ,
                    {
                        "orderable": false,
                        "data": null,
                        "render": function (data, type, row, meta) {
                            return "<button class='editar' data-caseid='" + row.CaseId + "'>Editar</button>";
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tablaCasosDieta.ajax.reload();
    }

    var showResults = function () {

        if (!validateSearch()) {
            msgBox("Debe ingresar o seleccionar un valor de busqueda");
            return;
        }

        if ($("#ddlclasedocumento").val() == "0") {
            msgBox("Debe seleccionar una clase de documento");
            return;
        }

        clasedocumento = $("#ddlclasedocumento").val()
        $("#panel-aprov-ipp").hide();
        $("#panel-aprov-invest").hide();
        $("#panel-aprov-third").hide();
        $("#panel-aprov-dieta").hide();

        switch (clasedocumento) {
            case "2":
                if ($("#panel-aprov-ipp").css("display") == "none")
                    $("#panel-aprov-ipp").show();

                loadGrillaEditarIpp();
                break;
            case "3":
                if ($("#panel-aprov-invest").css("display") == "none")
                    $("#panel-aprov-invest").show();

                loadGrillaEditarInversiones();
                break;
            case "4":
                if ($("#panel-aprov-perent").css("display") == "none")
                    $("#panel-aprov-perent").show();

                loadGrillaEditarPerent();
                break;
            case "5000":
                if ($("#panel-aprov-third").css("display") == "none")
                    $("#panel-aprov-third").show();

                loadGrillaEditarThird();
                break;
            case "6000":
                if ($("#panel-aprov-dieta").css("display") == "none")
                    $("#panel-aprov-dieta").show();

                loadGrillaEditarDieta();
                break;

        }
    }
   
    var start = function () {

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
            $("#fnacimiento").val("");
            $("#fradicacion").val("");
            $("#fnacimiento2").data("DatePicker").date(null);
            $("#fradicacion2").data("DatePicker").date(null);
            $("#fdesde").val("");
            $("#fhasta").val("");
            $("#fdesde2").data("DatePicker").date(null);
            $("#fhasta2").data("DatePicker").date(null);
            $("#ddlclasedocumento").val("0");
            $("#ddlestado").val("0");
        });
            
        $("#CasesTableInvest tbody").on("click", "td.details-control", function () {
            var tr = $(this).closest("tr");
            var row = tablaCasosInv.row(tr);

            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass("shown");
            }
            else {
                row.child(showDetallePagosInversiones(row.data())).show();
                tr.addClass("shown");
            }
        });

        $("#CasesTablePerent tbody").on("click", "button.editar", function () {
            var paymentid = $(this).data("paymentid");
            var transactionid = $(this).data("tx");
            $("#btnActualizarPerent").on("click", function () {
                showProgress("Actualizando...");
                var data = {
                    paymentid: $("#paymentid").val(),
                    transactionid: $("#transactionid").val(),
                    cantidad: $("#due_cantidadsolicitada").autoNumeric("get"),
                    observaciones: $("#observaciones").val()
                };

                $.ajax({
                    url: "/SIC/PaymentMethod/UpdatePayment",
                    data: data,
                    dataType: "json",
                    type: "POST",
                    success: function (data) {
                        closeProgress();
                        if (data.data.Status == "OK") {
                            if (winmodal) winmodal.modal("hide");
                            loadGrillaEditarPerent();
                        }
                        else {
                            msgBox("Error al actualizar.");
                        }

                    },
                    error: function () {
                        closeProgress();
                        msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                    }
                });
            });

            $.ajax({
                url: "/SIC/PaymentMethod/GetPayment/" + paymentid,
                dataType: "json",
                type: "GET",
                success: function (data) {

                    $("#paymentid").val(data.PaymentId);
                    $("#transactionid").val(transactionid);
                    $("#due_cantidadsolicitada").autoNumeric("set", data.Cantidad);             
                    $("#observaciones").val(data.Observaciones);

                    winmodal = $("#modalFormPerent").modal("show");
                },
                error: function () {
                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                }
            });
        });

        $("#CasesTableIpp tbody").on("click", "button.editar", function () {
            e.preventDefault();
            var paymentid = $(this).data("paymentid");
            var transactionid = $(this).data("tx");

            $("#btnActualizarIpp").on("click", function () {
                e.preventDefault();
                
                var isValid = true;
                if ($("#tblDesglosarIpp > tbody > tr").not(".hidden").length === 0) {
                    validationmessage = validationmessage + "<br />" + "Debe ingresar al menos un desglose";
                    isValid = false;
                }
                if (!isValid) {
                    showDialogMessage(validationmessage);
                    return false;
                }
                
                var model = {
                    PaymentId: 0,
                    TransactionId: 0,
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

                model.PaymentId = $("#paymentid").val();
                model.TransactionId = $("#transactionid").val();
                //model.CaseId = _caseData.CaseId;
                //model.CaseDetailId = _caseData.CaseDetailId;
                //model.EntityId = _caseData.EntityId;
                //model.CaseKey = _caseData.CaseKey;
                //model.CaseNumber = _caseData.NumeroCaso;
                model.CantidadAdjudicada = parseFloat($("#ipp_adjudicacionadicional").autoNumeric("get"));
                model.Mensualidad = parseFloat($("#ipp_mensualidad").autoNumeric("get"));
                model.Semanas = parseFloat($("#ipp_semanas").autoNumeric("get"));
                model.Comments = $("#ipp_observaciones").val();
                model.FechaAdjudicacion = $("#ipp_fechaadjudicacion").val();
                //model.CompSemanalInca = parseFloat(_caseData.CompSemanalInca);

                model.Desgloses = CDI.DesgloseIPP.getDesglose();                
                
                $.ajax({
                    type: "POST",
                    url: "/SIC/PaymentMethod/UpdatePayment",
                    data: JSON.stringify({ model: model }),
                    dataType: "json",                    
                    beforeSend: function () {
                        CDI.showWaitingMessage();
                    },
                    success: function (result) {
                        CDI.hideWaitingMessage();
                        if (result.data.Status == "OK") {                          
                            loadGrillaEditarIpp();
                        }
                        else {
                            showDialogMessage("Error al actualizar.");
                        }
                    },
                    error: function () {
                        CDI.hideWaitingMessage();
                        showDialogMessage("Ha ocurrido un error. Consulte con el Administrador.");
                    }
                });
            });

            $.ajax({
                url: "/SIC/PaymentMethod/GetPayment/" + paymentid,
                dataType: "json",
                type: "GET",
                success: function (data) {

                    $("#paymentid").val(data.PaymentId);
                    $("#transactionid").val(transactionid);
                    $("#ipp_adjudicacionadicional").autoNumeric("set", data.AdjudicacionAdicional);
                    $("#ipp_mensualidad").autoNumeric("set", data.Mensualidad);
                    $("#ipp_semanas").autoNumeric("set", data.NumberOfWeeks);
                    $("#observaciones").val(data.Observaciones);

                    winmodal = $("#modalFormPerent").modal("show");
                },
                error: function () {
                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                }
            });
        });

        $("#CasesTableThird tbody").on("click", "button.editar", function () {
            var thirdpartyscheduleid = $(this).data("thirdpartyscheduleid");
            $("#btnActualizar").on("click", function () {
                showProgress("Actualizando...");
                var data = {
                    thirdpartyscheduleid: $("#thirdpartyscheduleid").val(),
                    unsolopago: $("#unsolopago").autoNumeric("get"),
                    pago1: $("#pago1").autoNumeric("get"),
                    pago2: $("#pago2").autoNumeric("get"),
                    observaciones: $("#observaciones").val()
                };
              
                $.ajax({
                    url: "/SIC/PaymentMethod/UpdatePagoTerceros",
                    data: data,
                    dataType: "json",
                    type: "POST",
                    success: function (data) {
                        closeProgress();
                        if (data.data.Status == "OK") {
                            if (winmodal) winmodal.modal("hide");
                            loadGrillaEditarThird();
                        }
                        else {
                            msgBox("Error al actualizar.");
                        }
                       
                    },
                    error: function () {
                        closeProgress();
                        msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                    }
                });
            });

            $.ajax({
                url: "/SIC/PaymentMethod/GetPagoTercerosEditar/" + thirdpartyscheduleid,                
                dataType: "json",
                type: "GET",
                success: function (data) {
             
                    $("#thirdpartyscheduleid").val(data.ThirdPartyScheduleId);
                    $("#unsolopago").autoNumeric("set", data.UnSoloPago);
                    $("#pago1").autoNumeric("set", data.Pago1);
                    $("#pago2").autoNumeric("set", data.Pago2);
                    $("#observaciones").val(data.Observaciones);

                    winmodal = $("#modalFormTerceros").modal("show");
                },
                error: function () {                  
                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                }
            });            
            
        });

        $("#CasesTableInvest tbody").on("click", "button.editar", function () {
            winmodal = $("#modal-investments").modal("show");
        });
        $("#CasesTableDieta tbody").on("click", "button.editar", function () {
            winmodal = $("#modalFormDietas").modal("show");
        });


    };

    return {
        start: start
    }
})();

$(function () {
    CDI.DisplayAprobacion.start();
});