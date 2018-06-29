CDI.DesgloseIPP = (function () {
    var _compensation = 0;
    var _objSemanas = null;

	var _init = function () {	

		$("#btnShowDesglose").on("click", function (evt) {
			evt.preventDefault();

			$("#panel-desglose").fadeIn(1000).removeClass("hidden");

			 
		});

		$("#btnCancelDesglose").on("click", function (evt) {
			evt.preventDefault();

			$("#panel-desglose").fadeOut(1000).addClass("hidden");

			 
		});

		$("#btnAddDesglose").on("click", function (evt) {
			evt.preventDefault();

			var $tr = $("#tblDesglosarIpp > tbody").find(".hidden");
			var $clonedCodificacion = $tr.find(".desglose-codificacion").clone();
			$clonedCodificacion.find("option:gt(0)").remove();
            
			var newRow = "<tr>" +
                            "<td>" +
                                "<button type='button' class='btn btn-default btn-sm desglose-remove'><i class='fa fa-minus'></i></button>" +
                            "</td>" +
                            "<td>" +
                                $tr.find(".desglose-area").clone().wrap("<div>").parent().html() +
                            "</td>" +
                            "<td>" +
                                $clonedCodificacion.wrap("<div>").parent().html() +
                            "</td>" +
                            "<td>" +
                                "<input type='text' class='form-control desglose-percentage' />" +
                            "</td>" +
                         "</tr>";

			var $lastRow = $("#tblDesglosarIpp > tbody:last");
			$lastRow.append(newRow);
			$lastRow.find(".desglose-percentage").autoNumeric("init", {
				vMin: "0",
				aSign: ""
			});

			$("#tblDesglosarIpp tbody").not(".hidden").off("change").on("change", ".desglose-area", function (evt) {
				var $this = $(this);
				var selectedVal = $this.val();
				var $rowHidden = $("#tblDesglosarIpp > tbody > tr.hidden");
				var filtered = $rowHidden.find(".desglose-codificacion option").clone().filter(function (index, element) {
					return $(element).val().split("|")[1] === selectedVal;
				});

				var $ddlCodificacion = $this.closest("tr").find(".desglose-codificacion");
				$ddlCodificacion.find("option[value!='']").remove();
				$ddlCodificacion.append(filtered);
			});

			 
		});

		$("#tblDesglosarIpp tbody").on("click", ".desglose-remove", function (evt) {
			evt.preventDefault();

			$(this).closest("tr").remove();

		});

		$("#btnSaveDesglose").on("click", function () {
			if ($("#tblDesglosarIpp > tbody > tr").not(".hidden").length == 0) {
				alert("Debe ingresar al menos un desglose");
			}

			var semanas = 0;
			var totalAdjudicado = 0;
			$("#tblDesglosarIpp > tbody > tr").not(".hidden").each(function (index, element) {
				var $element = $(element);

				var percentage = 0;
				if ($element.find(".desglose-percentage") === undefined) {
					percentage = 0;
				}
				if ($element.find(".desglose-percentage") === "") {
					percentage = 0;
				}
				percentage = $element.find(".desglose-percentage").autoNumeric("get");
				percentage = parseFloat(percentage);
				if (isNaN(percentage)) {
					percentage = 0;
				}
				percentage = percentage / 100;

				var numberOfWeeks = $element.find(".desglose-codificacion").val().split("|")[3];
				numberOfWeeks = parseFloat(numberOfWeeks);
				if (isNaN(numberOfWeeks)) {
					numberOfWeeks = 0;
				}

				semanas = semanas + (numberOfWeeks * percentage);
				totalAdjudicado = totalAdjudicado + (numberOfWeeks * percentage * _compensation)
			});

			if (totalAdjudicado < $("#ipp_adjudicacionadicional").autoNumeric("get")) {
				CDI.displayNotification("El total adjudicado debe ser mayor o igual a la cantidad adjudicada", "warning");

				return false;
			}

			$(_objSemanas).autoNumeric("set", semanas);

			$("#panel-desglose").fadeOut(1000).addClass("hidden");
		});
	};

	var _setCompensationAndSemanas = function (objSemanas, CompSemanalInca) {
	    _objSemanas = objSemanas;
	    _compensation = CompSemanalInca;
	};

	var _obtenerDesglose = function () {
		var desgloses = [];

		var desglose = {
			CompensationRegionId: 0,
			Percent: 0,
			Weeks: 0
		};

		$("#tblDesglosarIpp > tbody > tr").not(".hidden").each(function (index, element) {
			var $element = $(element);
			var $ddlCodificacionValue = $element.find(".desglose-codificacion").val();

			desglose.CompensationRegionId = $ddlCodificacionValue.split("|")[4];
			desglose.Percent = $element.find(".desglose-percentage").autoNumeric("get") / 100;
			desglose.Weeks = $ddlCodificacionValue.split("|")[3];

			desgloses.push(desglose);

			desglose = {
				CompensationRegionId: 0,
				Percent: 0,
				Weeks: 0
			};
		});

		return desgloses;
	};

	var _inicializarDesglose = function (desgloses) {
	    var desgloses = [];

	    var desglose = {
	        CompensationRegionId: 0,
	        Percent: 0,
	        Weeks: 0
	    };

	    $("#tblDesglosarIpp > tbody > tr").not(".hidden").each(function (index, element) {
	        var $element = $(element);
	        var $ddlCodificacionValue = $element.find(".desglose-codificacion").val();

	        desglose.CompensationRegionId = $ddlCodificacionValue.split("|")[4];
	        desglose.Percent = $element.find(".desglose-percentage").autoNumeric("get") / 100;
	        desglose.Weeks = $ddlCodificacionValue.split("|")[3];

	        desgloses.push(desglose);

	        desglose = {
	            CompensationRegionId: 0,
	            Percent: 0,
	            Weeks: 0
	        };
	    });

	    return desgloses;
	};

	var _restartValues = function () {
	    $("#tblDesglosarIpp > tbody").find(".hidden").nextAll().remove();
	    $("#btnShowDesglose").prop("disabled", true);
	};

	return {
		init: _init,
		getDesglose: _obtenerDesglose,
		setDesglose: _inicializarDesglose,
		setCompensation: _setCompensationAndSemanas,
        restartValues: _restartValues
	}
})();