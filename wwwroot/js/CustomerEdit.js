
function openpopover(clicked_id) {
    var id = clicked_id + "_popup";

    closepopup();
    if (id != "_popup") {
        var obj = document.getElementById(id);

        obj.classList.toggle("show");

    }
}
function hideloader()
{
    $(".mvcloader").addClass("d-none");
}
function showloader() {
    $(".mvcloader").removeClass("d-none");
}
function closepopup() {
    var popuphide = document.getElementsByClassName("popuptext show");
    for (i = 0; i < popuphide.length; i++) {

        popuphide[i].classList.remove("show");
    }
}

function GetDocumentDataById(id) {
 

    $.ajax({
        type: 'GET',
        url: '/Customer/GetDocumentDataById/' + id,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (result) {
            if (result) {
                if (result.documentType == "application/pdf") {
                    $("#PreviewModelBody").addClass("modelpreviewheight");
                } else {
                    $("#PreviewModelBody").removeClass("modelpreviewheight");

                }
                
                var html = '<embed src="' + result.src + '" width="100%" height="100%"/>';
                $("#PreviewModelBody").append(html);
            }
        },
        error: function () {
            alert("error");
        }
    });
}

function GetCustomerDocument(customerId) {

    var data = {};
    data.CustomerId = parseInt(customerId);
   

    showloader();
    $.ajax({
        type: 'POST',
        url: '/Customer/GetCustomerDocuments',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (result) {


            hideloader();
            if (result.length > 0)
            {

                $("#DocumentGridTable").empty();
                // add table head
                var head = '  <thead><tr><th>Id</th><th>Name</th><th>Date</th><th class="text-center">Action</th></tr></thead><tbody>';

                $("#DocumentGridTable").append(head);

                 // table row 
                for (var i = 0; i < result.length; i++) {
                    // date formeting 
                    var date = result[i].uploadDate;
                    date = date.substring(0, 10).toString().split("-").reverse().join("/");

                    // popup field formeting

                    var popupfield = ' <p  class="DeleteAction" data-value=' + result[i].documentId + '> Delete</p> '
                        + '<p class="DownloadAction"  data-value=' + result[i].documentId + '> Download </p> '

                    var doctype = result[i].documentType

                            if (!doctype.includes("application/vnd.openxmlformats")) {
                                popupfield += '<p  class="PreviewAction" data-value=' + result[i].documentId + '> Preview </p>'

                            }

                    var body = '<tr><td>' + result[i].documentId + '</td><td>' + result[i].documentName +
                        '</td><td> ' + date +
                        '  </td><td class="text-center"> <div class="popup" >' +
                        '   <img class="actionimg"  id="' + result[i].documentId + '" src="/Images/group-38.png" alt="...">' +
                        '    <div class="popuptext" id="' + result[i].documentId + '_popup">' + popupfield + '    </div></div>' +
                        '   </td></tr> ';


                    // onclick="reply_click(this.id)"

                    $("#DocumentGridTable").append(body);

                }
                $("#DocumentGridTable").append("</tbody>");
                Datatablefunction("#DocumentGridTable");

            }
            else {
                $("#DocumentGridTable").empty();
            }

        },
        error: function () {
            alert("error");
        }
    });


}


function Datatablefunction(id) {
    if ($.fn.DataTable.isDataTable(id)) {
        $(id).DataTable().clear().destroy();
    }
    $(id).DataTable({

        dom: 'tip',
        responsive: true,
        pagingType: "full_numbers",
        language: {

            paginate: {
                first: "",
                previous: "previous",
                next: "next",
                last: "",
            },
            info: "Total Records : _MAX_",

            lengthMenu: "Show  _MENU_  Entries",
        },
        iDisplayLength: 10,
        aLengthMenu: [[5, 10, 15, -1], [5, 10, 15, "All"]],

        columnDefs: [{ orderable: false, targets: 3 }],
        order: [[0, "desc"]],



    });
}


function DeleteDocument(id) {


 

    $.ajax({
        type: 'GET',
        url: '/Customer/DeleteDocument/'+id,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        //data: data,
        success: function (result) {

            if (result) {

                GetCustomerDocument($("#CustomerId").val());

                swal("Document no:" + id + " is deleted successfully", {
                    icon: "success",
                    timer:3000,
                });
            }



        },
        error: function () {
            alert("error");
        }
    });









    
}




$(document).ready(function () {
    
    GetCustomerDocument($("#CustomerId").val());
});


$(document).on("click", ".actionimg", function (e) {
    openpopover(e.target.id);
});

$(document).on("mouseleave", "#DocumentGridTable", function () { closepopup() });

// upload document
$(document).on("click", "#DocumentUploadBtn", function () {


    const formdata = new FormData();
    formdata.append("formFile", $('#DocumentUpload').prop('files')[0]);
    formdata.append("customerId", $("#CustomerId").val());
    $.ajax({
        type: 'POST',
        url: '/Customer/UploadDocument',
        contentType: false,
        data: formdata,
        processData: false,
        success: function (result) {
            //con.log(result);
            GetCustomerDocument($("#CustomerId").val());
            $("#AddDocumentModel Button.close").click();
             firecustomtoast("", "File Upload Successful", "success");

        },
        error: function () {
            alert("error");
        }
    });


});





$(document).on('change', "#DocType", function () {

    $('#DocumentUpload').prop('disabled', false);
    $('#DocumentUpload').val("");
});




$(document).on('change', "#DocumentUpload", function () {

    var file = $('#DocumentUpload').prop('files')[0];
  
    if ($("#DocType").val() != file.type) {
        $('#DocumentUploadBtn').prop('disabled', true);
        $('#documentvalidation').text('please select valid file');

    } else if (file.size > 2097152) {
        $('#DocumentUploadBtn').prop('disabled', true);
        $('#documentvalidation').text('file size is more than 2mb ');


    } else {
        $('#DocumentUploadBtn').prop('disabled', false);
        $('#documentvalidation').empty();
    }

});




$(document).on("click", ".DeleteAction", function () {
  var docid = $(this).attr("data-value");
    swal({
        title: "Are you sure",
        text: "Press Delete to delete document no:" + docid,
        icon: "error",
        animation: true,
        toast: true,
            dangerMode: true,
        buttons: {
            cancel: true,

            confirm: {
                text: "Delete",
                value: docid,
            },
        }
    }).then((value) => {
        if (value==docid) {
            DeleteDocument(value);
        } 
    });;
});



$(document).on("click", ".PreviewAction", function () {
   var docid = $(this).attr("data-value");
    $("#PreviewModelBtn").click();
    $("#PreviewModelBody").empty();
    GetDocumentDataById(docid);
});

$(document).on("click", ".DownloadAction", function () {

    var docid = $(this).attr("data-value");

    window.location.href = "/Customer/DownloadDocument/" + docid;
  
});












