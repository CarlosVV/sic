var compensationRegion;
var regionDropdown;
var codeDropdown;
var weeks;
var transactionIdToDetail;
var totalAmountToDetail;



var showTransactionDetailModal = function (CaseNumber,TransactionId,Amount) {

    transactionIdToDetail = TransactionId;
    totalAmountToDetail = Amount;

    if (Amount != 0 && (_caseData.CompSemanalInca > 0 || (_caseData.TipoIncapacidad == "ITP" && _caseData.CompSemanal > 0))) {

        if (!DetailtTable) {
            x = 1;
        } else {
            DetailtTable.destroy();
            DetailtTable = null;
        }

        showTransactionDetailtTable(TransactionId);
        $('#detailModal').modal({
            backdrop: 'static',
            keyboard: false
        })

        $('#detailModalLabel').text('Desglose de caso: ' + CaseNumber);

    } else {
        $('#noDetailModal').modal('show');
        $('#noDetailModalLabel').text('Desglose caso: ' + CaseNumber);
    }
}

var showTransactionDetailtTable = function (TransactionId) {

    if (!DetailtTable) {

        DetailtTable = $("#DetailtTable").DataTable({
            "processing": true,
            "autoWidth": false,
            "drawCallback": function (settings) {
                $(".dataTables_empty").html('No se encontraron datos.');
            },
            "ajax": {
                "url": root + "Preexisting/GetTransactionDetail",
                "type": "POST",
                "data": {
                    "TransactionId": TransactionId
                }
            },
            "order": [],
            "columns": [
                        {
                            "class": "",
                            "orderable": false,
                            "data": null,
                            "defaultContent": " <button type=\"button\" class=\"btn btn-default btn-xs RemoveDetail\" aria-label=\"Left Align\"> \
                                                  <span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"\"></span>  \
                                                </button>"
                        },
                        {
                            "data": "CompensationRegionId",
                            "orderable": false,
                            "defaultContent": regionDropdown,
                            render: function (data, type, row) {
                                var rtn;
                                if (type === 'display' || type === 'filter') {
                                    if (data > 0) {
                                        var setvalue = compensationRegion["RegionsId"][data];
                                        var ix = regionDropdown.indexOf(setvalue);
                                        var str1 = regionDropdown.substring(0, ix + setvalue.length + 1);
                                        var str2 = regionDropdown.substring(ix + setvalue.length + 1, regionDropdown.length);
                                        rtn = str1 + ' selected ' + str2;
                                    }
                                }
                                return rtn;
                            }
                        },
                        {
                            "data": "CompensationRegionId",
                            "orderable": false,
                            "defaultContent": createDropDown(null, "code"),
                            render: function (data, type, row) {
                                var rtn;
                                if (type === 'display' || type === 'filter') {
                                    if (data > 0) {
                                        var setvalue = compensationRegion["Codes"][data];
                                        var dd = createDropDown(compensationRegion["Codes"], "code");
                                        var ix = dd.indexOf(setvalue);
                                        var str1 = dd.substring(0, ix - 1);
                                        var str2 = dd.substring(ix - 1, dd.length);
                                        rtn = str1 + ' selected ' + str2;
                                    }
                                }
                                return rtn;
                            }
                        },
                        {
                            "data": "Percent",
                            "orderable": false,
                            "defaultContent": "<input class=\"form-control DetailPercent\" type=\"number\" size=\"3\" onchange=\"CheckPercent(this)\" style=\"width: 70px;\" />",
                            render: function (data, type, row) {
                                var rtn;
                                if (type === 'display' || type === 'filter') {
                                    if (data>0) {
                                        rtn = '<input class=\"form-control DetailPercent\"  value=\"' + parseInt(data * 100) + '\" type=\"number\" size=\"3\" onchange=\"CheckPercent(this)\" style=\"width: 70px;\" />'
                                    }
                                }
                                return rtn;
                            }
                        },
                        {
                            "data": "Amount",
                            "defaultContent":"",
                            "visible": false
                        },
                        {
                            "data": "TransactionDetailId",
                            "defaultContent": "",
                            "visible": false
                        },
            ],
            select: 'single',
            "dom": "rt"
        });
    }
    else {

        DetailtTable.clear().draw();

        DetailtTable.ajax.data = { "TransactionId": TransactionId }

        DetailtTable.ajax.reload();
    }
    TotalDetail = 0;
}

