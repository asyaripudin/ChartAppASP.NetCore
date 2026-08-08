function getTemperature(tanggal)
{        
    var tanggal = $("#txtTanggal").val().split("/").reverse().join("-");
    if (tanggal == "") {
        swal({
            title: "Warning!",
            text: "Please Select date....",
            type: "warning",
            timer: 3000
        },
            function () {
                location.reload();
                $("#txtTanggal").focus();
            }
        );
       
    } else {

   
    var row = '';
    var counter = 1;
   
    $.ajax({
        url: '/Temperature/GetDataTemperature/',
        data: "tanggal=" + tanggal,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",        
        success: function (result) {  
           
             $.each(result, function (key, val) {
                row += '<tr><td style="text-align:center; font-size:12px">' + counter +
                    '<td style="text-align:center; font-size:12px;">' + val.time + '</td>' +
                    '<td style="text-align:center; font-size:12px;">' + val.radiasi + '</td>' +
                    '<td style="text-align:center;font-size:12px;">' + val.temperatur + '</td>' +
                    '</td></tr>';
                    counter++;          
            });
          
            $("#tableTemperature").append(row);
         
            $(document).ready(function () {
                $.noConflict();
                $('#tableTemperature').DataTable({
                    
                    pageLength: 25,
                    paging: true,
                    filter: true,
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    dom: "fltip",
                    lengthMenu: [[25, 50,100, 200, -1], [25,50, 100, 200, 'All']]
                });
               
            })
            
        }
    });

    }
}
function getGraphic(tanggal)
{
    var tanggal = $("#txtTanggal").val().split("/").reverse().join("-");
    if (tanggal == "") {
        swal({
            title: "Warning!",
            text: "Please Select date....",
            type: "warning",
            timer: 3000
        },
            function () {
                location.reload();
                $("#txtTanggal").focus();
            }
        );       
    } else {
        $(document).ready(function () {
            $.ajax({
                url: "/Temperature/GetGraphicTemperature/",
                data: "tanggal=" + tanggal,
                type: "GET",
                contentType: "application/json; charset=utf8",
                dataType: "json",
                success: OnSuccess,
            });
            function OnSuccess(data) {                         

                const lineChartRadiation = document.getElementById('lineChartRadiation');//1
                const lineChartTemperature = document.getElementById('lineChartTemperature');//1
                //let prev = 100;

                var _data = data;
                var _labels = _data[0];
                var _Radiation = _data[1];
                var _Temperature = _data[2];
                var color1 = ['Red'];
                var color2 = ['#FEB25C '];

                new Chart(lineChartRadiation,//2
                    {
                       
                            type: 'line',//3
                            data: {
                                labels: _labels,
                                datasets: [
                                    {
                                        label: 'Radiation',
                                        backgroundColor: color1,
                                        data: _Radiation,
                                        borderWidth: 1,
                                        fill: false
                                    }
                                ]
                            },
                            options: {

                                responsive: true,
                                scales: {
                                    x: {
                                        display: true,
                                        title: {
                                            display: true,
                                            text: 'Time',
                                            color: '#911',
                                            font: {
                                                family: 'Comic Sans MS',
                                                size: 20,
                                                weight: 'bold',
                                                lineHeight: 1.2,
                                            },
                                            padding: { top: 20, left: 0, right: 0, bottom: 0 }
                                        },
                                        //animation
                                            //type: 'number',
                                            //easing: 'linear',
                                            //duration: 5000,
                                            //from: NaN, // the point is initially skipped
                                            //delay(ctx) {
                                            //    if (ctx.type !== 'data' || ctx.xStarted) {
                                            //        return 0;
                                            //    }
                                            //    ctx.xStarted = true;
                                            //    return ctx.index * 5000;
                                            //}

                                    },
                                    y: {
                                        display: true,
                                        title: {
                                            display: true,
                                            text: 'Radiation',
                                            color: '#191',
                                            font: {
                                                family: 'Times',
                                                size: 20,
                                                style: 'normal',
                                                lineHeight: 1.2
                                            },
                                            padding: { top: 30, left: 0, right: 0, bottom: 0 }
                                        },
                                        // Animation
                                        //type: 'number',
                                        //easing: 'linear',
                                        //duration: 5000,
                                        //from: prev,
                                        //delay(ctx) {
                                        //    if (ctx.type !== 'data' || ctx.yStarted) {
                                        //        return 0;
                                        //    }
                                        //    ctx.yStarted = true;
                                        //    return ctx.index * 5000;
                                        //}
                                    }
                                },
                                plugins: {
                                    legend: {
                                        position: 'top',
                                    },
                                    title: {
                                        display: true,
                                        text: 'Radiation Chart' + ' ' + 'on' + ' ' + $("#txtTanggal").val()
                                    }
                                }
                            },
                    });

                    ////animatuon
                    //for (let i = 0; i < 1000; i++) {
                    //    prev += 5 - Math.random() * 10;
                    //    _data.push({ x: i, y: prev });
              
                    //}




                new Chart(lineChartTemperature,//2
                    {
                        type: 'line',//3
                        data: {
                            labels: _labels,
                            datasets: [
                                {
                                    label: 'Temperature',
                                    backgroundColor: color2,
                                    data: _Temperature,
                                    borderWidth: 1,
                                    fill: false
                                }
                            ]
                        },
                        options: {
                           
                            responsive: true,
                            scales: {
                                x: {
                                    display: true,
                                    title: {
                                        display: true,
                                        text: 'Time',
                                        color: '#911',
                                        font: {
                                            family: 'Comic Sans MS',
                                            size: 20,
                                            weight: 'bold',
                                            lineHeight: 1.2,
                                        },
                                        padding: { top: 20, left: 0, right: 0, bottom: 0 }
                                    }
                                },
                                y: {
                                    display: true,
                                    title: {
                                        display: true,
                                        text: 'Temperature',
                                        color: '#191',
                                        font: {
                                            family: 'Times',
                                            size: 20,
                                            style: 'normal',
                                            lineHeight: 1.2
                                        },
                                        padding: { top: 30, left: 0, right: 0, bottom: 0 }
                                    }
                                }
                            },
                            plugins: {
                                legend: {
                                    position: 'top',
                                },
                                title: {
                                    display: true,
                                    text: 'Temperature Chart'+ ' ' + 'on' + ' ' + $("#txtTanggal").val()
                                }
                            }
                        },
                    });


            }
        });
    }
}

function ImportFile() {  
    $(function () {
        var files = $("#importFile").get(0).files;
        var numFiles = $("input:file", this)[0].files.length;
        
        if (numFiles < 1) {
            swal({
                title: "Import Failed!",
                text: "Please Select File to Import....",
                type: "success",
                timer: 3000
            },
                function () {
                    location.reload();
                }
            );
        }
        else {
            var formData = new FormData();            
            formData.append('importFile', files[0]);
          
            $.ajax({
                url: '/Temperature/ImportFile/',
                data: formData,
                type: 'POST',
                contentType: false,
                processData: false,

                success: function (result) {
                    if (result != "") {
                        swal({
                            title: "Import Success...",
                            text: result,
                            type: "success",
                            timer: 3000
                        },
                            function () {
                                location.reload();
                            }
                        );
                    }
                    else {
                        swal({
                            title: "Import Failed!",
                            text: result,
                            type: "success",
                            timer: 3000
                        },
                            function () {
                                location.reload();
                            }
                        );
                    }
                }
            });
        }
       
    });
    
}

function refreshTable() {
    $(document).ready(function () {
        location.reload();
    });
}

