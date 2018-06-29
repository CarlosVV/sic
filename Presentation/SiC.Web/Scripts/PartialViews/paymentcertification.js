CDI.PaymentCertification = (function () {
    var _caseData;
    var concepts;
    var statuses;
    var editor;
    var table;
    var caseInformation;
    var showPaymentInsert;

    var _startFunction = function () {
        $('#info_numerocaso').text(_caseData.NumeroCaso);
        $('#info_lesionado').text(_caseData.Lesionado);
        $('#info_ssn').text(_caseData.SSN);
        $('#info_ebt').text(_caseData.EBT);
        $('#info_faccidente').text(_caseData.FechaAccidente);
        $('#info_fnacimiento').text(_caseData.FechaNacimiento);
        $('#info_ebtstatus').text(_caseData.EBTStatus);
        $('#direccion_linea1').text(_caseData.Direccion.Linea1);
        $('#direccion_linea2').text(_caseData.Direccion.Linea2);
        $('#direccion_ciudad').text(_caseData.Direccion.Ciudad);
        $('#direccion_estado').text(_caseData.Direccion.Estado);
        $('#direccion_zipcode').text(_caseData.Direccion.CodigoPostal);
        $('#info_tipoincapacidad').text(_caseData.TipoIncapacidad);


        $('#info_patrono').text(_caseData.Patrono.Nombre);
        $('#info_ein').text(_caseData.Patrono.EIN);
        $('#info_estatus_patronal').text(_caseData.Patrono.Estatus);
        $('#info_poliza').text(_caseData.Patrono.NumeroPoliza);

        //If beneficiary 
        if (_caseData.EsBeneficiario) {
            $("#info_Beneficiario").remove();
            $("#Label_Beneficiario").remove();
            Label = "<dl id='Label_Beneficiario'><dt>Beneficiario</dt></dl>";
            $("#Beneficiario_div").append(Label);

            Contenedor_Info = " <div id='info_Beneficiario'></div>";
            $("#Beneficiario_div").append(Contenedor_Info);

            BeneficiaryName = "<div class='row'><div class='col-md-3'><dl><dt>Relación</dt><dd>" + _caseData.Beneficiario.Relacion + "</dd></dl></div><div class='col-md-5'><dl><dt>Nombre</dt><dd>" + _caseData.Beneficiario.Nombre + "</dd></dl></div><div class='col-md-4'><dl><dt>Número de Seguro Social</dt><dd>" + _caseData.Beneficiario.SSN + "</dd></dl></div></div>";
            BeneficiaryAccount = "<div class='row'><div class='col-md-3'><dl><dt>Fecha de Nacimiento</dt><dd>" + _caseData.Beneficiario.FechaNacimiento + "</dd></dl></div><div class='col-md-4'><dl><dt>Número de EBT</dt><dd>" + _caseData.Beneficiario.EBT + "</dd></dl></div></div>";
            $("#info_Beneficiario").append(BeneficiaryName);
            $("#info_Beneficiario").append(BeneficiaryAccount);
        } else {
            $("#info_Beneficiario").remove();
            $("#Label_Beneficiario").remove();
        }

        if (_caseData.CasoMenor) {
            $('#info_menor').prop('checked', true);
        }

        if (_caseData.CasoMuerte) {
            $('#info_muerte').prop('checked', true);
        }

//------------------------------------------------------------------------------------------------------------------------
        if ($.fn.dataTable.isDataTable("#tblInsert")) {

            var buttons = [];

            $.each(table.buttons()[0].inst.s.buttons,
                function () {
                    buttons.push(this);
                });
            $.each(buttons,
                function () {
                    table.buttons()[0].inst.remove(this.node);
                });

            table.destroy();
        }

        editor = new $.fn.dataTable.Editor({
            ajax: root + "PaymentCertification/CertificationAdd",
            table: "#tblInsert",
            idSrc: "paymentId",
            fields: [{
                label: "Número de Caso",
                name: "caseNumber"
            }, {
                name: "caseKey",
                type: "hidden"
            }, {
                label: "Número de Factura",
                name: "invoiceNumber"
            }, {
                label: "Monto",
                name: "amount"
            }, {
                label: "Concepto",
                name: "concept",
                type: "select",
                options: concepts
            }, {
                label: "Tipo de Pago",
                name: "status",
                type: "select",
                options: statuses
            }, {
                label: "Desde",
                name: "fromDate",
                type: "datetime",
                format: 'DD-MM-YYYY',
                fieldInfo: 'Formato de fecha: DD-MM-YYYY'
            }, {
                label: "Hasta",
                name: "toDate",
                type: "datetime",
                format: 'DD-MM-YYYY',
                fieldInfo: 'Formato de fecha: DD-MM-YYYY'
            }],
            i18n: {
                create: {
                    button: "Nuevo",
                    title: "Crear nueva certificación",
                    submit: "Crear"
                },
                remove: {
                    button: "Eliminar",
                    title: "Eliminar certificación",
                    submit: "Eliminar",
                    confirm: {
                        _: "¿Está seguro de eliminar %d registros?",
                        1: "¿Está seguro de eliminar 1 registro?"
                    }
                }
            }
        });

        editor.on('initCreate', function () {
            editor.show();
            editor.hide('caseKey');
            editor.hide('fromDate');
            editor.hide('toDate');
            editor.hide('status');
        });

        $('select', editor.field('concept').node()).on('change', function () {
            if (editor.field('concept').val() == "Dieta" || editor.field('concept').val() == "Remanente Dieta") {
                editor.show('fromDate');
                editor.show('toDate');
            }
            else {
                editor.set('fromDate', '');
                editor.set('toDate', '');
                editor.hide('fromDate');
                editor.hide('toDate');
            }
        });

        editor.on('preSubmit', function (e, o, action) {
            var userConfirmSelection = true;
            if (action !== 'remove') {
                var caseNumber = editor.field('caseNumber');
                var amount = editor.field('amount');
                var fromDate = editor.field('fromDate');
                var toDate = editor.field('toDate');
                var invoiceNumber = editor.field('invoiceNumber');
                var concept = editor.field('concept');


                //var caseKey = editor.field('caseKey');

                //No existe la funcion. Funcion consiste de un ajax call para verificar si el caso existe.
                //if (!caseExist(caseNumber.Val())) {
                //    $('#CaseExistWarningModal').modal('show')
                //}

                //Amount Validations
                if (amount.val() <= 0) {
                    amount.error('La cantidad a certificar no puede ser menor o igual a cero.');
                }
                if (amount.val() == "") {
                    amount.error('La cantidad a certificar esta en blanco.');
                }
                //Date Validations
                if (concept.val() == "Dieta" || concept.val() == "Remanente Dieta") {
                    if (!moment(fromDate.val(), 'DD-MM-YYYY', true).isValid()) {
                        fromDate.error('El valor entrado esta en un formato incorrecto. Formato de fecha correcto(dd/MM/yyyy)');
                    }

                    if (!moment(toDate.val(), 'DD-MM-YYYY', true).isValid()) {
                        toDate.error('El valor entrado esta en un formato incorrecto. Formato de fecha correcto(dd/MM/yyyy)');
                    }
                    else if (moment(toDate.val()).isBefore(fromDate.val())) {
                        toDate.error('La fecha entrada es menor que la fecha desde.');
                    }

                    if (moment(fromDate.val()).isSame(toDate.val(), "day")) {
                        toDate.error('Los campos de fecha desde y fecha hasta no pueden ser iguales.');
                    }
                }
                //CaseNumber Validations
                if (caseNumber.val() == "") {
                    caseNumber.error('El campo de número de caso esta en blanco.')
                }
                else if (caseNumber.val().length != 9 && caseNumber.val().length != 11) {
                    caseNumber.error('El número de caso entrado esta incorrecto.');
                }
                else {
                    if (!CaseExist(caseNumber.val())) {
                        caseNumber.error('El número de caso entrado no existe en la base de datos.');
                    }
                    else {
                        userConfirmSelection = CaseRelated(caseNumber.val());
                    }
                }
                if (invoiceNumber.val().length > 9) {
                    invoiceNumber.error('El número de factura no puede ser mayor a 9 caracteres');
                }

                if (this.inError() || !userConfirmSelection) { 
                    return false;
                }
                else {
                    if (caseNumber.val() == _caseDetailData.caseNumber) {
                        editor.set('caseKey', _caseDetailData.caseKey);
                    }
                    else {
                        editor.set('caseKey', '00');
                    }
                }
            }
        });

        editor.on('postCreate', function () {
            var data = table.rows().data();
            var totalAmount = 0;
            for (i = 0; i < data.length; i++) {
                totalAmount += parseFloat(data[i].amount);
            }

            $('#txtTotalAmount').val(numeral(totalAmount).format('0,0.00'));

            if (data.length == 0)
                table.buttons(2).disable();
            else
                table.buttons(2).enable();
        });

        editor.on('postRemove', function () {
            var data = table.rows().data();
            var totalAmount = 0;
            for (i = 0; i < data.length; i++) {
                totalAmount += parseFloat(data[i].amount);
            }

            $('#txtTotalAmount').val(numeral(totalAmount).format('0,0.00'));

            if (data.length == 0)
                table.buttons(2).disable();
            else
                table.buttons(2).enable();
        });

        table = $('#tblInsert').DataTable({
            columns: [
                {
                    data: null,
                    render: function (localData, type, row) {
                        return localData.caseNumber + ' ' + localData.caseKey;
                    }
                },
                {
                    data: "concept"
                },
                {
                    data: "fromDate",
                    render: function (data, type, row) {
                        if (moment(data).format("DD-MM-YYYY") != '01-01-1900') {
                            return moment(data).format("DD-MM-YYYY");
                        }
                        else
                            return null;
                    }
                },
                {
                    data: "toDate",
                    render: function (data, type, row) {
                        if (moment(data).format("DD-MM-YYYY") != '01-01-1900') {
                            return moment(data).format("DD-MM-YYYY");
                        }
                        else
                            return null;
                    }
                },
                {
                    data: "status",
                },
                {
                    data: "invoiceNumber"
                },
                {
                    data: "amount",
                    "class": "text-right",
                    render: function (data, type, row) {
                        return numeral(data).format('$0,0.00');
                    }
                },
                {
                    data: "paymentId",
                    visible: false,
                    searchable: false
                }
            ],
            select: true,
            buttons: [
                { extend: "create", editor: editor },
                { extend: "remove", editor: editor },
                {
                    extend: "selectedSingle",
                    text: "Certificar",
                    action: function (e, dt, node, config) {
                        var dataList = {
                            CaseNumber: _caseDetailData.caseNumber,
                            CaseKey: _caseDetailData.caseKey,
                            Comment: $("#txtComment").val(),
                            Certification: []
                        };
                        for (i = 0; i < dt.data().length; i++) {
                            dataList.Certification.push({
                                CaseNumber: dt.data()[i].caseNumber,
                                InvoiceNumber: dt.data()[i].invoiceNumber,
                                Amount: dt.data()[i].amount,
                                FromDate: dt.data()[i].fromDate,
                                ToDate: dt.data()[i].toDate,
                                Concept: dt.data()[i].concept,
                                Status: dt.data()[i].status,
                                PaymentId: dt.data()[i].paymentId
                            });
                        }


                        $.ajax({
                            url: root + "PaymentCertification/CertificationEditor/",
                            type: "POST",
                            data: dataList,
                            dataType: "json",
                            async: false,
                            success: function (result) {
                                $('#txtTotalAmount').val(numeral(0).format('0,0.00'));
                                $('#txtComment').val('');
                                table.clear().draw();
                                table.draw();
                                alert('Las cantidades fueron certificadas.');
                            },
                            error: function (result) {
                                alert('Occurrio un error al momento de certificar los datos.');
                            },
                            beforeSend: function () {
                                CDI.showWaitingMessage();
                            }
                        }).done(function (response) {
                            CDI.hideWaitingMessage();
                        }).fail(function () {
                            CDI.hideWaitingMessage();
                        });
                    }
                }
            ],
            dom: "Bfrtip",
            paging: false,
            scrollY: "200px",
            scrollCollapse: true,
            ordering: false,
            searching: false,
            language: {
                paginate: {
                    "first": "Primera",
                    "last": "Última",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },
                infoFiltered: " (de un total _MAX_ de registros filtrados)",
                info: "Mostrando del _START_ al _END_ de _TOTAL_ registros",
                infoEmpty: "Se encontraron 0 registros",
                processing: "Buscando <i class='fa fa-spinner fa-spin'></i>",
                lengthMenu: "Mostrar _MENU_ gestiones"
            },
            ajax: {
                url: root + "PaymentCertification/Result",
                dataType: "json",
                type: "POST",
                cache: false,
                async: false,
                data: function (data) {
                    data.caseNumber = _caseDetailData.caseNumber;
                    data.caseKey = _caseDetailData.caseKey;
                    data.caseDetailId = _caseDetailData.caseDetailId;
                    return data;// = JSON.stringify(data);
                }                
            }
        });

        table.on('select', function () {
            var d = table.row({ selected: true }).data();
            editor.s.ajax.data = { "caseNumber": _caseData.NumeroCaso, "id": d["paymentId"] };

            table.buttons(1).disable(d.status != "Descuento");

            table.buttons(1).enable(d.status == "Descuento");
        });

        table.on('draw.dt', function (e, setting) {
            if (setting.json.data.length == 0)
                table.buttons(2).disable();
            else
                table.buttons(2).enable();
        });

        table.on('xhr.dt', function (e, settings, json, xhr) {
            var amount = 0;

            for (i = 0; i < json.data.length; i++) {
                amount += json.data[i].amount;
            }
            $('#txtTotalAmount').val(numeral(amount).format('0,0.00'));
        });

    }
//------------------------------------------------------------------------------------------------------------------------
    var CaseExist = function (caseNumber) {
        var json = { "caseNumber": caseNumber };
        var caseExist;
        $.ajax({
            url: root + "PaymentCertification/CaseExist/",
            type: "POST",
            data: json,
            dataType: "json",
            async: false,
            success: function (result) {
                caseExist = result.caseExist;
            },
            error: function (result) {
                caseExist = false;
            }
        });

        return caseExist;
    }

    var CaseRelated = function (caseNumber) {
        if (_caseDetailData.caseNumber == caseNumber.trim()) {
            return true;
        }
        else {
            var json = { "caseNumber": caseNumber, "searchNumber": _caseDetailData.caseNumber };
            var caseRelated;
            $.ajax({
                url: root + "PaymentCertification/CaseRelated/",
                type: "POST",
                data: json,
                dataType: "json",
                async: false,
                success: function (result) {
                    if (!result.caseRelated) {
                        //Si escoge continuar el caso entrado se trata como un caso relacionado para efectos de la logica.
                        caseRelated = confirm("El caso a descontar no está relacionado con el caso buscado. ¿Desea continuar?");
                    }
                    else {
                        caseRelated = true;
                    }
                },
                error: function (result) {
                    alert('Se encontro un error con la busqueda de casos relacionados.');
                    caseRelated = false;
                }
            });
            return caseRelated;
        }
    }

    function GetAllStatus() {
        $.ajax({
            url: root + "PaymentCertification/GetAllStatus/",
            type: "GET",
            dataType: "json",
            async: false,
            success: function (result) {
                statuses = result;
            },
            error: function (result) {
                alert(result.error);
            }
        });
    }

    function GetAllConcept() {
        $.ajax({
            url: root + "PaymentCertification/GetAllConcept/",
            type: "GET",
            dataType: "json",
            async: false,
            success: function (result) {
                concepts = result;
            },
            error: function (result) {
                alert(result.error);
            }
        });
    }

    var CaseDetailInfo = function (caseDetailId) {
        var json = { "caseDetailId": caseDetailId };

        $.ajax({
            url: root + "PaymentCertification/CaseDetailInfo/",
            type: "POST",
            data: json,
            dataType: "json",
            async: false,
            success: function (result) {
                if (result.success == true) {
                    _caseDetailData.caseNumber = result.caseNumber;
                    _caseDetailData.caseKey = result.caseKey;
                    _caseDetailData.caseDetailId = result.caseDetailId;
                }
            },
            error: function (result) {
                alert("Error: Función interna 'CaseDetailInfo' encontro un problema con el caso buscado.");
            }
        });
    }

    var eventFired = function () {
        if (table != undefined) {
            table.rows().deselect();
        }
    }

    var _bindUIActions = function () {
        $(CDI.CaseSearch).on('case.selected', function (e, data) {
            _caseData = data.selectedCase;
            CaseDetailInfo(_caseData.CaseDetailId);

            GetAllConcept();
            GetAllStatus();
            _startFunction();
            moment.locale('en');

            $("#certification-panel").fadeIn(1000).removeClass("hidden");

            $('html, body').animate({
                    scrollTop: $("#certification-panel").offset().top - 100
                }, 500);
        });
    }

    var _init = function () {
        _bindUIActions();
    };

    return {
        init: _init
    };

})();

$(function () {
    CDI.CaseSearch.init();
    CDI.PaymentCertification.init();
});