CDI.DisplayTableReference = (function () {
    var _start = function () {

        $("#ReferenceTables").change(function () {
            var valor = this.value;

            switch(valor)
            {
                case "1":
                    $('.tablepanel').hide();
                    $("#addressTypePanel").show();
                    break;
                case "2":
                    $('.tablepanel').hide();
                    $("#applyToPanel").show();
                    break;               
                case "4":
                    $('.tablepanel').hide();
                    $("#civilStatusPanel").show();                    
                    break;
                case "5":
                    $('.tablepanel').hide();
                    $("#genderPanel").show();                    
                    break;
                case "6":
                    $('.tablepanel').hide();
                    $("#internetTypePanel").show();                   
                    break;
                case "7":
                    $('.tablepanel').hide();
                    $("#paymentClassPanel").show();
                    break;
                case "8":
                    $('.tablepanel').hide();
                    $("#transferTypePanel").show();
                    break;
                case "9":
                    $('.tablepanel').hide();
                    $("#transactionTypePanel").show();
                    break;
                case "10":
                    $('.tablepanel').hide();
                    $("#paymentConceptPanel").show();
                    break;
                case "11":
                    $('.tablepanel').hide();
                    $("#relationshipTypePanel").show();
                    break;
                case "12":
                    $('.tablepanel').hide();
                    $("#relationshipCategoryToPanel").show();
                    break;
            }
        });

    };

    return {
        start: _start
    }
})();

$(function () {
    CDI.DisplayTableReference.start();
});