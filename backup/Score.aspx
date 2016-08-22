<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Score.aspx.cs" Inherits="UseJiebaSegmenter.Score" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="vendor/bootstrap-3.3.6-dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css" />
    <link href="vendor/css/layout.css" rel="stylesheet" />
    <link href="vendor/css/normalize.css" rel="stylesheet" />
    <link href="vendor/css/page.css" rel="stylesheet" />
    <link href="vendor/css/generic_class.css" rel="stylesheet" />
    <link href="vendor/css/hover-min.css" rel="stylesheet" />
    <script src="vendor/js/jquery-3.0.0.min.js"></script>
    <script type="text/javascript">
        //function Button_Disabled(obj) {
        //    var id = obj.id.substring(0, obj.id.length - 1);
        //    for (var i = 1; i <= 2; i++) {
        //        $("#" + id + i).attr("disabled", true);
        //        $("#" + id + i).attr("class", "btn btn-warning");
        //    }
        //}
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page_container">
            <div class="page_inner radius_CD_10">
                <h3 class="reset page_title bg_2color_orange">訊息明細表</h3>
                <span class="input-group-btn">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="Default.aspx" runat="server" CssClass="btn btn-success">回到列表</asp:HyperLink>
                </span>
                <asp:HiddenField ID="Hidden_score" runat="server" />
                <asp:ListView runat="server" ID="ListView_result" DataSourceID="objdsScoreDetail">
                    <LayoutTemplate>
                        <table class="table table-striped table-bordered table_list m_t10">
                            <tr class="bg_iron_gray">
                                <th class="text-center">序號</th>
                                <th class="text-center bg_blue_green" style="width: 40%;">訊息內容</th>
                                <th class="text-center bg_blue_green">關鍵字加權</th>
                                <th class="text-center bg_blue_green">得分</th>
                                <th class="text-center bg_blue_green">變異點</th>
                            </tr>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                        </table>
                        <div class="text-center">
                            <ul class="pagination">
                                <asp:DataPager ID="DataPager1" runat="server" PageSize="10">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" FirstPageText="«" />
                                        <asp:NumericPagerField ButtonCount="5" CurrentPageLabelCssClass="active" />
                                        <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" LastPageText="»" />
                                    </Fields>
                                </asp:DataPager>
                            </ul>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="text-center bg-info"><%# Eval("Number") %></td>
                            <td class="text-center"><%# Eval("Sentence") %></td>
                            <td class="text-center"><%# Eval("Memo") %></td>
                            <td class="text-center"><%# Eval("Score") %></td>
                            <td class="text-center">
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" OnCommand="LinkButton1_Command" CommandArgument='<%# Eval("ID") %>'>有</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-danger" OnCommand="LinkButton2_Command" CommandArgument='<%# Eval("ID") %>'>無</asp:LinkButton><%--OnClientClick="Button_Disabled(this);return true;"--%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </form>
    <asp:ObjectDataSource ID="objdsScoreDetail" runat="server" SelectMethod="onStart" TypeName="UseJiebaSegmenter.JiebaService">
        <SelectParameters>
            <asp:Parameter Name="type" DefaultValue="2" />
            <asp:ControlParameter ControlID="Hidden_score" Name="score" />
        </SelectParameters>
    </asp:ObjectDataSource>
</body>
</html>
