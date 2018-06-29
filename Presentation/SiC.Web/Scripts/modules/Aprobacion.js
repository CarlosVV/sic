CDI.ApprovalIndex = (function () {
    var ssn_format = [/^(\d{3})\s?\-?\s?(\d{2})\s?\-?\s?(\d{4})$/, '$1-$2-$3'];
    var tableInvestments;
    var tablePeremptories;
    var tableIpps;
    var tablePostMortemItp;
    var tableThirds;
    var tablePendingDiets;
    var tableLawyers;
    var tableItps;
    var tableMuerte;
    var modal;
    var modalReject;
    var allClinics = $("#Clinics option").clone();

    var startDateDecision = new Date();
    var startDateVisita = new Date();
    var startDateNotificacion = new Date();

    var caseData;

    var loadClinics = function () {
        $("#Clinics").empty();
        $("#Clinics").append("<option value=\"\">-- Seleccionar --</option>");
        $("#Regions").change(function () {
            var val = $(this).val();
            $("#Clinics").empty();
            allClinics.filter(function (idx, el) {
                return $(el).val() === "" || $(el).val().indexOf("[" + val + "]") >= 0;
            }).appendTo("#Clinics");
        });
    }

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
        Regions: {
            require_from_group: [1, ".search"]
        },
        Clinics: {
            require_from_group: [1, ".search"]
        },
        FromDate: {
            require_from_group: [1, ".search"],
            dateUS: true
        },
        ToDate: {
            require_from_group: [1, ".search"],
            dateUS: true
        },
        ddlDocumentType: {
            required: true
        },
        ddlStatus: {
            required: true
        }
    };

    var initControls = function () {
        $("#inv_totalinversiones").autoNumeric("init");

        $("#BirthDate, #FilingDate, #lawyer_fnotificacion, #lawyer_fvisita, #lawyer_fechadecision").datepicker({
            startDate: "-100y",
            endDate: "-1d"
        });
        $("#FromDate, #ToDate").datepicker();

        $("#dieta_fechadecision").datepicker({
            startDate: "-50y",
            endDate: "-1d"
        }).on("changeDate", function (selected) {
            startDateDecision = new Date(selected.date.valueOf());
            startDateDecision.setDate(startDateDecision.getDate(new Date(selected.date.valueOf())) - 1);
            $("#dieta_fvisita").datepicker("setEndDate", startDateDecision);
            $("#dieta_fvisita").datepicker("setStartDate", "-50y");
            $("#dieta_fnotificacion").datepicker("setEndDate", startDateDecision);

            $("#tblEditPendingDietPeriods tbody > tr").find("#desde").each(function (index, element) {
                $(element).datepicker("setEndDate", startDateDecision);
            });
            $("#tblEditPendingDietPeriods tbody > tr").find("#hasta").each(function (index, element) {
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
        //$("#dieta_numerocaso").inputmask({ mask: "99999999999[ 99]" });

        $("#ipp_adjudicacionadicional").autoNumeric("init", {
            vMin: "0"
        });
        $("#ipp_mensualidad").autoNumeric("init");
        $("#ipp_semanas").autoNumeric("init", {
            aSign: ""
        });
        $("#ipp_fechaadjudicacion").datepicker({
            startDate: "-100y",
            endDate: "-1d"
        });

        $("#due_cantidadsolicitada").autoNumeric("init", {
            vMin: "0"
        });

        $("#SocialSecurityNumber").inputmask({ mask: "999-99-9999" });
        $("#CaseNumber").inputmask({ mask: "99999999999[ 99]" });
        $("#EBTNumber").inputmask({ mask: "999999999" });

        $("#lawyer_montototal").autoNumeric("init");

        loadClinics();
    }



    var showPaymentInvestmentDetails = function (d) {
        var detalle = "<table id=\"CasesTable\" class=\"table table-striped table-bordered table-hover\">" +
            "<thead><tr class=\"active\">" +
            "<th>Pago dirigido a</th>" +
            "<th>Importe</th></tr></thead><tbody>";
        for (var i = 0; i < d.Pagos.length; i++) {
            detalle = detalle + "<tr>" +
                                "<td>" + d.Pagos[i].PagoDirigidoA + "</td>" +
                                "<td class=\"text-right\">" + d.Pagos[i].Importe + "</td>" +
                                "</tr>";
        }

        detalle = detalle + "</tbody></table>";
        return detalle;
    }

    var showTableMuerteDetails = function (d) {
        var detalle = "<table id=\"tblmuertedetail\" class=\"table table-striped table-bordered table-hover\">" +
            "<thead><tr class=\"active\">" +
            "<th>Numero de Caso</th>" +
            "<th>Beneficiario</th>" +
            "<th>SSN</th>" +
            "<th>DOB</th>" +
            "<th>Relacion</th>" +
            "<th>Estudiante</th>" +
            "<th>Tutor</th>" +
            "<th>Pago Inicial</th>" +
            "<th>Mens.</th>" +
            "<th>Mens. Vencidas</th>" +
            "<th>Total a Pagar</th>" +
            "<th>Estatus</th>" +
            "<th></th>" +
            "</tr></thead><tbody>";

        for (var i = 0; i < d.Beneficiarios.length; i++) {
            d.Beneficiarios[i].Ssn = d.Beneficiarios[i].Ssn.replace(ssn_format[0], ssn_format[1]);
            var operations = "";     
            operations += buttonApprove(d.Beneficiarios[i].TransactionId, d.Beneficiarios[i].CaseId, d.Beneficiarios[i].CaseDetailId);           
            operations += buttonReject(d.Beneficiarios[i].TransactionId, d.Beneficiarios[i].CaseId, d.Beneficiarios[i].CaseDetailId);
            operations += buttonCancel(d.Beneficiarios[i].TransactionId, d.Beneficiarios[i].CaseId, d.Beneficiarios[i].CaseDetailId);
            operations += buttonReverse(d.Beneficiarios[i].TransactionId, d.Beneficiarios[i].CaseId, d.Beneficiarios[i].CaseDetailId);
            detalle = detalle + "<tr>" +
                                "<td>" + d.Beneficiarios[i].CaseNumber + "</td>" +
                                "<td>" + d.Beneficiarios[i].Beneficiario + "</td>" +
                                "<td>" + d.Beneficiarios[i].Ssn + "</td>" +
                                "<td>" + d.Beneficiarios[i].FechaNacimiento + "</td>" +
                                "<td>" + d.Beneficiarios[i].Relacion + "</td>" +
                                "<td>" + d.Beneficiarios[i].Estudiante + "</td>" +
                                "<td>" + d.Beneficiarios[i].Tutor + "</td>" +
                                "<td class=\"text-right\">" + d.Beneficiarios[i].PagoInicial + "</td>" +
                                "<td class=\"text-right\">" + d.Beneficiarios[i].Mensualidad + "</td>" +
                                "<td class=\"text-right\">" + d.Beneficiarios[i].MensualidadesVencidas + "</td>" +
                                "<td class=\"text-right\">" + d.Beneficiarios[i].TotalAPagar + "</td>" +
                                "<td class=\"text-right\">" + d.Beneficiarios[i].Estatus + "</td>" +
                                "<td class=\"text-right\">" + operations + "</td>" +                              
                                "</tr>";
        }

        detalle = detalle + "</tbody></table>";
        return detalle;
    }

    var searchData = function () {
        var documentType;

        if ($("#ddlDocumentType").val() === "7000" || $("#ddlDocumentType").val() === "9000")
            documentType = 2;
        else
            documentType = $("#ddlDocumentType").val();

        return {
            CaseNumber: $("#CaseNumber").val(),
            EntityName: $("#EntityName").val(),
            SocialSecurityNumber: $("#SocialSecurityNumber").val(),
            BirthDate: $("#BirthDate").val() === "" ? null : $("#BirthDate").val(),
            FilingDate: $("#FilingDate").val() === "" ? null : $("#FilingDate").val(),
            RegionId: $("#Regions").val(),
            DispensaryId: $("#Clinics").val(),
            From: $("#FromDate").val() === "" ? null : $("#FilingDate").val(),
            To: $("#ToDate").val() === "" ? null : $("#FilingDate").val(),
            DocumentType: documentType,
            EBTNumber: $("#EBTNumber").val(),
            StatusId: $("#ddlStatus").val()
        };
    };

    var msgBox = function (message) {
        CDI.displayNotification(message, "warning");
    }

    var buttonApprove = function (transactionId, caseId, caseDetailId) {
        return "<button type='button' class='btn btn-default btn-sm aprobar' data-tx='" + transactionId + "' data-caseid='" + caseId + "' data-casedetailid='" + caseDetailId + "' title='Aprobar'><i class='fa fa-check'></i></button>";
    };

    var buttonReject = function (transactionId, caseId, caseDetailId) {
        return "<button type='button' class='btn btn-default btn-sm rechazar' data-tx='" + transactionId + "' data-caseid='" + caseId + "' data-casedetailid='" + caseDetailId + "' title='Rechazar'><i class='fa fa-ban'></i></button>";
    };

    var buttonReverse = function (transactionId, caseId, caseDetailId) {
        return "<button type='button' class='btn btn-default btn-sm reversar' data-tx='" + transactionId + "' data-caseid='" + caseId + "' data-casedetailid='" + caseDetailId + "' title='Reversar'><i class='fa fa-undo'></i></button>";
    };

    var buttonEdit = function (transactionId, caseId, caseDetailId) {
        return "<button type='button' class='btn btn-default btn-sm editar' data-tx='" + transactionId + "' data-caseid='" + caseId + "' data-casedetailid='" + caseDetailId + "' title='Editar'><i class='fa fa-pencil'></i></button>";
    };

    var buttonCancel = function (transactionId, caseId, caseDetailId) {
        return "<button type='button' class='btn btn-default btn-sm cancelar' data-tx='" + transactionId + "' data-caseid='" + caseId + "' data-casedetailid='" + caseDetailId + "' title='Cancelar'><i class='fa fa-remove'></i></button>";
    };

    var clearInvestmentModal = function () {
        $("#inv_totalinversiones").autoNumeric("set", 0);
        $("#inv_observaciones").val("");
        $("#tblEditInvestments tbody").find("tr").remove();
        $("#tblEditInvestments tfoot").find("tr").remove();
    };
    var showTableMuerte = function () {
        if (!tableMuerte) {
            tableMuerte = $("#tblMuerte").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionsdeath",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "className": "details-control",
                        "orderable": false,
                        "data": null,
                        "defaultContent": ""
                    },
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "Lesionado"
                    },
                     {
                         "data": "Ssn",
                         "render": function (data, type, row) {
                             return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                         }
                     },
                    {
                        "data": "FechaDecision"
                    },
                    {
                        "data": "FechaDefuncion"
                    },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {
                            if (row.StatusId !== CDI.PaymentStatus.Cancelado && row.StatusId !== CDI.PaymentStatus.SimeraS) {

                                var operations = "";

                                if (row.AllowAprobar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                    operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                if (row.TotalInversion !== null && row.AllowEditar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado)) {
                                    operations += buttonEdit(data, row.CaseId, row.CaseDetailId);
                                }

                                return operations;
                            }
                            else {
                                return "";
                            }
                        }
                    }
                ],
                "dom": "rtp",
                "fnRowCallback": function (nRow) {
                    $("td:eq(9)", nRow).addClass("text-center");
                }
            });
        }
        else {
            tableMuerte.ajax.reload();
        }
    };

    var showTableInvestment = function () {
        if (!tableInvestments) {
            tableInvestments = $("#tblInvestments").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionsinvestment",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "className": "details-control",
                        "orderable": false,
                        "data": null,
                        "defaultContent": ""
                    },
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "Lesionado"
                    },
                     {
                         "data": "Ssn",
                         "render": function (data, type, row) {
                             return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                         }
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
                        "orderable": false,
                        "data": "Estado",
                        "render": function (data, type, row) {
                            return "<span data-toggle=\"tooltip\" title=\"" + row.RazonRechazo + "\">" + row.Estado + "</span>";
                        }
                    },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {
                            if (row.StatusId !== CDI.PaymentStatus.Cancelado && row.StatusId !== CDI.PaymentStatus.SimeraS) {

                                var operations = "";

                                if (row.AllowAprobar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                    operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                if (row.TotalInversion !== null && row.AllowEditar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado)) {
                                    operations += buttonEdit(data, row.CaseId, row.CaseDetailId);
                                }

                                return operations;
                            }
                            else {
                                return "";
                            }
                        }
                    }
                ],
                "dom": "rtp",
                "fnRowCallback": function (nRow) {
                    $("td:eq(9)", nRow).addClass("text-center");
                }
            });
        }
        else {
            tableInvestments.ajax.reload();
        }
    };

    var showTablePeremptory = function () {
        if (!tablePeremptories) {
            tablePeremptories = $("#tblPeremptories").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionsperemptory",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "Lesionado"
                    },
                     {
                         "data": "Ssn",
                         "render": function (data, type, row) {
                             return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                         }
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
                       "orderable": false,
                       "data": "Estado",
                       "render": function (data, type, row) {
                           return "<span data-toggle=\"tooltip\" title=\"" + row.RazonRechazo + "\">" + row.Estado + "</span>";
                       }
                   },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {
                            if (row.StatusId !== CDI.PaymentStatus.Cancelado && row.StatusId !== CDI.PaymentStatus.SimeraS) {
                                var operations = "";

                                if (row.AllowAprobar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                    operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                if (row.PaymentId !== null && row.AllowEditar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado)) {
                                    operations = operations + "<button type='button' class='btn btn-default btn-sm editar' data-paymentid='" + row.PaymentId + "' data-tx='" + data + "' data-caseid='" + row.CaseId + "' data-casedetailid='" + row.CaseDetailId + "' title='Editar'><i class='fa fa-pencil'></i></button>";
                                }

                                return operations;
                            }
                            else {
                                return "";
                            }
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tablePeremptories.ajax.reload();
    }

    var showTableIpp = function () {
        if (!tableIpps) {
            tableIpps = $("#tblIpps").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionsipp",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "Lesionado"
                    },
                     {
                         "data": "Ssn",
                         "render": function (data, type, row) {
                             return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                         }
                     },
                    {
                        "data": "FechaAdjudicacion"
                    },
                    {
                        "data": "TipoAdjudicacion"
                    },
                    {
                        "data": "CantidadAdjudicada"
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
                        "data": "TotalPagar"
                    },
                    {
                        "orderable": false,
                        "data": "Estado",
                        "render": function (data, type, row) {
                            return "<span data-toggle=\"tooltip\" title=\"" + row.RazonRechazo + "\">" + row.Estado + "</span>";
                        }
                    },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {

                            if (row.StatusId !== CDI.PaymentStatus.Cancelado && row.StatusId !== CDI.PaymentStatus.SimeraS) {
                                var operations = "<div class='btn-group'>";

                                if (row.AllowAprobar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                    operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                if (row.PaymentId !== null && row.AllowEditar && row.StatusId === CDI.PaymentStatus.Registrado) {
                                    operations = operations + "<button type='button' class='btn btn-default btn-sm editar' data-paymentid='" + row.PaymentId + "' data-tx='" + data + "' data-caseid='" + row.CaseId + "' data-casedetailid='" + row.CaseDetailId + "' title='Editar'><i class='fa fa-pencil'></i></button>";
                                }
                                operations += " </div>";
                                return operations;
                            }
                            else {
                                return "";
                            }
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tableIpps.ajax.reload();
    }

    var showTableItp = function () {

        if (!tableItps) {
            tableItps = $("#tblItps").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionsitp",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "Lesionado"
                    },
                    {
                        "data": "Ssn",
                        "render": function (data, type, row) {
                            return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                        }
                    },
                    {
                        "data": "FechaAdjudicacion"
                    },
                    {
                        "data": "TotalPagar"
                    },
                    {
                        "data": "Reserva"
                    },
                    {
                        "data": "Mensualidad"
                    },
                    {
                        "orderable": false,
                        "data": "Estado",
                        "render": function (data, type, row) {
                            return "<span data-toggle=\"tooltip\" title=\"" + row.RazonRechazo + "\">" + row.Estado + "</span>";
                        }
                    },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {

                            if (!row.NoCase) {
                                if (row.StatusId !== CDI.PaymentStatus.Cancelado && row.StatusId !== CDI.PaymentStatus.SimeraS) {
                                    var operations = "";

                                    if (row.AllowAprobar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                        operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                        operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                        operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                        operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                    return operations;
                                }
                                else {
                                    return "";
                                }
                            }
                            else {
                                if (row.StatusId !== CDI.PaymentStatus.Cancelado) {
                                    operations = "";

                                    if (row.AllowAprobar && row.StatusId === CDI.PaymentStatus.SimeraP) {
                                        operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                        operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                    }
                                    if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado))
                                        operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado))
                                        operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                        operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                    return operations;
                                }
                                else {
                                    return "";
                                }
                            }
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tableItps.ajax.reload();
    };

    var showTablePostMortemItp = function () {

        if (!tablePostMortemItp) {
            tablePostMortemItp = $("#tblPostMortemItp").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionspostmortemitp",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "Lesionado"
                    },
                    {
                        "data": "Ssn",
                        "render": function (data, type, row) {
                            return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                        }
                    },
                    {
                        "data": "FechaAdjudicacion"
                    },
                    {
                        "data": "TotalPagar"
                    },
                    {
                        "data": "Reserva"
                    },
                    {
                        "data": "Mensualidad"
                    },
                    {
                        "orderable": false,
                        "data": "Estado",
                        "render": function (data, type, row) {
                            return "<span data-toggle=\"tooltip\" title=\"" + row.RazonRechazo + "\">" + row.Estado + "</span>";
                        }
                    },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {

                            if (!row.NoCase) {
                                if (row.StatusId !== CDI.PaymentStatus.Cancelado && row.StatusId !== CDI.PaymentStatus.SimeraS) {
                                    var operations = "";

                                    if (row.AllowAprobar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                        operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                        operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                        operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                        operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                    return operations;
                                }
                                else {
                                    return "";
                                }
                            }
                            else {
                                if (row.StatusId !== CDI.PaymentStatus.Cancelado) {
                                    operations = "";

                                    if (row.AllowAprobar && row.StatusId === CDI.PaymentStatus.SimeraP) {
                                        operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                        operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                    }
                                    if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado))
                                        operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado))
                                        operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                    if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                        operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                    return operations;
                                }
                                else {
                                    return "";
                                }
                            }
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tablePostMortemItp.ajax.reload();
    };


    var showTableThird = function () {
        if (!tableThirds) {
            tableThirds = $("#tblThirds").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionsthirdpayment",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "Lesionado"
                    },
                     {
                         "data": "Ssn",
                         "render": function (data, type, row) {
                             return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                         }
                     },
                    {
                        "data": "FechaDecision"
                    },
                    {
                        "data": "Custodio"
                    },
                    {
                        "data": "TotalPagar"
                    },
                    {
                        "orderable": false,
                        "data": "Estado",
                        "render": function (data, type, row) {
                            return "<span data-toggle=\"tooltip\" title=\"" + row.RazonRechazo + "\">" + row.Estado + "</span>";
                        }
                    },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {
                            console.log("row.PaymentId = " + row.PaymentId);
                            console.log("row.CaseId = " + row.CaseId);
                            console.log("row.CaseDetailId =" + row.CaseDetailId);

                            //if (row.StatusId != CDI.PaymentStatus.Cancelado && row.StatusId != CDI.PaymentStatus.SimeraS) {
                            var operations = "<div class='btn-group'>";

                            //if (row.AllowAprobar && row.StatusId == CDI.PaymentStatus.Registrado)
                            //operations += _buttonApprove(data, row.CaseId, row.CaseDetailId);
                            operations += "<button type='button' class='btn btn-default btn-sm aprobar' data-caseid='" + row.CaseId + "' data-casedetailid='" + row.CaseDetailId + "' data-thirdpartyscheduleid='" + row.ThirdPartyScheduleId + "' data-paymentid='" + row.PaymentId + "' title='Aprobar'><i class='fa fa-check'></i></button>";
                            //if (row.AllowRechazar && (row.StatusId == CDI.PaymentStatus.Aprobado || row.StatusId == CDI.PaymentStatus.Registrado))
                            //operations += _buttonReject(data, row.CaseId, row.CaseDetailId);
                            operations += "<button type='button' class='btn btn-default btn-sm rechazar' data-caseid='" + row.CaseId + "' data-casedetailid='" + row.CaseDetailId + "' data-thirdpartyscheduleid='" + row.ThirdPartyScheduleId + "' data-paymentid='" + row.PaymentId + "' title='Rechazar'><i class='fa fa-ban'></i></button>";
                            //if (row.AllowCancelar && (row.StatusId == CDI.PaymentStatus.Aprobado || row.StatusId == CDI.PaymentStatus.Registrado))
                            //operations += _buttonCancel(data, row.CaseId, row.CaseDetailId);
                            //if (row.AllowReversar && row.StatusId == CDI.PaymentStatus.SapS)
                            //operations += _buttonReverse(data, row.CaseId, row.CaseDetailId);

                            //if (row.PaymentId == 0 && row.AllowEditar) {
                            operations += "<button type='button' class='btn btn-default btn-sm editar' data-paymentid='" + row.PaymentId + "' data-caseid='" + row.CaseId + "' data-casedetailid='" + row.CaseDetailId + "' data-thirdpartyscheduleid='" + row.ThirdPartyScheduleId + "' title='Editar'><i class='fa fa-pencil'></i></button>";
                            //}

                            operations += " </div>";
                            return operations;
                            //}
                            //else {
                            //    return "";
                            //}
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tableThirds.ajax.reload();
    }

    var saveInvestments = function () {
        var total = $("#inv_totalinversiones").autoNumeric("get");
        if (total === 0) {
            CDI.displayNotification("Debe ingresar valores mayores a cero en los campos de importe", "error");
            return false;
        }
        var model = {
            CaseId: 0,
            CaseDetailId: 0,
            TransactionId: 0,
            Payments: [],
            Comment: "",
            CaseNumber: ""
        };
        var paymentsIsValid = true;
        var payments = [];
        $("#tblEditInvestments tbody tr:has(td)").each(function (index, element) {
            var $element = $(element);
            var entidad = $element.find("#entidad").val();
            var inversionInput = $element.find("#inversion");
            var inversion = inversionInput.autoNumeric("get");
            if (inversion === 0 || entidad === "") {
                paymentsIsValid = false;
                return false;
            }

            payments.push({
                PaymentId: $(this).data("paymentid") ,
                EntityId: $(this).data("entityid")  ,
                Entidad: entidad,
                Inversion: inversion
            });
        });

        if (!paymentsIsValid) {
            CDI.displayNotification("Todos los pagos y entidades deben ser llenados", "error");
            return false;
        }

        model.CaseId = caseData.CaseId;
        model.CaseDetailId = caseData.CaseDetailId;
        model.TransactionId = caseData.TransactionId;
        model.Comment = $("#inv_observaciones").val();
        model.CaseNumber = caseData.CaseNumber;
        model.Payments = payments;

        $.ajax({
            url: root + "payments/updateinvestments",
            data: model,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            if (response.data.Status === "OK") {
                CDI.displayNotification("Datos guardados", "info");
                if (modal) {
                    modal.modal("hide");
                    clearInvestmentModal();
                }
                showTableInvestment();
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var calculateNewInvestments = function () {
        var sumaInversiones = 0;
        $("#tblEditInvestments tbody tr:has(td)").each(function (index, element) {
            var $element = $(element);
            var $inversion = $element.find("#inversion");
            var inversionControl = $inversion.autoNumeric("get");
            var inversionValor = parseFloat(inversionControl);
            if (isNaN(inversionValor)) {
                inversionValor = 0;
            }
            sumaInversiones = sumaInversiones + inversionValor;
        });

        $("#inv_totalinversiones").autoNumeric("set", sumaInversiones);

        var foot = $("#tblEditInvestments").find("tfoot");
        if (!foot.length) {
            foot = $("<tfoot>").appendTo("#tblEditInvestments");
        }
        foot.html("<tr><td></td><td></td><td class=\"text-right\"><strong>Total " + $("#inv_totalinversiones").val() + "</strong></td></tr>");
    };

    var cleanPendingDietModal = function () {
        startDateDecision = new Date();
        startDateVisita = new Date();
        startDateNotificacion = new Date();

        $("#dieta_fechadecision").val("").datepicker("update");
        $("#dieta_fvisita").val("").datepicker("update");
        $("#dieta_fnotificacion").val("").datepicker("update");
        $("#dieta_numerocaso").val("");
        $("#dieta_observaciones").val("");
        $("#dieta_montototal").autoNumeric("set", 0);

        $("#tblEditPendingDietPeriods tbody").find("tr").remove();
    };

    var calculateTotalPendingDiets = function () {
        var sumaMontos = 0;
        $("#tblEditPendingDietPeriods > tbody > tr").each(function (index, element) {
            var $element = $(element);
            var $totalPagar = $element.find("#total-pagar");
            var cantidadValor = parseFloat($totalPagar.autoNumeric("get"));

            if (isNaN(cantidadValor)) {
                cantidadValor = 0;
            }

            sumaMontos += cantidadValor;
        });
        $("#dieta_montototal").autoNumeric("set", sumaMontos);

        var foot = $("#tblEditPendingDietPeriods").find("tfoot");
        if (!foot.length) {
            foot = $("<tfoot>").appendTo("#tblEditPendingDietPeriods");
        }
        foot.html("<tr><td colspan=\"6\" class=\"text-right\"><strong>Total: " + $("#dieta_montototal").val() + "</strong></td></tr>");
    };



    var showTablePendingDiet = function () {
        if (!tablePendingDiets) {
            tablePendingDiets = $("#tblPendingDiets").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionspendingdiet",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "Lesionado"
                    },
                     {
                         "data": "Ssn",
                         "render": function (data, type, row) {
                             return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                         }
                     },
                    {
                        "data": "FechaNacimiento"
                    },
                    {
                        "data": "Desde"
                    },
                    {
                        "data": "Hasta"
                    },
                    {
                        "data": "NroDias"
                    },
                    {
                        "data": "Jornal"
                    },
                    {
                        "data": "CompSemanal"
                    },
                    {
                        "data": "Dieta"
                    },
                    {
                        "orderable": false,
                        "data": "Estado",
                        "render": function (data, type, row) {
                            return "<span data-toggle=\"tooltip\" title=\"" + row.RazonRechazo + "\">" + row.Estado + "</span>";
                        }
                    },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {

                            if (row.StatusId !== CDI.PaymentStatus.Cancelado && row.StatusId !== CDI.PaymentStatus.SimeraS) {
                                var operations = "";

                                if (row.AllowAprobar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                    operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                if (row.PaymentId !== null && row.AllowEditar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado)) {
                                    operations = operations + "<button type='button' class='btn btn-default btn-sm editar' data-thirdpartyscheduleid='" + row.ThirdPartyScheduleId + "' data-caseid='" + row.CaseId + "' data-casedetailid='" + row.CaseDetailId + "' title='Editar'><i class='fa fa-pencil'></i></button>";
                                }

                                return operations;
                            }
                            else {
                                return "";
                            }
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tablePendingDiets.ajax.reload();
    }

    var showTableLawyer = function () {
        if (!tableLawyers) {
            tableLawyers = $("#tblLawyers").DataTable({
                "processing": true,
                "deferRender": true,
                "autoWidth": false,
                "drawCallback": function () {
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
                    "url": root + "approval/findtransactionslawyer",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "data": function () {
                        return JSON.stringify({ model: searchData() });
                    }
                },
                "order": [
                    [1, "desc"]
                ],
                "columns": [
                    {
                        "data": "CaseNumber"
                    },
                    {
                        "data": "CaseId",
                        "orderable": false,
                        "visible": false
                    },
                    {
                        "data": "Lesionado"
                    },
                    {
                        "data": "Ssn",
                        "render": function (data, type, row) {
                            return row.Ssn.replace(ssn_format[0], ssn_format[1]);
                        }
                    },
                    {
                        "data": "FechaDecision"
                    },
                    {
                        "data": "Abogado"
                    },
                    {
                        "data": "FechaNotificacion"
                    },
                    {
                        "data": "NumeroCasoCI"
                    },
                    {
                        "data": "TotalPagar"
                    },
                   {
                       "orderable": false,
                       "data": "Estado",
                       "render": function (data, type, row) {
                           return "<span data-toggle=\"tooltip\" title=\"" + row.RazonRechazo + "\">" + row.Estado + "</span>";
                       }
                   },
                    {
                        "orderable": false,
                        "data": "TransactionId",
                        "render": function (data, type, row) {

                            if (row.StatusId !== CDI.PaymentStatus.Cancelado && row.StatusId !== CDI.PaymentStatus.SimeraS) {
                                var operations = "";

                                if (row.AllowAprobar && (row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonApprove(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowRechazar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonReject(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowCancelar && (row.StatusId === CDI.PaymentStatus.Aprobado || row.StatusId === CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado))
                                    operations += buttonCancel(data, row.CaseId, row.CaseDetailId);
                                if (row.AllowReversar && row.StatusId === CDI.PaymentStatus.SapS)
                                    operations += buttonReverse(data, row.CaseId, row.CaseDetailId);

                                if(row.PaymentId !== null && row.AllowEditar && (row.StatusId == CDI.PaymentStatus.Registrado || row.StatusId == CDI.PaymentStatus.Generado)) {
                                    operations = operations + "<button type='button' class='btn btn-default btn-sm editar' data-thirdpartyscheduleid='" + row.ThirdPartyScheduleId + "' data-caseid='" + caseId + "' data-casedetailid='" + caseDetailId + "' title='Editar'><i class='fa fa-pencil'></i></button>";
                                }

                                return operations;
                            }
                            else {
                                return "";
                            }
                        }
                    }
                ],
                "dom": "rtp"
            });
        }
        else
            tableLawyers.ajax.reload();
    };

    var savePendingDiets = function () {
        var total = $("#dieta_montototal").autoNumeric("get");
        if (total === 0) {
            msgBox("Debe ingresar valores mayores a cero en los campos de cantidades");
            return false;
        }

        if ($("#dieta_fechadecision").val() === "") {
            msgBox("Debe ingresar un valor para Fecha Decisión");
            return false;
        }

        if ($("#dieta_fvisita").val() === "") {
            msgBox("Debe ingresar un valor para Fecha Visita");
            return false;
        }

        if ($("#dieta_fnotificacion").val() === "") {
            msgBox("Debe ingresar un valor para Fecha Notificación");
            return false;
        }

        if ($("#dieta_numerocaso").val() === "") {
            msgBox("Debe ingresar un valor para Número de Caso");
            return false;
        }

        var periodsIsValid = true;
        var periods = [];
        $("#tblEditPendingDietPeriods tbody tr:has(td)").each(function (index, element) {
            var $element = $(element);
            var desde = $element.find("#desde").val();
            var hasta = $element.find("#hasta").val();
            var cantidad = $element.find("#cantidad").autoNumeric("get");
            var descuento = $element.find("#descuento").autoNumeric("get");
            if (descuento === 0 || descuento === "" || desde === "" || hasta === "") {
                periodsIsValid = false;
            }

            periods.push({
                PaymentId: $element.data("paymentid"),
                Desde: desde,
                Hasta: hasta,
                Cantidad: cantidad,
                Descuento: descuento
            });
        });

        if (!periodsIsValid) {
            msgBox("Todos los periodos y cantidades deben ser ingresados");
            return false;
        }

        var data = {
            caseId: caseData.CaseId,
            caseDetailId: caseData.CaseDetailId,
            transactionId: caseData.TransactionId,
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
            url: root + "payments/updatependingdiets",
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

                if (modal) {
                    modal.modal("hide");
                }
                showTablePendingDiet();
            } else {
                msgBox("Error al grabar datos.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });

    };

    var approveTransaction = function (caseDetailId, transactionId) {
        var data = {
            "caseDetailId": caseDetailId,
            "transactionId": transactionId
        };

        $.ajax({
            url: root + "approval/approve",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            var selectedDocumentType = $("#ddlDocumentType").val();
            if (response.data.Status === "OK") {
                switch (selectedDocumentType) {
                    case "2":
                        showTableIpp();
                        break;
                    case "3":
                        showTableInvestment();
                        break;
                    case "4":
                        showTablePeremptory();
                        break;
                    case "5":
                        showTableLawyer();
                        break;
                    case "6000":
                        showTablePendingDiet();
                        break;
                    case "7000":
                        showTableItp();
                        break;
                    case "8000":
                        showTableMuerte();
                    case "9000":
                        showTablePostMortemItp();
                }
            }
            else {
                msgBox("Error al actualizar.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var rejectTransaction = function (caseDetailId, transactionId, reason) {
        var data = {
            "caseDetailId": caseDetailId,
            "transactionId": transactionId,
            "reason": reason
        };

        $.ajax({
            url: root + "approval/reject",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            if (modalReject)
                modalReject.modal("hide");

            var selectedDocumentType = $("#ddlDocumentType").val();
            if (response.data.Status === "OK") {
                switch (selectedDocumentType) {
                    case "2":
                        showTableIpp();
                        break;
                    case "3":
                        showTableInvestment();
                        break;
                    case "4":
                        showTablePeremptory();
                        break;
                    case "6000":
                        showTablePendingDiet();
                        break;
                    case "7000":
                        showTableItp();
                        break;
                    case "8000":
                        showTableMuerte();
                        break;
                }
            }
            else {
                msgBox("Error al actualizar.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var cancelTransaction = function (caseDetailId, transactionId) {
        var data = {
            "caseDetailId": caseDetailId,
            "transactionId": transactionId
        };

        $.ajax({
            url: root + "approval/cancel",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            var selectedDocumentType = $("#ddlDocumentType").val();
            if (response.data.Status === "OK") {
                switch (selectedDocumentType) {
                    case "2":
                        showTableIpp();
                        break;
                    case "3":
                        showTableInvestment();
                        break;
                    case "4":
                        showTablePeremptory();
                        break;
                    case "6000":
                        showTablePendingDiet();
                        break;
                    case "7000":
                        showTableItp();
                        break;
                    case "8000":
                        showTableMuerte();
                        break;
                }
            }
            else {
                msgBox("Error al actualizar.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var approvePendingDiets = function (caseDetailId) {
        var data = {
            "caseDetailId": caseDetailId
        };

        $.ajax({
            url: root + "approval/approvependingdiets",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            if (response.data.Status === "OK") {
                showTablePendingDiet();
            }
            else {
                msgBox("Error al actualizar.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var rejectPendingDiets = function (caseDetailId, reason) {
        var data = {
            "caseDetailId": caseDetailId,
            "razon": reason
        };

        $.ajax({
            url: root + "approval/rejectpendingdiets",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            if (modalReject)
                modalReject.modal("hide");

            if (response.data.Status === "OK") {
                showTablePendingDiet();
            }
            else {
                msgBox("Error al actualizar.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var approveThirdPartyPayment = function (paymentId, thirdPartyScheduleId) {
        var data = {
            "paymentId": paymentId,
            "thirdPartyScheduleId": thirdPartyScheduleId
        };

        $.ajax({
            url: root + "approval/approvethirdpartypayment",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            if (response.data.Status === "OK") {
                tableThirds.ajax.reload();
            }
            else {
                msgBox("Error al actualizar.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var rejectThirdPartyPayment = function (paymentId, thirdPartyScheduleId) {
        var data = {
            "paymentId": paymentId,
            "thirdPartyScheduleId": thirdPartyScheduleId
        };

        $.ajax({
            url: root + "approval/rejectthirdpartypayment",
            data: data,
            dataType: "json",
            type: "POST",
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (response) {
            CDI.hideWaitingMessage();

            if (response.data.Status === "OK") {
                tableThirds.ajax.reload();
            }
            else {
                msgBox("Error al actualizar.");
            }
        }).fail(function () {
            CDI.hideWaitingMessage();
        });
    };

    var clearFields = function () {
        $("#EntityName").val("");
        $("#SocialSecurityNumber").val("");
        $("#CaseNumber").val("");
        $("#Regions").val("");
        $("#Clinics").val("");
        $("#BirthDate").val("");
        $("#FilingDate").val("");
        $("#FromDate").val("");
        $("#ToDate").val("");
        $("#ddlDocumentType").val("");
        $("#ddlStatus").val("");
    };

    var init = function () {
        $("#frmSearch").validate();

        initControls();

        $("#btnSearch").on("click", function (evt) {
            evt.preventDefault();
            var isValid = true;
            var selectedDocumentType = $("#ddlDocumentType").val();
            if (selectedDocumentType !== "5000")
            {
                CDI.addValidationRules(searchRules);
                isValid = $("#frmSearch").valid();
            }

            if (isValid) {
                $("#panel-aprov-ipp").fadeOut(1000).addClass("hidden");
                $("#panel-aprov-invest").fadeOut(1000).addClass("hidden");
                $("#panel-aprov-perent").fadeOut(1000).addClass("hidden");
                $("#panel-aprov-third").fadeOut(1000).addClass("hidden");
                $("#panel-aprov-dieta").fadeOut(1000).addClass("hidden");
                $("#panel-aprov-lawyer").fadeOut(1000).addClass("hidden");
                $("#panel-aprov-itp").fadeOut(1000).addClass("hidden");
                $("#panel-aprov-muerte").fadeOut(1000).addClass("hidden");
                $("#panel-aprov-pm-itp").fadeOut(1000).addClass("hidden");
               
                switch (selectedDocumentType) {
                    case "2":
                        $("#panel-aprov-ipp").fadeIn(1000).removeClass("hidden");
                        showTableIpp();
                        break;
                    case "3":
                        $("#panel-aprov-invest").fadeIn(1000).removeClass("hidden");
                        showTableInvestment();
                        break;
                    case "4":
                        $("#panel-aprov-perent").fadeIn(1000).removeClass("hidden");
                        showTablePeremptory();
                        break;
                    case "5000":
                        $("#panel-aprov-third").fadeIn(1000).removeClass("hidden");
                        showTableThird();
                        break;
                    case "6000":
                        $("#panel-aprov-dieta").fadeIn(1000).removeClass("hidden");
                        showTablePendingDiet();
                        break;
                    case "5":
                        $("#panel-aprov-lawyer").fadeIn(1000).removeClass("hidden");
                        showTableLawyer();
                        break;
                    case "7000":
                        $("#panel-aprov-itp").fadeIn(1000).removeClass("hidden");
                        showTableItp();
                        break;
                    case "8000":
                        $("#panel-aprov-muerte").fadeIn(1000).removeClass("hidden");
                        showTableMuerte();
                        break;
                    case "9000":
                        $("#panel-aprov-pm-itp").fadeIn(1000).removeClass("hidden");
                        showTableMuerte();
                        break;
                }
            }

            CDI.removeValidationRules(searchRules);

        });

        $("#btnClear").on("click", function () {
            clearFields();

            if ($.fn.dataTable.isDataTable("#tblInvestments")) {
                tableInvestments.destroy();
                tableInvestments = null;
            }
            $("#tblInvestments tbody").find("tr").remove();

            if ($.fn.dataTable.isDataTable("#tblPeremptories")) {
                tablePeremptories.destroy();
                tablePeremptories = null;
            }
            $("#tblPeremptories tbody").find("tr").remove();

            if ($.fn.dataTable.isDataTable("#tblIpps")) {
                tableIpps.destroy();
                tableIpps = null;
            }
            $("#tblIpps tbody").find("tr").remove();

            if ($.fn.dataTable.isDataTable("#tblItps")) {
                tableItps.destroy();
                tableItps = null;
            }
            $("#tblItps tbody").find("tr").remove();

            if ($.fn.dataTable.isDataTable("#tblThirds")) {
                tableThirds.destroy();
                tableThirds = null;
            }
            $("#tblThirds tbody").find("tr").remove();

            if ($.fn.dataTable.isDataTable("#tblPendingDiets")) {
                tablePendingDiets.destroy();
                tablePendingDiets = null;
            }
            $("#tblPendingDiets tbody").find("tr").remove();

            if ($.fn.dataTable.isDataTable("#tblLawyers")) {
                tableLawyers.destroy();
                tableLawyers = null;
            }
            $("#tblLawyers tbody").find("tr").remove();

            $("#panel-aprov-ipp").fadeOut(1000).addClass("hidden");
            $("#panel-aprov-invest").fadeOut(1000).addClass("hidden");
            $("#panel-aprov-perent").fadeOut(1000).addClass("hidden");
            $("#panel-aprov-third").fadeOut(1000).addClass("hidden");
            $("#panel-aprov-dieta").fadeOut(1000).addClass("hidden");
            $("#panel-aprov-lawyer").fadeOut(1000).addClass("hidden");
            $("#panel-aprov-itp").fadeOut(1000).addClass("hidden");
            $("#panel-aprov-muerte").fadeOut(1000).addClass("hidden");
        });

        $("#search-panel input").keyup(function (event) {
            if (event.keyCode === 13) {
                $("#btnSearch").click();
            }
        });

        $("#tblInvestments tbody").on("click", "button.editar", function () {
            clearInvestmentModal();

            caseData = tableInvestments.row($(this).closest("tr")).data();
            for (var index = 0; index < caseData.Pagos.length; index++) {
                var newRow = "<tr data-paymentid=\"" + caseData.Pagos[index].PaymentId + "\" data-entityid=\"" + caseData.Pagos[index].EntityId + "\">" +
                                "<td><button type=\"button\" class=\"btn btn-default btn-sm deleterow\">" +
                                "<i class=\"fa fa-minus\"></i></button></td>" +
                                "<td><input id=\"entidad\" name=\"entidad\" type=\"text\" class=\"form-control\" value=\"" +
                                    caseData.Pagos[index].PagoDirigidoA + "\" /></td>" +
                                "<td><input id=\"inversion\" name=\"inversion\" type=\"text\" class=\"form-control text-right inversion\" value=\"" +
                                    caseData.Pagos[index].Importe.replace("$", "") + "\" /></td>" +
                             "</tr>";
                $("#tblEditInvestments > tbody:last").append(newRow);
                $("#tblEditInvestments tbody").find("tr").last().find("#inversion").autoNumeric("init", {
                    vMin: "0"
                });
            }
            $("#inv_totalinversiones").val(caseData.TotalInversion);
            $("#inv_observaciones").val(caseData.Comments);

            modal = $("#modal-investments").modal("show");
        });

        $("#tblInvestments tbody").on("click", "button.aprobar", function () {
            approveTransaction($(this).data("casedetailid"), $(this).data("tx"));
        });

        $("#tblInvestments tbody").on("click", "button.rechazar", function () {
            var caseDetailId = $(this).data("casedetailid");
            var transaccionId = $(this).data("tx");

            $("#razon_rechazo").val("");

            $("#btnRechazar").on("click", function () {
                var razon = $("#razon_rechazo").val();
                if (razon === "") {
                    msgBox("Debe ingresar una razón de rechazo");
                    return false;
                }

                rejectTransaction(caseDetailId, transaccionId, $("#razon_rechazo").val());
            });

            modalReject = $("#modal-reject").modal("show");
        });

        $("#tblInvestments tbody").on("click", "button.cancelar", function () {
            cancelTransaction($(this).data("casedetailid"), $(this).data("tx"));
        });

        $("#tblInvestments tbody").on("click", "td.details-control", function () {
            var tr = $(this).closest("tr");
            var row = tableInvestments.row(tr);

            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass("shown");
            }
            else {
                row.child(showPaymentInvestmentDetails(row.data())).show();
                tr.addClass("shown");
            }
        });

        ///Muerte
        $("#tblMuerte tbody").on("click", "td.details-control", function () {
            var tr = $(this).closest("tr");
            var row = tableMuerte.row(tr);

            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass("shown");
            }
            else
            {
                row.child(showTableMuerteDetails(row.data())).show();

                $("#tblmuertedetail tbody").on("click", "button.aprobar", function () {
                    approveTransaction($(this).data("casedetailid"), $(this).data("tx"));
                });

                $("#tblmuertedetail tbody").on("click", "button.rechazar", function () {
                    var caseDetailId = $(this).data("casedetailid");
                    var transaccionId = $(this).data("tx");

                    $("#razon_rechazo").val("");

                    $("#btnRechazar").on("click", function () {
                        var razon = $("#razon_rechazo").val();
                        if (razon === "") {
                            msgBox("Debe ingresar una razón de rechazo");
                            return false;
                        }

                        return rejectTransaction(caseDetailId, transaccionId, $("#razon_rechazo").val());
                    });

                    modalReject = $("#modal-reject").modal("show");
                });

                $("#tblmuertedetail tbody").on("click", "button.cancelar", function () {
                    cancelTransaction($(this).data("casedetailid"), $(this).data("tx"));
                });

                tr.addClass("shown");
            }
        });


        $("#tblIpps tbody").on("click", "button.aprobar", function () {
            approveTransaction($(this).data("casedetailid"), $(this).data("tx"));
        });

        $("#tblIpps tbody").on("click", "button.rechazar", function () {
            var caseDetailId = $(this).data("casedetailid");
            var transaccionId = $(this).data("tx");

            $("#razon_rechazo").val("");

            $("#btnRechazar").on("click", function () {
                var razon = $("#razon_rechazo").val();
                if (razon === "") {
                    msgBox("Debe ingresar una razón de rechazo");
                    return false;
                }

                return rejectTransaction(caseDetailId, transaccionId, $("#razon_rechazo").val());
            });

            modalReject = $("#modal-reject").modal("show");
        });

        $("#tblIpps tbody").on("click", "button.cancelar", function () {
            cancelTransaction($(this).data("casedetailid"), $(this).data("tx"));
        });

        $("#tblItps tbody").on("click", "button.aprobar", function () {
            approveTransaction($(this).data("casedetailid"), $(this).data("tx"));
          });

        $("#tblItps tbody").on("click", "button.rechazar", function () {
            var caseDetailId = $(this).data("casedetailid");
            var transaccionId = $(this).data("tx");

            $("#razon_rechazo").val("");

            $("#btnRechazar").on("click", function () {
                var razon = $("#razon_rechazo").val();
                if (razon === "") {
                    msgBox("Debe ingresar una razón de rechazo");
                    return false;
                }

                return rejectTransaction(caseDetailId, transaccionId, $("#razon_rechazo").val());
            });

            modalReject = $("#modal-reject").modal("show");
            });

        $("#tblItps tbody").on("click", "button.cancelar", function() {
            cancelTransaction($(this).data("casedetailid"), $(this).data("tx"));
            });

        $("#tblPeremptories tbody").on("click", "button.cancelar", function () {
            cancelTransaction($(this).data("casedetailid"), $(this).data("tx"));
        });

        $("#tblPeremptories tbody").on("click", "button.aprobar", function () {
            approveTransaction($(this).data("casedetailid"), $(this).data("tx"));
        });

        $("#tblPeremptories tbody").on("click", "button.rechazar", function () {
            $("#razon_rechazo").val($(this).data("casedetailid"));
            $("#transaccionid").val($(this).data("tx"));
            $("#razon_rechazo").val("");

            $("#btnRechazar").on("click", function () {
                var razon = $("#razon_rechazo").val();
                if (razon === "") {
                    msgBox("Debe ingresar una razón de rechazo");
                    return false;
                }

                rejectTransaction($(this).data("casedetailid").val(), $("#transaccionid").val(), $("#razon_rechazo").val());
            });

            modalReject = $("#modal-reject").modal("show");
        });

        $("#tblThirds tbody").on("click", "button.aprobar", function () {

            approveThirdPartyPayment($(this).data("paymentid"), $(this).data("thirdpartyscheduleid"));

        });

        $("#tblThirds tbody").on("click", "button.rechazar", function () {

            $("#paymentid").val($(this).data("paymentid"));
            $("#thirdpartyscheduleid").val($(this).data("thirdpartyscheduleid"));
            $("#razon_rechazo").val("");

            $("#btnRechazar").on("click", function () {
                var razon = $("#razon_rechazo").val();
                if (razon === "") {
                    msgBox("Debe ingresar una razón de rechazo");
                    return false;
                }

                var paymentId = $("#paymentid").val();
                var thirdpartyscheduleId = $("#thirdpartyscheduleid").val();

                return rejectThirdPartyPayment(paymentId, thirdpartyscheduleId);
            });

            modalReject = $("#modal-reject").modal("show");
        });

        $("#tblPendingDiets tbody").on("click", "button.aprobar", function () {
            approvePendingDiets($(this).data("casedetailid"));
        });

        $("#tblPendingDiets tbody").on("click", "button.rechazar", function () {

            $("#caseid").val($(this).data("caseid"));
            $("#casedetailid").val($(this).data("casedetailid"));
            $("#razon_rechazo").val("");

            $("#btnRechazar").on("click", function () {
                var razon = $("#razon_rechazo").val();

                if (razon === "") {
                    msgBox("Debe ingresar una razón de rechazo");
                    return false;
                }

                rejectPendingDiets($("#casedetailid").val(), razon);

            });

            modalReject = $("#modal-reject").modal("show");
        });

        $("#tblLawyers tbody").on("click", "button.aprobar", function () {
            approveTransaction($(this).data("caseid"), $(this).data("tx"));
        });

        $("#tblLawyers tbody").on("click", "button.rechazar", function () {
            var caseDetailId = $(this).data("casedetailid");
            var transaccionId = $(this).data("tx");

            $("#razon_rechazo").val("");

            $("#btnRechazar").on("click", function () {
                var razon = $("#razon_rechazo").val();
                if (razon === "") {
                    msgBox("Debe ingresar una razón de rechazo");
                    return false;
                }

                rejectTransaction(caseDetailId, transaccionId, $("#razon_rechazo").val());
            });

            modalReject = $("#modal-reject").modal("show");
        });

        $("#tblLawyers tbody").on("click", "button.cancelar", function () {
            cancelTransaction($(this).data("casedetailid"), $(this).data("tx"));
        });

        $("#tblPeremptories tbody").on("click", "button.editar", function () {
            var paymentid = $(this).data("paymentid");

            $("#btnUpdatePeremptory").on("click", function () {
                var data = {
                    paymentid: $("#paymentid").val(),
                    transactionid: $("#transactionid").val(),
                    cantidad: $("#due_cantidadsolicitada").autoNumeric("get"),
                    observaciones: $("#due_observaciones").val()
                };

                $.ajax({
                    url: root + "payments/updateperemptory",
                    data: data,
                    dataType: "json",
                    type: "POST",
                    beforeSend: function () {
                        CDI.showWaitingMessage();
                    }
                }).done(function (response) {
                    CDI.hideWaitingMessage();

                    if (response.data.Status === "OK") {
                        if (modal) {
                            modal.modal("hide");
                        }
                        showTablePeremptory();
                    }
                    else {
                        msgBox("Error al actualizar.");
                    }
                }).fail(function () {
                    CDI.hideWaitingMessage();

                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                });
            });

            $.ajax({
                url: root + "payments/findbyid/" + paymentid,
                type: "GET",
                cache: false,
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) {
                CDI.hideWaitingMessage();

                $("#paymentid").val(response.data.PaymentId);
                $("#transactionid").val(response.data.TransactionId);
                $("#due_cantidadsolicitada").autoNumeric("set", response.data.TransactionAmount);
                $("#due_observaciones").val(response.data.Observaciones);

                modal = $("#modal-peremptory").modal("show");
            }).fail(function () {
                CDI.hideWaitingMessage();

                msgBox("Ha ocurrido un error. Consulte con el Administrador.");
            });
        });

        $("#tblIpps tbody").on("click", "button.editar", function () {
            var $this = $(this);
            var paymentid = $this.data("paymentid");

            $("#btnUpdateIpp").off("click").on("click", function () {
                var data = {
                    paymentid: $("#paymentid").val(),
                    transactionid: $("#transactionid").val(),
                    fechaadjudicacion: $("#ipp_fechaadjudicacion").val(),
                    cantidad: $("#ipp_adjudicacionadicional").autoNumeric("get"),
                    mensualidad: $("#ipp_mensualidad").autoNumeric("get"),
                    semanas: $("#ipp_semanas").autoNumeric("get"),
                    observaciones: $("#observaciones").val()
                };

                $.ajax({
                    url: root + "payments/updateipp",
                    data: data,
                    dataType: "json",
                    type: "POST",
                    beforeSend: function () {
                        CDI.showWaitingMessage();
                    }
                }).done(function (response) {
                    CDI.hideWaitingMessage();

                    if (response.data.Status === "OK") {
                        if (modal) {
                            modal.modal("hide");
                        }
                        showTableIpp();
                    }
                    else {
                        msgBox("Error al actualizar.");
                    }
                }).fail(function () {
                    CDI.hideWaitingMessage();

                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                });
            });

            $.ajax({
                url: root + "payments/findbyid/" + paymentid,
                type: "GET",
                cache: false
            }).done(function (response) {
                $("#paymentid").val(response.data.PaymentId);
                $("#transactionid").val(response.data.TransactionId);
                $("#ipp_fechaadjudicacion").datepicker("update", response.data.FechaAdjudicacion);
                $("#ipp_adjudicacionadicional").autoNumeric("set", response.data.TransactionAmount);
                $("#ipp_mensualidad").autoNumeric("set", response.data.MonthlyInstallment);
                $("#ipp_semanas").autoNumeric("set", response.data.NumberOfWeeks);
                $("#ipp_observaciones").val(response.data.Observaciones);

                modal = $("#modal-ipp").modal("show");
            }).fail(function () {
                msgBox("Ha ocurrido un error. Consulte con el Administrador.");
            });
        });

        $("#tblThirds tbody").on("click", "button.editar", function () {
            var thirdpartyscheduleid = $(this).data("thirdpartyscheduleid");
            console.log("row.ThirdPartyScheduleId = " + thirdpartyscheduleid);
            $("#btnUpdateThirdPayment").on("click", function () {
                var data = {
                    ThirdPartyScheduleId: $("#thirdpartyscheduleid").val(),
                    SinglePaymentAmount: $("#unsolopago").autoNumeric("get"),
                    FirstInstallmentAmount: $("#pago1").autoNumeric("get"),
                    SecondInstallmentAmount: $("#pago2").autoNumeric("get"),
                    Comment: $("#third_observaciones").val()
                };

                $.ajax({
                    url: root + "thirdpartypayment/update",
                    data: data,
                    dataType: "json",
                    type: "POST",
                    beforeSend: function () {
                        CDI.showWaitingMessage();
                    }
                }).done(function (response) {
                    CDI.hideWaitingMessage();

                    if (response.data.Status === "OK") {
                        if (modal) {
                            modal.modal("hide");
                        }
                        showTableThird();
                    }
                    else {
                        msgBox("Error al actualizar.");
                    }
                }).fail(function () {
                    CDI.hideWaitingMessage();

                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                });
            });

            $.ajax({
                url: root + "thirdpartypayment/findtoeditbyid/" + thirdpartyscheduleid,
                dataType: "json",
                type: "GET"
            }).done(function (response) {
                $("#unsolopago").autoNumeric("init");
                $("#pago1").autoNumeric("init");
                $("#pago2").autoNumeric("init");

                $("option").attr("selected", false);
                $("select").val("");

                $("#thirdpartyscheduleid").val(response.ThirdPartyScheduleId);
                $("#unsolopago").autoNumeric("set", response.SinglePaymentAmount);
                $("#pago1").autoNumeric("set", response.FirstInstallAmount);
                $("#pago2").autoNumeric("set", response.SecondInstallAmount);
                $("#third_observaciones").val(response.Observaciones);

                if (response.SinglePaymentAmount > 0 && response.FirstInstallAmount === 0 && response.SecondInstallAmount === 0) {
                    $("[name=tipo_pago][value=1]").prop("checked", true);
                    $("#pago1").prop("readonly", true);
                    $("#pago2").prop("readonly", true);
                    $("#unsolopago").prop("readonly", false);
                    $("#unsolopago").autoNumeric("set", 0);
                    $("#IsSinglePayment").val("true");

                }
                else {
                    $("[name=tipo_pago][value=2]").prop("checked", true);
                    $("#unsolopago").prop("readonly", true);
                    $("#pago1").prop("readonly", false);
                    $("#pago2").prop("readonly", false);
                    $("#pago1").autoNumeric("set", 0);
                    $("#pago2").autoNumeric("set", 0);
                    $("#IsSinglePayment").val("false");
                }

                $("input:radio[name=tipo_pago]").click(function () {
                    if ($("input:radio[name=tipo_pago]:checked").val() === "1") {
                        $("#unsolopago").autoNumeric("set", 0);
                        $("#pago1").autoNumeric("set", 0);
                        $("#pago2").autoNumeric("set", 0);
                        $("#pago1").prop("readonly", true);
                        $("#pago2").prop("readonly", true);
                        $("#unsolopago").prop("readonly", false);
                        $("#IsSinglePayment").val("true");
                    }
                    if ($("input:radio[name=tipo_pago]:checked").val() === "2") {
                        $("#unsolopago").autoNumeric("set", 0);
                        $("#pago1").autoNumeric("set", 0);
                        $("#pago2").autoNumeric("set", 0);
                        $("#unsolopago").prop("readonly", true);
                        $("#pago1").prop("readonly", false);
                        $("#pago2").prop("readonly", false);
                        $("#IsSinglePayment").val("false");
                    }

                });

                modal = $("#modal-thirdpayment").modal("show");
            }).fail(function () {
                msgBox("Ha ocurrido un error. Consulte con el Administrador.");
            });
        });

        $("#tblPendingDiets tbody").on("click", "button.editar", function () {
            caseData = tablePendingDiets.row($(this).closest("tr")).data();

            $.ajax({
                url: root + "payments/findbyid/" + caseData.PaymentId,
                type: "GET",
                cache: false,
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) {
                CDI.hideWaitingMessage();

                cleanPendingDietModal();

                $("#dieta_fechadecision").val(response.data.TransactionDate);
                $("#dieta_fvisita").val(response.data.HearingDateIC);
                $("#dieta_fnotificacion").val(response.data.NotificationDateIC);
                $("#dieta_numerocaso").val(response.data.ICCaseNumber);
                $("#dieta_observaciones").val(response.data.Observaciones);

                var newRow = "<tr data-paymentid=\"" + response.data.PaymentId + "\">" +
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
                $("#tblEditPendingDietPeriods tbody").append(newRow);

                var $lastRow = $("#tblEditPendingDietPeriods tbody").find("tr").last();

                var $desde = $lastRow.find("#desde").eq(0);
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
                $desde.val(response.data.FromDate);

                var $hasta = $lastRow.find("#hasta").eq(0);
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
                        cantidad = diffDays * (parseFloat(caseData.CompSemanal.replace("$", "")) / caseData.DiasSemana);
                    }
                    $currentRow.find("#cantidad").autoNumeric("set", cantidad);
                });
                $hasta.datepicker("setEndDate", $("#dieta_fechadecision").datepicker("getDate"));
                $hasta.val(response.data.ToDate);

                $lastRow.find("#cantidad").autoNumeric("init");
                $lastRow.find("#cantidad").val(parseFloat(response.data.TransactionAmount) - parseFloat(response.data.Discount));

                $lastRow.find("#total-pagar").autoNumeric("init");
                $lastRow.find("#total-pagar").val(response.data.TransactionAmount);

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
                $lastRow.find("#descuento").autoNumeric("set", parseFloat(response.data.Discount));

                $lastRow.find("#nrodias").val(response.data.PaymentDay);

                $("#tblEditPendingDietPeriods tbody").off("change").on("change", function () {
                    $.ajax({
                        type: "GET",
                        url: root + "case/getbalance/" + caseData.CaseDetailId,
                        dataType: "json"
                    }).done(function (response) {
                        calculateTotalPendingDiets();

                        var balance = parseFloat(response);

                        var total = $("#dieta_montototal").autoNumeric("get");
                        if (total > balance) {
                            msgBox("El caso no tiene suficiente balance para registrar el pago.");
                        }
                    });
                });

                calculateTotalPendingDiets();

            }).fail(function () {
                CDI.hideWaitingMessage();
            });

            modal = $("#modal-pending-diet").modal("show");
        });

        $("#tblLawyers tbody").on("click", "button.editar", function () {
            var paymentid = $(this).data("paymentid");

            $("#btnUpdateLawyer").on("click", function () {
                var data = {
                    paymentid: $("#paymentlawyerid").val(),
                    transactionid: $("#transactionlawyerid").val(),
                    fechadecision: $("#lawyer_fechadecision").val(),
                    fechavisita: $("#lawyer_fvisita").val(),
                    fechanotificacion: $("#lawyer_fnotificacion").val(),
                    numerocaso: $("#lawyer_numerocaso").val(),
                    montototal: $("#lawyer_montototal").autoNumeric("get"),
                    observaciones: $("#lawyer_observaciones").val()
                };

                $.ajax({
                    url: root + "payments/updatelawyer",
                    data: data,
                    dataType: "json",
                    type: "POST",
                    beforeSend: function () {
                        CDI.showWaitingMessage();
                    }
                }).done(function (response) {
                    CDI.hideWaitingMessage();

                    if (response.data.Status === "OK") {
                        if (modal) {
                            modal.modal("hide");
                        }
                        showTableLawyer();
                    }
                    else {
                        msgBox("Error al actualizar.");
                    }
                }).fail(function () {
                    CDI.hideWaitingMessage();

                    msgBox("Ha ocurrido un error. Consulte con el Administrador.");
                });
            });

            $.ajax({
                url: root + "payments/findbyid/" + paymentid,
                type: "GET",
                cache: false,
                beforeSend: function () {
                    CDI.showWaitingMessage();
                }
            }).done(function (response) {
                CDI.hideWaitingMessage();

                $("#paymentlawyerid").val(response.data.PaymentId);
                $("#transactionlawyerid").val(response.data.TransactionId);
                $("#lawyer_fechadecision").val(response.data.DecisionDate);
                $("#lawyer_fvisita").val(response.data.HearingDateIC);
                $("#lawyer_fnotificacion").val(response.data.NotificationDateIC);
                $("#lawyer_numerocaso").val(response.data.ICCaseNumber);
                $("#lawyer_montototal").autoNumeric("set", response.data.TransactionAmount);
                $("#lawyer_observaciones").val(response.data.Observaciones);

                modal = $("#modal-lawyer").modal("show");
            }).fail(function () {
                CDI.hideWaitingMessage();

                msgBox("Ha ocurrido un error. Consulte con el Administrador.");
            });
        });

        $("#btnAddInvestment").on("click", function (e) {
            e.preventDefault();

            var newRow = "<tr data-paymentid=\"0\" data-entityid=\"0\">" +
                            "<td><button type=\"button\" class=\"btn btn-default btn-sm deleterow\">" +
                            "<i class=\"fa fa-minus\"></i></button></td>" +
                            "<td><input id=\"entidad\" name=\"entidad\" type=\"text\" class=\"form-control\" /></td>" +
                            "<td><input id=\"inversion\" name=\"inversion\" type=\"text\" class=\"form-control text-right inversion\" />" +
                            "</td>" +
                        "</tr>";
            $("#tblEditInvestments tbody").append(newRow);
            $("#tblEditInvestments tbody").find("tr").last().find("#inversion").autoNumeric("init", {
                vMin: "0"
            });

            $("#tblEditInvestments tbody > tr").find("input.inversion").change(function () {
                $.ajax({
                    type: "GET",
                    url: root + "case/getbalance/" + caseData.CaseDetailId,
                    dataType: "json"
                }).done(function (response) {
                    calculateNewInvestments();

                    var balance = parseFloat(response);

                    var total = $("#inv_totalinversiones").autoNumeric("get");
                    if (total > balance) {
                        msgBox("El caso no tiene suficiente balance para registrar el pago");
                    }
                });
            });

            calculateNewInvestments();

        });

        $("#tblEditInvestments").off("click").on("click", ".deleterow", function () {
            $(this).closest("tr").remove();

            //calculateTotalInversion();
        });

        $("#btnUpdateInvestments").on("click", function (e) {
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

                var balance = parseFloat(response);

                var total = $("#inv_totalinversiones").autoNumeric("get");
                if (total > balance) {
                    CDI.displayNotification("El caso no tiene suficiente balance para registrar el pago", "error");
                }
                else {
                    saveInvestments();
                }
            }).fail(function () {
                CDI.hideWaitingMessage();
            });

        });

        $("#btnAddPeriodDiet").on("click", function (evt) {
            evt.preventDefault();

            var newRow = "<tr data-paymentid=\"0\">" +
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
            $("#tblEditPendingDietPeriods tbody").append(newRow);

            var $lastRow = $("#tblEditPendingDietPeriods tbody").find("tr").last();

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
                    cantidad = diffDays * (parseFloat(caseData.CompSemanal.replace("$", "")) / caseData.DiasSemana);
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

            $("#tblEditPendingDietPeriods tbody").change(function () {
                $.ajax({
                    type: "GET",
                    url: root + "case/getbalance/" + caseData.CaseDetailId,
                    dataType: "json"
                }).done(function (response) {
                    calculateTotalPendingDiets();

                    var balance = parseFloat(response);

                    var total = $("#dieta_montototal").autoNumeric("get");
                    if (total > balance) {
                        msgBox("El caso no tiene suficiente balance para registrar el pago.");
                    }


                });
            });

            calculateTotalPendingDiets();

        });

        $("#tblEditPendingDietPeriods").on("click", ".delete-row-dieta", function () {
            $(this).closest("tr").remove();
            calculateTotalPendingDiets();
        });

        $("#btnUpdatePendingDiet").on("click", function (e) {
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

                var balance = parseFloat(response);
                var total = $("#dieta_montototal").autoNumeric("get");
                if (total > balance) {
                    msgBox("El caso no tiene suficiente balance para registrar el pago");
                }
                else {
                    savePendingDiets();
                }
            }).fail(function () {
                CDI.hideWaitingMessage();
            });


        });
    };

    return {
        init: init
    }
})();

$(function () {
    CDI.ApprovalIndex.init();
});