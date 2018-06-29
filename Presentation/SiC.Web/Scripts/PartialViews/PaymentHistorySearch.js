var myTable;
var nombre;
var ssn;
var ebt;
var numeroCaso;

var data = function () {
    return {
        "nombre":  $("#name").val(),
        "sSN": $("#ssn").val(),
        "eBT": $("#ebt").val(),
        "numeroCaso": $("#caso").val()
    };
}


var showResults = function () {
     
    if ($("#resultsPanel").css('display') == 'none')
        $("#resultsPanel").show();

    if (!myTable) {
        myTable = $("#CasesTable").DataTable({
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
                "processing": "Buscando <i class='fa fa-spinner fa-spin'></i>",
                "lengthMenu": "Mostrar _MENU_ gestiones"
            },
            "ajax": {
                "url": "/Payments/Search",
                "type": "POST",
                "data": data
            },
            "order": [
                [1, 'desc']
            ],
            "columns": [
            {
                "data": "Nombre"
            },
            {
                "data": "SSN"
            },
            {
                "data": "EBT"
            },
            {
                "data": "NumeroCaso"
            }
            ],
            "dom": "rtp"
        });
    }
    else
        myTable.ajax.reload();
}


$(function () {
    $("#searchBtn").on("click", showResults);

    $("#searchPanel input").keyup(function (event) {
        if (event.keyCode == 13) {
            $("#searchBtn").click();
        }
    });

    $("#clearBtn").on("click", function () {
        $('#searchPanel :input').each(function () {
            this.value = "";
        });

        $('#searchPanel').find(".has-error, .has-success").not(".help-block").removeClass("has-error").removeClass("has-success");
    });

    $('#CasesTable tbody').on('click', 'tr', function () {
        //window.alert("sometext");
        var mydata = myTable.row(this).data()
        mySelectedRow = mydata;
        if (mydata.CaseFolderId) {
            window.location.href = window.location.href;
            window.location.href = "/Payments/Resumen/" + mydata.CaseFolderId;
        }
    });
});