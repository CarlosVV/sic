var _demographicData;
var _legalGuardianData;
var HasChildren;
var Relationship;
var isEntityReadOnly;
var hasAddress;
var participantValidator;
var demographicValidator;
var _injuredDeceaseDate;
var selectedCaseNumber;
var selectedCaseKey;

CDI.InitializeDataTable = (function () {
    var initializeTable = function () {
        table = $('#tblParticipant').DataTable({
            columns: [
                {
                    data: "ParticipantType"
                },
                {
                    data: null,
                    render: function (localData, type, row) {
                        return localData.caseNumber + ' ' + localData.caseKey;
                    }
                },
                {
                    data: "FullName",
                },
                {
                    data: "Status",
                },
                {
                    data: "EntityId",
                    visible: false,
                    searchable: false
                }
            ],
            select: true,
            //buttons: [
            //    { extend: "create", editor: editor },
            //    { extend: "edit", editor: editor},
            //    { extend: "remove", editor: editor }
            //],
            //dom: "Bfrtip",
            paging: false,
            //scrollY: "200px",
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
            }
            //ajax: {
            //    url: root + "PaymentCertification/Result",
            //    dataType: "json",
            //    type: "POST",
            //    cache: false,
            //    async: false,
            //    data: function (data) {
            //        data.caseNumber = _caseDetailData.caseNumber;
            //        data.caseKey = _caseDetailData.caseKey;
            //        data.caseDetailId = _caseDetailData.caseDetailId;
            //        return data;// = JSON.stringify(data);
            //    }                
            //}
        });
    };

    var _bindUIActions = function () {
        initializeTable();
    }

    var _init = function () {
        _bindUIActions();
    };

    return {
        init: _init
    };
});

