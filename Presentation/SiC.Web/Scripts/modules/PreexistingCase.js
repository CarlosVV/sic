var PreCasesTable;
var OtherCasesTable;
var RelatedCasesByCompensationRegionTable;
var DetailtTable;
var TotalDetail;
var TransactionTable;
var TransactionTableObjs = {};
var _caseData;
var drawPreexisting = 0;

CDI.PreExisting = (function () {
    var _msgBox = function (msg) {
        $('#msg span').text(msg);
        $('#modalMsg').modal('show');
    }

    var showProgress = function (msg) {
        $('#msg span').text(msg);
        winp = $('#winprogress').modal('show');
    }

    var closeProgress = function () {

        winp.modal('hide');
    }

    var data = function () {
        return {
            "CaseNumber": _caseData.NumeroCaso
        };
    }

    var showPreexistingCases = function () {

        if ($("#PreexistingCases").css('display') == 'none')
            $("#PreexistingCases").show();

        if (!PreCasesTable) {
            PreCasesTable = $("#PreCasesTable").DataTable({
                "processing": true,
                "autoWidth": false,
                "drawCallback": function (settings) {
                    $(".dataTables_empty").html('No se encontraron casos.');
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
                    "processing": "Cargando <i class='fa fa-spinner fa-spin'></i>",
                    "lengthMenu": "Mostrar _MENU_ gestiones"
                },
                "ajax": {
                    "url": root + "PreExisting/SearchRelatedCases",
                    "type": "POST",
                    "data": data
                },
                "order": [[1, "desc"]],
                "columns": [
                            {
                                "class": "",
                                "orderable": false,
                                "data": "HideDelete",
                                "defaultContent": "",
                                render: function (data, type, row) {
                                    var rtn = '';
                                    // If display or filter data is requested, format the date
                                     if (type === 'display' || type === 'filter') {
                                         if (!data)
                                             rtn =  ' \
                                                    <button type=\"button\" class=\"btn btn-default btn-xs remove\" aria-label=\"Left Align\"> \
                                                    <span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>  \
                                                    </button> \
                                                    <button type=\"button\" class=\"btn btn-default btn-xs showTransactions\" aria-label=\"Left Align\"> \
                                                    <span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>  \
                                                    </button> '
                                         else 
                                             rtn = ' \
                                                    <button type=\"button\" class=\"btn btn-default btn-xs showTransactions\" aria-label=\"Left Align\"> \
                                                    <span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>  \
                                                    </button> \ '
                                     }
                                    // Otherwise the data type requested (`type`) is type detection or
                                    // sorting data, for which we want to use the integer, so just return
                                    // that, unaltered
                                     return rtn;
                                }
                            },
                            {
                                "data": "CaseNumber",
                                className: "text-center"
                            },
                            {
                                "data": "FullName",
                                className: "text-center"
                            },
                            {
                                "data": "SSN",
                                className: "text-center",
                                render: function (data, type, row) {

                                    // If display or filter data is requested, format the date
                                    if (type === 'display' || type === 'filter') {
                                        var ssn3 = data.substring(0, 3);
                                        var ssn2 = data.substring(3, 5);
                                        var ssn4 = data.substring(5, 9);
                                        return ssn3+"-"+ssn2+"-"+ssn4;
                                    }

                                    // Otherwise the data type requested (`type`) is type detection or
                                    // sorting data, for which we want to use the integer, so just return
                                    // that, unaltered
                                    return data;
                                }
                            },
                            {
                                "data": "Adjudication",
                                className: "text-right",
                                render: function (data, type, row) {
                                    return numeral(data).format("$0,00.00");
                                }
                            },
                            {
                                "data": "CaseId",
                                "visible": false
                            }
                ],
                select: 'single',
                "dom": "rtip"
            });
        }
        else
            PreCasesTable.ajax.reload();
    }

    var showOtherPreexistingCases = function () {

        if ($("#OtherPreexistingCases").css('display') == 'none')
            $("#OtherPreexistingCases").show();

        if (!OtherCasesTable) {
            OtherCasesTable = $("#OtherCasesTable").DataTable({
                "processing": true,
                "autoWidth": false,
                "drawCallback": function (settings) {
                    $(".dataTables_empty").html('No se encontraron casos.');
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
                    "processing": "Cargando <i class='fa fa-spinner fa-spin'></i>",
                    "lengthMenu": "Mostrar _MENU_ gestiones"
                },
                "ajax": {
                    "url": root + "PreExisting/SearchOtherRelatedCases",
                    "type": "POST",
                    "data": data
                },
                "order": [[1, "desc"]],
                "columns": [
                            {
                                "class": "",
                                "orderable": false,
                                "data": null,
                                "defaultContent": " <button type=\"button\" class=\"btn btn-default btn-xs addcase\" aria-label=\"Left Align\"> \
                                                    <span class=\"glyphicon glyphicon-ok\" aria-hidden=\"true\"\"></span>  \
                                                    </button> "
                                                    //<button type=\"button\" class=\"btn btn-default btn-xs showOthersTransactions\" aria-label=\"Left Align\"> \
                                                    //<span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>  \
                                                    //</button> "
                            },
                            {
                                "data": "CaseNumber",
                                className: "text-center"
                            },
                            {
                                "data": "FullName",
                                className: "text-center"
                            },
                            {
                                "data": "SSN",
                                className: "text-center",
                                render: function (data, type, row) {

                                    // If display or filter data is requested, format the date
                                    if (type === 'display' || type === 'filter') {
                                        var ssn3 = data.substring(0, 3);
                                        var ssn2 = data.substring(3, 5);
                                        var ssn4 = data.substring(5, 9);
                                        return ssn3 + "-" + ssn2 + "-" + ssn4;
                                    }

                                    // Otherwise the data type requested (`type`) is type detection or
                                    // sorting data, for which we want to use the integer, so just return
                                    // that, unaltered
                                    return data;
                                }
                            },
                            {
                                "data": "Adjudication",
                                className: "text-right",
                                render: function (data, type, row) {
                                     return numeral(data).format("$0,00.00");
                                }
                            },
                            {
                                "data": "CaseId",
                                "visible": false
                            }
                ],
                select: 'single',
                "dom": "rtip"
            });


        }
        else
            OtherCasesTable.ajax.reload();
    }

    var showTransactions = function () {

        var tr = $(this).closest('tr');
        var row = PreCasesTable.row(tr);
        var CaseId = row.data().CaseId;
        var buttom = $(tr).find('.showTransactions .glyphicon');

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
            buttom.removeClass('glyphicon-minus');
            buttom.addClass('glyphicon-plus');
        }
        else {
            row.child(getTransactionTable(row.data())).show();
            tr.addClass('shown');
            buttom.removeClass('glyphicon-plus');
            buttom.addClass('glyphicon-minus');
            loadTransactionTable(row);
        }
    }

    var _showOthersTransactions = function () {
        //var mydata = OtherCasesTable.row(this.closest("tr")).data();
        //var mySelectedRow = mydata.CaseNumber;
        //alert(mySelectedRow);

        var tr = $(this).closest('tr');
        var row = OtherCasesTable.row(tr);
        var CaseId = row.data().CaseId;
        var buttom = $(tr).find('.showOthersTransactions .glyphicon');

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
            buttom.removeClass('glyphicon-minus');
            buttom.addClass('glyphicon-plus');
        }
        else {
            row.child(getTransactionTable(row.data())).show();
            tr.addClass('shown');
            buttom.removeClass('glyphicon-plus');
            buttom.addClass('glyphicon-minus');
            loadTransactionTable(row);
        }
    }

    var getTransactionTable = function (d) {

        return '<table id="TransactionTable' + d.CaseId + '" class="table table-striped table-bordered table-hover"> ' +
                    '<thead> ' +
                        '<tr class="active"> ' +
                            '<th></th>' +
                            '<th>Fecha de Adjudicación</th> ' +
                            '<th>Tipo de Adjudicación</th> ' +
                            '<th>Adjudicación</th> ' +
                        '</tr> ' +
                    '</thead> ' +
                    '<tbody></tbody> ' +
                    '<tfoot></tfoot> ' +
                '</table> '
    }

    var loadTransactionTable = function (d) {

        var CaseId = d.data().CaseId

        var dta = {
            "CaseId": CaseId
        };

        var table = "#TransactionTable" + CaseId;

        var TempTransactionTable = $(table).DataTable({
            "processing": true,
            "autoWidth": false,
            "drawCallback": function (settings) {
                $(".dataTables_empty").html('No se encontraron transaciones.');
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
                //"url": "../GetTransaction",
                "url": root + "PreExisting/GetTransaction",
                "type": "POST",
                "data": dta
            },
            "order": [[1, "desc"]],
            "columns": [
                        {
                            "class": "",
                            "orderable": false,
                            "data": null,
                            "defaultContent": " \
                            <button type=\"button\" class=\"btn btn-default btn-xs showTransactionDetail\" aria-label=\"Left Align\"> \
                              <span class=\"glyphicon glyphicon-th-list\" aria-hidden=\"true\"\"></span>  \
                            </button>"
                        },
                        {
                            "data": "TransactionDate",
                            className: "text-center",
                            render: function (data, type, row) {
                                 if (data != "N/A")
                                     return moment(data).format("DD-MM-YYYY");
                                 else
                                     return data;
                            }
                        },
                        {
                            "data": "TransactionType"
                        },
                        {
                            "data": "TransactionAmount",
                            className: "text-right",
                            render: function (data, type, row) {
                                return numeral(data).format("$0,00.00");
                            }
                        },
                        {
                            "data": "TransactionId",
                            "visible": false
                        },
                        {
                            "data": "CaseNumber",
                            "visible": false
                        },
                        {
                            "data": "CaseId",
                            "visible": false
                        }
            ],
            select: 'single',
            "dom": "rt"
        });

        TransactionTableObjs[table] = TempTransactionTable;
    }

    var _showTransactionDetail = function () {

        var tr = $(this).closest('tr');
        var row = TransactionTableObjs['#'+tr.parent().parent()[0].id].row(tr);
        //var row = TransactionTable.row(tr);
        var mydata = row.data();
        var Total = mydata.TransactionAmount;
        var TransactionId = mydata.TransactionId;
        var LoadCaseNumber = mydata.CaseNumber;
        var CaseId = mydata.CaseId;
        

        showTransactionDetailModal(LoadCaseNumber, TransactionId, Total);
    }

    var _loadAwardAmount = function () {

        $.ajax({
            url: root + "PreExisting/GetTotalAdjudicationAmount/",// + _caseData.CaseId,
            type: "POST",
            data: {
                "CaseId": _caseData.CaseId
            },
        }).done(function (response) {
            $("#AdjudicationAmount").val(numeral(response.data.AdjudicationAmount).format("$0,00.00"));
            $("#OtherAdjudicationAmount").val(numeral(response.data.OtherAdjudicationAmount).format("$0,00.00"));
            $("#AdjudicationIW").val(numeral(response.data.AdjudicationIW).format("$0,00.00"));
        }).fail(function (response, err, msg) {

        });
    }

    var _showRelatedCasesByCompensationRegion = function () {

        if ($("#RelatedCasesByCompensationRegionTable").css('display') == 'none')
            $("#RelatedCasesByCompensationRegionTable").show();

        if (!RelatedCasesByCompensationRegionTable) {
            RelatedCasesByCompensationRegionTable = $("#RelatedCasesByCompensationRegionTable").DataTable({
                "processing": true,
                "autoWidth": false,
                "drawCallback": function (settings) {
                    $(".dataTables_empty").html('No se encontraron casos.');
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
                    "processing": "Cargando <i class='fa fa-spinner fa-spin'></i>",
                    "lengthMenu": "Mostrar _MENU_ gestiones"
                },
                "ajax": {
                    "url": root + "PreExisting/SearchRelatedCasesByCompensationRegion",
                    "type": "POST",
                    "data": data
                },
                "order": [[0, "desc"], [1, "asc"]],
                "columns": [
                            {
                                data: "CaseNumber",
                                className: "text-center",
                                defaultContent: "",
                            },
                            {
                                data: "Region",
                                className: "text-center",
                                defaultContent: "",
                            },
                            {
                                data: "Code",
                                className: "text-center",
                                defaultContent: "",
                            },
                            {
                                data: "Percent",
                                className: "text-center",
                                defaultContent: "",
                                render: function (data, type, row) {
                                    var rtn;
                                    if (type === 'display' || type === 'filter') {
                                        if (data > 0) {
                                            rtn = parseInt(data * 100) + '%'
                                        }
                                    }
                                    return rtn;
                                }
                            },
                            //{
                            //    data: "Amount",
                            //    className: "text-right",
                            //    render: function (data, type, row) {
                            //        return numeral(data).format("$0,00.00");
                            //    }
                            //},
                            {
                                data: "CaseId",
                                visible: false
                            },
                            {
                                data: "CaseDetailId",
                                visible: false
                            },
                            {
                                data: "TransactionId",
                                visible: false
                            },
                            {
                                data: "TransactionDetailId",
                                visible: false
                            }
                ],
                select: 'single',
                "dom": "rtip"
            });
        }
        else
            RelatedCasesByCompensationRegionTable.ajax.reload();
    }

    var remove = function () {
       // var mydata = PreCasesTable.row(this.closest("tr")).data();

        var tr = $(this).closest('tr');
        var row = PreCasesTable.row(tr);
        var CaseId = row.data().CaseId;
        var PreexistingCaseNumber = row.data().CaseNumber;

        //var PreexistingCaseNumber = mydata.CaseNumber;

        var data = {
            "CaseNumber": $('#CaseNumber').val(),
            "PreexistingCaseNumber": PreexistingCaseNumber,
            "Status": ""
        };

        showProgress("Removiendo...");
        $.ajax({
            url: root + "PreExisting/RemovePreexistingCase",
            data: data,
            dataType: 'json',
            type: "POST",
            success: function (data) {
                closeProgress();
                if (data.data.Status == 'OK') {
                    _msgBox('Removido');

                    PreCasesTable.ajax.reload();
                    OtherCasesTable.ajax.reload();
                }
                else {
                    _msgBox('Error al grabar datos.');
                }
            },
            error: function () {
                closeProgress();
                _msgBox('Ha ocurrido un error. Consulte con el Administrador.');
            }
        });

        _loadAwardAmount();
    }

    var addcase = function () {
       // var mydata = OtherCasesTable.row(this.closest("tr")).data();
       // var PreexistingCaseNumber = mydata.CaseNumber;


        var tr = $(this).closest('tr');
        var row = OtherCasesTable.row(tr);
        var CaseId = row.data().CaseId;
        var PreexistingCaseNumber = row.data().CaseNumber;

        var data = {
            "CaseNumber": $('#CaseNumber').val(),
            "PreexistingCaseNumber": PreexistingCaseNumber
            //"Status": ""
        };

        showProgress("Añadiendo...");
        $.ajax({
            url: root + "PreExisting/AddPreexistingCase",
            //url: "../AddPreexistingCase",
            data: data,
            dataType: 'json',
            type: "POST",
            success: function (data) {
                closeProgress();
                if (data.data.Status == 'OK') {
                    _msgBox('Datos guardados');
                    //limpiar_campos();
                    PreCasesTable.ajax.reload();
                    OtherCasesTable.ajax.reload();
                    _loadAwardAmount();
                }
                else {
                    _msgBox('Error al grabar datos.');
                }
            },
            error: function () {
                closeProgress();
                _msgBox('Ha ocurrido un error. Consulte con el Administrador.');
            }
        });
    }

    var _loadCase = function () {

        $(CDI.CaseSearch).on('case.selected', function (e, data) {
            _caseData = data.selectedCase;

            CDI.CaseSearch.header(_caseData);

            _loadAwardAmount();

            showPreexistingCases();

            showOtherPreexistingCases();

            $("#CaseInformation").fadeIn(1000).removeClass("hidden");
            $("#header-panel").fadeIn(1000).removeClass("hidden");
        });

        $(CDI.CaseSearch).on('started', function (e, data) {
            $("#CaseInformation").fadeOut(1000).addClass("hidden");
            $("#header-panel").fadeOut(1000).addClass("hidden");
        });

        $('#PreCasesTable tbody').on('click', '.showTransactions', showTransactions);
        $('#PreCasesTable tbody').on('click', '.remove', remove);
        $('#PreCasesTable tbody').on('click', '.showTransactionDetail', _showTransactionDetail);
        $('#OtherPreexistingCases  tbody').on('click', '.addcase', addcase);
        $('#OtherPreexistingCases  tbody').on('click', '.showOthersTransactions', _showOthersTransactions);
    }

    return {
        init: _loadCase,
        loadAwardAmount: _loadAwardAmount,
        showTransactionDetail: _showTransactionDetail,
        showRelatedCasesByCompensationRegion:_showRelatedCasesByCompensationRegion
    }
})();


$(document).ready(function () {

    CDI.CaseSearch.init();
    CDI.PreExisting.init();

    $("#searchOtherBtn").on("click", function (evt) {

        $('#searchModal').modal({
            backdrop: 'static',
            keyboard: false
        })
        evt.preventDefault();
    });

    $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
        if (e.target.innerHTML == "Por Región Anatómica") {
            CDI.PreExisting.showRelatedCasesByCompensationRegion();
        }
    });

    
});



//var _showTransactionDetailModal = function(CaseNumber) {
//    $("#TransactionDetailPartialView").load(root + "PreExisting/GetTransactionDetail/" + CaseNumber, function () {
//        $('#TransactionDetailModal').modal({
//            backdrop: 'static',
//            keyboard: false
//        })
//    });
//}