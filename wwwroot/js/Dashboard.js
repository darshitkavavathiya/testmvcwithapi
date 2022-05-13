
    $(document).ready(function () {



        getactivecustomer();

        getchartdata();


    });

function getchartdata() {
    $.ajax({
        type: 'GET',
        url: '/Customer/GetCustomerForChart',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',


        success: function (result) {

            var xValues = [], yValues = [], barColors = ["green", "red", "blue", "green", "red", "blue", "green", "red", "blue"];


            xValues[0] = "0"; yValues[0] = "0";
            for (var i=0; i < result.length; i++)
            {
                var date = result[i].createdDate;
                date = date.substring(0, 10).toString().split("-").reverse().join("/");


                xValues[i+1] = date;
                yValues[i+1] = result[i].customerCount;
               
            }

            drawchart(xValues, yValues, barColors);


           
        },
        error: function () {
            alert("error");
        }
    });
}

function drawchart(xValues, yValues, barColors) {
   



    var myChart = new Chart("customerDashboardGraph", {
        type: "line",
        data: {
            labels: xValues,
            datasets: [{

                //borderColor: "green",
                backgroundColor: " rgba(0,0,0,0)",
                pointRadius: 4,
                pointBackgroundColor: "rgba(0,0,255,1)",
                data: yValues
            }]
        },
        options: {
            scales: {
                xAxes: [{
                    stacked: true
                }],
                yAxes: [{
                    stacked: true
                }]
            },
            legend: { display: false }
        }
    });
}

//function drawchart(xValues, yValues, barColors) {




//    var myChart = new Chart("customerDashboardGraph", {
//        type: "bar",
//        data: {
//            labels: xValues,
//            datasets: [{

//                backgroundColor: barColors,
//                data: yValues
//            }]
//        },
//        options: {
//            scales: {
//                xAxes: [{
//                    stacked: true
//                }],
//                yAxes: [{
//                    stacked: true
//                }]
//            },
//            legend: { display: false }
//        }
//    });
//}


function getactivecustomer() {
    $.ajax({
        type: 'GET',
        url: '/Customer/GetCustomerStates',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',


        success: function (result) {



            $("#active").empty();
            $("#active").text(result.active);

            $("#inactive").empty();
            $("#inactive").text(result.inactive);

            $("#total").empty();
            $("#total").text(result.total);
        },
        error: function () {
            alert("error");
        }
    });
}



