
var resultTable;

var msgBox = function (msg) {
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

var showCasesForSelect = function () {

    $("#CasesTableResult tbody").find("tr").remove();

    if ($("#resultsPanel").css('display') == 'none')
        $("#resultsPanel").show();

    if ($("#CasesTableResult").css('display') == 'none') {
        $("#CasesTableResult").show();
    }

    if (!resultTable) {
        resultTable = $("#CasesTableResult").DataTable({
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
                //url: root + "case/search",
                url: root + "Preexisting/SearchForOtherRelatedCases",
                contentType: 'application/json; charset=utf-8',
                type: "POST",
                data: function (data) {
                    data.EntityName = $("#fullname_form").val();
                    data.SocialSecurityNumber = $("#ssn_form").val();
                    data.BirthDate = $("#birthdate_form").val();
                    data.CaseNumber = $("#casenumber_form").val();
                    data.FilingDate = $("#filingdate_form").val();
                    data.RegionId = $("#region_form").val() == 0? null:$("#region_form").val();
                    data.ClinicId = $("#clinic_form").val() == 0 ? null : $("#clinic_form").val();
                    data.EBTNumber = $("#ebtnumber_form").val();
                    data.CurrentCaseNumber = $("#CaseNumber").val();
                    data.Length = 10;

                    return data = JSON.stringify(data);
                }
            },
            "order": [[1, "desc"]],
            "columns": [

                         {
                             "class": "",
                             "orderable": false,
                             "data": "IsRelated",

                             render: function (data, type, row) {
                                 var rtn = '';
                                 // If display or filter data is requested, format the date
                                 if (type === 'display' || type === 'filter') {
                                     if (!data)
                                         rtn = " \
                                                        <button type=\"button\" class=\"btn btn-default btn-xs addcasefromresult\" aria-label=\"Left Align\"> \
                                                          <span class=\"glyphicon glyphicon-ok\" aria-hidden=\"true\"></span>  \
                                                        </button> "
                                     else
                                         rtn = ""
                                 }
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
                            "data": "EBT"
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
            "dom": "rtp"
            //"deferLoading": 0
        });

        $('#CasesTableResult tbody').on('click', '.addcasefromresult', addCaseFromSeachResult);
       // resultTable.ajax.reload();
    }
    else
        resultTable.ajax.reload();
}

var clearCasesForSelect = function () {
    $("#fullname_form").val("");
    $("#ssn_form").val(""); 
    $("#birthdate_form").val("");
    $("#casenumber_form").val("");
    $("#filingdate_form").val("");
    $("#region_form").val("");
    $("#clinic_form").val("");
    $("#ebtnumber_form").val("");
}

var clearResultTable = function () {
 

    clearCasesForSelect();

    $("#CasesTableResult tbody").find("tr").remove();
    $("#CasesTableResult").hide();
    

    if (!resultTable) {
        x = 1;
    } else {
        resultTable.destroy();
        resultTable = null;
        
    }

    //showCasesForSelect;
}

var addCaseFromSeachResult = function () {

    var tr = $(this).closest('tr');
    var row = resultTable.row(tr);
    var CaseId = row.data().CaseId;
    var PreexistingCaseNumber = row.data().CaseNumber;

    var data = {
        "CaseNumber": _caseData.NumeroCaso,
        "PreexistingCaseNumber": PreexistingCaseNumber
    };

    $.ajax({
        url: root + "PreExisting/AddPreexistingCase",
        data: data,
        dataType: 'json',
        type: "POST",
        success: function (data) {
            if (data.data.Status == 'OK') {
                msgBox('Datos guardados');

                PreCasesTable.ajax.reload();
                CDI.PreExisting.loadAwardAmount();
                //loadAwardAmount();
                resultTable.ajax.reload();
            }
            else {
                msgBox('Error al grabar datos.');
            }
        },
        error: function () {
            //closeProgress();
            msgBox('Ha ocurrido un error. Consulte con el Administrador.');
        }
    });

    
}

$(function () {

    $("#searchBtnForSelectCase").on("click", showCasesForSelect);

    $("#clearBtnForSelectCase").on("click", clearResultTable);

    $('#searchModal').on('hidden.bs.modal', clearResultTable);

});















//var searchdata = function (data) {
//    //return {
//    //    "casenumber": $("#casenumber_form").val(),
//    //    "name": $("#fullname_form").val(),
//    //    "ssn": $("#ssn_form").val(),
//    //    "birthdate": $("#birthdate_form").val(),
//    //    "casedate": $("#filingdate_form").val(),
//    //    "region": $("#region").val(),
//    //    "clinic": $("#clinic").val(),
//    //};
//}

//var showResults = function () {

//    if ($("#resultsPanel").css('display') == 'none')
//        $("#resultsPanel").show();

//    if ($("#CasesTableResult").css('display') == 'none')
//        $("#CasesTableResult").show();

//    if (!resultTable) {
//        resultTable = $("#CasesTableResult").DataTable({
//            //"processing": true,
//            //"autoWidth": false,
//            //"drawCallback": function (settings) {
//            //    $(".dataTables_empty").html('No se encontraron casos.');
//            //},
//            //"language": {
//            //    "paginate": {
//            //        "first": "Primera",
//            //        "last": "Última",
//            //        "next": "Siguiente",
//            //        "previous": "Anterior"
//            //    },
//            //    "infoFiltered": " (de un total _MAX_ de registros filtrados)",
//            //    "info": "Mostrando del _START_ al _END_ de _TOTAL_ registros",
//            //    "infoEmpty": "Se encontraron 0 registros",
//            //    "processing": "Buscando <i class='fa fa-spinner fa-spin'></i>",
//            //    "lengthMenu": "Mostrar _MENU_ gestiones"
//            //},
//            "ordering": true,
//            "autoWidth": false,
//            "pagingType": "simple_numbers",
//            "processing": true,
//            "paging": true,
//            "deferRender": true,
//            "serverSide": true,
//            "drawCallback": function (settings) {
//                $(".dataTables_empty").html('No se encontraron registros.');
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
//                "lengthMenu": "Mostrar _MENU_"
//            },
//            "ajax": {
//                "url": "/Search/SearchCase",
//                "type": "POST",
//                "data": searchdata
//            },
//            "order": [
//                [1, 'desc']
//            ],
//            "columns": [
//                        {
//                            "data": "CaseNumber"
//                        },
//                        {
//                            "data": "Name"
//                        },
//                        {
//                            "data": "SSN"
//                        },
//                        {
//                            "data": "BirthDate"
//                        },
//                        {
//                            "data": "CaseDate"
//                        },
//                        {
//                            "data": "Region"
//                        },
//                        {
//                            "data": "Clinic"
//                        }

//            ],
//            "dom": "rtp",
//            "deferLoading": 0
//        });

//        $('#CasesTableResult tbody').on('click', 'tr', function () {
//            var mydata = resultTable.row(this).data();
//            var mySelectedRow = mydata.CaseNumber;
//            window.location.href = _model.Url + mySelectedRow;
//        });
//    }
//    else
//        resultTable.ajax.reload();
//}