var addRowToDetail = function () {

    var table = $('#DetailtTable').DataTable();

    table.row.add({
        "TransactionDetailId": "",
    }).draw();

    //showDropDowns();
}

var removeRowFromDetail = function () {

    var tr = $(this).closest('tr');
    var row = DetailtTable.row(tr);
    row.remove().draw();

    return true;

    //calculateTotalAdjudicationSum();
}

var calculateTotalAdjudicationSum = function () {

    //var total = 0;
    //$('#DetailtTable tr td:nth-child(4)').each(function () {
    //
    //    var value = parseInt(this.children[0].value.trim());
    //
    //    if ($.isNumeric(value)) {
    //        total = parseInt(total) + parseInt(value);
    //    }
    //});
    //$('#TotalAdjudicationSum').text(total);
}

var reCalDropDown = function (T) {

    if (T.classList.contains("region")) {
        
        var options = compensationRegion[T.options[T.selectedIndex].value];
        var td = T.parentElement.parentElement.children[2];
        td.removeChild(td.children[0]);
        var newDD = createDropDown(options, "code");
        td.innerHTML = newDD;
    }
}

var createDropDown = function (myOptions, newClass) {

    if (newClass == null)
        newClass = "";

    var dropdown = "<select class='form-control " + newClass + "' onChange='reCalDropDown(this)'>";
    dropdown = dropdown + "<option value=''>--Seleccione un valor--</option>";
    $.each(myOptions, function (val, text) {

        dropdown = dropdown + "<option value='" + val + "'>" + text + "</option>";

    });
    dropdown = dropdown + "</select>";
    return dropdown;
}

var loadValuesforDropdown = function () {

    showProgress("Añadiendo...");
    $.ajax({
        url: root + "Preexisting/GetCompensationRegionCode",
        success: function (data) {
            compensationRegion = data.data;
            regionDropdown = createDropDown(compensationRegion["Regions"],"region");
            codeDropdown = createDropDown(null, "code");
            weeks = compensationRegion["Weeks"];
            codes = compensationRegion["Codes"];
        },
        error: function () {
            msgBox('Ha ocurrido un error.');
        }
    });
}

var submitDetailForm = function () {

    var TransactionDetailData = [];
    var row;
    var formCompleted = true;
    var weeklyCompDisability = _caseData.CompSemanalInca;
    var ix = 0;
    var totalAmount = 0;

    $('#DetailtTable tbody tr').each(function () {

        var code = $(this).children('td:nth-child(3)').children().val();
        var week = weeks[code];
        var percent = $(this).children('td:nth-child(4)').children().val();

        if ($.isNumeric(percent) && $.isNumeric(week)) {
            var codeValue = Number(code);
            var percentValue = percent/100;
            var amountValue = weeklyCompDisability * (percentValue * week)
            var transactionDetailId = Number(DetailtTable.data()[ix].TransactionDetailId);
            totalAmount += amountValue;
            row = {
                "CompensationRegionId": codeValue,
                "Percent": percentValue,
                "TransactionId": transactionIdToDetail,
                "Amount": amountValue,
                "TransactionDetailId": transactionDetailId
            };
            TransactionDetailData.push(row);
            ix++;
        }
        else {
            alert('Todos los campos deben estar llenos.');
            formCompleted = false;
            return false;
        }
    });

    if (totalAmount < totalAmountToDetail && _caseData.TipoIncapacidad != "ITP") {
        alert('Desglose aún no alcanza cantidad adjudicada de $' + totalAmountToDetail);
        formCompleted = false;
        return false;
    }

    if (formCompleted) {
        var jsonResponse = JSON.stringify(TransactionDetailData);
        
        //var elem = document.getElementById('ajaxResultC');
        //elem.innerHTML = jsonResponse;
        
        $.ajax({
            "url": root + "Preexisting/SetTransactionDetail",
            data: jsonResponse,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#detailModal').modal('toggle');
            },
            error: function (evt) {
                alert("PU~NETA" + evt);
            }
        });
    }
}

var CheckPercent = function (T) {

    var val = T.value;

    if (!$.isNumeric(val) || val < 1 || val > 100) {
        alert(val + ' no es un valor aceptado.')
        T.value = '';
    }
}

$(function () {

    //En pantalla de desglose añade una nueva fila
    $('.addRowToDetail').click(addRowToDetail);
    //Somete forma de desglose
    $('.submitDetailForm').click(submitDetailForm);

    $('#DetailtTable tbody').on('click', '.RemoveDetail', removeRowFromDetail);

    loadValuesforDropdown();
});

