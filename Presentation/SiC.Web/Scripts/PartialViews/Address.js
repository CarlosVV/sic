var prevPhyCityId;
var prevPosCityId;

var refreshCityList = function (targetSelector, countryId) {
    var ddl = $(targetSelector);
    ddl.find('option:gt(0)').remove();
    ddl.attr('readonly', 'readonly');

    var post = $.Deferred();

    if (countryId) {
        var post = $.ajax({
            url: "/Employer/GetFilteredCityList/" + countryId,
            dataType: "json",
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            async: true,
            success: function (data) {
                $.each(data, function (i, data) {
                    ddl.append('<option value="' + data.Value + '">' + data.Text + '</option>');
                });

                ddl.removeAttr('readonly');
            },
            error: function (jqxhr, textStatus, errorThrown) {
                ddl.attr('readonly', 'readonly');
            }
        })

    }
    else {
        ddl.attr('readonly', 'readonly');
        post.resolve;
    }
    return post.promise();
}

var setCity = function (id, countryId, cityId) {
    refreshCityList(id, countryId).then(function(){
        $(id).val(cityId);
    });
}


$(function () {

    $('#employerAddress_phisicalAddress_CountryId').on('change', function () {
        prevPhyCityId = $("#employerAddress_phisicalAddress_CityId").val();

        refreshCityList("#employerAddress_phisicalAddress_CityId", this.value).then(function () {
            $("#employerAddress_phisicalAddress_CityId").val(prevPhyCityId);
        });
    });

    $('#employerAddress_postalAddress_CountryId').on('change', function () {
        prevPosCityId = $("#employerAddress_postalAddress_CityId").val();

        refreshCityList("#employerAddress_postalAddress_CityId", this.value).then(function () {
            $("#employerAddress_postalAddress_CityId").val(prevPosCityId);
        });
    });

    $('#employerAddress_phisicalAddress_CountryId').change();
    $('#employerAddress_postalAddress_CountryId').change();

});