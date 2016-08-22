<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UseJiebaSegmenter._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    <link rel="stylesheet" href="vendor/bootstrap-3.3.6-dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css" />
    <link href="vendor/css/layout.css" rel="stylesheet" />
    <link href="vendor/css/normalize.css" rel="stylesheet" />
    <link href="vendor/css/page.css" rel="stylesheet" />
    <link href="vendor/css/generic_class.css" rel="stylesheet" />
    <link href="vendor/css/hover-min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="page_container">
            <asp:Panel ID="Panel1" runat="server">
                <div class="page_inner radius_CD_10">
                    <h3 class="reset page_title bg_2color_orange">分數列表</h3>
                    <span class="input-group-btn">
                        <asp:Button ID="Button1" runat="server" Text="重新整理" OnClick="Button1_Click" CssClass="btn btn-success"/>
                        <asp:Button ID="Button3" runat="server" Text="變異點列表" OnClick="Button3_Click" CssClass="btn btn-info"/>
                    </span>
                    <asp:ListView ID="ListView1" runat="server" DataSourceID="objdsFBScorePercent">
                        <LayoutTemplate>
                            <table class="table table-striped table_list m_t10">
                                <tr class="bg_iron_gray">
                                    <th class="text-center">分數</th>
                                    <th class="text-center bg_blue_green">筆數</th>
                                    <th class="text-center bg_blue_green">列表</th>
                                </tr>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="text-center bg-info"><%# Eval("FB_Score") %></td>
                                <td class="text-center"><%# Eval("number") %></td>
                                <td class="text-center">
                                    <a href='Score.aspx?score=<%# Eval("FB_Score") %>' target="_blank" class="btn btn-primary">列表</a>
                                    <asp:Button CssClass="btn btn-danger" ID="Button5" runat="server" Text="全部無變異點" OnClientClick="return confirm('確定全部無變異點嗎?');" OnCommand="Button5_Command" CommandArgument='<%# Eval("FB_Score") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <br />
                <br />

            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server">
                <div class="page_inner radius_CD_10">
                    <h3 class="reset page_title bg_2color_orange">分數區間</h3>
                    <span class="input-group-btn"><asp:Button ID="Button2" runat="server" Text="重新整理" OnClick="Button2_Click" CssClass="btn btn-success"/></span><br />
                    更新時間：<asp:Label ID="Label3" runat="server" Text='<%# DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>'></asp:Label><br />
                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label><br />

                    <div id="chartdiv"></div>
                </div>
                <br />
                <br />
            </asp:Panel>
        </div>
    </form>
    <asp:ObjectDataSource ID="objdsFBScorePercent" runat="server" SelectMethod="getFBScorePercent" TypeName="UseJiebaSegmenter.JiebaService"></asp:ObjectDataSource>
</body>
</html>