CDI.DemographicInformation = (function () {
    var _caseData;
    var injuredData;
    var beneficiaryData;
    var _entityInfo;

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
        
        var jsonLG = {"caseNumber": _caseData.NumeroCaso, "caseKey": _caseData.CaseKey}
        $.ajax({
            url: root + "Demographic/GetLegalGuardians",
            type: "POST",
            dataType: "json",
            async: false,
            data: jsonLG,
            success: function (result) {
                var legalGuardian = $("#legalGuardian");
                legalGuardian.empty();
                if (result.length > 0) {
                    legalGuardian.append($('<option/>', {
                        value: "",
                        text: '--Seleccionar--'
                    }));
                    $.each(result, function (index, value) {
                        legalGuardian.append($('<option/>', {
                            value: value.EntityId,
                            text: value.FullName
                        }));
                    });
                }
                else {
                    legalGuardian.append($('<option/>', {
                        value: "",
                        text: '--No existe un tutor legal--'
                    }));
                }
            },
            error: function () {
                legalGuardian.empty();
                CDI.displayNotification("ocurrió un error buscando los datos de tutor legal.", "error");
            }
        });

        var e = _caseData.EntityId;
        var json = { "caseDetailId": _caseData.CaseDetailId, "entityId": _caseData.EntityId, "caseNumber": _caseData.NumeroCaso};
        selectedCaseNumber = _caseData.NumeroCaso;
        selectedCaseKey = _caseData.CaseKey;

        $.ajax({
            url: root + "Demographic/Result/",
            type: "POST",
            data: json,
            dataType: "json",
            async: false,
            success: function (result) {
                if (result.Data == null) {
                    CDI.displayNotification("ocurrió un error en la carga de datos demograficos.", "error");
                }
                else {
                    $('#btnCancel').click();
                    _demographicData = result.Data;
                    _address = result.Address;
                    _injuredDeceaseDate = result.InjuredDeceaseDate;
                    hasAddress = _address != null ? true : false;

                    // If condition that check if a Sif entity is already created. If yes then the entity can't be modified.
                    if (_demographicData.EntitySif != null) {
                        isEntityReadOnly = true;
                    }
                    else {
                        isEntityReadOnly = false;
                    }
                    isReadOnlyForm(isEntityReadOnly);

                    if (_demographicData.CaseKey == "00") {
                        $("#row4Beneficiary").addClass("hidden");
                        $("#row5Beneficiary").addClass("hidden");
                        $("#row6Beneficiary").addClass("hidden");
                        $("#row7Beneficiary").addClass("hidden");
                    }
                    else {
                        $("#row4Beneficiary").removeClass("hidden");
                        $("#row5Beneficiary").removeClass("hidden");
                        $("#row6Beneficiary").removeClass("hidden");
                        $("#row7Beneficiary").removeClass("hidden");
                    }
                    /*--------------Row 1--------------*/
                    $("#ssn").val(_demographicData.Entity.SSN);
                    $("#numId").val(_demographicData.Entity.IDNumber);
                    var dob = moment(_demographicData.Entity.BirthDate);
                    if (dob.isValid()) {
                        $("#dob").val(dob.format('l'));
                    }
                                        
                    if (_caseData.CaseKey != "00") {
                        if (_demographicData.Entity.BirthDate != null && _injuredDeceaseDate != null) {
                            $("#deceasedAge").val(GetAge(_demographicData.Entity.BirthDate, _injuredDeceaseDate));
                            $("#deceasedAgeInfo").prop('disabled', true);
                            $("#deceasedAgeInfo").fadeIn(1000).removeClass("hidden");
                        }
                    }
                    else {

                        $("#deceasedAgeInfo").fadeOut(1000).addClass("hidden");
                    }
                    /*---------------------------------*/
                    /*--------------Row 2--------------*/
                    $("#updateForm .firstName").val(_demographicData.Entity.FirstName);
                    $("#updateForm .middleName").val(_demographicData.Entity.MiddleName);
                    $("#updateForm .lastName").val(_demographicData.Entity.LastName);
                    $("#updateForm .secondLastName").val(_demographicData.Entity.SecondLastName);
                    /*---------------------------------*/
                    /*--------------Row 3--------------*/
                    if (_demographicData.EntityId_LegalGuardian != null) {
                        $("#legalGuardian").val(_demographicData.EntityId_LegalGuardian);
                    }
                    if (_demographicData.Entity.CivilStatus != null) {
                        $("#civilStatus").val(_demographicData.Entity.CivilStatus.CivilStatusId).change();
                    }

                    var marriageDate = moment(_demographicData.Entity.MarriageDate);
                    var deceaseDate = moment(_demographicData.Entity.DeceaseDate);
                    if (marriageDate.isValid()) {
                        $("#marriageDate").val(marriageDate.format('l'));
                    }
                    if (deceaseDate.isValid()) {
                        $("#deceaseDate").val(deceaseDate.format('l'));
                    }
                    /*---------------------------------*/
                    /*-------------------------Beneficiary-------------------------*/
                    if (_caseData.CaseKey != "00") {
                        /*--------------Row 1--------------*/
                        /*Edad de beneficiario al momento de la muerte del lesionado*/
                        if (_demographicData.Entity.BirthDate != null && _demographicData.Entity.DeceaseDate != null) {
                            $("#deceasedAgeInfo").fadeIn(1000).removeClass("hidden"); 
                            $("#deceasedAge").val(GetAge(_demographicData.Entity.BirthDate, _demographicData.Entity.DeceaseDate));
                        }
                        /*---------------------------------*/
                        /*--------------Row 4--------------*/
                        if (_demographicData.RelationshipType != null) {
                            $("#relationshipType").val(_demographicData.RelationshipType.RelationshipType1).change();
                            $("#otherRelationshipType").val(_demographicData.OtherRelationshipType);
                        }
                        /* ---------------------------------*/
                        /*--------------Row 5--------------*/
                        if (_demographicData.Entity.HasDisability) {
                            $("#disabilityTrue").prop("checked", true);
                            $("#disabilityFalse").prop("checked", false);

                            if (_demographicData.Entity.IsRehabilitated) {
                                $("#rehabilitatedTrue").prop("checked", true);
                                $("#rehabilitatedFalse").prop("checked", false);
                            }
                            else {
                                $("#rehabilitatedTrue").prop("checked", false);
                                $("#rehabilitatedFalse").prop("checked", true);
                            }

                            if (_demographicData.Entity.IsWorking) {
                                $("#workingTrue").prop("checked", true);
                                $("#workingFalse").prop("checked", false);
                            }
                            else {
                                $("#workingTrue").prop("checked", false);
                                $("#workingFalse").prop("checked", true);
                            }

                            $("#isRehabilitatedInfo").fadein(1000).removeClass("hidden");
                            $("#isWorkingInfo").fadein(1000).removeClass("hidden");
                        }
                        else {
                            $("#disabilityTrue").prop("checked", false);
                            $("#disabilityFalse").prop("checked", true);

                            $("#rehabilitatedTrue").prop("checked", false);
                            $("#rehabilitatedFalse").prop("checked", true);

                            $("#workingTrue").prop("checked", false);
                            $("#workingFalse").prop("checked", true);

                            $("#isRehabilitatedInfo").fadeOut(1000).addClass("hidden");
                            $("#isWorkingInfo").fadeOut(1000).addClass("hidden");
                        }
                        /*---------------------------------*/
                        /*--------------Row 6--------------*/
                        if (_demographicData.Entity.IsStudying) {
                            $("#studyingTrue").prop("checked", true);
                            $("#studyingFalse").prop("checked", false);

                            studyFromDate = moment(_demographicData.Entity.SchoolStartDate);
                            studyToDate = moment(_demographicData.Entity.SchoolEndDate);
                            if (studyToDate.isValid()) {
                                $("#studyFromDate").val(studyFromDate.format('l'));
                            }
                            if (studyToDate.isValid()) {
                                $("#studyToDate").val(studyToDate.format('l'));
                            }

                            $("#studyingInfo").fadeIn(1000).removeClass("hidden");
                        }
                        else {
                            $("#studyingTrue").prop("checked", false);
                            $("#studyingFalse").prop("checked", true);

                            $("#studyFromDate").val(null);
                            $("#studyToDate").val(null);
                            $("#studyingInfo").fadeOut(1000).addClass("hidden");
                        }
                        /*---------------------------------*/
                        /*--------------Row 7--------------*/
                        if (_demographicData.Entity.HasWidowCertification) {
                            $("#widowTrue").prop("checked", true);
                            $("#widowFalse").prop("checked", false);
                        }
                        else {
                            $("#widowTrue").prop("checked", false);
                            $("#widowFalse").prop("checked", true);
                        }

                        widowCertificationDate = moment(_demographicData.Entity.WidowCertificationDate);
                        if (widowCertificationDate.isValid()) {
                            $("#widowCertificationDate").val(widowCertificationDate.format('l')).click();
                        }

                        if (_demographicData.Entity.Occcupation != null) {
                            $("#occupation").val(_demographicData.Entity.Occupation.OccupationId);
                        }
                        if (_demographicData.Entity.MonthlyIncome != null) {
                            $("#monthlyIncome").val(_demographicData.Entity.MonthlyIncome);//.format('0,0.00');
                        }
                        else {
                            $("#monthlyIncome").val(0);//format('0,0.00');
                        }
                        
                        /*---------------------------------*/
                    }
                    /*------------------------------------------------------------*/
                    /*--------------Address------------*/
                    if (hasAddress) {
                        $("#updateForm .addressLine1").val(_address.Line1);
                        $("#updateForm .addressLine2").val(_address.Line2);

                        if (_address.CityId != null) {
                            $("#updateForm .country").val(_address.CountryId).change();

                            if (_address.StateId != null) {
                                $("#updateForm .state").val(_address.StateId).change();
                            }
                            $("#updateForm .city").val(_address.CityId);
                            //if (_address.City.City1 == "Otro") {
                            //    $("#updateForm otherCity").val(_address.OtherCity);
                            //}
                        }
                        if (_address.ZipCodeExt != null && _address.ZipCodeExt.trim() != "") {
                            $("#updateForm .zipcode").val(_address.ZipCode + _address.ZipCodeExt).keyup();
                        }
                        else {
                            $("#updateForm .zipcode").val(_address.ZipCode).keyup();
                        }
                        $("#updateForm .email").val(_demographicData.Entity.Email);
                    }


                    /*---------------------------------*/
                    /*--------------Phone--------------*/
                    $("#updateForm .homePhone").val(_demographicData.Entity.HomePhoneNumber).keyup();
                    $("#updateForm .cellPhone").val(_demographicData.Entity.CellPhoneNumber).keyup();
                    $("#updateForm .faxPhone").val(_demographicData.Entity.FaxPhoneNumber).keyup();
                    $("#updateForm .workPhone").val(_demographicData.Entity.WorkPhoneNumber).keyup();
                    $("#updateForm .otherPhone").val(_demographicData.Entity.OtherPhoneNumber).keyup();;
                    /*---------------------------------*/
                    /*-----------Payment Info----------*/
                    var paymentStatus;
                    $("#updateForm .paymentType").val(_demographicData.TransferTypeId);

                    if (_demographicData.ActiveIdent == "A") {
                        paymentStatus = "Activo";
                        if (_demographicData.Cancellation.CancellationCode == "B") {
                            $("#updateForm .restartDate").val(_demographicData.RestartDate);
                            $("#updateForm .restartDateInfo").fadeIn(1000).removeClass("hidden");
                        }
                        else {
                            $("#updateForm .restartDate").val(null);
                            $("#updateForm .restartDateInfo").fadeOut(1000).addClass("hidden");
                        }
                        $("#updateForm .suspensionDate").val(null);
                        $("#updateForm .suspensionReason").val(null);
                        $("#updateForm .suspensionDateInfo").fadeOut(1000).addClass("hidden");
                        $("#updateForm .suspensionReasonInfo").fadeOut(1000).addClass("hidden");
                    }
                    else if (_demographicData.ActiveIdent == "S") {
                        paymentStatus = "Suspendido";
                        $("#updateForm .suspensionDate").val(_demographicData.CancellationDate);
                        $("#updateForm .suspensionReason").val(_demographicData.Cancellation.Cancellation1);
                        $("#updateForm .restartDate").val(null);
                        $("#updateForm .suspensionDateInfo").fadeIn(1000).removeClass("hidden");
                        $("#updateForm .suspensionReasonInfo").fadeIn(1000).removeClass("hidden");
                        $("#updateForm .restartDateInfo").fadeOut(1000).addClass("hidden");
                    }
                    else if (_demographicData.ActiveIdent == "I") {
                        paymentStatus = "Inactivo";
                        $("#updateForm .suspensionDate").val(null);
                        $("#updateForm .restartDate").val(null);
                        $("#updateForm .suspensionDateInfo").fadeOut(1000).addClass("hidden");
                        $("#updateForm .suspensionReasonInfo").fadeOut(1000).addClass("hidden");
                        $("#updateForm .restartDateInfo").fadeOut(1000).addClass("hidden");

                    }
                    else
                        paymentStatus = null;

                    $("#updateForm .paymentStatus").val(paymentStatus);
                    /*---------------------------------*/
                    /*---------Modified Reason---------*/
                    $("#updateForm .modifiedReason").val(_demographicData.Entity.ModifiedReasonId).change();
                    $("#updateForm .otherReason").val(_demographicData.Entity.OtherModifiedReason);
                    /*---------------------------------*/

                    /*----Displays Demographic Data----*/
                    $("#DemographicData").fadeIn(1000).removeClass("hidden");
                    // Scrolls to the top of the demographic update form.
                    //$('html, body').animate({
                    //    scrollTop: $("#DemographicData").offset().top - 100
                    //}, 500);
                    ScrollTop("DemographicData");
                    /*---------------------------------*/
                }
            },
            error: function (result) {
                var a = result;
            }
        });
    }
    
    var _bindUIActions = function () {
        $(CDI.CaseSearch).on('case.selected', function (e, data) {
            _caseData = data.selectedCase;

            _startFunction();
            moment.locale('en');

            $("#DemographicInjured").fadeIn(1000).removeClass("hidden");
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
    $('.zipcode').mask('00000-0000');
    $('.phone-group').mask('(000) 000-0000');
    $('.money').mask("#,##0.00", { reverse: true });
    $("#civilStatus").change(civliStatusChangeEvent);
    $("#legalguardian").change(legalGuardianChangeEvent);
    $("#dob").blur(dobBlurEvent);
    $("#deceaseDate").blur(deceaseDateBlurEvent);
    $(".relationshipType").change(relationshipTypeChangeEvent);
    $('input[name="isDisability"]:radio').click(isDisabilityClickEvent);
    $('input[name="isStudying"]:radio').click(isStudyingClickEvent);
    $('input[name="isWidow"]:radio').click(isWidowClickEvent);
    $(".country").change(countryChangeEvent);
    $(".state").change(stateChangeEvent);
    $(".modifiedReason").change(modifiedReasonChangeEvent);

    CDI.CaseSearch.init();
    //CDI.InitializeDataTable().init();
    CDI.DemographicInformation.init();

    $("#updateSearch").validate();
    validateForm();
});

//OnChange event that display or hide marriageDate based on the selection of the user.
var civliStatusChangeEvent = function (e) {
    if ($(this).val() != null && $(this).val() != "") {
        _demographicData.Entity.CivilStatus = { "CivilStatusId": $(this).val(), "CivilStatus1": $("#civilStatus option:selected").text() };
        if ($("#civilStatus option:selected").text() == "Casado") {
            $("#marriageDateInfo").fadeIn(1000).removeClass("hidden");
        }
        else {
            $("#marriageDateInfo").fadeOut(1000).addClass("hidden");
            $("#marriageDate").val(null);
        }
    }
    else {
        $("#marriageDateInfo").fadeOut(1000).addClass("hidden");
        $("#marriageDate").val(null);
        _demographicData.Entity.CivilStatus = null;
    }
};

//OnChange event that saves the legal guardian selected by the user.
var legalGuardianChangeEvent = function (e) {
    var legalGuardian = $(this);
    if (legalguardian.val() != null) {
        _demographicData.EntityId_LegalGuardian = $(this).val();
    }
    else {
        _demographicData.EntityId_LegalGuardian = null;
    }
};

// OnChange event that display or hide IsEnmancipated, HasChildren or Other based on the selection of the user.
// Display information based on the relationshipType selected by the user. 
var relationshipTypeChangeEvent = function (e) {
    var relationship = $(this);
    var otherRelationship = $(".otherRelationshipType", $(e.target).closest("form"));
    var orInfo = $(".otherRelationshipTypeInfo", $(e.target).closest("form"));

    if (relationship.val() == "Hijo") {
        $("#isEnmancipatedTrue").prop("checked", false);
        $("#isEnmancipatedFalse").prop("checked", true);

        $("#hasChildrenTrue").prop("checked", false);
        $("#hasChildrenFalse").prop("checked", true);
        $("#otherRelationship").val(null);

        orInfo.fadeOut(1000).addClass("hidden");
        $("#isEnmancipatedInfo").fadeIn(1000).removeClass("hidden");
        $("#hasChildrenInfo").fadeOut(1000).addClass("hidden");
    }
    else if (relationship.val() == "Viuda(o)" || relationship.val() == "Concubina(o)") {
        $("#hasChildrenTrue").prop("checked", false);
        $("#hasChildrenFalse").prop("checked", true);

        $("#isEnmancipatedTrue").prop("checked", false);
        $("#isEnmancipatedFalse").prop("checked", true);
        otherRelationship.val("");

        orInfo.fadeOut(1000).addClass("hidden");
        $("#hasChildrenInfo").fadeIn(1000).removeClass("hidden");
        $("#isEnmancipatedInfo").fadeOut(1000).addClass("hidden");
    }
    else if (relationship.val() == "Otros") {
        $("#hasChildrenTrue").prop("checked", false);
        $("#hasChildrenFalse").prop("checked", true);
        $("#isEnmancipatedTrue").prop("checked", false);
        $("#isEnmancipatedFalse").prop("checked", true);

        orInfo.fadeIn(1000).removeClass("hidden");
        $("#hasChildrenInfo").fadeOut(1000).addClass("hidden");
        $("#isEnmancipatedInfo").fadeOut(1000).addClass("hidden");
    }
    else {
        $("#hasChildrenTrue").prop("checked", false);
        $("#hasChildrenFalse").prop("checked", true);
        $("#isEnmancipatedTrue").prop("checked", false);
        $("#isEnmancipatedFalse").prop("checked", true);
        otherRelationship.val(null);

        orInfo.fadeOut(1000).addClass("hidden");
        $("#hasChildrenInfo").fadeOut(1000).addClass("hidden");
        $("#isEnmancipatedInfo").fadeOut(1000).addClass("hidden");
    }
};

//Calculate beneficiary age at the moment of injured death.
var dobBlurEvent = function () {
    if (_demographicData.CaseKey != "00") {
        var dob = $("#dob").val();
        if ((dob != null && dob != "") && (_injuredDeceaseDate != null && _injuredDeceaseDate != "")) {
            $("#deceasedAgeInfo").fadeIn(1000).removeClass("hidden");
            $("#deceasedAge").val(GetAge(dob, _injuredDeceaseDate));
        }
        else {
            $("#deceasedAgeInfo").fadeIn(1000).addClass("hidden");
        }
    }
};

//Calculate beneficiary age at the moment of injured death.
var deceaseDateBlurEvent = function () {
    if (_demographicData.CaseKey != "00") {
        var dob = $("#dob").val();
        var deceaseDate = $("#deceaseDate").val()
        if (dob != null && deceaseDate != null) {
            $("#deceasedAgeInfo").fadeIn(1000).removeClass("hidden");
            $("#deceasedAge").val(GetAge(_demographicData.Entity.BirthDate, _demographicData.Entity.DeceaseDate));
        }
        else {
            $("#deceasedAgeInfo").fadeIn(1000).addClass("hidden");
            $("#deceasedAge").val(GetAge(_demographicData.Entity.BirthDate, _demographicData.Entity.DeceaseDate));
        }
    }
};

//Click event that displays or hide working and rehabilitated question based on choice.
 var isDisabilityClickEvent = function () {
    //Verificar una manera de mejorar este if statement. 
    if ($("#disabilityTrue").is(":checked")) {
        $("#isRehabilitatedInfo").fadeIn(1000).removeClass("hidden");
        $("#isWorkingInfo").fadeIn(1000).removeClass("hidden");
    }
    else {
        $("#rehabilitatedTrue").prop("checked", false);
        $("#rehabilitatedFalse").prop("checked", true);

        $("#workingTrue").prop("checked", false);
        $("#workingFalse").prop("checked", true);

        $("#isRehabilitatedInfo").fadeOut(1000).addClass("hidden");
        $("#isWorkingInfo").fadeOut(1000).addClass("hidden");
    }
};

//Click event that displays or hide studyfrom and studyto based on choice.
 var isStudyingClickEvent = function () {
     if ($("#studyingTrue").is(":checked")) {
         $("#studyingInfo").fadeIn(1000).removeClass("hidden");
     }
     else {
         $("#studyFromDate").val(null);
         $("#studyToDate").val(null);
         $("#studyingInfo").fadeOut(1000).addClass("hidden");
     }
};

 //Click event that displays or hide widowCertificationDate based on choice.
 var isWidowClickEvent = function () {
     if ($("#widowTrue").is(":checked")) {
         $("#widowInfo").fadeIn(1000).removeClass("hidden");
     }
     else {
         $("#widowCertificationDate").val(null);
         $("#widowInfo").fadeOut(1000).addClass("hidden");
     }
 };

 $("#monthlyIncome").change(function (e) {
     //$(this).val(numeral(this.val().format('0,0.00')));
 })
 
//OnClick event that submits all demographics changes to the controller.
$("#btnEdit").on("click", function (e) {
    e.preventDefault();
    var isValid = true;

    isValid = $("#updateForm").valid();

    if (isValid) {
        _demographicData.Entity.SSN = $("#ssn").val();
        _demographicData.Entity.IDNumber = $("#numId").val();
        _demographicData.Entity.BirthDate = $("#dob").val();
        _demographicData.Entity.FirstName = $("#updateForm .firstName").val();
        _demographicData.Entity.MiddleName = $("#updateForm .middleName").val();
        _demographicData.Entity.LastName = $("#updateForm .lastName").val();
        _demographicData.Entity.SecondLastName = $("#updateForm .secondLastName").val();

        // FullName assigned in controller.
        // LegalGuardian will be assigned in select list on change event.
        // CivilStatus Assigned in dropdown on change event.
        _demographicData.Entity.MarriageDate = $("#marriageDate").val();
        _demographicData.Entity.DeceaseDate = $("#deceaseDate").val();

        _demographicData.Entity.HasDisability = $("#disabilityTrue").is(":checked");
        _demographicData.Entity.IsRehabilitated = $("#rehabilitatedTrue").is(":checked");
        _demographicData.Entity.IsWorking = $("#workingTrue").is(":checked");
        _demographicData.Entity.IsEmancipated = $("#enmancipatedTrue").is(":checked");

        _demographicData.Entity.IsStudying = $("#studyingTrue").is(":checked");
        _demographicData.Entity.SchoolStartDate = $("#studyFromDate").val();
        _demographicData.Entity.SchoolEndDate = $("#studyEndDate").val();

        _demographicData.Entity.HasWidowCertification = $("#widowTrue").is(":checked");
        _demographicData.Entity.WidowCertificationDate = $("#WidowCertificationDate").val();
        _demographicData.Entity.OccupationId = $("#occupation").val();

        var address =
            {
                "Line1": $("#updateForm .addressLine1").val(),
                "Line2": $("#updateForm .addressLine2").val(),
                "CityId": $("#updateForm .city").val(),
                "StateId": $("#updateForm .state").val() != null ? $("#updateForm .state").val() : null,
                "CountryId": $("#updateForm .country").val(),
                "OtherCity": $("#updateForm .otherCity").val() != null ? $("#updateForm .otherCity").val() : null,
                "ZipCode": $("#updateForm .zipcode").cleanVal().substring(0, 5) != null ? $("#updateForm .zipcode").cleanVal().substring(0, 5) : null,
                "ZipCodeExt": $("#updateForm .zipcode").cleanVal().substring(5, 9) != null ? $("#updateForm .zipcode").cleanVal().substring(5, 9) : null
            };

        address = ($("#updateForm .addressLine1").val().trim() == "" || $("#updateForm .addressLine1").val().trim() == null) ? null : address;

        _demographicData.Entity.Address = address;

        _demographicData.Entity.HomePhoneNumber  = $("#updateForm .homePhone").cleanVal();
        _demographicData.Entity.CellPhoneNumber  = $("#updateForm .cellPhone").cleanVal();
        _demographicData.Entity.WorkPhoneNumber  = $("#updateForm .workPhone").cleanVal();
        _demographicData.Entity.FaxPhoneNumber  = $("#updateForm .faxPhone").cleanVal();
        _demographicData.Entity.OtherPhoneNumber = $("#updateForm .otherPhone").cleanVal();

        _demographicData.Entity.MonthlyIncome = $("#monthlyIncome").val();
        _demographicData.Entity.Comments = $("#observation").val();

        _demographicData.TransferTypeId = $("#updateForm .paymentType").val();
        _demographicData.Entity.Comments = $("#observation").val();

        _demographicData.Entity.ModifiedReasonId = $("#updateForm .modifiedReason").val();
        _demographicData.Entity.OtherModifiedReason = $("#updateForm .otherReason").val();

        HasChildren = $("#hasChildrenTrue").is(":checked");
        Relationship = $("#updateForm .relationshipType").val();

        var dataList = {
            HasChildren: HasChildren,
            Relationship: Relationship,
            InsertNewEntity: _demographicData.EntitySic == null && _demographicData.EntitySif == null, //_demographicData.EntitySic == null (Means that there is a SiF Entity already attached to the case detail. 
            PostalAddress: address,
            CaseDetail: _demographicData,
            CaseDetailId: _demographicData.CaseDetailId
        };

        $.ajax({
            url: root + "Demographic/Edit/",
            type: "POST",
            data: dataList,
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (result) {
            _demographicData = result.Data;
            phone = [];
            CDI.hideWaitingMessage();
            ScrollTop("DemographicData");
            CDI.displayNotification("Se actualizo los datos demográficos para este caso.", "success");
        }).fail(function (result) {
            phone = [];
            CDI.hideWaitingMessage();
            ScrollTop("DemographicData");
            CDI.displayNotification("ocurrió un error al actualizar los datos demográficos.", "error");
        });
    }
});

$("#btnAddParticipant").on("click", function (e) {
    $("#DemographicParticipant").fadeOut(1000).removeClass("hidden");
    $("#DemographicData").fadeIn(1000).addClass("hidden");

    ScrollTop("DemographicParticipant");
});

//On Reset hide or disable necessary el;ements.
$("#btnCancel").on("click", function (e) {
    $(".state", $(e.target).closest("form")).prop("disabled", true);
    $(".city", $(e.target).closest("form")).prop("disabled", true);
    $(".otherRelationshipType", $(e.target).closest("form")).fadeIn(1000).addClass("hidden");
    $("#deceasedAgeInfo").fadeIn(1000).addClass("hidden");
    $("#widowCertificationInfo").fadeIn(1000).addClass("hidden");
    $("#isRehabilitatedInfo").fadeOut(1000).addClass("hidden");
    $("#isWorkingInfo").fadeOut(1000).addClass("hidden");
    $("#hasChildrenInfo").fadeOut(1000).addClass("hidden");
    $("#isEnmancipatedInfo").fadeOut(1000).addClass("hidden");
});

//After button is done resetting values.
$("#btnCancel").on("blur", function (e) {
    $(".radioFalse", $(e.target).closest("form")).prop("checked", true);
    demographicValidator.resetForm();
});

$("#participantType").on("change", function (e) {
    if ($("#participantType option:selected").text() == "Abogado") {
        $("#lawyerInfo").fadeIn(1000).removeClass("hidden");
        $("#firmInfo").fadeIn(1000).removeClass("hidden");
        $("#participantForm .paymentType option").filter(function () {
            return this.text == "Cheque";
        }).prop('selected', true);
        $("#participantForm .paymentType").prop('disabled', true);

    }
    else {
        $("#lawyerInfo").fadeOut(1000).addClass("hidden");
        $("#firmInfo").fadeOut(1000).addClass("hidden");
        $("#participantForm .paymentType").val("");
        $("#participantForm .paymentType").prop('disabled', false);
    }
});

//OnClick event that submits all demographics changes to the controller.
$("#createParticipant").on("click", function (e) {
    e.preventDefault();
    var isValid = true;
    var entity = {};
    var address;

    isValid = $("#participantForm").valid();

    if (isValid) {
        entity.SSN = $("#pssn").val();
        entity.IDNumber = $("#pnumId").val();
        entity.BirthDate = $("#pnumId").val();
        entity.FirstName = $("#participantForm .firstName").val().trim();
        entity.MiddleName = $("#participantForm .middleName").val();
        entity.LastName = $("#participantForm .lastName").val().trim();
        entity.SecondLastName = $("#participantForm .secondLastName").val();
        entity.ParticipantTypeId = $("#participantType").val();
        entity.Email = $("#participantForm .email").val();
        entity.HomePhoneNumber = $("#participantForm .homePhone").cleanVal();
        entity.CellPhoneNumber = $("#participantForm .cellPhone").cleanVal();
        entity.FaxPhoneNumber = $("#participantForm .faxPhone").cleanVal();
        entity.WorkPhoneNumber = $("#participantForm .workPhone").cleanVal();
        entity.OtherPhoneNumber = $("#participantForm .otherPhone").cleanVal();
        entity.Comments = $("#participantForm .observation").val();
        entity.CaseNumber = selectedCaseNumber;
        entity.CaseKey = selectedCaseKey;

        var address =
            {
                "Line1": $("#participantForm .addressLine1").val(),
                "Line2": $("#participantForm .addressLine2").val(),
                "CityId": $("#participantForm .city").val(),
                "StateId": $("#participantForm .state").val() != null ? $("#participantForm .state").val() : null,
                "CountryId": $("#participantForm .country").val(),
                "OtherCity": $("#participantForm .otherCity").val() != null ? $("#participantForm .otherCity").val() : null,
                "ZipCode": $("#participantForm  .zipcode").cleanVal().substring(0, 5) != null ? $("#updateForm .zipcode").cleanVal().substring(0, 5) : null,
                "ZipCodeExt": $("#participantForm  .zipcode").cleanVal().substring(5, 9) != null ? $("#updateForm .zipcode").cleanVal().substring(5, 9) : null
            };

        address = ($("#updateForm .addressLine1").val().trim() == "" || $("#updateForm .addressLine1").val().trim() == null) ? null : address;

        var dataList = {
            entity: entity,
            PostalAddress: address,
        };

        $.ajax({
            url: root + "Demographic/CreateParticipant/",
            type: "POST",
            data: dataList,
            beforeSend: function () {
                CDI.showWaitingMessage();
            }
        }).done(function (result) {
            var legalGuardian = $("#legalGuardian");
            legalGuardian.append($('<option/>', {
                value: result.EntityId,
                text: result.FullName
            }));
            $("#DemographicParticipant").fadeOut(1000).addClass("hidden");
            $("#DemographicData").fadeIn(1000).removeClass("hidden");
            //$("#cancelParticipant").click();
            ScrollTop("DemographicData");
            CDI.displayNotification("Se actualizo los datos demográficos para este caso.", "success");
            CDI.hideWaitingMessage();
        }).fail(function () {
            CDI.hideWaitingMessage();
            CDI.displayNotification("Ocurrió un error al momento de guardar el participante.", "error");
        });
    }
});

//OnClick returns to the demographic editing form.
$("#cancelParticipant").on('click', function (e) {
    $("#DemographicData").fadeOut(1000).removeClass("hidden");
    $("#DemographicParticipant").fadeIn(1000).addClass("hidden");

    ScrollTop("DemographicData");

    participantValidator.resetForm();
});

//OnBlur event that verifies if address line 1 is blank. If so Line1 = Line2.
$("input.addressLine2").on("blur", function (e) {
    var addressLine1 = $("input.addressLine1", $(e.target).closest("form"));
    var addressLine2 = $("input.addressLine2", $(e.target).closest("form"));
    if (addressLine1.val().trim() == "" && addressLine2.val().trim() != "") {
        addressLine1.val(addressLine2.val().trim());
        addressLine2.val("");
    }
});

//Display other reason information when other is selected.
var modifiedReasonChangeEvent = function (e) {
    var orInfo = $(".otherReasonInfo", $(e.target).closest("form"));
    var otherReason = $("input.otherReason", $(e.target).closest("form"));

    if ($("option:selected", this).text() == "Otros") {
        orInfo.fadeIn(1000).removeClass("hidden");
    }
    else {
        otherReason.val(null);
        orInfo.fadeOut(1000).addClass("hidden");
    }
}

//Gets all the state or city data based on the country selected.
var countryChangeEvent = function (e) {
    var json = { "countryId": $(this).val() };
    var state = $(".state", $(e.target).closest("form"));
    var city = $(".city", $(e.target).closest("form"));
    var country = $(this);

    city.empty();
    state.empty();
    state.prop("disabled", true);
    city.prop("disabled", true);

    if ($("option:selected", this).text() == "USA") {
        $.ajax({
            url: root + "Demographic/GetStates/",
            type: "POST",
            dataType: "json",
            data:json,
            async: false,
            success: function (result) {
                state.append($('<option/>', {
                    value: "",
                    text: "--Seleccionar--"
                }));
                $.each(result, function (index, value) {
                    state.append($('<option/>', {
                        value: value.StateId,
                        text: value.State1
                    }));
                });

                city.empty();
                city.prop("disabled", true);
                state.prop("disabled", false);
            },
            error: function (result) {
                CDI.displayNotification("ocurrió un error buscando los datos de estado.", "error");
            }
        });
    }
    else {
        $.ajax({
            url: root + "Demographic/GetCitiesByCountry/",
            type: "POST",
            data: json,
            dataType: "json",
            async: false,
            success: function (result) {
                state.empty();
                city.empty();
                state.append($('<option/>', {
                    value: "",
                    text: "--Seleccionar--"
                }));
                $.each(result, function (index, value) {
                    city.append($('<option/>', {
                        value: value.CityId,
                        text: value.City1
                    }));
                });


                state.prop("disabled", true);
                city.prop("disabled", false);
            },
            error: function (result) {
                CDI.displayNotification("ocurrió un error buscando los datos de ciudad.", "error");
            }
        });
    }
};

//Gets all cities based on the state selected.
var stateChangeEvent = function (e) {
    var json = { "stateId": $(this).val() };
    //var state = $('#state');
    var city = $(".city", $(e.target).closest("form"));

    city.prop("disabled", true);
    city.empty();

    if ($("option:selected", this).val() != "") {
        $.ajax({
            url: root + "Demographic/GetCitiesByState/",
            type: "POST",
            data: json,
            dataType: "json",
            async: false,
            success: function (result) {
                city.empty();
                city.append($('<option/>', {
                    value: "",
                    text: '--Seleccionar--'
                }));
                $.each(result, function (index, value) {
                    city.append($('<option/>', {
                        value: value.CityId,
                        text: value.City1
                    }));
                });

                city.prop("disabled", false);
            },
            error: function (result) {

            }
        });
    }
};

//Scroll to the top desire element
var ScrollTop = function (elementName) {
    $('html, body').animate({
        scrollTop: $("#" + elementName).offset().top - 100
    }, 500);
}

//Calculates the age of the deceased at the time of death.
//param:dob -- Date Of Birth
//param:comparisonDate -- Comparison Date
var GetAge = function (dob, comparisonDate) {
    var cDate = new Date(comparisonDate);
    var birthDate = new Date(dob);
    var age = cDate.getFullYear() - birthDate.getFullYear();
    var m = cDate.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && cDate.getDate() < birthDate.getDate())) {
        age--;
    }
    if (age < 0) {
        age = 0;
    }
    return age;
};

//Enable / Disable form element based on parameter value
var isReadOnlyForm = function (isReadOnly) {
    if (isReadOnly) {
        $('#updateForm input').prop('disabled', true);
        $('#updateForm select').prop('disabled', true);
        $('#updateForm textarea').prop('disabled', true);
        $('#updateForm input[type=button]').prop('disabled', true);
        $('#updateForm input[type=radio]').prop('disabled', true);
        $('#updateForm input[type=submit]').prop('disabled', true);
        $('#updateForm input[type=reset]').prop('disabled', true);
    }
    else {
        $('#updateForm input').prop('disabled', false);
        $('#updateForm select').prop('disabled', false);
        $('#updateForm textarea').prop('disabled', false);
        $('#updateForm input[type=button]').prop('disabled', false);
        $('#updateForm input[type=radio]').prop('disabled', false);
        $('#updateForm input[type=submit]').prop('disabled', false);
        $('#updateForm input[type=reset]').prop('disabled', false);
        if ($('#updateForm .country').val() != "" && $('#updateForm .country').val() != null) {
            if ($('#updateForm .state').val() == "" || $('#updateForm .state').val() == null) {
                $('#updateForm .state').prop('disabled', true);
            }
        }
    }
}

//Clears / Reset values from participant form.
var clearParticipant = function () {
    $("#participantForm input[type=text]").removeAttr("value");
    $("#participantForm select").removeAttr("value");
}

var relationshipTypeValidation = function () {
    var relationship = $("#updateForm .relationshipType");
    var otherRelationship = $("#updateForm .otherRelationshipType");
    var orInfo = $("#updateForm .otherRelationshipTypeInfo");

    if (relationship.val() == "Hijo") {
        $("#isEnmancipatedTrue").prop("checked", false);
        $("#isEnmancipatedFalse").prop("checked", true);

        $("#hasChildrenTrue").prop("checked", false);
        $("#hasChildrenFalse").prop("checked", true);
        $("#otherRelationship").val(null);

        orInfo.fadeOut(1000).addClass("hidden");
        $("#isEnmancipatedInfo").fadeIn(1000).removeClass("hidden");
        $("#hasChildrenInfo").fadeOut(1000).addClass("hidden");
    }
    else if (relationship.val() == "Viuda(o)" || relationship.val() == "Concubina(o)") {
        $("#hasChildrenTrue").prop("checked", false);
        $("#hasChildrenFalse").prop("checked", true);

        $("#isEnmancipatedTrue").prop("checked", false);
        $("#isEnmancipatedFalse").prop("checked", true);
        otherRelationship.val("");

        orInfo.fadeOut(1000).addClass("hidden");
        $("#hasChildrenInfo").fadeIn(1000).removeClass("hidden");
        $("#isEnmancipatedInfo").fadeOut(1000).addClass("hidden");
    }
    else if (relationship.val() == "Otros") {
        $("#hasChildrenTrue").prop("checked", false);
        $("#hasChildrenFalse").prop("checked", true);
        $("#isEnmancipatedTrue").prop("checked", false);
        $("#isEnmancipatedFalse").prop("checked", true);

        orInfo.fadeIn(1000).removeClass("hidden");
        $("#hasChildrenInfo").fadeOut(1000).addClass("hidden");
        $("#isEnmancipatedInfo").fadeOut(1000).addClass("hidden");
    }
    else {
        $("#hasChildrenTrue").prop("checked", false);
        $("#hasChildrenFalse").prop("checked", true);
        $("#isEnmancipatedTrue").prop("checked", false);
        $("#isEnmancipatedFalse").prop("checked", true);
        otherRelationship.val(null);

        orInfo.fadeOut(1000).addClass("hidden");
        $("#hasChildrenInfo").fadeOut(1000).addClass("hidden");
        $("#isEnmancipatedInfo").fadeOut(1000).addClass("hidden");
    }
} 

//Set rules of validation when the user submits the form.
var validateForm = function () {
    participantValidator = $("#participantForm").validate({
        rules: {
            participantType: {
                required: true
            },
            lawyer: {
                required: {
                    depends: function (element) {
                        return $("#participantType option:selected").text() == "Abogado" ? true : false;
                    }
                }
            },
            pssn: {
                required: true
            },
            pnumId: {
                required: true
            },
        },
        messages: {
            participantType: {
                required: "Este campo es obligatorio."
            },
            lawyer: {
                required: "Este campo es obligatorio."
            },
            pssn: {
                required: "Este campo es obligatorio."
            },
            pnumId: {
                required: "Este campo es obligatorio."
            },
        }
    });

    demographicValidator = $("#updateForm").validate({
        rules: {
            ssn: {
                required: true
                //minlength: 9
            },
            numId: {
                required: true
            },
            dob: {
                required: true
            },
            legalGuardian: {
                required: {
                    depends: function (element) {
                        return $("#disabilityTrue").is(":checked") || GetAge(($("#dob").val(), new Date()) < 18);
                    }
                }
            },
            deceaseDate: {
                required: {
                    depends: function (element) {
                        return $("#updateForm .modifiedReason option:selected").text() == "Muerte";
                    }
                }
            },
            civilStatus: {
                required: true
            },
            marriageDate: {
                required: {
                    depends: function (element) {
                        return $("#civilStatus option:selected").text() == "Casado";
                    }
                }
            },
            isStudying: {
                required: {
                    depends: function (element) {
                        if (_demographicData.CaseKey != "00")
                            return $("#relationship option:selected").text() == "Hijo" && (GetAge(($("#dob").val(), new Date()) >= 18) && GetAge(($("#dob").val(), new Date()) < 25))
                        else
                            return false;
                    }
                }
            },
            studyFromDate: {
                required: {
                    depends: function (element) {
                        if (_demographicData.CaseKey != "00")
                            return $("#studyingTrue").is(":checked")
                        else
                            return false;

                    }
                }
            },
            studyToDate: {
                required: {
                    depends: function (element) {
                        if (_demographicData.CaseKey != "00")
                            return $("#studyingTrue").is(":checked")
                        else
                            return false;
                    }
                },
                greaterThan: "#studyFromDate"
            },
            isWidow: {
                required: {
                    depends: function (element) {
                        if (_demographicData.CaseKey != "00")
                            return $("#relationship option:selected").text() == "Viuda(o)"
                        else
                            return false;
                    }
                }
            },
            widowCertificationDate: {
                required: {
                    depends: function (element) {
                        if (_demographicData.CaseKey != "00")
                            return $("#widowTrue").is(":checked")
                        else
                            return false;
                    }
                }
            },
            occupation: {
                required: {
                    depends: function (element) {
                        if (_demographicData.CaseKey != "00")
                            return !($("#disabilityTrue").is(":checked")) || GetAge(($("#dob").val(), new Date()) >= 18);
                        else
                            return false;
                    }
                }
            },
        },
        messages: {
            ssn: {
                required: "Este campo es obligatorio.",
                minlength: "Número de seguro social es invalido."
            },
            numId: {
                required: "Este campo es obligatorio."
            },
            dob: {
                required: "Este campo es obligatorio."
            },
            firstName: {
                required: "Este campo es obligatorio."
            },
            lastName: {
                required: "Este campo es obligatorio."
            },
            legalGuardian: {
                required: "Este campo es obligatorio."
            },
            civilStatus: {
                required: "Este campo es obligatorio."
            },
            marriageDate: {
                required: "Este campo es obligatorio."
            },
            relationship: {
                required: "Debe seleccionar una opción valida."
            },
            otherRelationship: {
                required: "Otro tipo de relación es obligatorio cuando la relacion seleccionada es otros."
            },
            isStudying: {
                required: "Este campo es obligatorio cuando la relacion seleccionada es hijo y la edad del beneficiario es entre los 18 y 25 años."
            },
            studyFromDate: {
                required: "Este campo es obligatorio cuando se elige Sí en el campo ¿Estudia?"
            },
            studyToDate: {
                required: "Este campo es obligatorio cuando se elige Sí en el campo ¿Estudia?",
                greaterThan: "La fecha Hasta debe ser mayor a la fecha Desde."
            },
            isWidow: {
                required: "Este campo es obligatorio cuando la relación seleccionada es viuda(o)."
            },
            widowCertificationDate: {
                required: "Este campo es obligatorio cuando se elige Sí en el campo ¿Certificación de Viuda?"
            },
            occupation: {
                required: "Este campo es obligatorio."
            },
            zipcode: {
                minlength: "El zipcode entrado es incorrecto."
            },
            paymentType: {
                required: "Este campo es obligatorio."
            },
            modifiedReason: {
                required: "Este campo es obligatorio."
            },
            otherReason: {
                required: "El campo Otro es obligatorio cuando se selecciona otro en el campo Razón de Modificación."
            },
            deceaseDate: {
                required: "Este campo es obligatorio cuando se escoge la opción de Muerte en el campo Razón de Modificación."
            }
        }
    });

    $.validator.addClassRules({
        firstName: {
            required: true
        },
        lastName: {
            required: true
        },
        relationship: {
            required: {
                depends: function (element) {
                    if (_demographicData.CaseKey != "00")
                        return true;
                    else
                        return false;
                }
            }
        },
        otherRelationship: {
            required: {
                depends: function (element) {
                    if (_demographicData.CaseKey != "00")
                        return $("#relationship option:selected").text() == "Otros";
                    else
                        return false;
                }
            }
        },
        zipcode: {
            validZipCode: true,
            minlength: 5
        },
        paymentType: {
            required: true
        },
        modifiedReason: {
            required: true
        },
        otherReason: {
            required: {
                depends: function (element) {
                    return $("#modifiedReason option:selected").text() == "Otros";
                }
            }
        },
        //"address-group":{
        //    required: true
        //},
        homePhone: {
            require_from_group: [1, ".phone-group"]
        },
        cellPhone: {
            require_from_group: [1, ".phone-group"]
        },
        faxPhone: {
            require_from_group: [1, ".phone-group"]
        },
        workPhone: {
            require_from_group: [1, ".phone-group"]
        },
        otherPhone: {
            require_from_group: [1, ".phone-group"]
        },
    });
}