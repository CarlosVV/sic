var editor;
var myTable;
var resumentable;
var nombre;
var ssn;
var numeroCaso;
var fnacimiento;
var fradicacion;
var region;
var dispensario;
var clinics = $("#ClinicList option").clone();
var casedata;
var idresumen;
var winp;
var balance;

CDI.DisplayThirdParties = (function () {

    $("#unsolopago").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
    $("#pago1").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
    $("#pago2").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
    $("#agencia_monto_orden").autoNumeric("init", { aSep: ",", aDec: ".", aSign: "$ " });
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
    $("#panel_entidad").hide();
    $("#panel_custodio").hide();

  
    $("#agencia_numerocaso").hide();
    $("#agencia_idparticipante").hide();
    $("#agencia_monto_orden").hide();
    $("label[for=\"agencia_numerocaso\"]").hide();
    $("label[for=\"agencia_idparticipante\"]").hide();
    $("label[for=\"agencia_monto_orden\"]").hide();

    $("#fecha_terminacion2").hide();
    $("#numero_orden_terminacion").hide();
    $("label[for=\"fecha_terminacion\"]").hide();
    $("label[for=\"numero_orden_terminacion\"]").hide();

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

    $("input:radio[name=orden_terminacion]").click(function () {
        if ($("input:radio[name=orden_terminacion]:checked").val() == "1") {
            $("#fecha_terminacion2").show();
            $("#numero_orden_terminacion").show();
            $("#agencia_numerocaso").hide();
            $("#agencia_idparticipante").hide();
            $("#agencia_monto_orden").hide();
            $("label[for=\"fecha_terminacion\"]").show();
            $("label[for=\"numero_orden_terminacion\"]").show();
            $("label[for=\"agencia_numerocaso\"]").hide();
            $("label[for=\"agencia_idparticipante\"]").hide();
            $("label[for=\"agencia_monto_orden\"]").hide();
        }
        else {
            $("#fecha_terminacion2").hide();
            $("#numero_orden_terminacion").hide();
            $("#agencia_numerocaso").show();
            $("#agencia_idparticipante").show();
            $("#agencia_monto_orden").show();
            $("label[for=\"fecha_terminacion\"]").hide();
            $("label[for=\"numero_orden_terminacion\"]").hide();
            $("label[for=\"agencia_numerocaso\"]").show();
            $("label[for=\"agencia_idparticipante\"]").show();
            $("label[for=\"agencia_monto_orden\"]").show();
        }
    });
    
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

    $("#fnacimiento2, #fradicacion2, #fecha_terminacion2").datepicker();

    $("#fnacimiento2").on("dp.change", function (e) {
        $("#fnacimiento").val($("#fnacimiento2").data("date"));
    });

    $("#fradicacion2").on("dp.change", function (e) {
        $("#fradicacion").val($("#fradicacion2").data("date"));
    });

    $("#fecha_terminacion2").on("dp.change", function (e) {
        $("#fecha_terminacion").val($("#fecha_terminacion2").data("date"));
    });

    $("#ClinicList").empty();
    $("#ClinicList").append("<option value=\"0\">--Seleccione Dispensario --</option>");
    $("#RegionList").change(function () {
        var val = $(this).val();
        $("#ClinicList").empty();
        clinics.filter(function (idx, el) {
            return $(el).val() == "0" || $(el).text().indexOf("[" + val + "]") >= 0;
        }).appendTo("#ClinicList");
    });

    $("#CourtList").prop("disabled", "disabled");
    $("#CourtList").change(function () {
        var val = $(this).val();
        var text = $("#CourtList option:selected").text();
        $("#panel_entidad").hide();
        $("#panel_custodio").hide();

        if (val == "0") {
        }

        if (text == "Otro") {
            $("#panel_entidad").show();
            $("#panel_custodio").hide();
        }
        else {
            var courtObject = JSON.parse(val);
            $("#nombre_entidad_court").val(courtObject.CourtName);
            $("#direccion_entidad").val(
                "" + courtObject.AddressLine1 + " " +
                "" + courtObject.AddressLine2 + " " +
                "" + courtObject.City + " " + courtObject.Region + " " +
                "" + courtObject.ZipCode + " " + courtObject.ZipCodeExt + " "
                );
        }

    });

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
            "entityId": idresumen
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

        if (!myTable) {
            myTable = $("#CasesTable").DataTable({
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
                    "url": "Search",
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
            myTable.ajax.reload();
    }


    var showCaseDetail = function (casedata) {
        balance = casedata.data.Balance;
        $("#info_numerocaso").val(casedata.data.NumeroCaso);
        $("#info_lesionado").val(casedata.data.Lesionado);
        $("#info_ssn").val(casedata.data.SSN);
        $("#info_ebt").val(casedata.data.EBT);
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
    }

    var _start = function () {

        var selector = "document";
        try {
            selector = eval("document");
        } catch (e) { }
             
        jQuery(selector).bind("contextmenu", function (e) { e.preventDefault(); });

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
            $("#formadepagos").hide();
        });

        $("#CasesTable tbody").on("click", "tr", function () {
            var selcaso = myTable.row(this).data()
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
        
        $("#guardarbtn").on("click", function (e) {                      

            //TipoEntidad =  1: Custodio, 2: Nueva Entidad/Otro, 3: Court
            var tipo_entidad = 0;
            if ($("#pago_dirigido_cust").is(":checked")) {
                tipo_entidad = 1;
            }

            if ($("#pago_dirigido_entidad").is(":checked") && $("#CourtList").val() == "-1") {
                tipo_entidad = 2;
            }

            if ($("#pago_dirigido_entidad").is(":checked") && $("#CourtList").val() != "0" && $("#CourtList").val() != "-1") {
                tipo_entidad = 3
            }
            if (tipo_entidad == 0) {
                msgBox("Debe elegir una entidad o custodio.");
                return false;
            }
            //Validar que se seleccione una corte o selecciona otro
            if ($("#pago_dirigido_entidad").is(":checked") && $("#CourtList").val() == "0") {
                msgBox("Debe elegir una corte");
                return false;
            }

            if (tipo_entidad == 1 && $("#custodio_primer_nombre").val().trim().length == 0) {
                msgBox("Primer nombre de Custodio debe estar lleno!");
                return false;
            }

            if (tipo_entidad == 2 && $("#nombre_entidad").val().trim().length == 0) {
                msgBox("Nombre de Entidad debe estar lleno");
                return false;
            }

            if ($("#direccion_entidad_linea2").val().trim().length > 0 && $("#direccion_entidad_linea1").val().trim().length == 0) {
                msgBox("Linea 1 en Dirección de Entidad debe estar lleno si Linea2 esta lleno");
                return false;
            }

            if ($("#custodio_segundo_nombre").val().trim().length > 0 && $("#custodio_primer_nombre").val().trim().length == 0) {
                msgBox("Primer nombre de Custodio debe estar lleno si Segundo Nombre esta lleno");
                return false;
            }

            if ($("#custodio_segundo_apellido").val().trim().length > 0 && $("#custodio_primer_apellido").val().trim().length == 0) {
                msgBox("Primer apellido de Custodio debe estar lleno si Segundo apellido esta lleno");
                return false;
            }

            if ($("#custodio_linea2").val().trim().length > 0 && $("#custodio_linea1").val().trim().length == 0) {
                msgBox("Linea 1 en Dirección de Custodio debe estar lleno si Linea2 esta lleno");
                return false;
            }

            if ($("input:radio[name=tipo_pago]:checked").val() == "1" && $("#unsolopago").autoNumeric("get") == 0) {
                msgBox("Debe ingresar un valor para Un Solo pago");
                return false;
            }
            if ($("input:radio[name=tipo_pago]:checked").val() == "2" && ($("#pago1").autoNumeric("get") + $("#pago2").autoNumeric("get") == 0)) {
                msgBox("Debe ingresar un Pago 1 o un Pago 2");
                return false;
            }            

            var ordenTerminacion = false;
            
            if ($("input:radio[name=orden_terminacion]:checked").val() == "0") {
                ordenTerminacion = true;
                var datosParticipanteVacios = false;
                
                if($("#agencia_numerocaso").val().trim().length +
                    $("#agencia_idparticipante").val().trim().length +
                    $("#agencia_monto_orden").val().trim().length === 0) {
                    msgBox("Si no hay orden de terminación, debe llenarse participante y monto de orden");
                    return false;
                }

                
                if ($("#pago1").autoNumeric("get") + $("#pago2").autoNumeric("get") > $("#agencia_monto_orden").autoNumeric("get")) {
                    msgBox("Si no hay orden de terminación, pago1+pago2 < monto según orden");
                    return false;
                }

                if ($("#unsolopago").autoNumeric("get") > $("#agencia_monto_orden").autoNumeric("get")) {
                    msgBox("Si no hay orden de terminación, un solo pago < monto según orden");
                    return false;
                }
            }
            else {
                ordenTerminacion = false;
                if($("#fecha_terminacion2").val().trim().length +
                $("#numero_orden_terminacion").val().trim().length == 0) {
                    msgBox("Si el caso tiene orden de terminación debe llenarse la fecha y número de orden de terminación.");
                    return false;
                }
            }                      

            var entidad = {};
            var court = 0;
            if ($("#pago_dirigido_entidad").is(":checked") && $("#CourtList").val() != "0" && $("#CourtList").val() != "-1") {
                var entidad = JSON.parse($("#CourtList").val());
                court = entidad.CourtId;
            }

            var total = $("#pago1").autoNumeric("get") + $("#pago2").autoNumeric("get") + $("#unsolopago").autoNumeric("get");
            if (total > balance) {
                msgBox("El caso no tiene suficiente balance para registrar el pago");
                return false;
            }

            var data = {
                "ben": casedata.data.entityId,
                "cla_num": ordenTerminacion ? $("#agencia_idparticipante").val() : null,
                "ord": ordenTerminacion?$("#agencia_numerocaso").val():null,
                "flag": $("input:radio[name=orden_terminacion]:checked").val() == "1" ? true : false,
                "efec": !ordenTerminacion ?$("#fecha_terminacion").val():null,
                "term_ord_num": $("#numero_orden_terminacion").val(),
                "unsolopago": $("#unsolopago").autoNumeric("get"),
                "pago1": $("#pago1").autoNumeric("get"),
                "pago2": $("#pago2").autoNumeric("get"),
                "comment": $("#observaciones").val(),
                "tipo_entidad": tipo_entidad,
                "court": court,
                "nombre_entidad": $("#nombre_entidad").val(),
                "direccion_entidad_linea1": $("#direccion_entidad_linea1").val(),
                "direccion_entidad_linea2": $("#direccion_entidad_linea2").val(),
                "direccion_entidad_ciudad": $("#direccion_entidad_ciudad").val(),
                "direccion_entidad_codigopostal": $("#direccion_entidad_codigopostal").val(),
                "custodio_primer_nombre": $("#custodio_primer_nombre").val(),
                "custodio_segundo_nombre": $("#custodio_segundo_nombre").val(),
                "custodio_primer_apellido": $("#custodio_primer_apellido").val(),
                "custodio_segundo_apellido": $("#custodio_segundo_apellido").val(),
                "custodio_linea1": $("#custodio_linea1").val(),
                "custodio_linea2": $("#custodio_linea2").val(),
                "custodio_ciudad": $("#custodio_ciudad").val(),
                "custodio_codigopostal": $("#custodio_codigopostal").val(),
                "agencia_monto_orden": ordenTerminacion ? $("#agencia_monto_orden").autoNumeric("get") : null,
                "fecha_terminacion": !ordenTerminacion ? $("#fecha_terminacion").val() : null,
                "caseId": casedata.data.caseId,
            };

            showProgress("Grabando...");

            $.ajax({
                url: "InsertThirdPartyPayment",
                data: data,
                dataType: "json",
                type: "POST",
                success: function (data) {
                    closeProgress();
                    if (data.data.Status == "OK") {
                        msgBox("Datos guardados");
                        $("#formPagoTerceros").trigger("reset");
                        $("option").attr("selected", false);
                        $("select").val(0);
                        $("#formadepagos").hide();
                    } else {
                        closeProgress();
                        msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                    }


                },
                error: function () {
                    closeProgress();
                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                }
            });
        });

        $("#nuevobtn").on("click", function (e) {
            $("#formPagoTerceros").trigger("reset");
            $("option").attr("selected", false);
            $("select").val(0);
            $("#formadepagos").hide();
        });

        $("#cancelarbtn").on("click", function (e) {
            $("#formPagoTerceros").trigger("reset");         
            $("option").attr("selected", false);
            $("select").val(0);
            $("#formadepagos").hide();
        });
    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayThirdParties.start();
});