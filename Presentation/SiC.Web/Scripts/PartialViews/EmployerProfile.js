var profileFormValidator;

$(function () {

    notifyPastSubmit();
  
    $("#btnReturn").on("click", function () {
        history.go(-1);
    });

    //Maskings and validations
    formatField("#employerInformation_PayrollCFSETotalWages", '0,0.00');
    formatField("#employerInformation_PayrollOtherTotalWages", '0,0.00');
    formatField("#employerInformation_PayrollDifference", "(0.00 %)");

    $("#employerContact_Phone_phoneNumber").mask("(000) 000-0000");
    $("#employerContact_Fax_phoneNumber").mask("(000) 000-0000");
    $("#employerContact_Phone_phoneExt").mask('0#');


    profileFormValidator = $('#profileForm').validate({
        rules: {
            "employerContact.ContactName": {
                required: false
            },
            "employerContact.Email": {
                required: false,
                email: true
            },
            "employerContact.Phone.phoneNumber": {
                required: false,
                minlength: 14
            },
            "employerContact.Phone.phoneExt": {
                required: false
            },
            "employerContact.Fax.phoneNumber": {
                required: false,
                minlength: 14
            },
            "employerAddress.phisicalAddress.Line1": {
                required: true
            },
            "employerAddress.postalAddress.Line1": {
                required: true
            },
            "employerAddress.phisicalAddress.CityId": {
                required: true
            },
            "employerAddress.postalAddress.CityId": {
                required: true
            },
            "employerAddress.phisicalAddress.CountryId": {
                required: true
            },
            "employerAddress.postalAddress.CountryId": {
                required: true
            }
        },
        messages: {
            "employerContact.ContactName": {
                required: "Requerido"
            },
            "employerContact.Email": {
                required: "Requerido",
                email: "El formato es nombre@dominio.com"
            },
            "Phone.phoneNumber": {
                required: "Requerido",
                minlength: "Debe tener 10 dígitos"
            },
            "Phone.phoneExt": {
                required: "Requerido"
            },
            "Fax.phoneNumber": {
                required: "Requerido",
                minlength: "Debe tener 10 dígitos"
            },
            "employerAddress.phisicalAddress.Line1": {
                required: "Requerido"
            },
            "employerAddress.postalAddress.Line1": {
                required: "Requerido"
            },
            "employerAddress.phisicalAddress.CityId": {
                required: "Requerido"
            },
            "employerAddress.postalAddress.CityId": {
                required: "Requerido"
            },
            "employerAddress.phisicalAddress.CountryId": {
                required: "Requerido"
            },
            "employerAddress.postalAddress.CountryId": {
                required: "Requerido"
            }
        },
        submitHandler: function(form){
            $("#employerContact_Phone_phoneNumber").unmask();
            $("#employerContact_Fax_phoneNumber").unmask();
            form.submit();
        }
    });
});