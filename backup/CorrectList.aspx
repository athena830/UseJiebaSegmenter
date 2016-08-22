<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CorrectList.aspx.cs" Inherits="UseJiebaSegmenter.CorrectList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        <div class="page_container">
                <div class="page_inner radius_CD_10">
                    <h3 class="reset page_title bg_2color_orange">變異點列表</h3>
                    <span class="input-group-btn">
                        <asp:Button ID="Button1" runat="server" Text="重新整理" OnClick="Button1_Click" CssClass="btn btn-success"/>
                        <asp:Button ID="Button3" runat="server" Text="回分數列表" OnClick="Button3_Click" CssClass="btn btn-info"/>
                    </span>
                <asp:ListView runat="server" ID="ListView_result" DataSourceID="objdsFBCorrectList">
                    <LayoutTemplate>
                        <table class="table table-striped table-bordered table_list m_t10">
                            <tr class="bg_iron_gray">
                                <th class="text-center">序號</th>
                                <th class="text-center bg_blue_green" style="width: 40%;">訊息內容</th>
                                <th class="text-center bg_blue_green">關鍵字加權</th>
                                <th class="text-center bg_blue_green">得分</th>
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
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                </div>
        </div>
    </form>
    <asp:ObjectDataSource ID="objdsFBCorrectList" runat="server" SelectMethod="onStart" TypeName="UseJiebaSegmenter.JiebaService">
        <SelectParameters>
            <asp:Parameter Name="type" DefaultValue="3" />
            <asp:Parameter Name="score" DefaultValue="0" />
        </SelectParameters>
    </asp:ObjectDataSource>
</body>
</html>
