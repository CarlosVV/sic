// Make sure jQuery has been loaded before app.js
if (typeof jQuery === "undefined") {
    throw new Error("Requires jQuery");
}

$.holdReady(true);

// Configure globalize
var locale = $("html").attr("lang") || "es";

$.when(
    $.getJSON(root + "scripts/cldr/supplemental/likelySubtags.json"),
    // Date
    $.getJSON(root + "scripts/cldr/main/" + locale + "/ca-gregorian.json"),
    $.getJSON(root + "scripts/cldr/main/" + locale + "/timeZoneNames.json"),
    $.getJSON(root + "scripts/cldr/supplemental/timeData.json"),
    $.getJSON(root + "scripts/cldr/supplemental/weekData.json"),
    // Number
    $.getJSON(root + "scripts/cldr/main/" + locale + "/numbers.json"),
    $.getJSON(root + "scripts/cldr/supplemental/numberingSystems.json")
).done(function (result1, result2, result3, result4, result5, result6, result7) {
    Globalize.load(result1[0]);
    Globalize.load(result2[0]);
    Globalize.load(result3[0]);
    Globalize.load(result4[0]);
    Globalize.load(result5[0]);
    Globalize.load(result6[0]);
    Globalize.load(result7[0]);

    Globalize.locale(locale);

    $.holdReady(false);
});

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}

var CDI = CDI || {};

