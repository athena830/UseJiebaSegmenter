<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RSS_List.aspx.cs" Inherits="UseJiebaSegmenter.RSS_List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <h3 class="reset page_title bg_2color_orange">分數列表</h3>
    <div class="page_inner radius_CD_10">
        <span class="input-group-btn">
            <%--<asp:Button ID="Button1" runat="server" Text="重新整理" OnClick="Button1_Click" CssClass="btn btn-success" />--%>
            <%--<asp:Button ID="Button3" runat="server" Text="變異點列表" OnClick="Button3_Click" CssClass="btn btn-info" />--%>
        </span>
        <asp:ListView ID="ListView1" runat="server" DataSourceID="objdsRSSScorePercent">
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
                    <td class="text-center bg-info"><%# Eval("score") %></td>
                    <td class="text-center"><%# Eval("number") %></td>
                    <td class="text-center">
                        <a href='RSS_Detail.aspx?score=<%# Eval("score") %>' class="btn btn-primary">列表</a>
                        <asp:Button CssClass="btn btn-danger" ID="Button5" runat="server" Text="全部無變異點" OnClientClick="return confirm('確定全部無變異點嗎?');" OnCommand="Button5_Command" CommandArgument='<%# Eval("score") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                無資料，請先計算分數。
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
    <asp:ObjectDataSource ID="objdsRSSScorePercent" runat="server" SelectMethod="getRSSScorePercent" TypeName="UseJiebaSegmenter.JiebaService"></asp:ObjectDataSource>
</asp:Content>
