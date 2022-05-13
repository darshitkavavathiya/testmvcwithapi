

$(document).ready(function () {

 
    stateandcountryselect();

});

function stateandcountryselect() {

    $("#Countryid").append(' <option value="">Select Country</option> ');

    $.ajax({
        type: 'GET',
        url: '/Customer/GetCountrys',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',

        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                $("#Countryid").append(' <option value="' + result[i].countryId + '">' + result[i].countryname + '</option> ');

            }
        },
        error: function () {
            alert("error");
        }
    });

    $("#Stateid").append(' <option value="">Please Select Country</option> ');

    $("#Countryid").on("change", function () {

        $("#Stateid").empty();
        $("#Stateid").append(' <option value=""> Select State</option> ');
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

            },
            error: function () {
                alert("error");
            }
        });

    });

}




$(document).on("click", "#filterreset", function () {



    GetDataforTable("/Home/CustomerDynemicTable", "#CustomerFilterTable", "iBtlp", null);


    });
$(document).on("change keyup", "#filterform .form-control", function () {

    var data = $("#filterform").serializeArray();



    GetDataforTable("/Home/CustomerDynemicTable", "#CustomerFilterTable", "iBtlp", data);

});




$(document).on("click", "#export", function () {


        var type = 'xlsx';
        var data = document.getElementById('CustomerFilterTable');
        var file = XLSX.utils.table_to_book(data, { sheet: "sheet1" });
        XLSX.write(file, { bookType: type, bookSST: true, type: 'base64' });
        XLSX.writeFile(file, 'CustomerList.' + type);
});











$(document).on('change', '#selectall', function () {

    
    checked = $(this).prop('checked');

        if (checked) {
            $(".selectAllBtn").click();
        } else {
            $(".selectNoneBtn").click();

        }
});