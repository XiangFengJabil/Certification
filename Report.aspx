<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Certification.MonthReport" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>培训月报</title>
    <link href="Styles/global.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/echarts.js"></script>


    <script type="text/javascript">
        function getData(requestType) {
            var val = "";
            $.ajax({
                type: "GET",
                url: "Handler/GetChartsData.ashx",
                async: false,
                data: "requestType=" + requestType,
                success: function (data) {
                    if (data != "" && data != undefined)
                        val = data;
                }
            });
            return val;
        }

        //带key值的json对象转化为二维数组
        function toArray(jsonData) {
            //[{},{},{}] To [[],[],[]]
            var res = []
            for (var i in jsonData) {
                var item = [];
                for (var j in jsonData[i]) item.push(jsonData[i][j]);
                res.push(item)
            }
            return res;
        }


        $(function () {


            //Workcell技能认证分布
            var workcellData = getData("GetWorkcell");
            var workcellChart = echarts.init(document.getElementById('workcell'));
            var wordOption = {
                title: {
                    text: 'Workcell技能认证分布',
                    left: 'center'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                series: [{
                    name: '证书个数',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '50%'],
                    data: JSON.parse(workcellData)
                }]
            };
            workcellChart.setOption(wordOption);

            //Category技能认证分布
            var categoryData = getData("GetCategory");
            var categoryChart = echarts.init(document.getElementById('category'));
            var categoryName = new Array();
            var categoryValue = new Array();

            $.each(JSON.parse(categoryData), function (i, e) {
                categoryName.push(e.name);
                categoryValue.push(e.value);
            });

            var categoryOption = {
                title: {
                    text: 'Category技能认证分布',
                    left: '25%'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{b} : {c} "
                },
                grid: {
                    left: '0',
                    right: '39%',
                    bottom: '10%',
                    containLabel: true
                },
                xAxis: {
                    type: 'value',
                    boundaryGap: [0, 0.01]
                },
                yAxis: {
                    data: categoryName
                },
                series: [
                    {
                        type: 'bar',
                        data: categoryValue
                    }
                ]
            };
            categoryChart.setOption(categoryOption);

            //所有证书分布
            var allChart = echarts.init(document.getElementById('allRecord'));
            var allData = JSON.parse(getData("AllData"));
            var res = toArray(allData);
            var allOption = {
                title: {
                    text: '所有证书分布',
                    x: 'center'
                },
                tooltip: {
                    trigger: 'axis'
                },
                xAxis: {
                    data: res.map(function (item) {
                        return item[0];
                    })
                },
                yAxis: {
                    splitLine: {
                        show: false
                    }
                },
                toolbox: {
                    right: '260',
                    top: '20',
                    feature: {
                        dataZoom: {
                            yAxisIndex: 'none'
                        },
                        restore: {},
                        saveAsImage: {}
                    }
                },
                dataZoom: [{
                    startValue: 'TS1'
                }, {
                    type: 'inside'
                }],
                visualMap: {
                    top: 30,
                    right: 170,
                    pieces: [{
                        gt: 0,
                        lte: 10,
                        color: '#096'
                    }, {
                        gt: 10,
                        lte: 20,
                        color: '#ffde33'
                    }, {
                        gt: 20,
                        lte: 30,
                        color: '#ff9933'
                    }, {
                        gt: 30,
                        lte: 40,
                        color: '#cc0033'
                    }, {
                        gt: 40,
                        color: '#660099'
                    }],
                    outOfRange: {
                        color: '#999'
                    }
                },
                series: {
                    type: 'line',
                    data: res.map(function (item) {
                        return item[1];
                    }),
                    markLine: {
                        silent: true,
                        data: [{
                            yAxis: 10
                        }, {
                            yAxis: 20
                        }, {
                            yAxis: 30
                        }, {
                            yAxis: 40
                        }]
                    }
                }
            }
            allChart.setOption(allOption);




        });


    </script>


</head>
<body>

    <div style="margin-left: 160px; width: 420px; height: 300px;">
        <div id="workcell" style="width: 100%; height: 100%;"></div>
    </div>

    <div style="width: 100%; height: 300px; position: relative; left: 580px; top: -300px;">
        <div id="category" style="width: 100%; height: 100%;"></div>
    </div>

    <div style="margin: 0,auto; margin-top: -320px;">
        <div id="allRecord" style="width: 100%; height: 490px;"></div>
    </div>


</body>
</html>
