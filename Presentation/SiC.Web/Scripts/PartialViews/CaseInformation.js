var table = "";
var _caseData;
var count = 0;
var IPPtotal  = {
    cantidad: 0,
    semanas: 0
}
var ITPtotal = {
    reserva: 0,

};

var Inversion = {
    cantidad: 0
}

var BeneficiaryCount = 0; 

CDI.Queries = (function () {
    var _showIPPSummary;
    var _showITPSummary;
    var _showBeneficiarySummary;
    var _showInversionSummary;

    var _showCaseInformation = function () {

      
        if ($.fn.dataTable.isDataTable("#IPPSummary")) {
            _showIPPSummary.destroy();
        }

        if ($.fn.dataTable.isDataTable("#ITPSummary")) {
            _showITPSummary.destroy();
        }

        if ($.fn.dataTable.isDataTable("#BeneficiarySummary")) {
            _showBeneficiarySummary.destroy();
        }

        if ($.fn.dataTable.isDataTable("#InversionSummary")) {
            _showInversionSummary.destroy();
        }

        if ($.fn.dataTable.isDataTable("#BeneficiaryDetail")) {
            _showBeneficiaryDetail.destroy();
        }


        _showIPPSummary =  $('#IPPSummary').DataTable({
                columns: [
                    {
                        data: "Date",
                        className: "text-center",
                        render: function (data, type, row) {
                            if (data != "N/A")
                                return moment(data).format("DD-MM-YYYY");
                            else
                                return data;
                        }
                    },
                    {
                        data: "Weeks",
                        className: "text-center"
                    },
                    {
                        data: "Amount",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,0.00");
                        }
                    }
                ],
                select: true,
                dom: "Bfrtip",
                bInfo: false,
                bFilter: false,
                paging: false,
                scrollY: "300px",
                scrollCollapse: true,
                ordering: false,
                searching: false,
                async: false,
                processing: true,
                language: {
                    infoFiltered: " (de un total _MAX_ de registros filtrados)",
                    processing: "Buscando <i class='fa fa-spinner fa-spin'></i>",
                    lengthMenu: "Mostrar _MENU_ gestiones",
                    emptyTable: "Se encontraron 0 registros"
                },
                ajax: {
                    url: root + "Queries/IPPSummary",
                    type: "GET",
                    data: {
                        CaseDetailId: _caseData.CaseDetailId,
                        CaseKey: _caseData.CaseKey
                    },
                    order: [0],
                    cache: false,
                    autoWidth: false,
                    drawCallback: function (setting) {
                        $(".dataTables_empty").html("No se encontraron registros.");
                    },
                    error: function (msg) {
                        alert(msg + "error ipp");
                    }
                },
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;

                    //Remove the formating 
                    var val = function (i) {
                        return typeof i === "string" ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === "number" ?
                            i : 0;
                    };

                    //Total 
                    quantity = api
                            .column(2)
                            .data()
                            .reduce(function (a, b) {
                                return parseFloat(a) + parseFloat(b);
                            }, 0);

                    //Weeks
                    Weeks = api
                             .column(1)
                             .data()
                             .reduce(function (a, b) {
                                 return parseInt(a) + parseInt(b);
                             }, 0);

                    //Verify if the quantity is NaN
                    if (isNaN(quantity)) {
                        quantity = 0;
                    }
                    IPPtotal.cantidad = quantity;
                    quantity = numeral(quantity).format("$0,00.00");

                    //Update footer


                    $(api.column(2).footer()).html(quantity);
                    $(api.column(1).footer()).html(Weeks);
                    total = parseFloat(ITPtotal.reserva) + parseFloat(IPPtotal.cantidad);
                    total = numeral(total).format("$0,00.00");
                    $("#TotalIPPITP").html("Total Adjudicado " + total);

                }

            });
        

        _showITPSummary =   $('#ITPSummary').DataTable({
                columns: [
                    {
                        data: "Date",
                        className: "text-center",
                        render: function (data, type, row) {
                            if (data != "N/A")
                                return moment(data).format("DD-MM-YYYY");
                            else
                                return data;
                        }
                    },
                    {
                        data: "Reserve",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,0.00");
                        }
                    },
                    {
                        data: "BalanceIPP",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,0.00")
                        }
                    },
                    {
                        data: "Installment",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,0.00")
                        }
                    },
                    {
                        data: "Payment",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,0.00")
                        }
                    }
                ],
                select: true,
                dom: "Bfrtip",
                bInfo: false,
                bFilter: false,
                paging: false,
                scrollY: "300px",
                scrollCollapse: true,
                ordering: false,
                searching: false,
                async: false,
                processing: true,
                language: {
                    infoFiltered: " (de un total _MAX_ de registros filtrados)",
                    info: "Mostrando del _START_ al _END_ de _TOTAL_ registros",
                    infoEmpty: "Se encontraron 0 registros",
                    processing: "Buscando <i class='fa fa-spinner fa-spin'></i>",
                    lengthMenu: "Mostrar _MENU_ gestiones",
                    emptyTable: "Se encontraron 0 registros"
                },
                ajax: {
                    url: root + "Queries/ITPSummary",
                    type: "GET",
                    data: {
                        CaseDetailId: _caseData.CaseDetailId,
                        CaseKey: _caseData.CaseKey
                    },
                    order: [0],
                    cache: false,
                    autoWidth: false,
                    drawCallback: function (setting) {
                        $(".dataTables_empty").html("No se encontraron registros.");
                    },
                    error: function (msg) {
                        alert(msg.Message + "Error itp");
                    }
                }, footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;

                    //Remove the formating 
                    var val = function (i) {
                        return typeof i === "string" ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === "number" ?
                            i : 0;
                    };

                    //Total reserva
                    reserve = api
                            .column(1)
                            .data()
                            .reduce(function (a, b) {
                                return parseFloat(a) + parseFloat(b);
                            }, 0);

                    //Total reserva
                    balance = api
                            .column(2)
                            .data()
                            .reduce(function (a, b) {
                                return parseFloat(a) + parseFloat(b);
                            }, 0);


                    //Verify if the value is Nan
                    if (isNaN(reserve)) {
                        reserve = 0;
                    }

                    if (isNaN(balance)) {
                        balance = 0;
                    }

                    ITPtotal.reserva = reserve;


                    //Update footer
                    var result = reserve - balance;
                    result = numeral(result).format("$0,00.00");


                    $(api.column(1).footer()).html(result);

                    total = parseFloat(ITPtotal.reserva) + parseFloat(IPPtotal.cantidad);
                    total = numeral(total).format("$0,00.00");
                    $("#TotalIPPITP").html("Total Adjudicado " + total);

                }

            });

        _showBeneficiarySummary =   $('#BeneficiarySummary').DataTable({
                columns: [
                    {
                        data: "FullName",
                        className: "text-center"
                    },
                    {
                        data: "Concept",
                        className: "text-left"
                    },
                    {
                        data: "Suffix",
                        className: "text-left"
                    },
                    {
                        data: "BirthDate",
                        className: "text-center",
                        render: function (data, type, row) {
                            if (data != "N/A")
                                return moment(data).format("DD-MM-YYYY");
                            else
                                return data;
                        }
                    },
                    {
                        data: "SSN",
                        className: "text-center"
                    },
                    {
                        data: "Relation",
                        className: "text-center"
                    },
                    {
                        data: "Reserve",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,00.00");
                        }
                    },
                    {
                        data: "Minstallment",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,00.00");
                        }
                    },
                    {
                        data: "Payed",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,00.00");
                        }
                    },
                    {
                        data: "Balance",
                        className: "text-right",
                        render: function (data, type, row) {
                            return numeral(data).format("$0,00.00")
                        }
                    }
                ],
                select: true,
                dom: "Bfrtip",
                bInfo: false,
                bFilter: false,
                LengthChange: false,
                paging: false,
                scrollY: "300px",
                scrollCollapse: true,
                ordering: false,
                searching: false,
                async: false,
                processing: true,
                language: {
                    infoFiltered: " (de un total _MAX_ de registros filtrados)",
                    info: "Mostrando del _START_ al _END_ de _TOTAL_ registros",
                    infoEmpty: "Se encontraron 0 registros",
                    processing: "Buscando <i class='fa fa-spinner fa-spin'></i>",
                    lengthMenu: "Mostrar _MENU_ gestiones",
                    emptyTable: "Se encontraron 0 registros"
                },
                ajax: {
                    url: root + "Queries/BeneficiarySummary",
                    type: "GET",
                    data: {
                        CaseDetailId: _caseData.CaseDetailId,
                        CaseKey: _caseData.CaseKey,
                        CaseId: _caseData.CaseId
                    },
                    order: [0],
                    cache: false,
                    autoWidth: false,
                    drawCallback: function (setting) {
                        $(".dataTables_empty").html("No se encontraron registros.");
                    },
                    error: function (xhr, texts, err) {
                        alert("ReadyState: " + xhr.readyState);
                        alert("responseText: " + xhr.responseText);
                        alert("status: " + xhr.status);
                        alert("text Status: " + texts);
                        alert("error: " + err);
                    }
                }
            });
        

        _showInversionSummary =   $('#InversionSummary').DataTable({
            columns: [
                {
                    data: "Date",
                    className: "text-left",
                    render: function (data, type, row) {
                        if (data != "N/A")

                            return moment(data).format("MM-DD-YYYY");
                        else
                            return data;
                    }
                },
                {
                    data: "Amount",
                    className: "text-right",
                    render: function (data, type, row) {
                        return numeral(data).format("$0,0.00");
                    }
                },
            ],
            select: true,
            dom: "Bfrtip",
            bInfo: false,
            bFilter: false,
            LengthChange: false,
            paging: false,
            scrollY: "300px",
            scrollCollapse: true,
            ordering: false,
            searching: false,
            async: false,
            processing: true,
            language: {
                infoFiltered: " (de un total _MAX_ de registros filtrados)",
                info: "Mostrando del _START_ al _END_ de _TOTAL_ registros",
                infoEmpty: "Se encontraron 0 registros",
                processing: "Buscando <i class='fa fa-spinner fa-spin'></i>",
                lengthMenu: "Mostrar _MENU_ gestiones",
                emptyTable: "Se encontraron 0 registros"
            },
            ajax: {
                url: root + "Queries/InversionSummary",
                type: "GET",
                data: {
                    CaseDetailId: _caseData.CaseDetailId,
                    CaseKey: _caseData.CaseKey
                },
                order: [0],
                cache: false,
                autoWidth: false,
                drawCallback: function (setting) {
                    $(".dataTables_empty").html("No se encontraron registros.");
                },
                error: function (msg) {
                    alert(msg + "error inversion");
                }
            },
            footerCallback: function (row, data, start, end, display) {
                var api = this.api(), data;

                //Remove the formating 
                var val = function (i) {
                    return typeof i === "string" ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === "number" ?
                        i : 0;
                };

                //Total 
                total = api
                        .column(1)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b);
                        }, 0);

                //Update footer

                //Verify if the valus is NaN
                if (isNaN(total)) {
                    total = 0;
                }

                total = numeral(total).format("$0,00.00");

                $(api.column(1).footer()).html(total);

            }
        });


        _showBeneficiaryDetail = $('#BeneficiaryDetail').DataTable({
            columns: [
                 {
                     data: "FullName",
                     className: "text-left"
                 },
                {
                    data: "TType",
                    className: "text-left"
                },
                {
                    data: "Amount",
                    className: "text-right",
                    render: function (data, type, row) {
                        return numeral(data).format("$0,0.00")
                    }
                },
                {
                  data: "Date",
                  className: "text-center",
                  render: function (data, type, row) {
                      if (data != "N/A")
                          return moment(data).format("DD-MM-YYYY");
                      else
                          return data;
                  }
              },
            ],
            select: true,
            dom: "Bfrtip",
            bInfo: false,
            bFilter: false,
            paging: false,
            scrollY: "300px",
            scrollCollapse: true,
            ordering: false,
            searching: false,
            async: false,
            processing: true,
            language: {
                infoFiltered: " (de un total _MAX_ de registros filtrados)",
                info: "Mostrando del _START_ al _END_ de _TOTAL_ registros",
                infoEmpty: "Se encontraron 0 registros",
                processing: "Buscando <i class='fa fa-spinner fa-spin'></i>",
                lengthMenu: "Mostrar _MENU_ gestiones",
                emptyTable: "Se encontraron 0 registros"
            },
            ajax: {
                url: root + "Queries/BeneficiaryDetail",
                type: "GET",
                data: {
                    CaseDetailId: _caseData.CaseDetailId,
                    CaseKey: _caseData.CaseKey,
                    CaseId: _caseData.CaseId
                },
                order: [0],
                cache: false,
                autoWidth: false,
                drawCallback: function (setting) {
                    $(".dataTables_empty").html("No se encontraron registros.");
                },
                error: function (msg) {
                    alert(msg.Message + "Error Detail");
                }
            }
        });

        _expandPanel($("#searchPanel .panel-heading a.collapse-click"));

        //Get the amount of beneficiaries per case 
     //   var info = _showBeneficiarySummary.fnGetData.length;
      //  alert(_showBeneficiarySummary.fnGetData().length);

    }; //End showCaseInformation

    var _expandPanel = function (button) {
        if (button.hasClass('panel-collapsed')) {
            button.parents('.panel').find('.panel-body').slideDown();
            button.removeClass('panel-collapsed');
            button.find('i').removeClass('fa-chevron-down').addClass('fa-chevron-up');
        }
    };

    var _collapsePanel = function (button) {
        if (!button.hasClass('panel-collapsed')) {
            button.parents('.panel').find('.panel-body').slideUp();
            button.addClass('panel-collapsed');
            button.find('i').removeClass('fa-chevron-up').addClass('fa-chevron-down');
        }
    };

    var _bindUIActions = function () {

        $(CDI.CaseSearch).on('case.selected', function (e, data) {
            _caseData = data.selectedCase;
            _showCaseInformation();
            CDI.CaseSearch.header(_caseData);
            $("#CaseInformation").fadeIn(1000).removeClass("hidden");
        });

        $(CDI.CaseSearch).on('started', function (e, data) {
            $("#CaseInformation").fadeOut(1000).addClass("hidden");
        });

        $(".panel-heading").on('click', 'a.collapse-click', function (evt) {
            evt.preventDefault();

            var $this = $(this);

            if ($this.hasClass('panel-collapsed')) {
                // expand the panel
                _expandPanel($this);
            } else {
                // collapse the panel
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
        CDI.Queries.init();
        
        //If the number of beneficiaries is zero, then hide de div 
    //    if (BeneficiaryCount == 0) {
     //       jQuery("#deathBeneficiary").hide();
    //    }
});

