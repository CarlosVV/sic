CDI.CaseSearch = (function () {
    var searchRules = {
        CaseNumber: {
            require_from_group: [1, ".search"]
        },
        EntityName: {
            require_from_group: [1, ".search"]
        },
        SocialSecurityNumber: {
            require_from_group: [1, ".search"]
        },
        BirthDate: {
            require_from_group: [1, ".search"],
            dateUS: true
        },
        FilingDate: {
            require_from_group: [1, ".search"],
            dateUS: true
        },
        RegionId: {
            require_from_group: [1, ".search"]
        },
        ClinicId: {
            require_from_group: [1, ".search"]
        },
        EBTNumber: {
            require_from_group: [1, ".search"]
        }
    };
    var tableCases;

    var clinics = $("#ClinicId option").clone();

    var clearFields = function () {
        $("#EntityName").val("");
        $("#SocialSecurityNumber").val("");
        $("#CaseNumber").val("");
        $("#RegionId").val("");
        $("#ClinicId").val("");
        $("#EBTNumber").val("");
    };

    var initCaseTable = function () {
        tableCases = $("#tblCases").DataTable({
            "ordering": true,
            "autoWidth": false,
            "pagingType": "simple_numbers",
            "processing": true,           
            "deferRender": true,
            "serverSide": true,
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
                "lengthMenu": "Mostrar _MENU_"
            },
            "ajax": {
                url: root + "case/search",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                data: function (data) {
                    data.EntityName = $("#EntityName").val();
                    data.SocialSecurityNumber = $("#SocialSecurityNumber").val();
                    data.BirthDate = $("#BirthDate").val();
                    data.CaseNumber = $("#CaseNumber").val();
                    data.FilingDate = $("#FilingDate").val();
                    data.RegionId = $("#RegionId").val();
                    data.ClinicId = $("#ClinicId").val().split("-")[0];
                    data.EBTNumber = $("#EBTNumber").val();

                    return JSON.stringify(data);
                }
            },
            "order": [
                [1, "desc"]
            ],
            "columns": [
                {
                    "data": "NumeroCasoMostrado"
                },
                {
                    "data": "Lesionado"
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
                    "data": "Beneficiario.Relacion"
                },
                {
                    "data": "Region"
                },
                {
                    "data": "Dispensario"
                },
                {
                    "data": "EBT"
                }
            ],
            "dom": "rtp",
            "rowCallback": function (nRow) {
                $(nRow).addClass("clickable");
            },
            "deferLoading": 0
        });
    }

    var clearTable = function () {
        if ($.fn.dataTable.isDataTable("#tblCases")) {
            tableCases.destroy();
        }
        $("#tblCases tbody").find("tr").remove();

        initCaseTable();
    };

    var bindUiActions = function () {

        $("#RegionId").change(function () {
            var val = $(this).val();

            $("#ClinicId").empty();

            clinics.filter(function (index, element) {
                return $(element).val() === "" || $(element).val().indexOf("[" + val + "]") >= 0;
            }).appendTo("#ClinicId");
        });

        $("#btnSearch").on("click", function (evt) {
            evt.preventDefault();

            CDI.addValidationRules(searchRules);

            var isValid = $("#frmCaseSearch").valid();
            if (isValid) {
                $(CDI.CaseSearch).trigger("started");

                tableCases.ajax.reload();
            }

            CDI.removeValidationRules(searchRules);

             
        });

        $("#btnClear").on("click", function () {
            clearFields();

            $(CDI.CaseSearch).trigger("cleaned");

            clearTable();
        });

        $("#tblCases tbody").on("click", "tr", function () {
            var $this = $(this);
            var caso = tableCases.row(this).data();

            $.ajax({
                url: root + "case/getcasedetailbyid/" + caso.CaseDetailId,
                datatype: "text",
                type: "GET",
                cache: false,
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) {
                $this.siblings().removeClass("selected");
                $this.addClass("selected");

                $(CDI.CaseSearch).trigger("case.selected", [{ selectedCase: response.data }]);

                CDI.hideWaitingMessage();
            }).fail(function () {
                CDI.hideWaitingMessage();
            });
        });
    };

    var init = function () {
        $("#frmCaseSearch").validate();

        $("#BirthDate, #FilingDate").datepicker({
            startDate: "-100y",
            endDate: "-1d"
        });

        $("#ClinicId").find("option:gt(0)").remove();

        $("#SocialSecurityNumber").inputmask({ mask: "999-99-9999" });

        $("#EBTNumber").inputmask({ mask: "999999999" });

        $("#CaseNumber").inputmask({ mask: "99999999999[ 99]" });

        initCaseTable();

        bindUiActions();
    };

    var showHeaderCase = function (caseData) {
        $("#info_balance_ebt").autoNumeric("init", {
            vMin: "0"
        });

        $("#info_benef_balance_ebt").autoNumeric("init", {
            vMin: "0"
        });

        $("#info_menor").prop("checked", false);
        $("#info_muerte").prop("checked", false);

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
        $("#direccion_zipcode").text(caseData.Direccion.CodigoPostal);
        $("#info_region").text(caseData.Region);
        $("#info_dispensario").text(caseData.Dispensario);
        $("#info_ebt").text(caseData.EBT);
        $("#info_balance").val(caseData.BalanceFormateado);
        $("#info_estatus_pago").text(caseData.PaymentStatus);
        $("#info_fecha_suspension").text(caseData.FechaSuspension);
        $("#info_razon_suspension").text(caseData.RazonSuspension);
        $("#info_fecha_reanudacion").text(caseData.FechaReanudacion);

        if (caseData.EBTBalance > 0 || caseData.EBTBalance < 0)
            $("#info_balance_ebt").autoNumeric("set", caseData.EBTBalance);

        $("#info_estatus_ebt").text(caseData.EBTStatus);
        $("#info_tipoinca").text(caseData.TipoIncapacidad);

        $("#info_patrono").text(caseData.Patrono.Nombre);
        $("#info_ein").text(caseData.Patrono.EIN);
        $("#info_estatus_patronal").text(caseData.Patrono.Estatus);
        $("#info_poliza").text(caseData.Patrono.NumeroPoliza);

        if (caseData.CasoMenor) {
            $("#info_menor").prop("checked", true);
        }

        if (caseData.CasoMuerte) {
            $("#info_muerte").prop("checked", true);
        }

        if (caseData.EsBeneficiario) {
            $("#info_benef_relacion").text(caseData.Beneficiario.Relacion);
            $("#info_benef_nombre").text(caseData.Beneficiario.Nombre);
            $("#info_benef_ssn").text(caseData.Beneficiario.SSN);
            $("#info_benef_fnac").text(caseData.Beneficiario.FechaNacimiento);
            $("#info_benef_ebt").text(caseData.Beneficiario.EBT);

            if (caseData.Beneficiario.EBTBalance > 0 || caseData.Beneficiario.EBTBalance < 0)
                $("#info_benef_balance_ebt").autoNumeric("set", caseData.Beneficiario.EBTBalance);

            $("#info_benef_estatus_ebt").text(caseData.Beneficiario.EBTStatus);
            $("#info_benef_sufijo").text(caseData.Beneficiario.Sufijo);
            $("#info_beneficiario_balance").val(caseData.Beneficiario.BalanceFormateado);
            $("#info_beneficiario_estatus_pago").text(caseData.Beneficiario.PaymentStatus);
            $("#info_beneficiario_fecha_suspension").text(caseData.Beneficiario.FechaSuspension);
            $("#info_beneficiario_razon_suspension").text(caseData.Beneficiario.RazonSuspension);
            $("#info_beneficiario_fecha_reanudacion").text(caseData.Beneficiario.FechaReanudacion);

            $("#case-beneficiary-info").fadeIn(1000).removeClass("hidden");
        }
        else {
            $("#info_benef_relacion").text("");
            $("#info_benef_nombre").text("");
            $("#info_benef_ssn").text("");
            $("#info_benef_fnac").text("");
            $("#info_benef_ebt").text("");
            $("#info_benef_balance_ebt").text("");
            $("#info_benef_estatus_ebt").text("");
            $("#info_benef_sufijo").text("");
            $("#info_beneficiario_balance").val("");
            $("#info_beneficiario_estatus_pago").text("");
            $("#info_beneficiario_fecha_suspension").text("");
            $("#info_beneficiario_razon_suspension").text("");
            $("#info_beneficiario_fecha_reanudacion").text("");

            $("#case-beneficiary-info").fadeOut(1000).addClass("hidden");
        }

        $("#header-panel").fadeIn(1000).removeClass("hidden");
    };

    return {
        init: init,
        header: showHeaderCase
    };
})();