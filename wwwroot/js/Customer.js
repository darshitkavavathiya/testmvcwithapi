

$(document).ready(function () {

    var countryid = $("#idforcountry").text();
    var stateid = $("#idforstate").text();
    countryid = parseInt(countryid);
    stateid = parseInt(stateid);
    $("#Countryid").append(' <option value="0">Select Country</option> ');

    $.ajax({
        type: 'GET',
        url: '/Customer/GetCountrys',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',

        success: function (result) {

            for (var i = 0; i < result.length; i++) {
       
                    $("#Countryid").append(' <option value="' + result[i].countryId + '">' + result[i].countryname + '</option> ');
                
            }
            $("#Countryid").val(countryid).change();
        },
        error: function () {
            alert("error");
        }
    });

    $("#Stateid").append(' <option value="0">Please Select Country</option> ');

    $("#Countryid").on("change", function () {
        
        $("#Stateid").empty();
        $("#Stateid").append(' <option value="0"> Select State</option> ');
        var data = {};
        var temp = $(this).val();
      
        data.CountryId = parseInt(temp);

        $.ajax({
            type: 'Post',
            url: '/Customer/GetStates',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: data,

            success: function (result) {

               
                for (var i = 0; i < result.length; i++) {
                   
                        $("#Stateid").append(' <option value="' + result[i].stateId + '">' + result[i].statename + '</option> ');
                    
                   

                }
                if (countryid == data.CountryId) {
                    $("#Stateid").val(stateid).change();
                } else {
                    $("#Stateid").val(0).change();
                }
            },
            error: function () {
                alert("error");
            }
        });

    });


});