CDI = (function () {
    "use strict";

    var paymentStatus =
        {
            Cancelado: 1,
            Movimiento: 3,
            Pagado: 4,
            Generado: 11,
            Registrado: 12,
            Aprobado: 13,
            SimeraS: 14,
            SimeraP: 15,
            SapS: 16,
            SapP: 17
        };

    var addValidationRules = function (rules) {
        for (var item in rules) {
            if (rules.hasOwnProperty(item)) {
                $("#" + item).rules("add", rules[item]);
            }
        }
    };

    var removeValidationRules = function (rules) {
        for (var item in rules) {
            if (rules.hasOwnProperty(item)) {
                $("#" + item).rules("remove");
            }
        }
    };

    var displayNotification = function (message, messagetype) {
        if (messagetype === "success") {
            toastr.success(message, "SiC");
        }
        else if (messagetype === "error") {
            toastr.error(message, "SiC");
        }
        else if (messagetype === "warning") {
            toastr.warning(message, "SiC");
        }
        else if (messagetype === "info") {
            toastr.info(message, "SiC");
        }
        else {
            toastr.success(message, "SiC");
        }
    };

    var showWaitingMessage = function () {
        $.blockUI({ message: "Processing..." });
    };

    var hideWaitingMessage = function () {
        $.unblockUI();
    };

    //US Date Validation when jquery.validation is not being used
    var isValidDateUs = function (value) {
        var check = false;
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if (re.test(value)) {
            var adata = value.split("/");
            var mm = parseInt(adata[0], 10);
            var dd = parseInt(adata[1], 10);
            var yyyy = parseInt(adata[2], 10);
            var xdata = new Date(yyyy, mm - 1, dd);
            if ((xdata.getFullYear() === yyyy) && (xdata.getMonth() === mm - 1) && (xdata.getDate() === dd))
                check = true;
        } else
            check = false;

        return check;
    }

    var init = function () {
        $.ajaxSetup({
            error: function (xhr, textStatus) {
                try {
                    var message;

                    if (xhr.status === 0) {
                        message = "Not connect. Verify Network.";
                    }
                    else if (xhr.status === 404) {
                        message = "Requested page not found. [404]";
                    }
                    else if (xhr.status === 500) {
                        message = "Internal Server Error [500].";
                    }
                    else if (xhr.status === 401) {
                        location.href = "/";
                        return;
                    }
                    else if (textStatus === "parsererror") {
                        message = "Requested JSON parse failed.";
                    }
                    else if (textStatus === "timeout") {
                        message = "Time out error.";
                    }
                    else if (textStatus === "abort") {
                        message = "Ajax request aborted.";
                    }
                    else {
                        try {
                            message = JSON.parse(xhr.responseText);
                        } catch (ex) {
                            message = xhr.responseText;
                        }
                    }

                    CDI.displayNotification(message, "error");
                }
                catch (err) {
                    CDI.displayNotification("Error", "error");
                }
            }
        });

        // Collapse panel
        $(".collapse-link").click(function () {
            var panel = $(this).closest("div.panel");
            var button = $(this).find("i");
            var content = panel.find("div.panel-body");
            content.slideToggle(200);
            button.toggleClass("fa-chevron-up").toggleClass("fa-chevron-down");
            panel.toggleClass("").toggleClass("border-bottom");
        });

        $(".modal").appendTo("body");

        // Datepicker
        if ($.fn.datepicker) {
            $.fn.datepicker.defaults.todayBtn = "linked";
            $.fn.datepicker.defaults.clearBtn = false;
            $.fn.datepicker.defaults.keyboardNavigation = false;
            $.fn.datepicker.defaults.forceParse = false;
            $.fn.datepicker.defaults.calendarWeeks = false;
            $.fn.datepicker.defaults.autoclose = true;
            $.fn.datepicker.defaults.todayHighlight = true;
            $.fn.datepicker.defaults.orientation = "bottom";
            $.fn.datepicker.defaults.templates = {
                leftArrow: "<i class='fa fa-long-arrow-left'></i>",
                rightArrow: "<i class='fa fa-long-arrow-right'></i>"
            };
        }

        // Validation
        if ($.validator) {
            $.validator.setDefaults({
                ignore: ":hidden",
                ignoreTitle: true,
                focusInvalid: true,
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                errorClass: "has-error",
                validClass: "has-success",
                errorPlacement: function (error, element) {
                    if (element.is(":checkbox") || element.parent(".input-group").length) {
                        error.addClass("text-danger").insertAfter(element.parent());
                    }
                    else {
                        error.addClass("text-danger").insertAfter(element.closest(".form-group").children().last());
                    }
                },
                invalidHandler: function (form, validator) {
                    if (!validator.numberOfInvalids())
                        return;

                    $("html, body").animate({
                        scrollTop: $(validator.errorList[0].element).focus().offset().top - 100
                    }, 300);
                }
            });

            $.validator.addMethod("require_from_group", function (value, element, options) {
                var numberRequired = options[0];
                var selector = options[1];
                var fields = $(selector, element.form);
                var filledFields = fields.filter(function () {
                    // it's more clear to compare with empty string
                    return $(this).val() !== "";
                });
                var emptyFields = fields.not(filledFields);
                // we will mark only first empty field as invalid
                if (filledFields.length < numberRequired && emptyFields[0] === element) {
                    return false;
                }
                return true;
                // {0} below is the 0th item in the options field
            }, $.validator.format("Por favor, complete al menos {0} de estos campos."));

            $.validator.addMethod(
               "dateUS",
               function (value, element) {
                   var check = false;
                   var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
                   if (re.test(value)) {
                       var adata = value.split("/");
                       var mm = parseInt(adata[0], 10);
                       var dd = parseInt(adata[1], 10);
                       var yyyy = parseInt(adata[2], 10);
                       var xdata = new Date(yyyy, mm - 1, dd);
                       if ((xdata.getFullYear() === yyyy) && (xdata.getMonth() === mm - 1) && (xdata.getDate() === dd))
                           check = true;
                   } else
                       check = false;
                   return this.optional(element) || check;
               },
               "Por favor, escribe una fecha válida."
            );

            $.validator.addMethod(
                "lessThanZero",
                function (value, element) {
                    var check = false;

                    if (parseFloat($(element).autoNumeric("get")) > 0)
                        check = true;

                    return this.optional(element) || check;
                },
                "Por favor, ingrese un monto mayor a 0."
            );

            $.validator.addMethod(
                "greaterThan",
                function (value, element, params) {
                    var target = $(params).val();
                    var isValueNumeric = !isNaN(parseFloat(value)) && isFinite(value);
                    var isTargetNumeric = !isNaN(parseFloat(target)) && isFinite(target);

                    if (isValueNumeric && isTargetNumeric) {
                        return Number(value) > Number(target);
                    }

                    if (!/Invalid|NaN/.test(new Date(value))) {
                        return new Date(value) > new Date(target);
                    }

                    return false;
                },
                'El valor entrado debe ser mayor a {0}');

            $.validator.addMethod(
               "lessOrEqualThan",
               function (value, element, params) {
                   var target = $(params).val();
                   var isValueNumeric = !isNaN(parseFloat(value)) && isFinite(value);
                   var isTargetNumeric = !isNaN(parseFloat(target)) && isFinite(target);

                   if (isValueNumeric && isTargetNumeric) {
                       return Number(value) <= Number(target);
                   }

                   if (!/Invalid|NaN/.test(new Date(value))) {
                       return new Date(value) <= new Date(target);
                   }

                   return false;
               },
                'El valor entrado debe ser mayor a {0}');

            $.validator.addMethod("validZipCode",
                function (value, element) {
                    return this.optional(element) || /^\d{5}(?:-\d{4})?$/.test(value);
                },
                "Zipcode entrado es incorrecto.");
        }

        // toastr
        if (toastr) {
            toastr.options = {
                preventDuplicates: true,
                progressBar: false,
                positionClass: "toast-bottom-right",
                onclick: null,
                timeOut: 5000,
                extendedTimeOut: 1000,
                fadeOut: 250,
                fadeIn: 250,
                showDuration: 400,
                hideDuration: 1000,
                escapeHtml: true,
                closeButton: true,
                showEasing: "swing",
                hideEasing: "linear",
                showMethod: "fadeIn",
                hideMethod: "fadeOut",
                debug: false
            };
        }

        // autonumeric
        if ($.fn.autoNumeric) {
            $.extend($.fn.autoNumeric.defaults, {
                aSep: "",
                aDec: ".",
                aSign: "$ ",
                vMin: "-9999999999999.99",
                vMax: "9999999999999.99"
            });
        }

        // blockUI
        if (jQuery.blockUI !== undefined) {
            jQuery.blockUI.defaults.message = null;
            jQuery.blockUI.defaults.css.padding = 15;
            jQuery.blockUI.defaults.css.margin = 0;
            jQuery.blockUI.defaults.css.width = "30%";
            jQuery.blockUI.defaults.css.top = "35%";
            jQuery.blockUI.defaults.css.left = "35%";
            jQuery.blockUI.defaults.css.textAlign = "center";
            jQuery.blockUI.defaults.css.color = "#fff";
            jQuery.blockUI.defaults.css.border = "0px none transparent";
            jQuery.blockUI.defaults.css.backgroundColor = "#272822";
            jQuery.blockUI.defaults.css.cursor = "wait";

            jQuery.blockUI.defaults.fadeOut = 300;
            jQuery.blockUI.defaults.baseZ = 100000;
            jQuery.blockUI.defaults.theme = false;
            jQuery.blockUI.defaults.overlayCSS.backgroundColor = "#DBDBDB";
            jQuery.blockUI.defaults.overlayCSS.opacity = 0.7;
            jQuery.blockUI.defaults.overlayCSS.cursor = "wait";
        }

        if ($.fn.dataTable) {
            $.extend(true, $.fn.dataTable.ext.classes, {
                sProcessing: "dataTables_processing bg-info"
            });
        }
    };

    return {
        init: init,
        displayNotification: displayNotification,
        addValidationRules: addValidationRules,
        removeValidationRules: removeValidationRules,
        showWaitingMessage: showWaitingMessage,
        hideWaitingMessage: hideWaitingMessage,
        isValidDateUS: isValidDateUs,
        PaymentStatus: paymentStatus
    }
})();

$(function () {
    CDI.init();
});