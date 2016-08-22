<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RSS_Correct.aspx.cs" Inherits="UseJiebaSegmenter.RSS_Correct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <h3 class="reset page_title bg_2color_orange">變異點列表</h3>
    <div class="page_inner radius_CD_10">
<%--        <span class="input-group-btn">
            <asp:Button ID="Button1" runat="server" Text="重新整理" OnClick="Button1_Click" CssClass="btn btn-success" />
            <asp:Button ID="Button3" runat="server" Text="回分數列表" OnClick="Button3_Click" CssClass="btn btn-info" />
        </span>--%>
        <asp:ListView runat="server" ID="ListView_result" DataSourceID="objdsRSSCorrectList">
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
            <EmptyDataTemplate>
                無資料，請先到列表判斷是否為變異點。
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
    <asp:ObjectDataSource ID="objdsRSSCorrectList" runat="server" SelectMethod="onStart" TypeName="UseJiebaSegmenter.JiebaService">
        <SelectParameters>
            <asp:Parameter Name="type" DefaultValue="4" />
            <asp:Parameter Name="score" DefaultValue="0" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
