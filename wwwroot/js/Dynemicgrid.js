


function GetDataforTable(url,id,lftip,data) {

   
    

    $.ajax({
        type: 'Post',
        url: url,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (result) {

            renderDataInTheTable(result,id,lftip)
        },
        error: function () {
            alert("error");
        }
    });

}





function Datatablefunction(id, Blftip) {
    if ($.fn.DataTable.isDataTable(id)) {
        $(id).DataTable().clear().destroy();
    }

    if (Blftip == undefined) {
        Blftip = "lftip"
    }
    var table=  $(id).DataTable({
        dom: Blftip,
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
        //buttons: [
        //     'csv', 'excel'
        //], 
       //scrollX: "800px",

        buttons: [
            { extend: 'csv', className: 'curdbtn' },
            { extend: 'excel', className: 'curdbtn' },
            { extend: 'selectAll', className: 'd-none selectAllBtn' },
            { extend: 'selectNone', className: 'd-none selectNoneBtn' }

        ],
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        iDisplayLength: 5,
        aLengthMenu: [[5, 10, 15, -1], [5, 10, 15, "All"]],
        columnDefs: [{ className: 'select-checkbox', targets: 0 }, { orderable: false, targets: 0 }],

       //columnDefs: [
       //             {
       //                 "targets": [2],
       //                 "visible": false,
       //                 "searchable": false
       //             },
       //             {
       //                 "targets": [3],
       //                 "visible": false
       //             }
       //         ],
               //scrollX: true,

        order: [[1, "desc"]],

    });


    $('input.form-check-input').on('change', function (e) {
        e.preventDefault();

        var column = table.column($(this).val());


        column.visible(!column.visible());
        console.log($(this).prop('checked'));

    });


}





function renderDataInTheTable(data, id,lftip) {

    const iddiv = id.slice(0, -5)+"Div";




    $(id).empty();

    $(iddiv).empty();
    var hideshowcolumn = "";
    //body
    $.each(data, function (i,item) {


        if (i == 0) {
            var j = 0;
            var newRow = "<thead><tr><th><div class='checkbox mass_select_all_wrap'> <input type='checkbox'  id='selectall' data-to-table='tasks'><label></label> </div> </th>";
            $.map(item, function (value, key) {

                if (value != null) {
                    j++;

                    newRow += "<th>" + key + "</th>";
                    hideshowcolumn += '<li class="form-check">                        <input class="form-check-input" type="checkbox" value="'+j+'" id="'+j+'checkbox" checked>' +
                                            '<label class="form-check-label" for="' + j + 'checkbox">' + key + '</label></li > ';

                   
                            
                }

            });
            newRow += "</tr></thead><tbody>";


            $(id).append(newRow);
        }





       var newRow = "<tr><td></td>"
        $.map(item, function (value, key) {
            if(value!=null)
            newRow += "<td>" + value + "</td>"
        });
        newRow += "</tr>"


        $(id).append(newRow);

    });


    $(iddiv).append(hideshowcolumn);
    Datatablefunction(id,lftip)


}









$(document).ready(function () {

    //GetDataforTable("/Home/Dynemictablejson", "#DynemicGrid", "iftlp", null);
    GetDataforTable("/Home/CustomerDynemicTable", "#CustomerFilterTable", "iBtlp", null);


});













