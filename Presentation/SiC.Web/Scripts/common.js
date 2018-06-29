var formatDate = function(dateStr, dateFormatType){
    var parsedDate = new Date(parseInt(dateStr.substr(6)));
    var jsDate = new Date(parsedDate);

    if (dateFormatType = 1)
        return (jsDate.getMonth() + 1) + '/' + jsDate.getDate() + '/' + jsDate.getFullYear();
    else
        return jsDate.getDate() + '/' + (jsDate.getMonth() + 1) + '/' + jsDate.getFullYear();
};

var generateNoty = function (txt,tp,mod,lay,closeBtn,btns, callbk, tm,row,id) {
    var n = noty({
        text: txt,
        type: tp,
        modal: false,
        dismissQueue: true,
        maxVisible: 50,
        layout: lay,
        closeWith: (closeBtn === 'y' ? ['button'] : []),
        theme: 'defaultTheme',
        buttons:btns==='y' ? [
            { addClass: 'btn btn-success',
              text: 'No',
              onClick: function ($noty) {
                  $noty.close();
              }
            },
            { addClass: 'btn btn-danger', 
              text: 'Sí',
              onClick: function ($noty) {
                  $noty.close();
                  callbk(row, id);
              }    
            }]:false
    });

    if (!tm) {
        setTimeout(function () { n.close(); }, 4000);
    }
};

var formatField = function (selector, format) {
    $(selector).val(numeral($(selector).val()).format(format))
}

var getNextYear = function(){
    return new Date().getFullYear() + 1;
}

jQuery.validator.setDefaults({
    validClass: "has-success",
    errorClass: "has-error",
    highlight: function (element, errorClass, validClass) {
        $(element).closest(".form-group").addClass(errorClass).removeClass(validClass);
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).closest(".form-group").removeClass(errorClass).addClass(validClass);
    },
    errorElement: "span",
    errorPlacement: function (error, element) {
        if (element.parent().hasClass("input-group")) {
            error.insertAfter(element.parent()).addClass("help-block");
        } else {
            error.insertAfter(element).addClass("help-block");
        }
    },
    focusInvalid: false,
    invalidHandler: function (form, validator) {
        if (!validator.numberOfInvalids())
            return;

        $('html, body').animate({
            scrollTop: $(validator.errorList[0].element).focus().offset().top - 100
        }, 300);
    }
});