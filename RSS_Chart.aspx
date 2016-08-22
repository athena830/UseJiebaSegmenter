<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RSS_Chart.aspx.cs" Inherits="UseJiebaSegmenter.RSS_Chart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        #chartdiv {
            width: 100%;
            height: 500px;
            font-size: 11px;
        }

        .amcharts-pie-slice {
            transform: scale(1);
            transform-origin: 50% 50%;
            transition-duration: 0.3s;
            transition: all .3s ease-out;
            -webkit-transition: all .3s ease-out;
            -moz-transition: all .3s ease-out;
            -o-transition: all .3s ease-out;
            cursor: pointer;
            box-shadow: 0 0 30px 0 #000;
        }

            .amcharts-pie-slice:hover {
                transform: scale(1.1);
                filter: url(#shadow);
            }
    </style>
    <script type="text/javascript" src="https://www.amcharts.com/lib/3/amcharts.js"></script>
    <script type="text/javascript" src="https://www.amcharts.com/lib/3/pie.js"></script>
    <script type="text/javascript" src="https://www.amcharts.com/lib/3/themes/light.js"></script>
    <script type="text/javascript">
        var chart = AmCharts.makeChart("chartdiv", {
            "type": "pie",
            "startDuration": 0,
            "theme": "light",
            "addClassNames": true,
            "legend": {
                "position": "right",
                "marginRight": 100,
                "autoMargins": false
            },
            "innerRadius": "30%",
            "defs": {
                "filter": [{
                    "id": "shadow",
                    "width": "200%",
                    "height": "200%",
                    "feOffset": {
                        "result": "offOut",
                        "in": "SourceAlpha",
                        "dx": 0,
                        "dy": 0
                    },
                    "feGaussianBlur": {
                        "result": "blurOut",
                        "in": "offOut",
                        "stdDeviation": 5
                    },
                    "feBlend": {
                        "in": "SourceGraphic",
                        "in2": "blurOut",
                        "mode": "normal"
                    }
                }]
            },
            "dataProvider": <%= Chart_data %>,
            "valueField": "percent",
            "titleField": "score",
            "export": {
                "enabled": true
            }
        });

        chart.addListener("init", handleInit);

        chart.addListener("rollOverSlice", function (e) {
            handleRollOver(e);
        });

        function handleInit() {
            chart.legend.addListener("rollOverItem", handleRollOver);
        }

        function handleRollOver(e) {
            var wedge = e.dataItem.wedge.node;
            wedge.parentNode.appendChild(wedge);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <h3 class="reset page_title bg_2color_orange">分數區間分析</h3>
    <div class="page_inner radius_CD_10">
        <span class="input-group-btn">
            <asp:Button ID="Button2" runat="server" Text="重新整理" OnClick="Button2_Click" CssClass="btn btn-success" /></span><br />
        更新時間：<asp:Label ID="Label3" runat="server" Text='<%# DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>'></asp:Label><br />
        <asp:Label ID="Label2" runat="server" Text=""></asp:Label><br />

        <div id="chartdiv"></div>
    </div>
</asp:Content>
