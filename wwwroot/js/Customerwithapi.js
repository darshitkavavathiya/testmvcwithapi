
function listofcustomer() {

    $.ajax({
        type: "GET",
        url: "https://localhost:5001/api/customer",
        headers: {
            'Content-Type': 'application/json'
        },

        success: function (result) {
            renderDataInTheTable(result, "#Customertable", "iBtlp")

        }, //End of AJAX Success function  

        failure: function (data) {
            alert(data.responseText);
            console.log(data);
        }, //End of AJAX failure function  
        error: function (data) {
            alert(data.responseText);
            console.log(data);
        } //End of AJAX error function  

    });


}
function detailsofcustomer(id) {
    $.ajax({
        type: "GET",
        url: "https://localhost:5001/api/customer/"+id,
        headers: {
            'Content-Type': 'application/json'
        },

        success: function (result) {
            var details;
            $.each(result, function (i, item) {
                details = '<dl class="row">'
                $.map(item, function (value, key) {
                    if (value != null)
                        details += '<dt class="col-sm-4">' + key + '</dt><dd class="col-sm-8">' + value + '</dd>';
                });
                details+='</dl>'
            });

            $("#CustomerDetailsBody").html(details);

            $("#RowPopup a.btn-primary").attr("href", "/Customerapi/Edit/" + id);

        },   

        failure: function (data) {
            alert(data.responseText);
            console.log(data);
        }, 
        error: function (data) {
            alert(data.responseText);
            console.log(data);
        }   

    });
}
function deletecustomer(id) {

    swal({
        title: "Are you sure",
        text: "Press Delete to delete customer no:" + id,
        icon: "error",
        animation: true,
        toast: true,
        dangerMode: true,
        buttons: {
            cancel: true,

            confirm: {
                text: "Delete",
                value: id,
            },
        }
    }).then((value) => {
        if (value == id) {
            deletecustomerajax(id);
        }
    });;
}


function deletecustomerajax(id) {
    $.ajax({
        type: "DELETE",
        url: "https://localhost:5001/api/customer/" + id,
        headers: {
            'Content-Type': 'application/json'
        },

        success: function (result) {
            listofcustomer();

            if (result == 'Deleted') {
                swal("Customer no:" + id + " is deleted successfully", {
                    icon: "success",
                    timer: 3000,
                });
            }
        }, 
        failure: function (data) {
            alert(data.responseText);
            console.log(data);
        },  
        error: function (data) {
            alert(data.responseText);
            console.log(data);
        } 

    });

}

function updatecustomer(data) {
    console.log(JSON.stringify(data));

    $.ajax({
        type: "Put",
        url: "https://localhost:5001/api/customer",
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(data),

        success: function (result) {
         

            if (result == 'success') {
                window.location.href = "/Customerapi/Index";
                swal({

                    title: "Success",
                    text: "customer Updated Successfully",
                    icon: "success",
                    animation: true,
                    timer: 3000,
                    buttons: false,
                    toast: true,
                    timerProgressBar: true,
                    
                    showCloseButton: false,
                    showCancelButton: false,
                    width: 800,
                    showConfirmButton: false,
                    onClose: () => {
                        window.location.href = "/Customerapi/Index";

                    }
                });
            }
        }, 
        failure: function (data) {
            alert(data.responseText);
            console.log(data);
        },
        error: function (data) {
            alert(data.responseText);
            console.log(data);
        } 
    });







}
function addcustomer(data) {

    $.ajax({
        type: "Post",
        url: "https://localhost:5001/api/customer",
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(data),

        success: function (result) {


            if (result == 'success') {
                swal({

                    title: "Success",
                    text: "customer Added Successfully",
                    icon: "success",
                    animation: true,
                    timer: 3000,
                    buttons: false,
                    toast: true,
                    timerProgressBar: true,

                    showCloseButton: false,
                    showCancelButton: false,
                    width: 800,
                    showConfirmButton: false,
                    onClose: () => {
                        window.location.href = "/Customerapi/Index";

                    }
                });
            }
        },
        failure: function (data) {
            alert(data.responseText);
            console.log(data);
        },
        error: function (data) {
            alert(data.responseText);
            console.log(data);
        }
    });







}

var customerid;
$(document).ready(function () {
    listofcustomer();

    $("#Customertable").delegate("tbody tr", "click", function (e) {

        customerid = $(this).find("td:eq(1)").text();
        $("#RowPopupModelBtn").click();
        detailsofcustomer(customerid);

    });

    $("#RowPopup .btn-danger").on("click", function (e) {
        deletecustomer(customerid);
    })

});






$(document).on('change', '#selectall', function () {


    checked = $(this).prop('checked');

    if (checked) {
        $(".selectAllBtn").click();
    } else {
        $(".selectNoneBtn").click();

    }
});

$(document).on('click', '#Update input.curdbtn', function () {

    

    var data = {};
    data.customerid = parseInt($("#CustomerId").val());
    data.firstName = $("#FirstName").val();
    data.lastname = $("#LastName").val();
    data.email = $("#Email").val();
    data.mobile = $("#Mobile").val();
    data.address = $("#Address").val();
    
    $.get("/home/getsessionvalue", function (res) {
        data.modifiedby = res;
    });


    data.isactive = $('#IsActive').is(":checked");
    data.countryid = parseInt($("#Countryid").val());
    data.stateid = parseInt($("#Stateid").val());
    data.password = "abcD@1234";

    
    console.log(data);
   updatecustomer(data);
});

$(document).on('click', "#Create input.curdbtn", function () {
    var data = {};
    data.firstName = $("#FirstName").val();
    data.lastname = $("#LastName").val();
    data.email = $("#Email").val();
    data.mobile = $("#Mobile").val();
    data.address = $("#Address").val();

    $.get("/home/getsessionvalue", function (res) {
        data.createdby = res;
    });


    data.isactive = $('#IsActive').is(":checked");
    data.countryid = parseInt($("#Countryid").val());
    data.stateid = parseInt($("#Stateid").val());
    data.password = $('#Password').val();


    console.log(data);
    addcustomer(data);

});


