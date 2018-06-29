var applyEmployerInformationFormatting = function () {
    formatField("#PayrollCFSETotalWages", '0,0.00');
    formatField("#PayrollOtherTotalWages", '0,0.00');
    formatField("#PayrollDifference", "(0.00 %)");
}

$(function () {

    //Maskings, formatting and validations
    applyEmployerInformationFormatting();
});