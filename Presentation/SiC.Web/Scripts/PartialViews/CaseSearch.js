//var myTable;
//var nombre;
//var ssn;
//var ebt;
//var numeroCaso;

//var data = function () {
//    return {
//        "nombre":  $("#name").val(),
//        "sSN": $("#ssn").val(),
//        "eBT": $("#ebt").val(),
//        "numeroCaso": $("#caso").val()
//    };
//}


//var showResults = function () {
     
//    if ($("#resultsPanel").css('display') == 'none')
//        $("#resultsPanel").show();

//    if (!myTable) {
//        myTable = $("#CasesTable").DataTable({
//            "processing": true,
//            "autoWidth": false,
//            "drawCallback": function (settings) {
//                $(".dataTables_empty").html('No se encontraron casos.');
//            },
//            "language": {
//                "paginate": {
//                    "first": "Primera",
//                    "last": "Última",
//                    "next": "Siguiente",
//                    "previous": "Anterior"
//                },
//                "infoFiltered": " (de un total _MAX_ de registros filtrados)",
//                "info": "Mostrando del _START_ al _END_ de _TOTAL_ registros",
//                "infoEmpty": "Se encontraron 0 registros",
//                "processing": "Buscando <i class='fa fa-spinner fa-spin'></i>",
//                "lengthMenu": "Mostrar _MENU_ gestiones"
//            },
//            "ajax": {
//                "url": "/Payments/Search",
//                "type": "POST",
//                "data": data
//            },
//            "order": [
//                [1, 'desc']
//            ],
//            "columns": [
//            {
//                "data": "Nombre"
//            },
//            {
//                "data": "SSN"
//            },
//            {
//                "data": "EBT"
//            },
//            {
//                "data": "NumeroCaso"
//            }
//            ],
//            "dom": "rtp"
//        });
//    }
//    else
//        myTable.ajax.reload();
//}


//$(function () {
//    $("#searchBtn").on("click", showResults);

//    $("#searchPanel input").keyup(function (event) {
//        if (event.keyCode == 13) {
//            $("#searchBtn").click();
//        }
//    });

//    $("#clearBtn").on("click", function () {
//        $('#searchPanel :input').each(function () {
//            this.value = "";
//        });

//        $('#searchPanel').find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");
//    });

//    $('#CasesTable tbody').on('click', 'tr', function () {
//        //window.alert("sometext");
//        var mydata = myTable.row(this).data()
//        mySelectedRow = mydata;
//        if (mydata.CaseFolderId) {
//            window.location.href = window.location.href;
//            window.location.href = "/Payments/Resumen/" + mydata.CaseFolderId;
//        }
//    });
//});

var table = "";
var _caseData;

CDI.PaymentHistory = (function () {

    var _showCaseInformation = function () {
        if (_caseData.FromCase) {
            var _conceptTable;
            var _beneficiaryTable;

            //window.location.href = window.location.href;
            // window.location.href = "/Payments/Resumen/" + _caseData.CaseFolderId;

            if ($.fn.dataTable.isDataTable("#beneficiaryTable")) {
                _beneficiaryTable.destroy();
            }

            if ($.fn.dataTable.isDataTable("#conceptTable")) {
                _conceptTable.destroy();
            }

            _conceptTable = $("#conceptTable").DataTable({
                "processing": true,
                "autoWidth": false,
                "drawCallback": function (settings) {
                    $(".dataTables_empty").html('No se encontraron pagos.');
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
                    "url": root + "Payments/ChequesPorConcepto",
                    "type": "GET",
                    "data": { "caseNumber": _caseData.CaseFolderId }
                },
                "order": [
                    [1, 'desc']
                ],
                "columns": [
                    {
                        "data": "Concepto"
                    },
                    {
                        "data": "Pagado",
                        "class": "text-right",
                        "render": function (data, type, row, meta) {
                            return numeral(data).format('$0,0.00');
                        }
                    },
                    {
                        "data": "No_Cobrado",
                        "class": "text-right",
                        "render": function (data, type, row, meta) {
                            return numeral(data).format('$0,0.00');
                        }
                    }
                ],
                "dom": "rt"
            });

            _beneficiaryTable = $("#beneficiaryTable").DataTable({
                "processing": true,
                "autoWidth": false,
                "drawCallback": function (settings) {
                    $(".dataTables_empty").html('No se encontraron pagos.');
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
                    "url": root + "Payments/ChequesPorBeneficiario",
                    "type": "GET",
                    "data": { "caseNumber": _caseData.CaseFolderId }
                },
                "order": [
                    [1, 'desc']
                ],
                "columns": [
                    {
                        "data": "Beneficiario"
                    },
                    {
                        "data": "Pagado",
                        "class": "text-right",
                        "render": function (data, type, row, meta) {
                            return numeral(data).format('$0,0.00');
                        }
                    },
                    {
                        "data": "No_Cobrado",
                        "class": "text-right",
                        "render": function (data, type, row, meta) {
                            return numeral(data).format('$0,0.00');
                        }
                    }
                ],
                "dom": "rt"
            });

           _expandPanel($("#searchPanel .panel-heading a.collapse-click"));

        }

    }


    var _expandPanel = function (button) {
        if (button.hasClass('panel-collapsed')) {
            button.parents('.panel').find('.panel-body').slideDown();
            button.removeClass('panel-collapsed');
            button.find('i').removeClass('fa-chevron-down').addClass('fa-chevron-up');
        }
    }

    var _collapsePanel = function (button) {
        if (!button.hasClass('panel-collapsed')) {
            button.parents('.panel').find('.panel-body').slideUp();
            button.addClass('panel-collapsed');
            button.find('i').removeClass('fa-chevron-up').addClass('fa-chevron-down');

        }
    }

    var _bindUIActions = function () {
        $(CDI.CaseSearch).on('case.selected', function (e, data) {
            _caseData = data.selectedCase;
            _showCaseInformation();
            CDI.CaseSearch.header(_caseData);
            $("#PaymentSearch").fadeIn(1000).removeClass("hidden");
        });

        $(CDI.CaseSearch).on('started', function (e, data) {
            $("#PaymentSearch").fadeIn(1000).addClass("hidden");
        });

        $(".panel-heading").on("click", "a-collapse-click", function (evt) {
            evt.preventDefault();

            var $this = $(this);

            if ($this.hasClass('panel-collapsed')) {
                _expandPanel($this);
            } else {
                _collapsePanel($this);
            }
        });
    }

    var _init = function () {
        _bindUIActions();
    };

    return {
        init: _init
    }
})();


$(document).ready(function () {
    CDI.CaseSearch.init();
    CDI.PaymentHistory.init();
});